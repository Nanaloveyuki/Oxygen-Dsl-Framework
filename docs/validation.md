# Oxygendsl 验证记录

## lang 与 0.0.1 发布（2026-10-10）

- 框架与 PrisonerNames Release 构建均为 0 警告、0 错误；框架 508 项行为检查、消费端 20 项命名检查及 Blockly 11 项检查通过。
- 独立 Mono 进程运行真实 `Verse.Translator`、`NamedArgument` 与格式化器：18 个 smoke 场景覆盖已打包中英 Keyed、当前语言／默认回退／缺键（含开发者模式）、富文本保留、导入设置回调随语言变化、已迁移的 `settings.oxy`、心情注册、运行思想规则、重复回调标量参数替换、实际 `NamingPreview`、加载期内容拒绝翻译宿主缺失、离线错误、无活动语言及已加载自定义 Def 完整命名空间目录。预置真实语言对象的 Keyed 字典和 worker，未启动 Unity 或加载完整游戏。原生开发者模式会伪翻译完全缺失的键；实际 smoke 复现后修正为保留键名，只对真实默认语言回退保留原生伪翻译行为。
- 真实 CLI 的 7 个临时 Mod fixture：`SiteDefs/RHAH_RecordSite` 明确报告 `Verse.MapGeneratorDef`，`IncidentDefs/RHAH_RefugeeCamp` 明确报告 `Verse.GenStepDef`，退出 1；正确类型目录与兼容复数目录退出 0；不完整目录的缺失外部 Def／未知自定义类型退出 2。报告只校验类型与 Def 身份，不声称字段路径或注入完成。
- 实际 Editor 进程的 4 个请求验证免导入 `lang`、导出函数使用命名参数、保留内置名称及消费端设置脚本；17 个片段已包含 `lang`。更新的 VSIX 0.2.0 打包成功，发布 Core／Editor 程序集，不携带游戏程序集。
- 0.0.1 元数据与框架程序集版本已设置；网页资产重新打包。发布目录为 `tmp/releases/0.0.1/`，框架 54 个文件、PrisonerNames 12 个文件逐项 SHA-256 校验一致，不携带共享依赖或游戏程序集；框架另带自己的可选适配 DLL。未修改 RHAH，未生成真实游戏翻译报告，未新增游戏窗口视觉验收，也未覆盖玩家安装目录。发布包仍应与此前游戏验收边界分开看待。

## 本机网页编辑器与囚犯玩家配置（2026-10-10）

