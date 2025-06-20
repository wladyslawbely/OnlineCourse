<template>
  <div class="modules">
    <h1>{{ course.title }}</h1>
    <p class="course-description">{{ course.description }}</p>

    <div v-for="module in course.modules" :key="module.moduleId" class="module">
      <div class="module-header">
        <h2>{{ module.title }}</h2>
        <router-link :to="`/modules/${module.moduleId}/test`">
          <button
            class="test-btn"
            :disabled="!isModuleTestAvailable(module)"
            @click="goToTest(module.moduleId)">
            📝 Пройти тест
          </button>
        </router-link>
      </div>
      <ul class="lessons-list">
        <li
          v-for="lesson in module.lessons"
          :key="lesson.lessonId"
          class="lesson-item"
          :class="{ locked: isLessonLocked(module, lesson) }"        >
          <template v-if="!isLessonLocked(module, lesson)">
            <router-link :to="`/lessons/${lesson.lessonId}`" class="lesson-link">
              <span class="lesson-icon">📘</span>
              {{ lesson.title }}
              <span v-if="isLessonCompleted(lesson.lessonId)" class="completed-indicator">✅</span>
            </router-link>
          </template>
          <template v-else>
            <div class="lesson-link disabled">
              <span class="lesson-icon">🔒</span>
              {{ lesson.title }}
            </div>
          </template>
        </li>
      </ul>
    </div>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import axios from '../services/axios'

const route = useRoute()
const course = ref({})
const errorMessage = ref('')
const completedLessonIds = ref([])

async function fetchModulesAndLessons() {
  try {
    const response = await axios.get(`/courses/${route.params.id}`)
    course.value = response.data
  } catch (error) {
    errorMessage.value = 'Не удалось загрузить модули и уроки.'
    console.error(error)
  }
}

async function fetchCompletedLessons() {
  try {
    const response = await axios.get('/lesson-progress/completed-lessons')
    completedLessonIds.value = response.data
  } catch (error) {
    console.error('Ошибка при получении прогресса:', error)
  }
}

function isLessonCompleted(lessonId) {
  return completedLessonIds.value.includes(lessonId)
}

function isLessonLocked(module, lesson) {
  const sortedLessons = [...module.lessons].sort((a, b) => a.orderIndex - b.orderIndex);
  const index = sortedLessons.findIndex(l => l.lessonId === lesson.lessonId);
  if (index === 0) return false; 
  const previousLesson = sortedLessons[index - 1];
  return !completedLessonIds.value.includes(previousLesson.lessonId);
}

function isModuleTestAvailable(module) {
  return module.lessons.every(lesson => completedLessonIds.value.includes(lesson.lessonId));
}

function goToTest(moduleId) {
  window.location.href = `/modules/${moduleId}/test`;
}


onMounted(() => {
  fetchModulesAndLessons()
  fetchCompletedLessons()
})
</script>

<style scoped>
.modules {
  max-width: 800px;
  margin: 40px auto;
  padding: 0 20px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.modules h1 {
  text-align: center;
  font-size: 2rem;
  color: #333;
  margin-bottom: 8px;
}

.course-description {
  text-align: center;
  font-size: 1.1rem;
  color: #444;
  margin-bottom: 30px;
}

.module {
  background: #ffffff;
  border: 1px solid #e1e4e8;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.05);
  padding: 20px;
  margin-bottom: 20px;
  transition: transform 0.2s ease;
}

.module:hover {
  transform: translateY(-2px);
}

.module-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 15px;
}

.module-header h2 {
  font-size: 1.5rem;
  margin: 0;
  color: #24292e;
}

.test-btn {
  background: linear-gradient(45deg, #4CAF50, #43A047);
  border: none;
  padding: 10px 16px;
  color: #fff;
  border-radius: 5px;
  font-size: 1rem;
  cursor: pointer;
}

.test-btn:hover {
  background: linear-gradient(45deg, #43A047, #388E3C);
  transform: translateY(-1px);
}

.lessons-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.lesson-item {
  margin-bottom: 10px;
}

.lesson-link {
  display: flex;
  align-items: center;
  text-decoration: none;
  color: #0366d6;
  padding: 6px 10px;
  border-radius: 5px;
}

.lesson-link:hover {
  background: #f1f8ff;
}

.lesson-icon {
  margin-right: 8px;
  font-size: 1.2rem;
}

.completed-indicator {
  margin-left: auto;
  color: #28a745;
  font-size: 1.2rem;
}

.lesson-item.locked .lesson-link {
  opacity: 0.5;
  pointer-events: none;
  cursor: default;
}

.lesson-link.disabled {
  color: gray;
  text-decoration: none;
}

.error {
  color: #d73a49;
  text-align: center;
  margin-top: 20px;
  font-size: 1.1rem;
}
</style>
