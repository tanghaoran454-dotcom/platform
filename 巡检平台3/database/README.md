# 数据库初始化说明

## 数据库配置

### 创建数据库
```sql
CREATE DATABASE IF NOT EXISTS DroneInspection DEFAULT CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 执行初始化脚本
```bash
mysql -u root -p DroneInspection < init.sql
```

## 默认账号

- **用户名**: admin
- **密码**: admin123
- **角色**: 系统管理员

> 注意：实际密码已通过BCrypt加密存储，您需要修改 `init.sql` 中的密码哈希值。

## 权限角色

| 角色代码 | 角色名称 | 权限说明 |
|---------|---------|---------|
| admin | 系统管理员 | 拥有所有权限，可管理系统配置 |
| inspector | 巡检员 | 可创建任务、控制设备、查看数据 |
| viewer | 查看员 | 只能查看数据，无操作权限 |

## 数据库结构

- `sys_user` - 用户表
- `sys_role` - 角色表
- `sys_user_role` - 用户角色关联表
- `sys_permission` - 权限表
- `sys_role_permission` - 角色权限关联表

## 密码加密

使用 BCrypt 进行密码加密：

```csharp
// 加密密码
string hashedPassword = BCrypt.Net.BCrypt.HashPassword("yourPassword");

// 验证密码
bool isValid = BCrypt.Net.BCrypt.Verify("yourPassword", hashedPassword);
```

