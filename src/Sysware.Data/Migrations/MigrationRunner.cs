using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using Sysware.Data.Entities.System;

namespace Sysware.Data.Migrations;

public interface IMigrationRunner
{
    Task RunAsync();
}

public sealed class MigrationRunner : IMigrationRunner
{
    private readonly ISqlSugarClient _db;
    private readonly IEnumerable<IMigration> _migrations;

    public MigrationRunner(ISqlSugarClient db, IEnumerable<IMigration> migrations)
    {
        _db = db;
        _migrations = migrations.OrderBy(m => m.Id).ToList();
    }

    public async Task RunAsync()
    {
        _db.CodeFirst.InitTables(typeof(MigrationRecord));
        var applied = await _db.Queryable<MigrationRecord>().Select(x => x.Id).ToListAsync();
        foreach (var migration in _migrations)
        {
            if (applied.Contains(migration.Id))
            {
                continue;
            }

            await migration.UpAsync();
            await _db.Insertable(new MigrationRecord
            {
                Id = migration.Id,
                Name = migration.Name,
                AppliedAtUtc = DateTime.UtcNow
            }).ExecuteCommandAsync();
        }
    }
}


