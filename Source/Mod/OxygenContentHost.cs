using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using RimWorld;
using HarmonyLib;
using Oxygendsl.Core;
using Oxygendsl.Language;
using Oxygendsl.Mod.Content;
using Oxygendsl.Mod.Runtime;
using Verse;

namespace Oxygendsl.Mod
{
    public sealed class OxygenContentEntry
    {
        public ModContentPack Provider { get; internal set; }
        public string PackageId { get; internal set; }
        public string ContentSignature { get; internal set; }
        public string LayoutSignature { get; internal set; }
        public ContentPlan Content { get; internal set; }
        public ContentPlan Rules { get; internal set; }
        public IReadOnlyList<NativeContentChange> Changes { get; internal set; } = Array.Empty<NativeContentChange>();
        public bool Enabled { get; internal set; } = true;
        public string Error { get; internal set; } = string.Empty;
        public string EffectMode { get; internal set; } = "Startup content; rules reloadable";
    }

    public static class OxygenContentHost
    {
        static readonly List<OxygenContentEntry> entries = new List<OxygenContentEntry>();
        static bool installed;
        static int revision;
        public static IReadOnlyList<OxygenContentEntry> Entries { get { return entries; } }
        public static int Revision { get { return revision; } }

        internal static void Install()
        {
            if (installed) return;
            new Harmony("nanaloveyuki.oxygendsl.content").Patch(
                AccessTools.Method(typeof(LoadedModManager), nameof(LoadedModManager.ParseAndProcessXML)),
                prefix: new HarmonyMethod(typeof(OxygenContentHost), nameof(BeforeDefParsing)));
            installed = true;
        }

        // One framework-owned bridge, before XML inheritance, cross refs and native caches.
        static void BeforeDefParsing(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
        {
            for (int i = 0; i < entries.Count; i++) NativeRuleRuntime.RemoveProvider(entries[i].PackageId);
            entries.Clear();
            List<string> packages = new List<string>();
            foreach (ModContentPack active in LoadedModManager.RunningModsListForReading) packages.Add(active.PackageIdPlayerFacing);
            NativeContentSchema.Shared.SetActivePackages(packages);
            NativeContentSchema.Shared.MarkComplete();
            foreach (ModContentPack provider in LoadedModManager.RunningModsListForReading)
            {
                if (!HasEntry(provider, "content") && !HasEntry(provider, "rules")) continue;
                OxygenContentEntry entry = new OxygenContentEntry
                {
                    Provider = provider,
                    PackageId = provider.PackageIdPlayerFacing.ToLowerInvariant()
                };
                entries.Add(entry);
                try
                {
                    string source;
                    ReadEntry(provider, "content", out source);
                    RecordingCatalog modules = new RecordingCatalog();
                    ContentCompilation result = ContentCompiler.Compile(source, modules);
                    if (!result.Success) throw new InvalidOperationException(FormatDiagnostics(result));
                    if (result.Plan.Thoughts.Count != 0 || result.Plan.Gates.Count != 0)
                        throw new InvalidOperationException("content.oxy accepts definitions and patches; put runtime bindings in rules.oxy.");
                    NativeContentPipeline.Apply(result.Plan, xmlDoc, assetlookup, provider);
                    entry.Content = result.Plan;
                    entry.ContentSignature = modules.Signature(source);
                    entry.Changes = NativeContentPipeline.LastChanges;
                }
                catch (Exception exception)
                {
                    entry.Error = entry.PackageId + "/Oxygen/content.oxy: " + exception.Message;
                    entry.EffectMode = "Rejected before native registration";
                    Log.Error("[Oxygendsl] " + entry.Error);
                }
            }
            revision++;
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                foreach (OxygenContentEntry entry in entries)
                    if (entry.Content != null) entry.LayoutSignature = LoadedLayoutSignature(entry.Content);
                ReloadRules(null);
            });
        }

        public static bool ReloadRules(Pawn previewPawn)
        {
            return ReloadRules(previewPawn, null);
        }

        public static bool ReloadRulesFor(string packageId, Pawn previewPawn)
        {
            if (string.IsNullOrWhiteSpace(packageId)) throw new ArgumentException("A provider package is required.", nameof(packageId));
            return ReloadRules(previewPawn, packageId);
        }

