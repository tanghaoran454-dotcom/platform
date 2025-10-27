using DroneInspectionPlatform.Application.DTOs;
using DroneInspectionPlatform.Entity.Entities;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DroneInspectionPlatform.Application.Services;

/// <summary>
/// 认证服务
/// </summary>
[ApiDescriptionSettings("Auth", Module = "v1")]
public class AuthService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(ISqlSugarClient db, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="input">登录信息</param>
    /// <returns>登录结果</returns>
    [HttpPost("login")]
    public async Task<LoginOutput> Login(LoginInput input)
    {
        // 验证用户名密码
        var user = await _db.Queryable<User>()
            .Where(u => u.Username == input.Username && u.Status == 1 && !u.IsDeleted)
            .FirstAsync();

        if (user == null)
            throw Oops.Bah("用户名或密码错误");

        // 验证密码
        if (!VerifyPassword(input.Password, user.Password))
            throw Oops.Bah("用户名或密码错误");

        // 获取用户角色
        var roles = await GetUserRoles(user.Id);
        var roleCodes = roles.Select(r => r.Code).ToList();

        // 生成Token
        var token = GenerateToken(user, roleCodes);
        var refreshToken = GenerateRefreshToken();

        return new LoginOutput
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresIn = 7200 // 2小时
        };
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns>用户信息</returns>
    [HttpPost("userinfo"), NonUnify]
    public async Task<UserInfoOutput> GetUserInfo()
    {
        // 从Token中获取用户ID
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw Oops.Bah("未登录");

        var userIdLong = long.Parse(userId);

        // 获取用户信息
        var user = await _db.Queryable<User>()
            .Where(u => u.Id == userIdLong && !u.IsDeleted)
            .FirstAsync();

        if (user == null)
            throw Oops.Bah("用户不存在");

        // 获取角色
        var roles = await GetUserRoles(user.Id);
        var roleCodes = roles.Select(r => r.Code).ToList();

        // 获取权限
        var permissions = await GetUserPermissions(user.Id);

        return new UserInfoOutput
        {
            Id = user.Id,
            Username = user.Username,
            Nickname = user.Nickname,
            Email = user.Email,
            Phone = user.Phone,
            Avatar = user.Avatar,
            Roles = roleCodes,
            Permissions = permissions
        };
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="input">刷新Token</param>
    /// <returns>新的Token</returns>
    [HttpPost("refresh")]
    public async Task<LoginOutput> RefreshToken(RefreshTokenInput input)
    {
        // 验证刷新Token（实际应用中应从Redis验证）
        // var userId = await ValidateRefreshToken(input.RefreshToken);
        // if (userId == null)
        //     throw Oops.Bah("刷新Token无效");

        // 获取用户ID（示例代码，实际需要从Redis获取）
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw Oops.Bah("刷新Token无效");

        var userIdLong = long.Parse(userId);

        // 获取用户信息
        var user = await _db.Queryable<User>()
            .Where(u => u.Id == userIdLong && !u.IsDeleted)
            .FirstAsync();

        if (user == null)
            throw Oops.Bah("用户不存在");

        // 获取角色
        var roles = await GetUserRoles(user.Id);
        var roleCodes = roles.Select(r => r.Code).ToList();

        // 生成新Token
        var token = GenerateToken(user, roleCodes);
        var refreshToken = GenerateRefreshToken();

        return new LoginOutput
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresIn = 7200
        };
    }

    /// <summary>
    /// 获取用户角色
    /// </summary>
    private async Task<List<Role>> GetUserRoles(long userId)
    {
        return await _db.Queryable<UserRole>()
            .LeftJoin<Role>((ur, r) => ur.RoleId == r.Id)
            .Where((ur, r) => ur.UserId == userId && r.Status == 1 && !r.IsDeleted && !ur.IsDeleted)
            .Select((ur, r) => r)
            .ToListAsync();
    }

    /// <summary>
    /// 获取用户权限
    /// </summary>
    private async Task<List<string>> GetUserPermissions(long userId)
    {
        // 获取用户角色
        var roleIds = await _db.Queryable<UserRole>()
            .Where(ur => ur.UserId == userId && !ur.IsDeleted)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        if (!roleIds.Any())
            return new List<string>();

        // 获取角色权限
        var permissions = await _db.Queryable<RolePermission>()
            .LeftJoin<Permission>((rp, p) => rp.PermissionId == p.Id)
            .Where((rp, p) => roleIds.Contains(rp.RoleId) && !p.IsDeleted && !rp.IsDeleted)
            .Select((rp, p) => p.Code)
            .ToListAsync();

        return permissions.Distinct().ToList();
    }

    /// <summary>
    /// 生成JWT Token
    /// </summary>
    private string GenerateToken(User user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Username", user.Username),
            new Claim(JwtRegisteredClaimNames.Name, user.Nickname ?? user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // 添加角色
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"] ?? throw new Exception("JWT SecretKey未配置")));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 生成刷新Token
    /// </summary>
    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 密码加密
    /// </summary>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
