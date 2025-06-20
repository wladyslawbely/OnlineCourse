import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()

const instance = axios.create({
  baseURL: 'https://localhost:7054/api', 
  withCredentials: true
})

const token = localStorage.getItem('token')
if (token) {
  instance.defaults.headers.common['Authorization'] = `Bearer ${token}`
}

instance.interceptors.response.use(
  response => response, 
  error => {
    if (error.response && error.response.status === 401) {
      localStorage.removeItem('token')
      router.push('/login')
    }
    return Promise.reject(error)
  }
)

export default instance