- 框架与 PrisonerNames Release 编译均为 0 警告、0 错误；ODF 494 项行为检查、消费端 20 项命名行为检查、Blockly 11 项生成／文档检查通过。最终 VSIX 打包成功，包含囚犯模板及本地资源，不携带游戏程序集。
- 链接生产 `GameComponent_PrisonerNames` 的 30 项状态 smoke：新囚犯初始分数、配置系数与脚本分数叠加、阈值升级、各囚犯独立值、手动分数／等级／代号优先、事件异常原子回滚、原名保留、暂停／释放间隔不计托管时间、独立命名／事件草稿、配置与自定义规则切换、记录恢复及旧存档自定义规则保留。游戏／Pawn 事实与 Scribe 边界用替身，不算游戏渲染或完整存读档证明。
- 实际 RimWorld `Scribe_Deep`／`Scribe_Values` 在独立 Mono 进程往返 XML，保存并恢复单人代号／分数／等级、编号、托管余量／时间基线及数字／文本／布尔／null。关闭独立进程中的 DeepProfiler 并预置类型缓存后完成；没有加载完整游戏或执行 Pawn 交叉引用，不能据此宣布游戏存读档验收完成。
- 链接生产事件总线并用 Harmony patch 引擎替身：按实际 before／after 抵抗差发布、无变化不发布、未确认越狱不发布、同次越狱去重、新游戏隔离和取消订阅通过。未在真实 RimWorld 执行新增原生事件补丁。
- IrisMenus 173 项引擎替身断言通过，包含所属 Mod 操作与确认期资格重查。链接生产文件 host 的独立 smoke 通过活动依赖筛选、真实文件保存／创建、路径逃逸与覆盖拒绝、同会话重开、手动停止及游戏退出回收。没有新的 Unity 设置窗口视觉验收。
- 生产 net48 HTTP 后端在 Mono 中运行，真实 Chromium 加载打包的 CodeMirror／Blockly。实际源码编辑／物理保存、`ART2005` 错误位置、显式事实预览 `Ada-007`、有效应用 `Ada-001`、错误应用保留有效结果、积木新建／字段编辑／JSON 保存／指定目标导出及重开通过。最终画布显示修改后的前缀「俘」，Human／number=2／score=20／Ben 试算为 `俘002·Noted·Ben`；无浏览器 JavaScript 错误。
- 同一 UI 观察外部文件修改后保存返回冲突，保留本地草稿；积木重载后为已保存而非假脏状态。实际 HTTP 验证 token、Host／Origin、CORS、固定静态资产、提前拒绝过大请求体、旧版本保存／应用冲突。后端 fixture 断言所有文档回调在 `Pump` 线程执行。
- 共用 Blockly 生成器 → 生产 HTTP → 实际 `NamingPreview`／`NamingRules` 得到 Ratkin=`鼠007·Valued·Ada`、Human=`囚002·Noted·Ben`；离线 CLI 读取生成源码也输出 `鼠007·Valued·Ada`。只执行显式事实，不模拟游戏行为。
- 实际结束后端后，页面保留画布并显示断连；修正断连遮罩宽度与 Blockly 层级遮挡后，在桌面和 640px 窄视口观察居中提示，焦点在重连按钮，保存／检查积木／导出均禁用。服务正常退出码为 0；页面不报告假保存成功。
- 最终本地发布打包校验：框架 54 个文件、PrisonerNames 12 个文件 SHA-256 一致。框架携带完整网页资源与许可证；消费包不重复携带共享程序集，未覆盖玩家游戏安装目录。
- 本轮未部署到玩家游戏目录，未实测新的 IrisMenus 窗口、真实 Pawn 事件、完整游戏存读档、跨地图／商队及大型 Mod 组合；此前的旧版代号游戏证据与本轮新增能力分开记录。

## VSCode 积木编辑器（2026-10-10）

- `npm test`：11 项生成／文档行为检查通过，覆盖表达式括号、字符串与控制字符转义、缺失连接、脱离语句的值积木、重复 ID／字段、路径边界、生成摘要和最小范围错误映射。
- 实际 Blockly 工作区分别加载、保存四个模板，再由共用生成器生成源码，无连接错误。实际 Core 消费端编译并执行心情模板：food=0.5 为 true，0.25／null 为 false；原生内容模板接受 TraitDef／ThoughtDef，设置模板在 IrisMenus 可用时注册一页、缺失时跳过。
- 实际 Editor 进程与文件系统配合 VSCode API shim：首次导出、原摘要复用、手改文件取消覆盖、确认期间目标变化、未保存目标、过期积木提交及未信任工作区拒绝均通过。shim 不算真实 UI 证明。
- 在真实 VSCode Server 的自定义编辑器打开 `.oxyblocks`，通过字段表单与画布原生编辑修改阈值，观察源码预览同步；`Ctrl+S` 后读取实际 JSON，重载后仍为修改值。临时自建工作区通过 VSCode 的 Trust 按钮授予信任，实际「检查积木」通过并由「导出 .oxy」写出匹配预览的文件。
- 最终 VSIX 在同一真实界面再次观察数值 0.6 → 0.7 → 撤销 0.6 → 重做 0.7，字段面板与预览同步。命令面板执行 **Oxygen: New Block Program**，选择模板、保存 `created.oxyblocks` 后自动进入画布；改输出路径并导出 `created.oxy`。实际 Core 消费端读取该 UI 导出文件，food=0.5 为 true，0.25／null 为 false，保存的积木含目标 URI 与 SHA-256。
- 真实撤销／重做复现重复文档回执覆盖尚未提交状态，修正为同版本回执不重载画布；最终路径观察 0.4 → 0.6 → 撤销 0.4 → 重做 0.6。切换焦点保留字段面板选中块，不再依赖 Blockly 全局焦点对象。
- 实际 MissingType 返回标注得到 `ART3018`，错误按钮定位函数块并高亮对应源码；`missing_rule` 动态引用没有静态报错，确认当前检查器不能证明所有动态绑定，不据此宣称完整类型检查。
- `npm run package:vsix` 成功，生成 `tmp/vscode/oxygen-language-support-0.2.0.vsix`，包含本地 Blockly、资源与 Apache-2.0 许可，无 CDN、独立网页服务或游戏／Unity 程序集。截图在忽略目录 `tmp/editor-ui/blocks-final.png`、`blocks-undo.png`、`blocks-diagnostic.png`。
- 未运行新的 RimWorld 游戏场景；这是编辑工具验收，不是新玩法验收。未覆盖 Windows／macOS 桌面、全部拖拽组合和大型画布；复杂 trait／impl 等仍使用文本入口，不支持任意源码反向转换。

