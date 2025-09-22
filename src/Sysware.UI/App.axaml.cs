using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Sysware.UI.ViewModels;
using Sysware.UI.Views;
using Sysware.UI.Services;

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
            // 注册导航路由
            var nav = Program.ServiceProvider.GetRequiredService<INavigationService>();
            nav.Register("LogManagement", () =>
            {
                var logger = Program.ServiceProvider.GetRequiredService<Sysware.Core.Services.ILoggerService>();
                var vm = new LogManagementViewModel(logger);
                return new LogManagementView(vm);
            }, title: "日志管理");
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
