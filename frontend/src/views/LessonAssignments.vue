<template>
    <div class="assignments">
      <h2>Задания</h2>
      
      <ul v-if="assignments.length">
        <li v-for="assignment in lessonAssignments[lesson.lessonId]" :key="assignment.assignmentId">
        <div>
          <p>📌 {{ assignment.title }} — {{ assignment.description }}</p>

          <div v-if="submissions[assignment.assignmentId]">
            <p>✅ Отправлено</p>
            <p>Оценка: {{ submissions[assignment.assignmentId].grade ?? 'Ожидает проверки' }}</p>
            <p>Комментарий: {{ submissions[assignment.assignmentId].feedback || '—' }}</p>
          </div>
          <div v-else>
            <textarea v-model="newSubmissions[assignment.assignmentId]" placeholder="Ваш ответ..."></textarea>
            <button @click="submitAssignment(assignment.assignmentId)">Сдать</button>
          </div>
          </div>
        </li>

      </ul>
  
      <p v-else>Заданий пока нет.</p>
  
      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import axios from '../services/axios'
  
  const route = useRoute()
  const assignments = ref([])
  const error = ref('')
  const submissions = ref({})
  const newSubmissions = ref({})

  async function fetchUserSubmissions() {
    const res = await axios.get(`/submission/user/${authStore.user.userId}`)
    for (const sub of res.data) {
      submissions.value[sub.assignmentId] = sub
    }
  }

  async function submitAssignment(assignmentId) {
    try {
      await axios.post('/submission', {
        assignmentId,
        userId: authStore.user.userId,
        content: newSubmissions.value[assignmentId]
      })
      alert('Отправлено!')
      await fetchUserSubmissions()
    } catch (err) {
      console.error('Ошибка при отправке:', err)
    }
  }

  async function fetchAssignments() {
    try {
      const res = await axios.get(`/assignments/lesson/${route.params.lessonId}`)
      assignments.value = res.data
    } catch (e) {
      console.error('Ошибка при загрузке заданий:', e)
      error.value = 'Не удалось загрузить задания.'
    }
  }
  
  function formatDate(dateStr) {
    const date = new Date(dateStr)
    return date.toLocaleDateString()
  }
  
  onMounted(() => {
    fetchAssignments()
  })
  </script>
  
  <style scoped>
  .assignments {
    margin-top: 20px;
  }
  .error {
    color: red;
  }
  </style>
  