        static bool ReloadRules(Pawn previewPawn, string packageId)
        {
            bool success = true;
            bool found = false;
            for (int i = 0; i < entries.Count; i++)
            {
                OxygenContentEntry entry = entries[i];
                if (packageId != null && !string.Equals(entry.PackageId, packageId, StringComparison.OrdinalIgnoreCase)) continue;
                found = true;
                if (entry.Content == null) { success = false; continue; }
                try
                {
                    // Do not partially hot-apply structural edits or their dependent rules.
                    string content;
                    ReadEntry(entry.Provider, "content", out content);
                    RecordingCatalog modules = new RecordingCatalog();
                    ContentCompilation contentCheck = ContentCompiler.Compile(content, modules);
                    if (!contentCheck.Success) throw new InvalidOperationException(FormatDiagnostics(contentCheck));
                    if (!string.Equals(modules.Signature(content), entry.ContentSignature, StringComparison.Ordinal))
                    {
                        entry.EffectMode = "Restart required; file edits pending; previous rules retained";
                        throw new InvalidOperationException("Content or imported content changed. Restart to register it; runtime rules were not replaced.");
                    }
                    string source;
                    ReadEntry(entry.Provider, "rules", out source);
                    ContentCompilation result = ContentCompiler.Compile(source, OxygenModModuleCatalog.Instance, OxygenLocalization.Instance);
                    if (!result.Success) throw new InvalidOperationException(FormatDiagnostics(result));
                    if (result.Plan.Definitions.Count != 0 || result.Plan.Patches.Count != 0)
                        throw new InvalidOperationException("rules.oxy accepts thought_rule and gate_rule; put native declarations in content.oxy.");
                    NativeRuleRuntime.Validate(result.Plan);
                    if (previewPawn != null) NativeRuleRuntime.ValidateForPawn(result.Plan, previewPawn);
                    NativeRuleRuntime.SetProvider(entry.PackageId, result.Plan, entry.Enabled);
                    entry.Rules = result.Plan;
                    entry.Error = string.Empty;
                    entry.EffectMode = "Startup content active; current runtime rules active";
                }
                catch (Exception exception)
                {
                    entry.Error = entry.PackageId + "/Oxygen/rules.oxy: " + exception.Message;
                    success = false;
                }
            }
            revision++;
            return success && (packageId == null || found);
        }

        public static bool SetProviderEnabled(string packageId, bool enabled)
        {
            OxygenContentEntry entry = Find(packageId);
            if (entry == null || entry.Rules == null) return false;
            try { NativeRuleRuntime.SetEnabled(entry.PackageId, enabled); }
            catch (Exception exception)
            {
                entry.Error = exception.Message;
                revision++;
                return false;
            }
            entry.Enabled = enabled;
            revision++;
            return true;
        }

