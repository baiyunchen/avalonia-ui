using System.Threading.Tasks;
using SqlSugar;
using Sysware.Data.Entities.Sample;

namespace Sysware.Data.Migrations;

public sealed class SampleInitialDataMigration : IMigration
{
    private readonly ISqlSugarClient _db;
    public SampleInitialDataMigration(ISqlSugarClient db)
    {
        _db = db;
    }

    public string Id => "20240922_0001";
    public string Name => "Seed initial dictionary items";

    public async Task UpAsync()
    {
        _db.CodeFirst.InitTables(typeof(DictionaryItem));
        var exists = await _db.Queryable<DictionaryItem>().AnyAsync();
        if (!exists)
        {
            await _db.Insertable(new[]
            {
                new DictionaryItem { Category = "Common", Code = "OK", Name = "确定" },
                new DictionaryItem { Category = "Common", Code = "CANCEL", Name = "取消" }
            }).ExecuteCommandAsync();
        }
    }
}