## VSCode 作者工具（2026-10-10）

- `npm run package:vsix` 成功发布 Core 与 Editor 检查器，生成 `tmp/vscode/oxygen-language-support-0.1.0.vsix`；最终包在隔离的 VSCode Server 扩展目录安装成功，不携带游戏／Unity 程序集。
- 16 个代码片段展开默认值后全部通过实际 parser/analyzer。TextMate 运行时检查 trait 方法签名、泛型、块注释、插值内注释、比较运算及 optional use 别名；语言配置正则在 JavaScript 中可编译，中文标识符和点号分隔通过定向检查。
- 实际 Editor 进程配合 VSCode API shim，观察错误文本 → 未保存修正 → 保存错误的诊断更新、Outline 声明、关闭文件撤销诊断、关闭自动诊断后手动检查仍有效。未受信任时不返回诊断或 Outline，授予信任后恢复检查。shim 结果不视为真实 UI 验收。
- 在真实浏览器中的 VSCode Server 打开 `.oxy`，观察 Oxygen 语言识别和 trait／struct／impl／泛型语法高亮。受限模式执行 **Oxygen: Check Current Document** 显示需要工作区信任的提示。高亮截图：忽略目录 `tmp/editor-ui/oxygen-highlight-final.png`。
- 当时未获得临时工作区 Trust 操作确认，未绕过信任；文本 Problems、Outline、片段插入及受信任状态下的编辑更新尚未完成 UI 验收。未覆盖 Windows／macOS、SSH／容器环境。该阶段 Blockly 仅完成设计；后续积木实现与验收见上节。

## IrisMenus 设置适配、语言扩展与 optional use（2026-10-10）

- 框架解决方案及 PrisonerNames 真实消费端 Release 编译：0 警告、0 错误；Oxygen 行为检查 492 项通过。
- 定向 smoke 通过 11 个场景：trait 约束与静态 `Self` 工厂、不同 struct 特化、重复推断回调、动态别名成员约束、`Self` 字段与参数、泛型转发、插值中的块注释、多行注释语句分隔。
- smoke 直接链接实际 `OxygenSettingsCompiler`、`ScriptCompiler` 与 `OxygenSettingsHost`：设置页回调和 mood 规则回调均执行成功；导入模块坏载入时保留旧设置计划，修复后才替换新计划。
- IrisMenus 引擎边界检查 164 项通过；覆盖设置页注册／滚动、number／slider／text／select 写入、同 ID 页面保留、旧弹窗拒绝写入、空计划移除、适配器较晚初始化、optional use 缺失／存在、读取异常和坏源码等边界。
- 发布脚本在临时目录执行：框架 29 文件、PrisonerNames 11 文件 SHA-256 校验通过；可选适配程序集分目录，消费包不捆绑共享依赖。临时 smoke 工程与发布副本在验证后删除。
- 未重启正在运行的玩家游戏，未部署新框架／消费端到游戏安装目录；本轮没有真实 Unity 设置 UI、屏幕缩放或新入口的存读档实测，不将引擎 doubles 当作视觉验收。未覆盖大型模组集合、全部 DLC 组合和长时 GC。

