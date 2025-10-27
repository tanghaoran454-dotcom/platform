import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'
import { ElMessage, ElMessageBox } from 'element-plus'

// 扩展Axios配置接口
interface CustomAxiosRequestConfig extends AxiosRequestConfig {
  showLoading?: boolean
}

// 创建axios实例
const service: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  timeout: 30000,
})

// 请求拦截器
service.interceptors.request.use(
  (config: CustomAxiosRequestConfig) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers = config.headers || {}
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  error => {
    return Promise.reject(error)
  }
)

// 响应拦截器
service.interceptors.response.use(
  (response: AxiosResponse) => {
    const res = response.data

    // 业务状态码处理
    if (res.code !== 200) {
      ElMessage.error(res.message || '请求失败')

      // Token失效，跳转登录页
      if (res.code === 401) {
        localStorage.removeItem('token')
        localStorage.removeItem('refreshToken')
        // 使用动态import避免循环依赖
        import('@/router').then(({ default: router }) => {
          router.push('/login')
        })
      }

      return Promise.reject(new Error(res.message || 'Error'))
    }

    return response
  },
  error => {
    console.error('请求错误:', error)

    if (error.response) {
      const { status, data } = error.response

      switch (status) {
        case 401:
          ElMessage.error('登录状态已过期，请重新登录')
          localStorage.removeItem('token')
          localStorage.removeItem('refreshToken')
          import('@/router').then(({ default: router }) => {
            router.push('/login')
          })
          break
        case 403:
          ElMessage.error('没有权限访问该资源')
          break
        case 404:
          ElMessage.error('请求的资源不存在')
          break
        case 500:
          ElMessage.error('服务器内部错误')
          break
        default:
          ElMessage.error(data?.message || '请求失败')
      }
    } else {
      ElMessage.error('网络错误，请检查网络连接')
    }

    return Promise.reject(error)
  }
)

export default service

