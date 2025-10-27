import request from '@/utils/request'

export interface LoginForm {
  username: string
  password: string
  rememberMe?: boolean
}

export interface LoginResponse {
  token: string
  refreshToken: string
  expiresIn: number
}

export interface UserInfo {
  id: number
  username: string
  nickname: string
  email: string
  avatar?: string
  roles: string[]
  permissions: string[]
}

// 登录接口
export function login(form: LoginForm) {
  return request.post<{ data: LoginResponse }>('/api/auth/login', form)
}

// 获取用户信息
export function getUserInfo() {
  return request.get<{ data: UserInfo }>('/api/auth/userinfo')
}

// 刷新Token
export function refreshToken(data: { refreshToken: string }) {
  return request.post<{ data: LoginResponse }>('/api/auth/refresh', data)
}

// 退出登录
export function logout() {
  return request.post('/api/auth/logout')
}

