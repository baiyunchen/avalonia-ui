using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Avalonia.Threading;
using Sysware.UI.Models;
using Sysware.Core.Services;

namespace Sysware.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ILoggerService _logger;
    private int _clickCount;
    private string _currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    private string _platformInfo = Environment.OSVersion.ToString();
    private string _contentTitle = "欢迎页面";
    private bool _showToolbar = true;
    private bool _showStatusBar = true;
    private NavigationItem? _selectedNavigationItem;
    private List<NavigationItem> _navigationItems = new();

    public int ClickCount
    {
        get => _clickCount;
        set => this.RaiseAndSetIfChanged(ref _clickCount, value);
    }

    public string CurrentTime
    {
        get => _currentTime;
        set => this.RaiseAndSetIfChanged(ref _currentTime, value);
    }

    public string PlatformInfo
    {
        get => _platformInfo;
        set => this.RaiseAndSetIfChanged(ref _platformInfo, value);
    }

    public string ContentTitle
    {
        get => _contentTitle;
        set => this.RaiseAndSetIfChanged(ref _contentTitle, value);
    }

    public bool ShowToolbar
    {
        get => _showToolbar;
        set => this.RaiseAndSetIfChanged(ref _showToolbar, value);
    }

    public bool ShowStatusBar
    {
        get => _showStatusBar;
        set => this.RaiseAndSetIfChanged(ref _showStatusBar, value);
    }

    public NavigationItem? SelectedNavigationItem
    {
        get => _selectedNavigationItem;
        set => this.RaiseAndSetIfChanged(ref _selectedNavigationItem, value);
    }

    public List<NavigationItem> NavigationItems
    {
        get => _navigationItems;
        set => this.RaiseAndSetIfChanged(ref _navigationItems, value);
    }

    // 命令
    public ReactiveCommand<Unit, Unit> ClickCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetCommand { get; }
    public ReactiveCommand<Unit, Unit> NewCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<Unit, Unit> UndoCommand { get; }
    public ReactiveCommand<Unit, Unit> RedoCommand { get; }
    public ReactiveCommand<Unit, Unit> CutCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyCommand { get; }
    public ReactiveCommand<Unit, Unit> PasteCommand { get; }
    public ReactiveCommand<Unit, Unit> OptionsCommand { get; }
    public ReactiveCommand<Unit, Unit> AboutCommand { get; }

    public MainWindowViewModel(ILoggerService logger)
    {
        _logger = logger;
        _logger.LogInformation("MainWindowViewModel 初始化开始");

        // 初始化导航项
        InitializeNavigationItems();

        // 初始化命令 - 确保在 UI 线程上执行
        ClickCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                ClickCount++;
                _logger.LogInformation("点击计数增加到 {ClickCount}", ClickCount);
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行点击命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        ResetCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("重置点击计数");
                ClickCount = 0; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行重置命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        NewCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行新建文件命令");
                ContentTitle = "新建文件"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行新建文件命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        OpenCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行打开文件命令");
                ContentTitle = "打开文件"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行打开文件命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        SaveCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行保存文件命令");
                ContentTitle = "保存文件"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行保存文件命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        ExitCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行退出应用程序命令");
                Environment.Exit(0); 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行退出命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        UndoCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行撤销操作命令");
                ContentTitle = "撤销操作"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行撤销命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        RedoCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行重做操作命令");
                ContentTitle = "重做操作"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行重做命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        CutCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行剪切操作命令");
                ContentTitle = "剪切操作"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行剪切命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        CopyCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行复制操作命令");
                ContentTitle = "复制操作"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行复制命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        PasteCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行粘贴操作命令");
                ContentTitle = "粘贴操作"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行粘贴命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        OptionsCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行选项设置命令");
                ContentTitle = "选项设置"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行选项设置命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);
        
        AboutCommand = ReactiveCommand.Create<Unit, Unit>(_ => 
        { 
            try
            {
                _logger.LogInformation("执行关于命令");
                ContentTitle = "关于 Sysware.ModSim"; 
                return Unit.Default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行关于命令时发生异常");
                throw;
            }
        }, outputScheduler: RxApp.MainThreadScheduler);

        // 每秒更新时间
        var timer = Observable.Interval(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            });

        _logger.LogInformation("MainWindowViewModel 初始化完成");
    }

    private void InitializeNavigationItems()
    {
        NavigationItems = new List<NavigationItem>
        {
            new NavigationItem 
            { 
                Name = "用户指南", 
                Icon = "📖",
                Children = new List<NavigationItem>
                {
                    new NavigationItem { Name = "快速开始", Icon = "🚀" },
                    new NavigationItem { Name = "基础教程", Icon = "📚" },
                    new NavigationItem { Name = "高级功能", Icon = "⚡" }
                }
            },
            new NavigationItem 
            { 
                Name = "模型库", 
                Icon = "📦",
                Children = new List<NavigationItem>
                {
                    new NavigationItem { Name = "电气模型", Icon = "⚡" },
                    new NavigationItem { Name = "机械模型", Icon = "⚙️" },
                    new NavigationItem { Name = "流体模型", Icon = "💧" },
                    new NavigationItem { Name = "热力模型", Icon = "🔥" }
                }
            },
            new NavigationItem 
            { 
                Name = "工具", 
                Icon = "🛠️",
                Children = new List<NavigationItem>
                {
                    new NavigationItem { Name = "仿真器", Icon = "🎮" },
                    new NavigationItem { Name = "分析器", Icon = "📊" },
                    new NavigationItem { Name = "可视化", Icon = "📈" }
                }
            },
            new NavigationItem 
            { 
                Name = "示例", 
                Icon = "📋",
                Children = new List<NavigationItem>
                {
                    new NavigationItem { Name = "基础示例", Icon = "🔰" },
                    new NavigationItem { Name = "进阶示例", Icon = "🎯" },
                    new NavigationItem { Name = "项目模板", Icon = "📁" }
                }
            }
        };
    }
}
