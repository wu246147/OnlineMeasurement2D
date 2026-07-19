# 项目协作说明

## 项目位置

- 项目根目录：`E:\code\project\交接项目\炯南\0723ZY04 汽车焊装在线尺寸检测系统的研发\2D\P07250099 广丰二厂GBL线\OnlineMeasurement 1.17.0`
- 解决方案：`OnlineMeasurement.sln`
- 主项目目录：`OnlineMeasurement\`
- 项目类型：.NET Framework 4.8、WinForms、C#。

## 文件读取与写入约定

- 当前环境使用 PowerShell，路径包含中文和空格；读取文件时优先使用 `-LiteralPath` 和绝对路径。
- 工作区沙箱可能出现 `helper_sid_resolve_failed` / `CodexSandboxOffline` 账户解析错误。遇到该错误时，先使用 `login:false` 重试；如果仍然失败，再按当前会话权限请求沙箱外执行，并说明这是只读检查或必要的项目操作。
- 修改代码必须使用 `apply_patch`，不要使用 PowerShell 重定向、`cat`、脚本批量重写等方式，以免破坏中文编码、换行符或用户已有改动。
- 修改前先执行只读的 `git status --short`，确认工作区状态；不得使用 `git reset --hard`、`git checkout --` 覆盖已有改动。
- 现有工作区可能包含用户尚未提交的修改。只编辑完成当前任务所必需的文件，保留其他改动。
- 读取大量文件时优先使用 `rg` / `rg --files` 定位，再按需读取局部内容，避免无目的递归读取。

## 外部参考代码

- 用户提供的海康参考代码位于：`E:\code\project\GB\3DLaserGlueInspection-master\3dlaserglueinspection\3DLaserGlueInspection\tool\`
- 外部参考目录只作为只读来源；不要直接修改外部项目文件。
- 参考海康代码依赖原生 `MvCameraControl.dll`，涉及运行时验证时要确认目标机器已安装匹配位数的海康 MVS Runtime。

## 相机兼容改造背景

- 当前项目原有 Basler 相机实现位于 `OnlineMeasurement\BaslerCam.cs`。
- `OnlineMeasurement\Setting.cs` 中已有名为 `Cam` 的相机配置集合类；不要把外部海康类也直接命名为 `Cam`，避免冲突。
- 相机配置按 `Data\Cam\<相机名>\CamSetting.xml` 保存；相机类别应作为每台相机的配置字段保存，并兼容旧配置（旧配置默认 Basler）。
- 运行项目使用 x64 配置时要注意 Basler、Halcon 和海康 MVS 原生依赖的位数一致。
