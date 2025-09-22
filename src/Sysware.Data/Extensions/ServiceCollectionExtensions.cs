using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using SqlSugar;
using Sysware.Data.Options;
using Sysware.Data.Repositories;
using Sysware.Data.Migrations;

namespace Sysware.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection("Database").Bind(databaseOptions);

        services.AddSingleton(databaseOptions);

        // 确保 Sqlite 数据目录存在
        if (databaseOptions.Provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
        {
            var dataSource = new SqliteConnectionStringBuilder(databaseOptions.ConnectionString).DataSource;
            if (!string.IsNullOrWhiteSpace(dataSource))
            {
                var directory = Path.GetDirectoryName(Path.GetFullPath(dataSource));
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        services.AddSingleton<ISqlSugarClient>(_ =>
        {
            var config = BuildConnectionConfig(databaseOptions);
            var client = new SqlSugarClient(config);

            if (databaseOptions.EnableSqlLog)
            {
                client.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);
                };
            }

            return client;
        });

        // 注册通用仓储
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // 迁移与初始化器
        services.AddSingleton<IMigration, SampleInitialDataMigration>();
        services.AddSingleton<IMigrationRunner, MigrationRunner>();
        services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

        return services;
    }

    private static ConnectionConfig BuildConnectionConfig(DatabaseOptions options)
    {
        var dbType = ToDbType(options.Provider);
        return new ConnectionConfig
        {
            ConnectionString = options.ConnectionString,
            DbType = dbType,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        };
    }

    private static DbType ToDbType(string provider)
    {
        return provider.ToLowerInvariant() switch
        {
            "sqlite" => DbType.Sqlite,
            "mysql" => DbType.MySql,
            "sqlserver" => DbType.SqlServer,
            "postgresql" => DbType.PostgreSQL,
            "oracle" => DbType.Oracle,
            _ => DbType.Sqlite
        };
    }
}


