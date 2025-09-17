@echo off
REM Sysware 桌面应用启动脚本 (Windows)

echo 正在启动 Sysware 桌面应用...

REM 检查.NET SDK是否安装
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo 错误: 未找到 .NET SDK，请先安装 .NET 8.0 SDK
    pause
    exit /b 1
)

REM 还原依赖
echo 还原项目依赖...
dotnet restore

REM 构建项目
echo 构建项目...
dotnet build

REM 运行应用程序
echo 启动应用程序...
dotnet run --project src\Sysware.UI

pause

