<template>
    <div class="manager-panel">
      <h1>Панель менеджера</h1>
  
      <button @click="goToCreate">Добавить курс</button>
      
      <table>
        <thead>
          <tr>
            <th>Название</th>
            <th>Преподаватель</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="course in courses" :key="course.courseId">
            <td>{{ course.title }}</td>
            <td>{{ course.instructorName || 'Неизвестен' }}</td>
            <td>
              <button @click="editCourse(course.courseId)">✏️</button>
              <button @click="deleteCourse(course.courseId)">🗑️</button>
              <router-link :to="`/manager/courses/${course.courseId}/modules`">
                🛠 Редактировать модуль
              </router-link>
            </td>
            
          </tr>
        </tbody>
      </table>
      
  
      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </template>
  
  <script setup>
  import { onMounted, ref } from 'vue'
  import axios from '../services/axios'
  import { useRouter } from 'vue-router'
  
  const courses = ref([])
  const error = ref('')
  const router = useRouter()
  
  async function fetchCourses() {
    try {
      const response = await axios.get('/courses')
      courses.value = response.data
    } catch (err) {
      error.value = 'Ошибка загрузки курсов'
    }
  }
  
  function goToCreate() {
    router.push('/manager/courses/create')
  }
  
  function editCourse(id) {
    router.push(`/manager/courses/${id}/edit`)
  }
  
  async function deleteCourse(id) {
    if (confirm('Вы уверены, что хотите удалить курс?')) {
      try {
        await axios.delete(`/courses/${id}`)
        fetchCourses()
      } catch (err) {
        error.value = 'Ошибка удаления курса'
      }
    }
  }
  
  onMounted(() => {
    fetchCourses()
  })
  </script>
  
  <style scoped>
 .manager-panel {
  max-width: 900px;
  margin: 40px auto;
  padding: 24px;
  background-color: #f9f9fb;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.06);
}

.manager-panel h1 {
  text-align: center;
  font-size: 26px;
  margin-bottom: 20px;
  color: #333;
}

button {
  padding: 8px 14px;
  font-size: 14px;
  background-color: #1e88e5;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  margin-bottom: 12px;
  transition: background-color 0.2s ease;
}

button:hover {
  background-color: #1565c0;
}

table {
  width: 100%;
  border-collapse: collapse;
  background-color: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 6px;
  overflow: hidden;
}

thead {
  background-color: #f0f0f0;
}

th, td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #eaeaea;
  font-size: 15px;
  color: #333;
}

td button {
  margin-right: 6px;
  background-color: #ff7043;
}

td button:hover {
  background-color: #e64a19;
}

td a {
  margin-left: 8px;
  font-size: 14px;
  color: #1e88e5;
  text-decoration: none;
}

td a:hover {
  text-decoration: underline;
}

.error {
  margin-top: 20px;
  text-align: center;
  color: #d32f2f;
  font-weight: bold;
}

  </style>
  