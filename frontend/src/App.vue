<template>
  <div id="app">
    <NavBar />
    <main>
      <div v-if="isLoading" class="loading">Загрузка...</div>
      <router-view v-else />
    </main>
  </div>
  <footer>
      <p>© 2025 Онлайн-курсы. Все права защищены.</p>
  </footer>
</template>

<script setup>
import NavBar from './views/Navbar.vue'
import { ref, onMounted } from 'vue'
import{useNotificationsStore} from './stores/notificationsStore'

const notificationsStore = useNotificationsStore();
const isLoading = ref(true)

setTimeout(() => {
  isLoading.value = false
}, 2000)

onMounted(() => {
    notificationsStore.fetchNotifications();
});
</script>

<style>
body {
  display: flex; 
  flex-direction: column; 
  min-height: 100vh; 
  margin: 0;
  font-family: 'Arial', sans-serif;
  color: #333;
  background: linear-gradient(135deg, #dcdaff, #5a6ac8);
}

#app {
  display: flex; 
  flex-direction: column; 
  flex-grow: 1; 
  width: 90%;
  max-width: 1400px;
  margin: 0 auto; 
}

main {
  flex: 1;
  padding: 20px;
  box-sizing: border-box;
}

footer {
  border-top: 1px solid #ccc;
  text-align: center; 
  flex-shrink: 0;
  box-sizing: border-box; 

  padding-top: 15px;
  padding-bottom: 15px;

  width: 100%;    
  position: static; 

  color: white;
}

.loading {
  text-align: center;
  color: #555;
  font-size: 20px;
  margin: 20px;
}
</style>