## 原生内容与 RHAH 第一批验收

- 最终 `dotnet build Oxygendsl.slnx -c Release --nologo`：0 警告、0 错误；`dotnet Tests/bin/Release/net9.0/Oxygendsl.Checks.dll`：373 项行为检查通过。
- 离线真实消费端读取 `Examples/NativeTraits`，声明 3 个 TraitDef、3 个 ThoughtDef 并修改 Industriousness commonality；规则试算得到幼儿阶段 1、自身疾病未激活、群体疾病阶段 0。目录记录 Core Def SHA-256；RHAH 示例明确事实试算为 allow，未指定完整目录时退出 2／不完整而非假成功。
- 定向修正 smoke：原生 Def 继承按 defName 解析，嵌套 `degreeDatas.0.label`／`statOffsets.0.value` 修改成功；原生 XML 继承列表追加／移除保留其它成员，连续字段修改保留先前修改和正确前后值。
- Windows Unity Mono／RimWorld 1.6.4871，隔离安装 `C:/RimCarbon14/oxygendsl-native-game-20261009`，配置 `C:/RimCarbon14/oxygendsl-native-acceptance-20261009`。执行文件和 Mods 独立，游戏 Data 只读共享；没有覆盖另一正在运行的玩家游戏安装。
- 实例 `493bfc13e18e4695a14fed50d9f21e77`：原生 Def 归属正确，Industriousness commonality 2 → 1.5；授予 HardWorker 后实际 WorkSpeedGlobal 1 → 1.15。幼儿饥饿产生阶段 0／-4，添加 Flu 后阶段 1／-8；移除所需特质后原生思想消失。
- 同进程规则重载把实际思想阶段 1 改为 0；未知 Pawn 字段导致重载失败，仍保留阶段 0；恢复原规则回到阶段 1。内容倍率编辑拒绝热更新并显示待重启，仍保留上一规则。重复闸门注册失败不清空原快照，停用／启用提供者不影响其它提供者。
- 保存并读回 `oxygendsl-native-disabled`：已授予特质保留、该提供者规则仍停用；启用后恢复原生阶段 1／-8。不是原生内容卸载验收。
- 加入真实 HAR、NewRatkinPlus、IrisMenus 和已部署 RHAH 后，实例 `6f4adb58727a4cc7b4e064aa597336f0`：真实 Ratkin／RHAH_Api.Allows 消费脚本许可，覆盖 hungry allow、Pawn deny 优先、fed abstain／默认允许、Leaving deny、停用策略恢复原默认。不是 mock API 或周期 SetGate。
- 实际脚本窗口「重新加载 Mod 规则」按钮返回 activated，截图显示加载反馈和原生内容检查；截图在隔离配置 `RimCarbon14/artifacts/6f4adb58727a4cc7b4e064aa597336f0/7a49bc4a3db949a5867e369ddcbec2f0.png`。
- 最终编译候选实例 `eb850b20be4244e7ae660de832a85247`／PID 17808 再加载隔离存档，特质仍在、阶段 1／-8、规则重载成功、真实 RHAH 权限路径通过；另存 `oxygendsl-native-final` 后正常退出。原始回执在忽略的 `tmp/carbon-live.jsonl`，简要结果 `tmp/native-results.json`；临时原生探针不进入发布包。

本批未新增动作 DSL，没有证明新的寻路／Job 完整生命周期、大型 Mod 组合或无损卸载。不重跑原 MVP 性能矩阵；最终只集中做定向 checks/smoke 与上述游戏路径，后续问题按实际反馈修正。

## 类型、struct 与模块实现验收

