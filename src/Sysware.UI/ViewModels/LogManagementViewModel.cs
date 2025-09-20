using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Threading;
using Sysware.Core.Models;
using Sysware.Core.Services;

namespace Sysware.UI.ViewModels;

/// <summary>
/// 日志管理视图模型
/// </summary>
public class LogManagementViewModel : ViewModelBase
{
    private readonly ILoggerService _loggerService;
    private DateTime? _selectedDate = DateTime.Today;
    private string _searchText = string.Empty;
    private LogLevel? _selectedLogLevel;
    private SelectItem<LogLevel?, string>? _selectedLogLevelItem;
    private ObservableCollection<LogEntry> _logEntries = new();
    private bool _isLoading;
    private LogEntry? _selectedLogEntry;
    private bool _autoRefresh = true;
    private int _refreshInterval = 5; // 秒

    public LogManagementViewModel(ILoggerService loggerService)
    {
        _loggerService = loggerService;

        // 设置默认选中"全部"
        _selectedLogLevel = null; // 对应 LogLevelOptions 中的第一项 "全部"
        _selectedLogLevelItem = LogLevelOptions[0]; // 默认选中第一项 "全部"
        
        // 设置默认日期为今天
        _selectedDate = DateTime.Today;

        // 初始化命令
        LoadLogsCommand = ReactiveCommand.CreateFromTask(LoadLogsAsync);
        SearchCommand = ReactiveCommand.CreateFromTask(SearchLogsAsync);
        RefreshCommand = ReactiveCommand.CreateFromTask(RefreshAsync);
        ClearSearchCommand = ReactiveCommand.Create(ClearSearch);
        ExportLogsCommand = ReactiveCommand.CreateFromTask(ExportLogsAsync);

        // 设置属性变化监听
        this.WhenAnyValue(x => x.SelectedDate)
            .Where(date => date.HasValue)
            .Subscribe(_ => LoadLogsCommand.Execute().Subscribe());

        this.WhenAnyValue(x => x.SearchText, x => x.SelectedLogLevel)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Where(_ => !IsLoading)
            .Subscribe(_ => SearchCommand.Execute().Subscribe());

        // 自动刷新
        SetupAutoRefresh();
    }

    #region 属性

