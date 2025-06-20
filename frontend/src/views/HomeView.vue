<template>
  <div>
    <h1 v-if="authStore.user">Добро пожаловать, {{ authStore.user.firstName }}</h1>
    <button @click="handleAuth">
      {{ authStore.user ? 'Выйти' : 'Войти' }}
    </button>
  </div>
</template>

<script setup>
import { useAuthStore } from '../stores/authStore'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

function handleAuth() {
  if (authStore.user) {
    authStore.logout()
  } else {
    router.push('/login')
  }
}
</script>  