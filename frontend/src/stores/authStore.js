import { defineStore } from 'pinia'
import axios from '../services/axios'
import { useRouter } from 'vue-router'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    token: localStorage.getItem('token') || null,
    error: null
  }),
  
  getters: {
    isAuthenticated: (state) => !!state.token,
    isAdmin: (state) => state.user?.role === 'admin'
  },

  actions: {
    async login(email, password) {
      try {
        const response = await axios.post('/auth/login', { email, password })
        this.token = response.data.token
        localStorage.setItem('token', this.token)
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`

        await this.getUser()
      } catch (error) {
        this.error = error.response?.data?.message || 'Ошибка входа'
      }
    },

    async getUser() {
      try {
        const response = await axios.get('/auth/me')
        this.user = response.data
      } catch (error) {
        this.logout()
      }
    },

    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
      delete axios.defaults.headers.common['Authorization']
    }
  }
})
