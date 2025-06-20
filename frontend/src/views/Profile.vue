<template>
  <div class="profile">
    <h1>Профиль</h1>

    <div v-if="authStore.user">
      <p><strong>Имя:</strong> {{ authStore.user.firstName }}</p>
      <p><strong>Фамилия:</strong> {{ authStore.user.lastName }}</p>
      <p><strong>Email:</strong> {{ authStore.user.email }}</p>
      <p><strong>Роль:</strong> {{ authStore.user.role }}</p>
    </div>
    <div v-else>
      <p>Загрузка данных пользователя...</p>
    </div>

    <hr />

    <div v-if="authStore.user.role === 'student'">
    <div v-if="courses.length">
      <h2>Мои курсы</h2>
      <ul>
        <li v-for="course in courses" :key="course.courseId">
          <router-link :to="`/courses/${course.courseId}`">
            {{ course.courseTitle }} (Прогресс: {{ course.progress }}%)
          </router-link>
          <button
            v-if="course.progress === 100"
            @click="sendCertificate(course.courseId)"
            class="cert-button">
            📩 Получить сертификат
          </button>
        </li>
      </ul>
    </div>
    <div v-else>
      <p>Вы еще не записаны ни на один курс.</p>
    </div>

    </div>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '../stores/authStore'
import axios from '../services/axios'

const authStore = useAuthStore()
const courses = ref([])
const errorMessage = ref('')

async function fetchMyCourses() {
  try {
    const user = authStore.user;
    if (user && user.role === 'student') {
      const response = await axios.get(`/enrollments/user/${user.userId}`);
      courses.value = response.data;
    }
  } catch (error) {
    errorMessage.value = 'Ошибка при загрузке курсов.';
    console.error(error);
  }
}

const sendCertificate = async (courseId) => {
  try {
    await axios.post(`/certificates/${courseId}/send`);
    alert("Сертификат отправлен на вашу почту!");
  } catch (error) {
    console.error("Ошибка при отправке сертификата:", error);
    alert("Не удалось отправить сертификат.");
  }
};

onMounted(() => {
  fetchMyCourses()
})
</script>


<style scoped>
.profile {
  max-width: 600px;
  margin: 40px auto;
  background: #fff;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  text-align: center;
}

h1 {
  font-size: 24px;
  color: #333;
  margin-bottom: 20px;
}

p {
  font-size: 16px;
  color: #444;
  margin: 5px 0;
}

strong {
  color: #222;
}

hr {
  border: 0;
  height: 1px;
  background: #ddd;
  margin: 20px 0;
}

.error {
  color: red;
  font-weight: bold;
}

h2 {
  font-size: 20px;
  color: #007bff;
  margin-bottom: 15px;
}

ul {
  list-style: none;
  padding: 0;
}

li {
  padding: 10px;
  margin: 5px 0;
  background: #eef2ff;
  border-radius: 8px;
}

li:hover {
  background: #dde7ff;
}

a {
  text-decoration: none;
  color: #1e88e5;
}

a:hover {
  text-decoration: underline;
}
</style>