<template>
  <div>
    <h1>{{ course.title }}</h1>
    <p>{{ course.description }}</p>

   <router-link v-if="isEnrolled || userRole != 'student' " :to="`/courses/${course.courseId}/modules`" class="btn">
      Перейти к курсу
   </router-link>

   <button v-else @click="enroll" class="btn">
      Записаться на курс
   </button>


    <p v-if="progress !== null"><strong>Прогресс:</strong> {{ progress }}%</p>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>

    <div class="course-content">
      <h2>📖 Содержание курса</h2>
      <ul class="modules-list">
        <li v-for="module in course.modules" :key="module.moduleId" class="module-item">
          <strong>📌 {{ module.title }}</strong>
          <ul class="lessons-list">
            <li v-for="lesson in module.lessons" :key="lesson.lessonId" class="lesson-item">
              {{ lesson.title }}
            </li>
          </ul>
        </li>
      </ul>
    </div>

    <div class="comments">
      <h2>Комментарии к курсу</h2>
      <textarea v-model="newComment" placeholder="Оставьте комментарий..."></textarea>
      <button @click="postComment">💬 Отправить</button>

      <div v-for="comment in comments" :key="comment.commentId" class="comment">
        <p><strong>{{ comment.userName }}</strong>: {{ comment.content }}</p>
        <small>{{ new Date(comment.createdAt).toLocaleString() }}</small>
        
        <button v-if="isAdmin" @click="setReplyTarget(comment.commentId)">Ответить</button>
        <button v-if="isAdmin" @click="deleteComment(comment.commentId)">Удалить</button>

        <div v-if="replyTarget === comment.commentId">
          <textarea v-model="newReply" placeholder="Введите ответ"></textarea>
          <button @click="replyToComment(comment.commentId)">💬 Отправить ответ</button>
        </div>

        <div v-for="reply in comment.replies" :key="reply.commentId" class="reply">
          <p><strong>{{ reply.userName }}</strong>: {{ reply.content }}</p>
          <small>{{ new Date(reply.createdAt).toLocaleString() }}</small>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted,computed } from 'vue'
import { useRoute } from 'vue-router'
import axios from '../services/axios'
import { useAuthStore } from '../stores/authStore'
const authStore = useAuthStore()
const isAdmin = authStore.isAdmin 

const route = useRoute()
const course = ref({})
const isEnrolled = ref(false)
const errorMessage = ref('')
const progress = ref(null)
const comments = ref([])
const newComment = ref('')
const replyTarget = ref(null)
const newReply = ref('')
const userRole = authStore.role 


async function fetchCourseDetails() {
  try {
    const response = await axios.get(`/courses/${route.params.id}`)
    course.value = response.data
  } catch (error) {
    console.error('Ошибка загрузки курса:', error)
    errorMessage.value = 'Ошибка загрузки курса'
  }
}

async function checkEnrollment() {
  try {
    const response = await axios.get(`/enrollments/${route.params.id}`)
    isEnrolled.value = response.data.isEnrolled
  } catch (error) {
    console.error('Ошибка проверки записи:', error)
  }
}

async function fetchProgress() {
  try {
    const res = await axios.get(`/lesson-progress/${route.params.id}/progress`)
    progress.value = res.data.progress
  } catch (error) {
    console.error('Ошибка при получении прогресса:', error)
  }
}

async function enroll() {
  try {
    await axios.post('/enrollments', { CourseId: course.value.courseId })
    isEnrolled.value = true
  } catch (error) {
    if (error.response && error.response.status === 401) {
      errorMessage.value = 'Вы не авторизованы. Пожалуйста, войдите в систему.'
    } else {
      errorMessage.value = 'Не удалось записаться на курс.'
    }
  }
}

async function fetchComments() {
  try {
    const response = await axios.get(`/comments/course/${route.params.id}`)
    comments.value = response.data
  } catch (error) {
    console.error('Ошибка загрузки комментариев:', error)
  }
}

async function postComment() {
  if (!newComment.value.trim()) return

  try {
    await axios.post('/comments', {
      courseId: route.params.id,
      content: newComment.value
    })
    newComment.value = ''
    fetchComments()
  } catch (error) {
    console.error('Ошибка отправки комментария:', error)
  }
}

function setReplyTarget(commentId) {
  replyTarget.value = commentId
}

async function replyToComment(parentCommentId) {
  if (!newReply.value.trim()) return

  try {
    await axios.post(`/comments/reply/${parentCommentId}`, {
      courseId: route.params.id,
      content: newReply.value,
      isAdmin: true
    })
    newReply.value = ''
    replyTarget.value = null
    fetchComments()
  } catch (error) {
    console.error('Ошибка отправки ответа:', error)
  }
}

async function deleteComment(commentId) {
  try {
    await axios.delete(`/comments/${commentId}`)
    fetchComments() 
  } catch (error) {
    console.error('Ошибка удаления комментария:', error)
  }
}

onMounted(() => {
  fetchCourseDetails()
  checkEnrollment()
  fetchProgress()
  fetchComments()
})
</script>


<style scoped>
.btn {
  padding: 12px 24px;
  background-color: #007bff;
  color: white;
  font-size: 16px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  margin-top: 10px;
}

.btn:hover {
  background-color: #0056b3;
}

.comments {
  margin-top: 20px;
  background: #f9f9f9;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.comment {
  padding: 15px;
  margin-bottom: 10px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
  transition: transform 0.2s ease;
}

.comment:hover {
  transform: scale(1.02);
}

.reply {
  padding: 10px;
  background: #eef2ff;
  margin-left: 25px;
  border-left: 4px solid #007bff;
  border-radius: 5px;
  transition: all 0.3s ease;
}

.reply:hover {
  background: #e0e8ff;
}

textarea {
  width: 100%;
  padding: 10px;
  font-size: 14px;
  border: 1px solid #ccc;
  border-radius: 5px;
  resize: none;
  transition: border-color 0.3s ease;
}

textarea:focus {
  border-color: #007bff;
}

button {
  padding: 10px 16px;
  margin-top: 10px;
  background-color: #28a745;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

button:hover {
  background-color: #218838;
}

.course-content {
  margin-top: 20px;
  padding: 20px;
  background: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.course-content h2 {
  font-size: 22px;
  font-weight: bold;
  margin-bottom: 15px;
}

.modules-list {
  list-style: none;
  padding: 0;
}

.module-item {
  margin-bottom: 15px;
  padding: 10px;
  background: white;
  border-radius: 8px;
  border-left: 4px solid #007bff;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.module-item:hover {
  background: #eef5ff;
  transform: scale(1.02);
}

.lessons-list {
  list-style: none;
  padding-left: 20px;
}

.lesson-item {
  padding: 8px;
  border-radius: 5px;
  transition: all 0.3s ease;
}

.lesson-item:hover {
  background: #dfeaff;
}

.video-link {
  margin-left: 10px;
  color: #007bff;
  text-decoration: none;
  font-size: 14px;
}

.video-link:hover {
  text-decoration: underline;
}

.btn {
  padding: 12px 24px;
  background-color: #007bff;
  color: white;
  font-size: 16px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  margin-top: 10px;
}

.btn:hover {
  background-color: #0056b3;
}
</style>


