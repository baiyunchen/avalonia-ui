using System;
using System.Threading.Tasks;
using SqlSugar;
using Sysware.Data.Entities.Sample;
using Sysware.Data.Options;
using Sysware.Data.Migrations;

namespace Sysware.Data;

public sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ISqlSugarClient _db;
    private readonly DatabaseOptions _options;
    private readonly IMigrationRunner _migrationRunner;

    public DatabaseInitializer(ISqlSugarClient db, DatabaseOptions options, IMigrationRunner migrationRunner)
    {
        _db = db;
        _options = options;
        _migrationRunner = migrationRunner;
    }

    public async Task InitializeAsync()
    {
        if (_options.AutoCreateTable)
        {
            _db.CodeFirst.InitTables(typeof(DictionaryItem));
        }

        await _migrationRunner.RunAsync();
    }
}


