using Microsoft.Extensions.Logging;
using Sysware.Core.Models;

namespace Sysware.Core.Services;

/// <summary>
/// 日志服务接口
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// 记录信息日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    void LogInformation(string message, params object[] args);

    /// <summary>
    /// 记录警告日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    void LogWarning(string message, params object[] args);

    /// <summary>
    /// 记录错误日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    void LogError(string message, params object[] args);

    /// <summary>
    /// 记录错误日志（包含异常信息）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    void LogError(Exception exception, string message, params object[] args);

    /// <summary>
    /// 记录调试日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    void LogDebug(string message, params object[] args);

    /// <summary>
    /// 记录跟踪日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">参数</param>
    void LogTrace(string message, params object[] args);

    /// <summary>
    /// 获取指定日期的日志条目
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>日志条目列表</returns>
    Task<List<LogEntry>> GetLogEntriesAsync(DateTime date);

    /// <summary>
    /// 搜索指定日期的日志条目
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="searchText">搜索文本</param>
    /// <param name="logLevel">日志级别过滤</param>
    /// <returns>匹配的日志条目列表</returns>
    Task<List<LogEntry>> SearchLogEntriesAsync(DateTime date, string searchText, Models.LogLevel? logLevel = null);

    /// <summary>
    /// 获取可用的日志日期列表
    /// </summary>
    /// <returns>日期列表</returns>
    Task<List<DateTime>> GetAvailableLogDatesAsync();
}
