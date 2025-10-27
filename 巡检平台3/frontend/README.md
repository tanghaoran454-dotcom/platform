# 无人机巡检检测平台 - 前端项目

## 技术栈
- **Vue 3** - 前端框架
- **TypeScript** - 类型检查
- **Element Plus** - UI组件库
- **Pinia** - 状态管理
- **Vue Router** - 路由管理
- **Axios** - HTTP请求
- **Three.js** - 3D渲染

## 快速开始

### 安装依赖
```bash
npm install
```

### 开发运行
```bash
npm run dev
```

### 构建生产
```bash
npm run build
```

## 项目结构
```
frontend/
├── src/
│   ├── api/          # API接口定义
│   ├── views/        # 页面组件
│   ├── layouts/      # 布局组件
│   ├── stores/       # Pinia状态管理
│   ├── router/       # 路由配置
│   ├── utils/        # 工具函数
│   ├── App.vue       # 根组件
│   └── main.ts       # 入口文件
├── package.json
└── vite.config.ts
```

## 核心功能

### 权限体系
- JWT Token认证
- 基于角色的访问控制（RBAC）
- 路由守卫和动态菜单

### 业务模块
- 系统管理（用户、角色、权限）
- 航线管理（地图绘制）
- 设备控制（实时监控）
- AI识别（智能分析）
- 数据可视化（3D模型）

## 开发规范
- 使用TypeScript进行类型约束
- 遵循Vue 3 Composition API规范
- 代码格式化使用ESLint + Prettier

