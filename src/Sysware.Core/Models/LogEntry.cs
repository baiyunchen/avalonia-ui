using System;

namespace Sysware.Core.Models;

/// <summary>
/// 日志条目模型
/// </summary>
public class LogEntry
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 源上下文
    /// </summary>
    public string SourceContext { get; set; } = string.Empty;

    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// 属性信息
    /// </summary>
    public string? Properties { get; set; }

    /// <summary>
    /// 日志级别的显示颜色
    /// </summary>
    public string LevelColor => Level switch
    {
        LogLevel.Fatal => "#CC0000",
        LogLevel.Error => "#FF4444",
        LogLevel.Warning => "#FF8800",
        LogLevel.Information => "#0066CC",
        LogLevel.Debug => "#666666",
        LogLevel.Trace => "#999999",
        _ => "#000000"
    };

    /// <summary>
    /// 日志级别的显示图标
    /// </summary>
    public string LevelIcon => Level switch
    {
        LogLevel.Fatal => "💀",
        LogLevel.Error => "❌",
        LogLevel.Warning => "⚠️",
        LogLevel.Information => "ℹ️",
        LogLevel.Debug => "🐛",
        LogLevel.Trace => "🔍",
        _ => "📝"
    };
}

/// <summary>
/// 日志级别枚举
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// 跟踪
    /// </summary>
    Trace = 0,

    /// <summary>
    /// 调试
    /// </summary>
    Debug = 1,

    /// <summary>
    /// 信息
    /// </summary>
    Information = 2,

    /// <summary>
    /// 警告
    /// </summary>
    Warning = 3,

    /// <summary>
    /// 错误
    /// </summary>
    Error = 4,

    /// <summary>
    /// 致命错误
    /// </summary>
    Fatal = 5
}
