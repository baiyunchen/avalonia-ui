using System;
using SqlSugar;

namespace Sysware.Data.Entities.Sample;

/// <summary>
/// 示例：数据字典条目（用于验证 CodeFirst 与仓储）。
/// </summary>
[SugarTable("dictionary_items")]
public class DictionaryItem
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    [SugarColumn(Length = 64, IsNullable = false)]
    public string Category { get; set; } = string.Empty;

    [SugarColumn(Length = 64, IsNullable = false)]
    public string Code { get; set; } = string.Empty;

    [SugarColumn(Length = 128, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(IsNullable = true)]
    public string? Remark { get; set; }

    [SugarColumn(IsNullable = false)]
    public bool IsActive { get; set; } = true;

    [SugarColumn(IsNullable = false)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}


