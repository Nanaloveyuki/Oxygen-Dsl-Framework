using System;
using System.Collections.Generic;
using Oxygendsl.Core;
using Oxygendsl.Language;
using Verse;

namespace Oxygendsl.Mod
{
    public interface IOxygenSettingsBridge
    {
        void Replace(Verse.Mod owner, OxygenSettingsPlan plan, IOxygenSettingsValues values);
    }

    public static class OxygenSettingsHost
    {
        public static OxygenHostModuleRegistry Modules { get; } = new OxygenHostModuleRegistry();
        static readonly Dictionary<Verse.Mod, IOxygenSettingsValues> owners = new Dictionary<Verse.Mod, IOxygenSettingsValues>();
        static readonly HashSet<Verse.Mod> initialized = new HashSet<Verse.Mod>();
        static IOxygenSettingsBridge bridge;

        // Game thread only; the optional adapter is loaded without adding an IrisMenus assembly reference here.
        public static void InstallBridge(IOxygenSettingsBridge settingsBridge)
        {
            if (settingsBridge == null) throw new ArgumentNullException(nameof(settingsBridge));
            if (bridge != null) throw new InvalidOperationException("A settings bridge is already installed.");
            bridge = settingsBridge;
            foreach (Verse.Mod owner in initialized) Reload(owner);
        }

        public static void Register(Verse.Mod owner, IOxygenSettingsValues values)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (owners.ContainsKey(owner)) throw new ArgumentException("Settings owner already registered.", nameof(owner));
            owners.Add(owner, values);
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                Reload(owner);
                initialized.Add(owner);
            });
        }

        public static bool Reload(Verse.Mod owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (!owners.TryGetValue(owner, out IOxygenSettingsValues values))
                throw new ArgumentException("Settings owner is not registered.", nameof(owner));
            try
            {
                string packageId = owner.Content.PackageIdPlayerFacing;
                if (!OxygenModModuleCatalog.Instance.TryGetSource(packageId, "settings", out string source))
                    throw new InvalidOperationException("Missing Oxygen/settings.oxy.");
                OxygenSettingsCompilation compilation = OxygenSettingsCompiler.Compile(source, bridge != null,
                    OxygenModModuleCatalog.Instance, Modules, Modules, OxygenLocalization.Instance);
                if (!compilation.Success)
                {
                    var messages = new List<string>();
                    foreach (ScriptDiagnostic diagnostic in compilation.Diagnostics) messages.Add(diagnostic.ToString());
                    throw new InvalidOperationException(string.Join("; ", messages));
                }
                bridge?.Replace(owner, compilation.Plan, values);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("[Oxygendsl] " + owner.Content.PackageIdPlayerFacing + "/Oxygen/settings.oxy: "
                    + exception.Message + " Previous settings pages retained.");
                return false;
            }
        }
    }
}
