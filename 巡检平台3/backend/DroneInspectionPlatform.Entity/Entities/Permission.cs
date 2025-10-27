using SqlSugar;

namespace DroneInspectionPlatform.Entity.Entities;

/// <summary>
/// 权限实体
/// </summary>
[SugarTable("sys_permission")]
public class Permission : EntityBase
{
    /// <summary>
    /// 权限名称
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 权限代码
    /// </summary>
    [SugarColumn(Length = 100)]
    public string Code { get; set; }

    /// <summary>
    /// 权限描述
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? Description { get; set; }

    /// <summary>
    /// 父权限ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 权限类型（0-菜单，1-按钮）
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } = 0;
}