- `dotnet run --project Tests/Oxygendsl.Checks.csproj -c Release`：360 项行为检查通过，含数值边界、可空与名义类型、上下文字面量、方法接收者、失败写入原子性、模块实例/导出隔离、错误位置及调用预算。集合状态模型使用种子 `20261009`，20 条、每条 30 步增删改轨迹，与独立 C# list 模型逐步比较。
- 独立消费端 smoke 读取真实 `Oxygen/helpers.oxy` 与 `Examples/typed-mood.oxy`，完成导入、类型化组合和心情注册，检查 food=0.5/0.25/null、倒地分支、250 次复用条件调用、+5/+8 重新编译及依赖/字段错误拒绝。
- `bash scripts/deploy.sh` 构建 0 警告、0 错误，SHA-256 校验 19 个发布文件；发布清单包含 `Oxygen/` 辅助库。
- 真实游戏进程 PID `16348`、实例 `29cb67652b9242edb9f8f078e8d15850`，使用下方相同隔离配置与 Mod 集合。`tmp/syntax_live.py` 已运行至保存恢复与文件按钮；应用按钮遇到桥接 `stale_layout`，重新读取布局后仅重做该未执行动作，观察成功，没有将整组失败当作通过。
- 30 个实际殖民者获得 `fed_comfort` +5，改为「模块化 +8」后思想对象句柄不变；缺失模块及错误 bonus 类型拒绝应用，版本和已有效果不变。另存并读回 `oxygendsl-syntax-dcc4479a` 后仍为 +8。
- 实际「从文件读取」按钮加载已部署的 typed-mood，效果变为 +5；实际「应用脚本」按钮提交 +8，截图显示活动效果 30、匹配 30、跳过 0。停用后活动效果为 0。
- 截图：隔离配置 `RimCarbon14/artifacts/29cb67652b9242edb9f8f078e8d15850/22005d58754c4ff79f187962cd4700c9.png`。回执与恢复记录保留在 `tmp/carbon-live.jsonl`、`tmp/syntax-results.json`；可复用客户端与验收脚本保留，不进入发布包。
- 最终程序集另在 PID `30792`、实例 `96f66df9e8a345dcaf798fb72d4276b0` 的 Unity Mono 中验证：加载上述存档、应用发布示例得到 30 个效果；绑定方法的 array 字面量与 present-null/缺失索引 exists 查询正常；错误 `_ : int` 参数及未调用函数内的缺失导入类型均拒绝，保留版本和 30 个效果。停用清理后正常退出；结果在 `tmp/syntax-release-results.json`。

当时没有重跑下方旧版性能矩阵，不能用旧数字声称类型/模块无性能影响。模块可变状态和源码快照不持久化，读档重新读取依赖；当时游戏能力仍是动态心情宿主，不含任意 C#、Harmony 或文件权限。

## 原始动态心情验收环境与证据

- RimWorld `1.6.4871`，Windows x64 / Unity Mono；1280×720，简体中文。
- 隔离配置：`C:/RimCarbon14/oxygendsl-20261009`，不覆盖玩家存档。
- 模组集合：Harmony `2.4.2`、Core、Royalty、Ideology、Biotech、Anomaly、Odyssey、RimCarbon14、Oxygendsl；隔离配置自动启用了已安装 DLC，不把此结论扩大到大型模组列表。
- 最终候选进程：PID `5324`，实例 `2fa3c3c5ee584c20911ac3f63005b877`。脚本修改和性能窗口均在同一进程；为加载修正过的程序集而进行的开发重启不算脚本热更新。
- `dotnet build Source/Mod/Oxygendsl.Mod.csproj -c Release`：0 警告、0 错误。
- `dotnet run --project Tests/Oxygendsl.Checks.csproj -c Release`：26 个行为检查通过，含注册表达式、错误位置、只读上下文、边界与调用预算。
- 部署：`bash scripts/deploy.sh`，逐文件 SHA-256 校验。发布包仅包含两份 Oxygendsl DLL，不包含 Harmony、游戏程序集或临时测量程序集。

原始回执和可复用工具保留在忽略的 `tmp/`：`carbon.py`、`live_smoke.py`、`edge_smoke.py`、`final_edges.py`、`perf_run.py`，以及 `smoke-results.json`、`edge-results.json`、`perf-results.json`、`carbon-live.jsonl`。`tmp/Perf` 原生探针由桥接显式加载，不注册 GameComponent、不创建 Harmony 补丁、不进入发布包。跨进程对象句柄和方法 ID 必须重新发现。

