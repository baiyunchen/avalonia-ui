using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Sysware.Core.Models;
using Sysware.Core.Configuration;

namespace Sysware.Core.Services;

/// <summary>
/// 日志服务实现
/// </summary>
public class LoggerService : ILoggerService
{
    private readonly ILogger<LoggerService> _logger;

    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    public void LogDebug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }

    public void LogTrace(string message, params object[] args)
    {
        _logger.LogTrace(message, args);
    }

    public async Task<List<LogEntry>> GetLogEntriesAsync(DateTime date)
    {
        var logFilePath = GetLogFilePathForDate(date);
        if (!File.Exists(logFilePath))
        {
            return new List<LogEntry>();
        }

        var logEntries = new List<LogEntry>();
        var lines = await File.ReadAllLinesAsync(logFilePath);

        return ParseMultiLineLogEntries(lines).OrderByDescending(x => x.Timestamp).ToList();
    }

    public async Task<List<LogEntry>> SearchLogEntriesAsync(DateTime date, string searchText, Models.LogLevel? logLevel = null)
    {
        var allEntries = await GetLogEntriesAsync(date);
        
        if (string.IsNullOrWhiteSpace(searchText) && logLevel == null)
        {
            return allEntries;
        }

        return allEntries.Where(entry =>
        {
            // 日志级别过滤
            if (logLevel.HasValue && entry.Level != logLevel.Value)
            {
                return false;
            }

            // 文本搜索
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                return entry.Message.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                       entry.SourceContext.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                       (entry.Exception?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true);
            }

            return true;
        }).ToList();
    }

    public async Task<List<DateTime>> GetAvailableLogDatesAsync()
    {
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            return new List<DateTime>();
        }

        var dates = new List<DateTime>();
        var logFiles = Directory.GetFiles(logDirectory, "sysware-*.log");

        foreach (var file in logFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var dateMatch = Regex.Match(fileName, @"sysware-(\d{8})");
            
            if (dateMatch.Success && DateTime.TryParseExact(dateMatch.Groups[1].Value, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var date))
            {
                dates.Add(date);
            }
        }

        return dates.OrderByDescending(d => d).ToList();
    }

    private string GetLogFilePathForDate(DateTime date)
    {
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        return Path.Combine(logDirectory, $"sysware-{date:yyyyMMdd}.log");
    }

    private List<LogEntry> ParseMultiLineLogEntries(string[] lines)
    {
        var logEntries = new List<LogEntry>();
        LogEntry? currentEntry = null;
        var exceptionLines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            
            // 尝试解析为新的日志条目
            var newEntry = ParseLogLine(line);
            
            if (newEntry != null)
            {
                // 如果有当前条目，先处理它的异常信息
                if (currentEntry != null)
                {
                    if (exceptionLines.Count > 0)
                    {
                        currentEntry.Exception = string.Join(Environment.NewLine, exceptionLines);
                    }
                    logEntries.Add(currentEntry);
                }
                
                // 开始新的日志条目
                currentEntry = newEntry;
                exceptionLines.Clear();
            }
            else if (currentEntry != null)
            {
                // 这是多行日志的后续行（可能是异常堆栈跟踪）
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // 检查是否是异常相关的行
                    if (IsExceptionLine(line))
                    {
                        exceptionLines.Add(line.Trim());
                    }
                    else
                    {
                        // 可能是消息的延续
                        currentEntry.Message += Environment.NewLine + line.Trim();
                    }
                }
            }
        }

        // 处理最后一个条目
        if (currentEntry != null)
        {
            if (exceptionLines.Count > 0)
            {
                currentEntry.Exception = string.Join(Environment.NewLine, exceptionLines);
            }
            logEntries.Add(currentEntry);
        }

        return logEntries;
    }

    private bool IsExceptionLine(string line)
    {
        // 检查是否是异常相关的行
        var trimmedLine = line.Trim();
        
        return trimmedLine.Contains("Exception:") ||
               trimmedLine.Contains("Error:") ||
               trimmedLine.StartsWith("at ") ||
               trimmedLine.StartsWith("   at ") ||
               trimmedLine.StartsWith("--- End of stack trace") ||
               trimmedLine.Contains(".dll!") ||
               Regex.IsMatch(trimmedLine, @"^\s*at\s+[\w\.\<\>]+") ||
               Regex.IsMatch(trimmedLine, @"^[\w\.]+Exception\s*:") ||
               Regex.IsMatch(trimmedLine, @"^\s*in\s+.*\.cs:line\s+\d+");
    }

    private LogEntry? ParseLogLine(string line)
    {
        // 解析日志行格式: [2024-01-01 12:00:00.000 +08:00 INF] SourceContext Message Properties
        var pattern = @"\[([^\]]+)\s+([A-Z]{3})\]\s+(\S*)\s+(.+?)(?:\s+(\{.*\}))?$";
        var match = Regex.Match(line, pattern);

        if (!match.Success)
        {
            return null;
        }

        try
        {
            var timestampStr = match.Groups[1].Value;
            var levelStr = match.Groups[2].Value;
            var sourceContext = match.Groups[3].Value;
            var message = match.Groups[4].Value;
            var properties = match.Groups[5].Success ? match.Groups[5].Value : null;

            // 解析时间戳
            if (!DateTime.TryParse(timestampStr, out var timestamp))
            {
                return null;
            }

            // 解析日志级别
            var logLevel = levelStr switch
            {
                "TRC" => Models.LogLevel.Trace,
                "DBG" => Models.LogLevel.Debug,
                "INF" => Models.LogLevel.Information,
                "WRN" => Models.LogLevel.Warning,
                "ERR" => Models.LogLevel.Error,
                "FTL" => Models.LogLevel.Fatal,
                _ => Models.LogLevel.Information
            };

            return new LogEntry
            {
                Timestamp = timestamp,
                Level = logLevel,
                SourceContext = sourceContext,
                Message = message,
                Properties = properties,
                Exception = null // 异常信息将在 ParseMultiLineLogEntries 中设置
            };
        }
        catch
        {
            return null;
        }
    }
}
