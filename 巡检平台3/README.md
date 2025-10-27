# 无人机巡检检测平台 - 基础框架

本项目基于 `.NET 6.0 + Furion` 后端和 `Vue3 + Element Plus` 前端构建的无人机巡检检测平台基础框架。

## 技术栈

### 后端
- **框架**: .NET 6.0 + Furion
- **ORM**: SqlSugar
- **数据库**: MySQL 8.0
- **认证**: JWT Bearer Token

### 前端
- **框架**: Vue 3 + TypeScript
- **UI**: Element Plus
- **状态管理**: Pinia
- **路由**: Vue Router
- **HTTP**: Axios

## 项目结构

```
无人机巡检检测平台/
├── backend/                      # 后端项目
│   ├── DroneInspectionPlatform.Server/      # API服务层
│   ├── DroneInspectionPlatform.Application/ # 应用层
│   ├── DroneInspectionPlatform.Entity/      # 实体层
│   └── DroneInspectionPlatform.Core/       # 核心层
├── frontend/                     # 前端项目
│   ├── src/
│   │   ├── views/               # 页面组件
│   │   ├── layouts/             # 布局组件
│   │   ├── stores/              # Pinia状态管理
│   │   ├── router/              # 路由配置
│   │   └── api/                 # API接口
│   └── package.json
└── database/                     # 数据库脚本
    └── init.sql                 # 初始化脚本
```

## 快速开始

### 1. 后端启动

#### 安装依赖
```bash
cd backend
dotnet restore
```

#### 配置数据库
1. 修改 `DroneInspectionPlatform.Server/appsettings.json` 中的数据库连接字符串
2. 执行数据库初始化脚本
```bash
mysql -u root -p DroneInspection < database/init.sql
```

#### 运行项目
```bash
dotnet run --project DroneInspectionPlatform.Server
```

访问地址：http://localhost:5000/swagger

### 2. 前端启动

#### 安装依赖
```bash
cd frontend
npm install
```

#### 启动开发服务器
```bash
npm run dev
```

访问地址：http://localhost:5173

## 权限体系

### 角色说明

1. **管理员 (admin)** - 系统管理、用户管理、所有功能权限
2. **巡检员 (inspector)** - 任务管理、设备控制、数据查看
3. **查看员 (viewer)** - 仅数据查看权限

### 认证流程

1. 用户登录 → 验证用户名密码
2. 生成JWT Token → 包含用户ID、角色信息
3. Token存入前端Storage → localStorage
4. 请求携带Token → Authorization Header
5. 后端验证Token → 路由守卫验证角色权限

## 核心功能

### 已完成
- ✅ 前后端项目初始化
- ✅ JWT认证体系
- ✅ 基于RBAC的权限控制
- ✅ 用户登录/权限验证
- ✅ 路由守卫
- ✅ 动态菜单渲染

### 待开发
- ⏳ 航线管理（地图绘制）
- ⏳ 设备控制（实时监控）
- ⏳ AI识别（图片标注）
- ⏳ 数据可视化（3D模型）
- ⏳ 任务调度
- ⏳ 异常管理

## API接口

### 认证接口
- `POST /api/v1/auth/login` - 用户登录
- `POST /api/v1/auth/userinfo` - 获取用户信息
- `POST /api/v1/auth/refresh` - 刷新Token
- `POST /api/v1/auth/logout` - 退出登录

## 开发规范

### 后端
- 使用Furion动态API特性
- 服务层使用DI注入
- 实体继承EntityBase
- 使用SqlSugar进行数据访问

### 前端
- 使用Composition API
- TypeScript严格模式
- 组件化开发
- 统一错误处理

## 许可证

MIT License

