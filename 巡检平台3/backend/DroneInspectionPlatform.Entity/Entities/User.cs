using SqlSugar;

namespace DroneInspectionPlatform.Entity.Entities;

/// <summary>
/// 用户实体
/// </summary>
[SugarTable("sys_user")]
public class User : EntityBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Username { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string? Nickname { get; set; }

    /// <summary>
    /// 密码（已加密）
    /// </summary>
    [SugarColumn(Length = 100)]
    public string Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(Length = 100, IsNullable = true)]
    public string? Email { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [SugarColumn(Length = 20, IsNullable = true)]
    public string? Phone { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(Length = 255, IsNullable = true)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 状态（0-禁用，1-启用）
    /// </summary>
    public int Status { get; set; } = 1;

    /// <summary>
    /// 用户角色关联（导航属性）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(UserRole.UserId))]
    public List<UserRole> UserRoles { get; set; }
}

/// <summary>
/// 实体基类
/// </summary>
public class EntityBase
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(IsOnlyIgnoreUpdate = true)]
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(IsOnlyIgnoreInsert = true)]
    public DateTime UpdateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}

