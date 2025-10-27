import { defineStore } from 'pinia'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { login, getUserInfo, refreshToken as refreshTokenApi, type LoginForm, type UserInfo } from '@/api/auth'

export const useAuthStore = defineStore('auth', () => {
  const router = useRouter()

  // 状态定义
  const token = ref<string>(localStorage.getItem('token') || '')
  const refreshToken = ref<string>(localStorage.getItem('refreshToken') || '')
  const userInfo = ref<UserInfo | null>(null)

  // 登录
  const loginAction = async (form: LoginForm) => {
    try {
      const response = await login(form)
      token.value = response.data.token
      refreshToken.value = response.data.refreshToken

      localStorage.setItem('token', token.value)
      localStorage.setItem('refreshToken', refreshToken.value)

      // 获取用户信息
      await getUserInfo()

      ElMessage.success('登录成功')
      router.push('/dashboard')
    } catch (error: any) {
      ElMessage.error(error.message || '登录失败')
      throw error
    }
  }

  // 获取用户信息
  const getUserInfoAction = async () => {
    try {
      const response = await getUserInfo()
      userInfo.value = response.data
    } catch (error) {
      throw error
    }
  }

  // 刷新Token
  const refreshTokenAction = async () => {
    try {
      const response = await refreshTokenApi({ refreshToken: refreshToken.value })
      token.value = response.data.token
      refreshToken.value = response.data.refreshToken

      localStorage.setItem('token', token.value)
      localStorage.setItem('refreshToken', refreshToken.value)
    } catch (error) {
      logout()
      throw error
    }
  }

  // 退出登录
  const logout = () => {
    token.value = ''
    refreshToken.value = ''
    userInfo.value = null

    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')

    router.push('/login')
  }

  return {
    token,
    refreshToken,
    userInfo,
    login: loginAction,
    getUserInfo: getUserInfoAction,
    refreshToken: refreshTokenAction,
    logout,
  }
})

