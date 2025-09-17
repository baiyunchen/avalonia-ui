# Assets 目录

这个目录用于存放应用程序的资源文件，如图标、图片等。

## 文件说明

- 目前为空，可以根据需要添加以下文件：
  - `app-icon.ico` - 应用程序图标
  - `logo.png` - 应用程序Logo
  - 其他图片资源

## 使用方法

在XAML中引用资源文件：
```xml
<Window Icon="/Assets/app-icon.ico" />
```

在代码中引用资源文件：
```csharp
var icon = new Bitmap("/Assets/logo.png");
```

