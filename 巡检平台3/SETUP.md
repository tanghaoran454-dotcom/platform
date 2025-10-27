# 无人机巡检检测平台 - 搭建指南

本文档提供完整的前后端项目初始化步骤和权限体系实现说明。

## 📁 项目结构

```
巡检平台3/
├── backend/                          # 后端项目
│   ├── DroneInspectionPlatform.Server/      # API服务
│   ├── DroneInspectionPlatform.Application/  # 应用层
│   ├── DroneInspectionPlatform.Entity/      # 实体层
│   └── DroneInspectionPlatform.Core/        # 核心层
├── frontend/                         # 前端项目
│   ├── src/
│   │   ├── api/                     # API接口
│   │   ├── stores/                  # Pinia状态管理
│   │   ├── router/                  # 路由配置
│   │   ├── views/                   # 页面组件
│   │   └── layouts/                 # 布局组件
│   └── package.json
└── database/                         # 数据库脚本
    └── init.sql                     # 初始化SQL
```

## 🚀 快速开始

### 一、后端环境搭建

#### 1. 创建数据库

```bash
# 连接MySQL
mysql -u root -p

# 创建数据库
CREATE DATABASE IF NOT EXISTS DroneInspection DEFAULT CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci;

# 退出MySQL
exit;
```

#### 2. 执行初始化脚本

```bash
# 导入SQL
mysql -u root -p DroneInspection < database/init.sql
```

#### 3. 配置数据库连接

编辑 `backend/DroneInspectionPlatform.Server/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=DroneInspection;Uid=root;Pwd=你的密码;Charset=utf8mb4;"
  },
  "JWT": {
    "SecretKey": "MySecretKeyForJWTTokenGeneration2024!@#$%",
    "Issuer": "DroneInspectionPlatform",
    "Audience": "DroneInspectionPlatform"
  }
}
```

#### 4. 运行后端

```bash
cd backend
dotnet restore
dotnet run --project DroneInspectionPlatform.Server
```

访问地址：http://localhost:5000/swagger

### 二、前端环境搭建

#### 1. 安装依赖

```bash
cd frontend
npm install
```

#### 2. 配置API地址

创建 `frontend/.env.development`：

```env
VITE_API_BASE_URL=/api
VITE_APP_TITLE=无人机巡检检测平台
```

#### 3. 运行前端

```bash
npm run dev
```

访问地址：http://localhost:5173

### 三、测试账号

- **用户名**: admin
- **密码**: admin123
- **角色**: 系统管理员

> ⚠️ 注意：实际环境中请修改默认密码

## 🔐 权限体系说明

### 角色定义

| 角色代码 | 角色名称 | 权限范围 |
|---------|---------|---------|
| admin | 系统管理员 | 所有功能权限 |
| inspector | 巡检员 | 任务管理、设备控制、数据查看 |
| viewer | 查看员 | 仅数据查看 |

### 认证流程

```
1. 用户登录 → 验证用户名密码
2. 生成JWT Token → 包含用户ID、角色信息
3. Token存入Storage → localStorage
4. 请求携带Token → Authorization Header
5. 后端验证Token → 路由守卫验证角色权限
```

### 核心代码说明

#### 后端认证服务 (`AuthService.cs`)

```csharp
// 用户登录
[HttpPost("login")]
public async Task<LoginOutput> Login(LoginInput input)
{
    // 1. 验证用户名密码
    // 2. 获取用户角色
    // 3. 生成JWT Token
    // 4. 返回Token和用户信息
}
```

#### 前端路由守卫 (`router/index.ts`)

```typescript
router.beforeEach(async (to, from, next) => {
  // 1. 检查Token是否存在
  // 2. 获取用户信息
  // 3. 验证角色权限
  // 4. 动态过滤菜单
})
```

## 📝 关键文件说明

### 前端

- `frontend/src/main.ts` - 应用入口
- `frontend/src/router/index.ts` - 路由配置和守卫
- `frontend/src/stores/auth.ts` - 认证状态管理
- `frontend/src/api/auth.ts` - 认证API接口
- `frontend/src/views/login/LoginPage.vue` - 登录页面
- `frontend/src/layouts/MainLayout.vue` - 主布局（含导航菜单）

### 后端

- `backend/DroneInspectionPlatform.Server/Program.cs` - 程序入口
- `backend/DroneInspectionPlatform.Server/appsettings.json` - 配置文件
- `backend/DroneInspectionPlatform.Entity/Entities/` - 实体模型
- `backend/DroneInspectionPlatform.Application/Services/AuthService.cs` - 认证服务

### 数据库

- `database/init.sql` - 数据库初始化脚本

## 🛠️ 开发规范

### 后端

1. **实体类** 继承 `EntityBase`
2. **服务层** 使用 `IDynamicApiController` 特性
3. **数据访问** 使用 SqlSugar ORM
4. **异常处理** 使用 `Oops.Bah()` 抛出业务异常

### 前端

1. **组件** 使用 Composition API
2. **类型** 使用 TypeScript 严格模式
3. **状态管理** 使用 Pinia
4. **UI组件** 使用 Element Plus

## 📚 下一步开发

1. **航线管理** - 集成Cesium地图，实现航点绘制
2. **设备控制** - WebSocket实时通信，设备状态监控
3. **AI识别** - 图片上传和标注功能
4. **数据可视化** - Three.js加载3D模型，ECharts图表
5. **任务调度** - 任务创建、执行、监控流程

## ❓ 常见问题

### Q: 如何添加新角色？

A: 在数据库 `sys_role` 表中插入新角色，然后在路由配置中设置 `meta.roles`。

### Q: 如何自定义权限验证？

A: 在路由守卫中添加自定义验证逻辑，或在后端接口上添加 `[Authorize(Roles="role")]` 特性。

### Q: Token过期如何处理？

A: 前端实现Token自动刷新机制，在401时调用 `refreshToken` 接口。

## 📞 技术支持

如有问题，请联系开发团队。

