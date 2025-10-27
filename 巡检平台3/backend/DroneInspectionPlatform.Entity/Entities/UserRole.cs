using SqlSugar;

namespace DroneInspectionPlatform.Entity.Entities;

/// <summary>
/// 用户角色关联实体
/// </summary>
[SugarTable("sys_user_role")]
public class UserRole : EntityBase
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }
}

