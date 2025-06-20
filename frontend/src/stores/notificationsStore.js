import { defineStore } from 'pinia'
import axios from '../services/axios'
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7054/notificationHub", {
      accessTokenFactory: () => localStorage.getItem("token") 
  })
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => console.log("SignalR подключен!"))
    .catch(err => console.error("Ошибка подключения SignalR:", err));
    console.log("Статус SignalR:", connection.state);
   

connection.on("ReceiveNotification", (notification) => {
    console.log("Новое уведомление:", notification);
   
    const notificationsStore = useNotificationsStore(); 
    notificationsStore.addNotification(notification);
});

export default connection;

export const useNotificationsStore = defineStore('notifications', {
  state: () => ({
    notifications: [],
  }),

  getters: {
    unreadCount: (state) => state.notifications.filter(n => !n.isRead).length, 
  },

  actions: {
    async fetchNotifications() {
        const response = await axios.get("/notifications/me");
        this.notifications = response.data;
    },

    async addNotification(notification) {
      
      
      this.notifications = [...this.notifications, notification];   
      try {

        console.log(notification.userId)
        console.log(notification)
          await axios.post("/notifications", { 
              userId: notification.userId, 
              message: notification.message 
          }); 
      } catch (error) {
          console.error("Ошибка при сохранении уведомления:", error);
      }
      this.fetchNotifications(); 
  },
  
  

    async markAsRead(notificationId) {
      try {
        await axios.put(`/notifications/${notificationId}/mark-as-read`) 
        const notification = this.notifications.find(n => n.notificationId === notificationId)
        if (notification) notification.isRead = true
        this.fetchNotifications(); 
      } catch (error) {
        console.error('Ошибка при отметке уведомления:', error)
      }
    }

  }
})
