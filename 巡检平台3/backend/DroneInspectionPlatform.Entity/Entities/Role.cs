using SqlSugar;

namespace DroneInspectionPlatform.Entity.Entities;

/// <summary>
/// 角色实体
/// </summary>
[SugarTable("sys_role")]
public class Role : EntityBase
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 角色代码（admin-管理员，inspector-巡检员，viewer-查看员）
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Code { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = true)]
    public string? Description { get; set; }

    /// <summary>
    /// 状态（0-禁用，1-启用）
    /// </summary>
    public int Status { get; set; } = 1;
}

