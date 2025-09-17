using Microsoft.Extensions.Logging;

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
}
