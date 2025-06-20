<template>
    <div class="modules-view">
      <h1>Управление модулями и уроками курса "{{ course.title }}"</h1>
  
      <button @click="showModuleForm = true">➕ Добавить модуль</button>
  
      <div v-if="showModuleForm" class="modal">
        <h3>Новый модуль</h3>
        <input v-model="newModule.title" placeholder="Название модуля" />
        <button @click="addModule">Сохранить</button>
        <button @click="showModuleForm = false">Отмена</button>
      </div>
  
      <div v-for="module in course.modules" :key="module.moduleId" class="module-block">
        <div class="module-header">
          <h2>{{ module.title }}</h2>
          <div class="button-group">
            <button @click="editModule(module)">✏️</button>
            <button @click="deleteModule(module.moduleId)">🗑️</button>
          </div>
        </div>
  
        <ul>
          <li v-for="lesson in module.lessons" :key="lesson.lessonId">
            <span class="lesson-title">{{ lesson.title }}</span>
            <div class="button-group">
              <button @click="editLesson(lesson)">✏️</button>
              <button @click="deleteLesson(lesson.lessonId)">🗑️</button>
            </div>
          </li>
        </ul>
  
        <button @click="startAddingLesson(module.moduleId)">➕ Добавить урок</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import axios from '../services/axios'
  
  const route = useRoute()
  const course = ref({ modules: [] })
  
  const showModuleForm = ref(false)
  const newModule = ref({ title: '' })
  const selectedCourseId = route.params.id
  
  async function fetchCourse() {
    try {
      const res = await axios.get(`/courses/${selectedCourseId}`)
      course.value = res.data
    } catch (err) {
      console.error('Ошибка загрузки курса', err)
    }
  }
  
  async function addModule() {
    try {
      await axios.post('/modules', {
        courseId: selectedCourseId,
        title: newModule.value.title,
        orderIndex: course.value.modules.length + 1
      })
      showModuleForm.value = false
      newModule.value.title = ''
      fetchCourse()
    } catch (err) {
      console.error('Ошибка создания модуля', err)
    }
  }
  
  async function deleteModule(moduleId) {
    if (confirm('Удалить модуль?')) {
      await axios.delete(`/modules/${moduleId}`)
      fetchCourse()
    }
  }
  
  function startAddingLesson(moduleId) {
    const title = prompt('Введите название урока')
    if (!title) return
  
    axios.post('/lessons', {
      moduleId,
      title,
      orderIndex: 1
    }).then(fetchCourse).catch(console.error)
  }
  
  async function deleteLesson(lessonId) {
    if (confirm('Удалить урок?')) {
      await axios.delete(`/lessons/${lessonId}`)
      fetchCourse()
    }
  }
  
  function editModule(module) {
    const newTitle = prompt('Новое название модуля:', module.title)
    if (newTitle) {
      axios.put(`/modules/${module.moduleId}`, {
        ...module,
        title: newTitle
      }).then(fetchCourse)
    }
  }
  
  function editLesson(lesson) {
    const newTitle = prompt('Новое название урока:', lesson.title)
    if (newTitle) {
      axios.put(`/lessons/${lesson.lessonId}`, {
        ...lesson,
        title: newTitle
      }).then(fetchCourse)
    }
  }
  
  onMounted(() => {
    fetchCourse()
  })
  </script>
  
  <style scoped>
  .modules-view {
  max-width: 900px;
  margin: 40px auto;
  padding: 24px;
  background-color: #fdfdfd;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.06);
}

.modules-view h1 {
  font-size: 24px;
  margin-bottom: 20px;
  text-align: center;
  color: #333;
}

button {
  padding: 8px 14px;
  font-size: 14px;
  margin: 6px 6px 6px 0;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  background-color: #1e88e5;
  color: white;
  transition: background-color 0.2s ease;
}

button:hover {
  background-color: #1565c0;
}

.module-block {
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 20px;
  background-color: #f9f9fb;
}

.module-block h2 {
  font-size: 18px;
  margin-bottom: 10px;
  color: #444;
}

.module-block ul {
  list-style: none;
  padding-left: 0;
  margin: 10px 0;
}

.module-block li {
  padding: 8px 10px;
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 6px;
  margin-bottom: 6px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.module-block li button {
  margin-left: 8px;
  background-color: #ff7043;
}

.module-block li button:hover {
  background-color: #e64a19;
}

.modal {
  background-color: #f1f5f9;
  padding: 20px;
  border-radius: 8px;
  margin: 20px 0;
  border: 1px solid #ccc;
}

.modal h3 {
  margin-bottom: 12px;
}

.modal input {
  padding: 8px;
  font-size: 15px;
  width: 100%;
  margin-bottom: 12px;
  border-radius: 5px;
  border: 1px solid #bbb;
}


.module-block {
  position: relative;
  padding: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
  margin-bottom: 20px;
  background-color: #f9f9fb;
}

.module-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.lesson-title {
  flex: 1;
  font-size: 15px;
}

.module-block ul {
  list-style: none;
  padding: 0;
  margin-top: 10px;
}

.module-block li {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #fff;
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 6px;
  margin-bottom: 8px;
}

.button-group {
  display: flex;
  gap: 8px;
}

.button-group button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  padding: 4px 6px;
  transition: transform 0.15s ease;
}

.button-group button:hover {
  transform: scale(1.1);
}

  </style>
  