using System;

namespace Sysware.Data.Options;

/// <summary>
/// 数据库配置选项。
/// </summary>
public sealed class DatabaseOptions
{
    /// <summary>
    /// 提供程序名称，例如：Sqlite、MySql、SqlServer、PostgreSQL、Oracle。
    /// 默认 Sqlite。
    /// </summary>
    public string Provider { get; set; } = "Sqlite";

    /// <summary>
    /// 连接字符串。
    /// 对于 Sqlite，示例：Data Source=./data/sysware.db
    /// </summary>
    public string ConnectionString { get; set; } = "Data Source=./data/sysware.db";

    /// <summary>
    /// 是否在启动时自动创建/更新表结构（CodeFirst）。
    /// </summary>
    public bool AutoCreateTable { get; set; } = true;

    /// <summary>
    /// 是否输出 SQL 日志。
    /// </summary>
    public bool EnableSqlLog { get; set; } = false;

    /// <summary>
    /// 是否在首次启动时插入示例/种子数据。
    /// </summary>
    public bool SeedSampleData { get; set; } = false;
}


