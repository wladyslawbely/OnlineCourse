<template>
  <header>
    <h1>Онлайн-курсы</h1>
  </header>
  <nav>
      <router-link to="/">Главная</router-link>
      <router-link to="/courses">Курсы</router-link>
      <router-link v-if="authStore.isAuthenticated" to="/profile">Профиль</router-link>
      <router-link v-if="!authStore.isAuthenticated" to="/login">Вход</router-link>
      <router-link v-if="!authStore.isAuthenticated" to="/register">Регистрация</router-link>
      <router-link v-if="authStore.user?.role === 'admin'" to="/admin">Админ-панель</router-link>
      <router-link v-if="authStore.user?.role === 'manager'" to="/manager">Менеджерская панель</router-link>
      <router-link v-if="authStore.user?.role === 'instructor'" to="/instructor/courses">Преподавательская панель</router-link>
      <router-link v-if="authStore.user?.role != 'student' && authStore.isAuthenticated" to="/categories">📚 Категории</router-link>

      <NotificationDropdown v-if="authStore.isAuthenticated" />

      <button v-if="authStore.isAuthenticated" @click="handleLogout">Выход</button>
  </nav>
</template>
  
<script setup>
import { useAuthStore } from '../stores/authStore'
import { useRouter } from 'vue-router'
import NotificationDropdown from "../views/NotificationDropdown.vue";

const authStore = useAuthStore()
const router = useRouter()
  
async function handleLogout() {
  authStore.logout()
  router.push('/login') 
}

</script>
  
<style scoped>
header {
  width: 100%;
}

nav {
  display: flex;
  gap: 15px;
  background: #ffffff;
  padding: 10px 20px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  align-items: center;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  z-index: 100;
}

nav a {
  text-decoration: none;
  color: #333;
  padding: 8px 12px;
  border-radius: 5px;
  transition: background-color 0.3s, color 0.3s;
}

nav a:hover {
  background-color: #ddd;
  color: #000;
}

nav a.router-link-active {
  background-color: #007bff;
  color: #fff;
}

nav button {
  background-color: #f44336;
  color: white;
  border: none;
  border-radius: 5px;
  padding: 8px 12px;
  cursor: pointer;
  transition: background-color 0.3s;
  width: auto; 
  max-width: 120px; 
  flex: none; 
}

nav button:hover {
  background-color: #d32f2f;
}

@media (max-width: 768px) {
  nav {
    flex-wrap: wrap;
    justify-content: center;
  }

  nav a {
    flex: 1;
    text-align: center;
  }

  nav button {
    flex: none;
    width: auto;
    max-width: 120px;
  }
}

body {
  padding-top: 60px;
}
</style>