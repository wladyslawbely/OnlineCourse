<template>
    <div class="notifications-container">
      <button class="notifications-btn" @click="showNotifications = !showNotifications">
        🔔 <span v-if="notificationsStore.unreadCount">{{ notificationsStore.unreadCount }}</span>
      </button>
  
      <div v-if="showNotifications" class="notifications-dropdown">
        <template v-if="notificationsStore.notifications.length">
          <div v-for="notification in notificationsStore.notifications" :key="notification.notificationId" class="notification">
            <p @click="markAsRead(notification.notificationId)">
              {{ notification.message }}
            </p>
            <small>{{ new Date(notification.createdAt).toLocaleString() }}</small>
          </div>
         
        </template>
  
        <p v-else class="no-notifications">Нет новых уведомлений</p>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useNotificationsStore } from '../stores/notificationsStore'
  
  const notificationsStore = useNotificationsStore()
  const showNotifications = ref(false)
  
  onMounted(() => {
    notificationsStore.fetchNotifications() 
  })
  
  function markAsRead(notificationId) {
    console.log(notificationId);
    notificationsStore.markAsRead(notificationId) 
  }
  </script>
  
  <style scoped>
  .notifications-container {
    position: relative;
  }
  
  .notifications-btn {
    background: transparent;
    border: none;
    font-size: 20px;
    cursor: pointer;
    position: relative;
  }
  
  .notifications-btn span {
    position: absolute;
    top: -5px;
    right: -5px;
    background: red;
    color: white;
    padding: 3px 7px;
    border-radius: 50%;
    font-size: 12px;
  }
  
  .notifications-dropdown {
    position: absolute;
    top: 30px;
    right: 0;
    background: white;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    padding: 10px;
    border-radius: 5px;
    width: 250px;
    max-height: 350px; 
    overflow-y: auto;
  }
  
  .notification {
    padding: 5px;
    border-bottom: 1px solid #ddd;
    cursor: pointer;
  }
  
  .notification:hover {
    background: #f0f0f0;
  }
  
  .no-notifications {
    text-align: center;
    color: #777;
    padding: 10px;
  }
  </style>
  