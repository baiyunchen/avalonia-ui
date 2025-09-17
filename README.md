# Sysware 信创桌面应用

这是一个基于 .NET Core 和 Avalonia UI 的跨平台桌面应用程序，专为国内信创环境（Linux）设计，同时支持 Windows 和 macOS。

## 项目结构

```
Sysware/
├── Sysware.sln              # 解决方案文件
├── src/
│   ├── Sysware.Core/        # 核心业务逻辑项目
│   │   ├── Sysware.Core.csproj
│   │   └── Class1.cs
│   └── Sysware.UI/          # 用户界面项目
│       ├── Sysware.UI.csproj
│       ├── Program.cs       # 应用程序入口
│       ├── App.axaml        # 应用程序定义
│       ├── App.axaml.cs
│       ├── Views/           # 主视图
│       │   ├── MainWindow.axaml
│       │   └── MainWindow.axaml.cs
│       ├── Controls/        # 用户控件组件
│       │   ├── MainMenuBar.axaml
│       │   ├── MainToolBar.axaml
│       │   ├── NavigationPanel.axaml
│       │   ├── MainContentArea.axaml
│       │   └── StatusBar.axaml
│       ├── ViewModels/      # 视图模型
│       │   ├── ViewModelBase.cs
│       │   └── MainWindowViewModel.cs
│       ├── Models/          # 数据模型
│       │   └── NavigationItem.cs
│       └── Assets/          # 资源文件
├── README.md
├── COMPONENTS.md            # 组件架构说明
└── demo.md                  # 演示说明
```

## 技术栈

- **.NET 8.0** - 运行时框架
- **Avalonia UI 11.0** - 跨平台UI框架
- **ReactiveUI** - 响应式编程框架
- **Fluent Theme** - 现代化UI主题

## 支持的平台

- ✅ Linux (信创环境)
- ✅ Windows 10/11
- ✅ macOS

## 快速开始

### 环境要求

- .NET 8.0 SDK
- 支持的操作系统：Linux、Windows、macOS

### 常见问题

**Q: 运行时出现图标文件找不到的错误？**
A: 项目默认不包含图标文件。如果需要自定义图标，请在 `src/Sysware.UI/Assets/` 目录中添加图标文件，然后在 `MainWindow.axaml` 中引用。

### 构建和运行

1. **克隆项目**
   ```bash
   git clone <repository-url>
   cd Sysware
   ```

2. **还原依赖**
   ```bash
   dotnet restore
   ```

3. **构建项目**
   ```bash
   dotnet build
   ```

4. **运行应用程序**
   ```bash
   dotnet run --project src/Sysware.UI
   ```

### 在Linux信创环境中的部署

1. **发布应用程序**
   ```bash
   dotnet publish src/Sysware.UI -c Release -r linux-x64 --self-contained
   ```

2. **部署到目标机器**
   - 将发布文件夹复制到目标Linux机器
   - 确保目标机器有必要的运行时依赖

## 功能特性

- 🖥️ 跨平台桌面应用
- 🎨 现代化UI设计，类似IDE界面
- 🧩 组件化架构，易于维护和扩展
- 📋 完整的菜单栏和工具栏
- 🌳 左侧导航树面板
- 📄 右侧主内容区域
- ⚡ 响应式编程
- 🔄 实时数据更新
- 📱 适配不同屏幕尺寸

## 开发指南

### 添加新功能

1. 在 `Sysware.Core` 项目中添加业务逻辑
2. 在 `Sysware.UI/ViewModels` 中添加视图模型
3. 在 `Sysware.UI/Views` 中添加对应的视图

### 项目配置

- 使用 `AvaloniaUseCompiledBindingsByDefault=true` 启用编译时绑定
- 配置了应用程序清单文件以支持Windows
- 使用Fluent主题提供现代化外观

## 许可证

本项目采用 MIT 许可证。

## 贡献

欢迎提交 Issue 和 Pull Request！
