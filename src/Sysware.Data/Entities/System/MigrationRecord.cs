using System;
using SqlSugar;

namespace Sysware.Data.Entities.System;

[SugarTable("__migrations")]
public sealed class MigrationRecord
{
    [SugarColumn(IsPrimaryKey = true, Length = 64)]
    public string Id { get; set; } = string.Empty;

    [SugarColumn(Length = 128, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    public DateTime AppliedAtUtc { get; set; } = DateTime.UtcNow;
}