    /// <summary>
    /// 选中的日期
    /// </summary>
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDate, value);
            this.RaisePropertyChanged(nameof(CurrentDateDisplay));
        }
    }

    /// <summary>
    /// 搜索文本
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    /// <summary>
    /// 选中的日志级别
    /// </summary>
    public LogLevel? SelectedLogLevel
    {
        get => _selectedLogLevel;
        set => this.RaiseAndSetIfChanged(ref _selectedLogLevel, value);
    }

    /// <summary>
    /// 选中的日志级别项目
    /// </summary>
    public SelectItem<LogLevel?, string>? SelectedLogLevelItem
    {
        get => _selectedLogLevelItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedLogLevelItem, value);
            // 同步更新 SelectedLogLevel
            SelectedLogLevel = value?.Value;
        }
    }

    /// <summary>
    /// 日志条目集合
    /// </summary>
    public ObservableCollection<LogEntry> LogEntries
    {
        get => _logEntries;
        set => this.RaiseAndSetIfChanged(ref _logEntries, value);
    }


    /// <summary>
    /// 是否正在加载
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    /// <summary>
    /// 选中的日志条目
    /// </summary>
    public LogEntry? SelectedLogEntry
    {
        get => _selectedLogEntry;
        set => this.RaiseAndSetIfChanged(ref _selectedLogEntry, value);
    }

    /// <summary>
    /// 是否自动刷新
    /// </summary>
    public bool AutoRefresh
    {
        get => _autoRefresh;
        set
        {
            this.RaiseAndSetIfChanged(ref _autoRefresh, value);
            SetupAutoRefresh();
        }
    }

    /// <summary>
    /// 刷新间隔（秒）
    /// </summary>
    public int RefreshInterval
    {
        get => _refreshInterval;
        set
        {
            this.RaiseAndSetIfChanged(ref _refreshInterval, value);
            SetupAutoRefresh();
        }
    }

    /// <summary>
    /// 日志级别选项
    /// </summary>
    public List<SelectItem<LogLevel?, string>> LogLevelOptions { get; } =
    [
        new(null, "全部"), // 全部
        new(LogLevel.Fatal, "致命错误"),
        new(LogLevel.Error, "错误"),
        new(LogLevel.Warning, "警告"),
        new(LogLevel.Information, "信息"),
        new(LogLevel.Debug, "调试"),
        new(LogLevel.Trace, "跟踪")
    ];

    /// <summary>
    /// 日志统计信息
    /// </summary>
    public string LogStatistics => GetLogStatistics();

    /// <summary>
    /// 当前查看日期显示文本
    /// </summary>
    public string CurrentDateDisplay => SelectedDate?.ToString("当前查看日期: yyyy年MM月dd日") ?? "未选择日期";

    #endregion

    #region 命令

    /// <summary>
    /// 加载日志命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> LoadLogsCommand { get; }

    /// <summary>
    /// 搜索命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    /// <summary>
    /// 刷新命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    /// <summary>
    /// 清除搜索命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ClearSearchCommand { get; }

    /// <summary>
    /// 导出日志命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ExportLogsCommand { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 初始化
    /// </summary>
    public async Task InitializeAsync()
    {
        IsLoading = true;
        try
        {
            // 设置默认日期为今天并加载日志
            SelectedDate = DateTime.Today;
            await LoadLogsAsync();
        }
        catch (Exception ex)
        {
            _loggerService.LogError(ex, "初始化日志管理失败");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadLogsAsync()
    {
        IsLoading = true;
        try
        {
            if (!SelectedDate.HasValue) return;

            var logs = await _loggerService.GetLogEntriesAsync(SelectedDate.Value.Date);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                LogEntries.Clear();
                foreach (var log in logs)
                {
                    LogEntries.Add(log);
                }

                this.RaisePropertyChanged(nameof(LogStatistics));
            });
        }
        catch (Exception ex)
        {
            _loggerService.LogError(ex, "加载日志失败: {Date}", SelectedDate?.ToString("yyyy-MM-dd") ?? "未知日期");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SearchLogsAsync()
    {
        IsLoading = true;
        try
        {
            if (!SelectedDate.HasValue) return;

            var logs = await _loggerService.SearchLogEntriesAsync(SelectedDate.Value.Date, SearchText, SelectedLogLevel);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                LogEntries.Clear();
                foreach (var log in logs)
                {
                    LogEntries.Add(log);
                }

                this.RaisePropertyChanged(nameof(LogStatistics));
            });
        }
        catch (Exception ex)
        {
            _loggerService.LogError(ex, "搜索日志失败");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task RefreshAsync()
    {
        await LoadLogsAsync();
    }

    private void ClearSearch()
    {
        SearchText = string.Empty;
        SelectedLogLevelItem = LogLevelOptions[0]; // 重置为"全部"
    }

    private async Task ExportLogsAsync()
    {
        try
        {
            // TODO: 实现导出功能
            _loggerService.LogInformation("导出日志功能待实现");
        }
        catch (Exception ex)
        {
            _loggerService.LogError(ex, "导出日志失败");
        }
    }

    private string GetLogStatistics()
    {
        if (LogEntries.Count == 0)
        {
            return "无日志数据";
        }

        var fatalCount = LogEntries.Count(x => x.Level == LogLevel.Fatal);
        var errorCount = LogEntries.Count(x => x.Level == LogLevel.Error);
        var warningCount = LogEntries.Count(x => x.Level == LogLevel.Warning);
        var infoCount = LogEntries.Count(x => x.Level == LogLevel.Information);
        var debugCount = LogEntries.Count(x => x.Level == LogLevel.Debug);
        var traceCount = LogEntries.Count(x => x.Level == LogLevel.Trace);

        return
            $"总计: {LogEntries.Count} | 致命: {fatalCount} | 错误: {errorCount} | 警告: {warningCount} | 信息: {infoCount} | 调试: {debugCount} | 跟踪: {traceCount}";
    }

    private void SetupAutoRefresh()
    {
        // TODO: 实现自动刷新逻辑
        if (AutoRefresh && RefreshInterval > 0)
        {
            // 使用定时器定期刷新
        }
    }

    #endregion
}