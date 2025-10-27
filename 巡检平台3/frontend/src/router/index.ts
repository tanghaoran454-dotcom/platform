import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { ElMessage } from 'element-plus'

// 路由元信息接口
declare module 'vue-router' {
  interface RouteMeta {
    title?: string
    requiresAuth?: boolean
    roles?: string[]
    icon?: string
  }
}

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/login/LoginPage.vue'),
    meta: { title: '登录' },
  },
  {
    path: '/',
    component: () => import('@/layouts/MainLayout.vue'),
    redirect: '/dashboard',
    meta: { requiresAuth: true },
    children: [
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: () => import('@/views/dashboard/DashboardView.vue'),
        meta: { title: '数据大屏', requiresAuth: true, icon: 'DataBoard' },
      },
      {
        path: 'system',
        meta: { title: '系统管理', requiresAuth: true, roles: ['admin'], icon: 'Setting' },
        children: [
          {
            path: 'user',
            name: 'UserManagement',
            component: () => import('@/views/system/UserManagement.vue'),
            meta: { title: '用户管理', requiresAuth: true, roles: ['admin'] },
          },
          {
            path: 'role',
            name: 'RoleManagement',
            component: () => import('@/views/system/RoleManagement.vue'),
            meta: { title: '角色管理', requiresAuth: true, roles: ['admin'] },
          },
        ],
      },
      {
        path: 'route',
        name: 'RouteManagement',
        component: () => import('@/views/route/RouteManagement.vue'),
        meta: { title: '航线管理', requiresAuth: true, roles: ['admin', 'inspector'], icon: 'MapLocation' },
      },
      {
        path: 'device',
        name: 'DeviceControl',
        component: () => import('@/views/device/DeviceControl.vue'),
        meta: { title: '设备控制', requiresAuth: true, roles: ['admin', 'inspector'], icon: 'Monitor' },
      },
      {
        path: 'ai',
        name: 'AIRecognition',
        component: () => import('@/views/ai/AIRecognition.vue'),
        meta: { title: 'AI识别', requiresAuth: true, roles: ['admin', 'inspector', 'viewer'], icon: 'View' },
      },
      {
        path: 'data',
        name: 'DataVisualization',
        component: () => import('@/views/data/DataVisualization.vue'),
        meta: { title: '数据可视化', requiresAuth: true, icon: 'DataAnalysis' },
      },
    ],
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('@/views/error/404.vue'),
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// 路由守卫
router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()

  // 页面需要认证
  if (to.meta.requiresAuth) {
    // 检查Token是否存在
    if (!authStore.token) {
      ElMessage.warning('请先登录')
      next({ path: '/login', query: { redirect: to.fullPath } })
      return
    }

    // 获取用户信息（如果未获取）
    if (!authStore.userInfo) {
      try {
        await authStore.getUserInfo()
      } catch (error) {
        ElMessage.error('获取用户信息失败')
        authStore.logout()
        next({ path: '/login' })
        return
      }
    }

    // 检查角色权限
    if (to.meta.roles && to.meta.roles.length > 0) {
      const userRoles = authStore.userInfo?.roles || []
      const hasPermission = to.meta.roles.some(role => userRoles.includes(role))

      if (!hasPermission) {
        ElMessage.error('无权限访问该页面')
        next({ path: '/dashboard' })
        return
      }
    }
  }

  // 已登录用户访问登录页，重定向到首页
  if (to.path === '/login' && authStore.token) {
    next({ path: '/dashboard' })
    return
  }

  next()
})

export default router