## 游戏内行为

`tmp/live_smoke.py` 在最终候选通过 50 个断言，保存并加载 `oxygendsl-smoke-20112ce0`：

验证结束另存 `oxygendsl-demo-db520fa6`（30 人、实际编辑 +12），随后正常停止隔离测试进程；玩家原配置和存档未改动。

| 契约 | 实际结果 |
| --- | --- |
| 新增与修改持续效果 | 3 个真实殖民者得到 +5；改为 +12，思想对象身份不变，进程实例不变 |
| 条件切换 | 真条件保留，假条件清理；同 Pawn 的不同规则分别判断 |
| 固定选择 | 缩小到一个 Pawn；临时切换 UI 选择不改变绑定 |
| 反复应用 | 持续规则连续应用 20 次不累积；记忆连续应用 20 次不重置触发时间或冷却 |
| 修改失败 | 语法或条件运行错误保留有效版本和效果；错误包含行列与规则／Pawn |
| 归属隔离 | 同标签不同 ID 共存；删除其中一条不删除另一条 |
| 限时记忆 | 冷却到期刷新同一对象；叠加不超上限；条件变假保留已有记忆，到期贡献为零 |
| 保存恢复 | 触发 tick、截止 tick、精确保存年龄和固定选择恢复；立即评估不重复触发 |
| 停用／启用 | 停用清理拥有的全部效果；重新启用尊重尚未到期冷却 |
| 空脚本 | 清理规则和效果；周期入口不执行目标评估 |

`tmp/edge_smoke.py` 最终通过 3 个断言：叠加增长到三条、热更新上限降为一立即清理、改为刷新立即保留一条。修正前真实游戏复现了后两个场景失败。

`tmp/final_edges.py` 的 18 个实际断言已观察通过：空固定选择、显式重新捕获、每次应用单次评估、地图目标跳过动物、周期错误保留效果并恢复、清理不影响原版记忆、暂停应用不改写当前心情条、规则类型切换、空脚本保存恢复、实际文件读取、键鼠输入编辑与按钮应用。`tmp/final-edge-results.json` 保存各断言和截图。桥接桌面输入受到本机 PowerShell 策略阻止时，调用已安装的同一输入脚本，仅对该进程使用 `-ExecutionPolicy Bypass`，不修改系统策略。

### 实际 UI

脚本窗口使用原版控件，需求页显示“热更新 +12”和实际 `12` 点数。真实“应用脚本”按钮已执行，UI 与对象读取一致。UI 缩放从 1.0 改为 1.25 后，窗口仍完整位于屏幕，关闭与文件读取按钮可见。

最终候选截图位于隔离配置的 `RimCarbon14/artifacts/2fa3c3c5ee584c20911ac3f63005b877/`：

- `713766a867ae480ea402f41d615f1abf.png`：原版需求页与 +12 效果。
- `a311c57ca7a64cef9ac5926c4bf8cea7.png`：1.25 缩放后的脚本窗口。
- `f84d935ce9d3438b9e9b879c0b66dbfb.png`：完整边界脚本通过后，真实键鼠编辑并应用的 30 人 +12 规则与文件来源。

## 性能方法

条件统一为 `pawn.food != null && pawn.food > 0.25 && !pawn.downed`。原生对照只把同一 `RuleProgram` 的条件替换为等价 C# `IRuleCondition`；目标收集、上下文、效果维护、清理和运行检查仍经过同一个宿主路径。不是拿空循环代替 DSL 玩法。

真实殖民者逐步增加至 3、10、30 人；规则数为 1、10，匹配时维持 `Pawn 数 × 规则数` 个真实思想对象。评估间隔 150 tick。

每组先预热 20 次，再在同一主线程方法内执行 200 次立即评估，记录宿主 `LastEvaluationMs` 的中位数和 p95；桥接请求序列化不在单次计时范围内。该计时包含目标收集、条件、效果同步和运行检查，但不含解析／注册。

