using Serilog;
using Serilog.Events;

namespace Sysware.Core.Configuration;

/// <summary>
/// 日志配置类
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// 配置 Serilog
    /// </summary>
    /// <returns>配置好的 Logger</returns>
    public static ILogger ConfigureSerilog()
    {
        // 创建日志目录
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // 配置 Serilog
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.File(
                path: Path.Combine(logDirectory, "sysware-.log"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10MB
                retainedFileCountLimit: 7, // 保留7天的日志
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}")
            .CreateLogger();
    }

    /// <summary>
    /// 获取日志文件路径
    /// </summary>
    /// <returns>日志文件路径</returns>
    public static string GetLogFilePath()
    {
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        return Path.Combine(logDirectory, $"sysware-{DateTime.Now:yyyyMMdd}.log");
    }
}