        internal static void RestoreGameState(IReadOnlyList<string> disabled, IReadOnlyList<ContentLayoutRecord> layouts)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                OxygenContentEntry entry = entries[i];
                bool desired = !Contains(disabled, entry.PackageId);
                if (layouts != null)
                    for (int j = 0; j < layouts.Count; j++)
                    {
                        ContentLayoutRecord saved = layouts[j];
                        if (saved == null || saved.PackageId != entry.PackageId || saved.Signature == entry.LayoutSignature) continue;
                        desired = false;
                        entry.Error = "Saved native content layout differs from installed content. Rules disabled; no stage/degree migration was performed.";
                        break;
                    }
                if (entry.Rules != null) SetProviderEnabled(entry.PackageId, desired);
            }
            NativeRuleRuntime.InvalidateSituationalThoughts();
            revision++;
        }

        public static string Inspect()
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < entries.Count; i++)
            {
                OxygenContentEntry entry = entries[i];
                text.Append(entry.PackageId).Append(entry.Enabled ? " [enabled]" : " [disabled]").AppendLine();
                text.AppendLine(entry.EffectMode);
                if (entry.Content != null)
                {
                    text.Append("Native definitions: ").Append(entry.Content.Definitions.Count)
                        .Append("; patches: ").Append(entry.Content.Patches.Count).AppendLine();
                    for (int j = 0; j < entry.Content.Definitions.Count; j++)
                    {
                        ContentDefinition definition = entry.Content.Definitions[j];
                        text.Append("  ").Append(definition.Type).Append('/').Append(definition.Id)
                            .AppendLine(" — referenced native content is not deleted when rules are disabled");
                    }
                    foreach (NativeContentChange change in entry.Changes)
                        text.Append("  ").Append(change.Order).Append(' ').Append(change.Kind).Append(' ').Append(change.Type)
                            .Append('/').Append(change.Id).Append('.').Append(change.Field).Append(": ")
                            .Append(change.Before).Append(" -> ").Append(change.After).AppendLine();
                }
                if (!string.IsNullOrEmpty(entry.Error)) text.AppendLine(entry.Error);
                text.AppendLine();
            }
            text.Append(NativeRuleRuntime.Inspection);
            return text.ToString();
        }

        public static string Preview(Pawn pawn)
        {
            if (pawn == null) return "Select a pawn to preview native thoughts and gates.";
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < entries.Count; i++)
                if (entries[i].Rules != null) text.AppendLine(NativeRuleRuntime.PreviewThoughts(entries[i].PackageId, pawn));
            return text.ToString();
        }

        static OxygenContentEntry Find(string packageId)
        {
            for (int i = 0; i < entries.Count; i++)
                if (string.Equals(entries[i].PackageId, packageId, StringComparison.OrdinalIgnoreCase)) return entries[i];
            return null;
        }

        static bool Contains(IReadOnlyList<string> values, string value)
        {
            if (values != null)
                for (int i = 0; i < values.Count; i++)
                    if (string.Equals(values[i], value, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        static bool HasEntry(ModContentPack provider, string name)
        {
            string unused;
            return ReadEntry(provider, name, out unused);
        }

        internal static bool ReadEntry(ModContentPack provider, string name, out string source)
        {
            source = string.Empty;
            // Version/DLC folders override the same relative entry, matching native file lookup.
            for (int i = 0; i < provider.foldersToLoadDescendingOrder.Count; i++)
            {
                string path = Path.Combine(provider.foldersToLoadDescendingOrder[i], "Oxygen", name + ".oxy");
                if (!File.Exists(path)) continue;
                source = File.ReadAllText(path);
                return true;
            }
            return false;
        }

        static string FormatDiagnostics(ContentCompilation result)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < result.Diagnostics.Count; i++) text.AppendLine(result.Diagnostics[i].ToString());
            return text.ToString().TrimEnd();
        }

        static string LoadedLayoutSignature(ContentPlan plan)
        {
            StringBuilder text = new StringBuilder();
            foreach (ContentDefinition definition in plan.Definitions)
            {
                text.Append(definition.Type).Append('/').Append(definition.Id).Append(':');
                if (definition.Type == "TraitDef")
                {
                    TraitDef trait = DefDatabase<TraitDef>.GetNamedSilentFail(definition.Id);
                    if (trait == null) text.Append("missing");
                    else foreach (TraitDegreeData degree in trait.degreeDatas) text.Append('[').Append(degree.degree).Append(':').Append(degree.label).Append(']');
                }
                else if (definition.Type == "ThoughtDef")
                {
                    ThoughtDef thought = DefDatabase<ThoughtDef>.GetNamedSilentFail(definition.Id);
                    if (thought == null) text.Append("missing");
                    else
                    {
                        text.Append(thought.workerClass == null ? string.Empty : thought.workerClass.FullName);
                        text.Append(thought.thoughtClass == null ? string.Empty : thought.thoughtClass.FullName);
                        foreach (ThoughtStage stage in thought.stages) text.Append('[').Append(stage.label).Append(']');
                    }
                }
                text.AppendLine();
            }
            return Hash(text.ToString());
        }

        static string Hash(string value)
        {
            using (SHA256 sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty);
        }

        sealed class RecordingCatalog : IOxygenModuleCatalog
        {
            readonly SortedDictionary<string, string> sources = new SortedDictionary<string, string>(StringComparer.Ordinal);
            public bool TryGetSource(string packageId, string modulePath, out string source)
            {
                bool found = OxygenModModuleCatalog.Instance.TryGetSource(packageId, modulePath, out source);
                if (found) sources[packageId.ToLowerInvariant() + "/" + modulePath] = source;
                return found;
            }
            internal string Signature(string entry)
            {
                StringBuilder text = new StringBuilder(entry);
                foreach (KeyValuePair<string, string> source in sources)
                    text.Append('\0').Append(source.Key).Append('\0').Append(source.Value);
                return Hash(text.ToString());
            }
        }
    }

    public sealed class ContentLayoutRecord : IExposable
    {
        public string PackageId;
        public string Signature;
        public void ExposeData()
        {
            Scribe_Values.Look(ref PackageId, "packageId");
            Scribe_Values.Look(ref Signature, "layoutSignature");
        }
    }
}