随后使用 RimCarbon14 `watch`、深度 0、确切 Oxygendsl 覆盖方法 `GameComponentTick`，观察 600 个真实 tick。每组入口调用 600 次；非空组周期评估 4 次，空组 0 次。15 个窗口覆盖 tick `4755..13755`。表中的“组件 ms/tick”包含观测器开销；“总 ms/tick”只覆盖同步模拟，不含渲染。顺序采样的地图行为与 JIT／GC 会变化，不能把总 ms/tick 相减当作精确 Mod 净开销。

| Pawn | 规则 | 条件 | 评估中位数 ms | 评估 p95 ms | 组件 ms/tick | 总 ms/tick |
| --- | --- | --- | --- | --- | --- | --- |
| 3 | 0 | 空 | 0.0001 | 0.0001 | 0.0070 | 0.7120 |
| 3 | 1 | 原生 | 0.0037 | 0.0046 | 0.0027 | 0.7001 |
| 3 | 1 | DSL | 0.0194 | 0.0220 | 0.0032 | 0.8711 |
| 3 | 10 | 原生 | 0.0711 | 0.0768 | 0.0032 | 0.7371 |
| 3 | 10 | DSL | 0.2245 | 0.2660 | 0.0044 | 0.7153 |
| 10 | 0 | 空 | 0.0001 | 0.0001 | 0.0023 | 0.8409 |
| 10 | 1 | 原生 | 0.0102 | 0.0114 | 0.0027 | 0.8556 |
| 10 | 1 | DSL | 0.0620 | 0.0678 | 0.0036 | 0.8488 |
| 10 | 10 | 原生 | 0.3135 | 0.3630 | 0.0047 | 0.8170 |
| 10 | 10 | DSL | 0.8394 | 1.1110 | 0.0088 | 0.8339 |
| 30 | 0 | 空 | 0.0001 | 0.0001 | 0.0024 | 1.1728 |
| 30 | 1 | 原生 | 0.0306 | 0.0424 | 0.0028 | 1.1582 |
| 30 | 1 | DSL | 0.1919 | 0.2625 | 0.0047 | 1.1836 |
| 30 | 10 | 原生 | 1.7627 | 2.2519 | 0.0142 | 1.3189 |
| 30 | 10 | DSL | 3.4474 | 4.5744 | 0.0278 | 1.3180 |

最大规模 DSL 批次最大值为 14.3604 ms，中位数不能掩盖长尾。当前数据只支持该条件、规模和 150 tick 间隔。

### 修改到应用的反馈

30 个真实殖民者、1 条条件规则，预热 20 次后连续应用 200 次，交替修改 +5／+12。完整 `Apply` 包含解析、分析、注册、候选条件与效果提交：中位数 0.2259 ms，p95 0.2820 ms，最大值 0.6596 ms。原始结果在 `tmp/feedback-results.json`，临时程序集在 `tmp/Feedback`；同主线程内部计时，不含桥接或屏幕刷新延迟。首次 JIT 和未预热应用不由此数值代表。

作者只需修改一个脚本文档，不必在 XML Def 与 C# 间切换；本次实际验证了窗口编辑与一个 UTF-8 文件导入两条路径。无效果时可在同窗检查有效版本、活动数量、条件假／无心情需求原因与带规则的错误；尚未由真实作者衡量学习和定位成本。

### 分配计数限制

本机 Mono 虽提供 `GC.GetAllocatedBytesForCurrentThread`，真实调用始终返回零。用实际数组分配校准后判定不可用；UI 和结果记为 `-1`／不可用，而非“零分配”。因此本次没有可信的每次评估分配字节结论。

## 决策

这组数据没有证明必须先实现编译后端。30 人、10 条规则时，原生条件加同一宿主也有可见成本；继续扩大能力前，优先检查宿主的重复查找、效果维护与解释器函数调用分配，再用同一对照复测。当前周期默认值仍为 150 tick，不把立即评估成本乘到每 tick。

热更新的玩法闭环已观察到；尚无真实 Mod 作者试用，不能据此宣称易用性已经验证。未测大型模组集合、跨地图迁移、死亡／世界 Pawn 路径、长时 GC 和全部 DLC 组合；不承诺零冲突。
