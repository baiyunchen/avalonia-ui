using Avalonia;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Sysware.Core.Configuration;
using Sysware.Core.Services;
using Sysware.UI.ViewModels;

namespace Sysware.UI;

class Program
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // 配置 Serilog
        Log.Logger = LoggingConfiguration.ConfigureSerilog();

        // 设置全局异常处理器
        SetupGlobalExceptionHandling();

        // 配置依赖注入
        ConfigureServices();

        try
        {
            Log.Information("应用程序启动");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "应用程序启动失败");
            throw;
        }
        finally
        {
            Log.Information("应用程序关闭");
            ServiceProvider?.Dispose();
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // 添加日志服务
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });
        
        // 注册业务服务
        services.AddSingleton<ILoggerService, LoggerService>();
        
        // 注册 ViewModels
        services.AddTransient<MainWindowViewModel>();
        
        ServiceProvider = services.BuildServiceProvider();
        
        Log.Information("依赖注入服务配置完成");
    }

    private static void SetupGlobalExceptionHandling()
    {
        // 捕获 AppDomain 中未处理的异常
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Log.Fatal(e.ExceptionObject as Exception, "AppDomain 未处理异常: {IsTerminating}", e.IsTerminating);
        };

        // 捕获 Task 中未观察到的异常
        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            Log.Error(e.Exception, "未观察到的 Task 异常");
            e.SetObserved(); // 标记为已观察，防止进程终止
        };
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

