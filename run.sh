#!/bin/bash

# Sysware 桌面应用启动脚本

echo "正在启动 Sysware 桌面应用..."

# 检查.NET SDK是否安装
if ! command -v dotnet &> /dev/null; then
    echo "错误: 未找到 .NET SDK，请先安装 .NET 8.0 SDK"
    exit 1
fi

# 还原依赖
echo "还原项目依赖..."
dotnet restore

# 构建项目
echo "构建项目..."
dotnet build

# 运行应用程序
echo "启动应用程序..."
dotnet run --project src/Sysware.UI

