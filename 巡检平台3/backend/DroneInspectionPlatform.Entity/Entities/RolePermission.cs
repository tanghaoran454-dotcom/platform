using SqlSugar;

namespace DroneInspectionPlatform.Entity.Entities;

/// <summary>
/// 角色权限关联实体
/// </summary>
[SugarTable("sys_role_permission")]
public class RolePermission : EntityBase
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 权限ID
    /// </summary>
    public long PermissionId { get; set; }
}

