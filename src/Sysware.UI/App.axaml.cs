using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Sysware.UI.ViewModels;
using Sysware.UI.Views;
using Sysware.UI.Services;
using Sysware.UI.Configuration;
using Sysware.Data;

namespace Sysware.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 从 Program.cs 中配置的 ServiceProvider 获取服务
            var mainWindowViewModel = Program.ServiceProvider.GetService<MainWindowViewModel>();
            
            // 配置所有路由
            var navigationService = Program.ServiceProvider.GetRequiredService<INavigationService>();
            RouteConfiguration.ConfigureRoutes(navigationService, Program.ServiceProvider);
            
            // 初始化数据库
            var dbInitializer = Program.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            _ = dbInitializer.InitializeAsync();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
