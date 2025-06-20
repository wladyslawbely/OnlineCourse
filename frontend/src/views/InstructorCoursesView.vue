<template>
  <div class="instructor-courses">
    <h1>Мои курсы</h1>

    <div v-if="courses.length">
      <div v-for="course in courses" :key="course.courseId" class="course-card">
        <div class="course-header">
          <h3>{{ course.title }}</h3>
          <div class="button-group">
        <button @click="startAddingModule(course.courseId)">➕ Добавить модуль</button>
      </div>
        </div>
        <p>{{ course.description }}</p>

        <div v-for="module in course.modules" :key="module.moduleId" class="module-block">
          <div class="module-header">
            <h4>{{ module.title }}</h4>
            <div class="button-group">
              <button @click="editModule(module)">✏️</button>
              <button @click="deleteModule(module.moduleId)">🗑️</button>
              <button @click="startAddingLesson(module.moduleId)">➕ Добавить урок</button>
              <button v-if="!module.testJson" @click="startAddingTest(module.moduleId)">➕ Добавить тест </button>
              <button v-else @click="startEditingTest(module.testJson)">✏️ Редактировать тест</button>
            </div>
          </div>

          <ul>
            <li v-for="lesson in module.lessons" :key="lesson.lessonId">
              <div class="lesson-header">
                <span>{{ lesson.title }}</span>
                <div class="button-group">
                  <button @click="editLesson(lesson)">✏️</button>
                  <button @click="deleteLesson(lesson.lessonId)">🗑️</button>
                  <button @click="startCreatingAssignment(lesson.lessonId)">📄 Создать задание</button>
                  <button @click="fetchAssignments(lesson.lessonId)">➕ Просмотр</button>
                </div>
              </div>

              <div class="upload-container">
                <input type="file" @change="handleFileChange($event, lesson.lessonId)" />
                <button @click="uploadFile(lesson.lessonId)">📁</button>
              </div>

              <ul v-if="lessonAssignments[lesson.lessonId]">
                <li v-for="assignment in lessonAssignments[lesson.lessonId]" :key="assignment.assignmentId">
                  <div class="assignment-header">
                    <span>📌 {{ assignment.title }} — {{ assignment.description }}</span>
                    <div class="button-group">
                      <button @click="startEditingAssignment(assignment)">✏️</button>
                      <button @click="deleteAssignment(assignment.assignmentId, lesson.lessonId)">🗑️</button>
                      <button @click="toggleSubmissions(assignment.assignmentId)">
                        {{ assignmentSubmissionsVisible[assignment.assignmentId] ? 'Скрыть ответы' : '📬 Ответы' }}
                      </button>
                    </div>
                  </div>

                  <ul v-if="assignmentSubmissionsVisible[assignment.assignmentId]">
                    <li v-for="submission in assignmentSubmissions[assignment.assignmentId]" :key="submission.submissionId">
                      <div class="submission-info">
                        👤 {{ submission.userEmail }} {{ submission.userName }}  
                        📄 {{ submission.content }}
                      </div>

                      <div v-if="submission.editing" class="submission-edit">
                        <label>Оценка:
                          <input type="number" v-model.number="submission.grade"
                            :min="1"
                            :max="10"
                            @input="enforceGradeBounds(submission)"/>
                        </label>
                        <input type="text" v-model="submission.feedback" placeholder="Комментарий" />
                        <div class="button-group">
                          <button @click="saveGrade(submission)">💾</button>
                          <button @click="cancelEdit(submission)">❌</button>
                        </div>
                      </div>

                      <div v-else class="button-group">
                        <button @click="editSubmission(submission)">✏️</button>
                      </div>
                    </li>
                  </ul>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </div>

    <p v-else>У вас пока нет курсов.</p>
    <p v-if="error" class="error">{{ error }}</p>

    <div v-if="showAssignmentForm" class="overlay" @click="closeModal"></div>
    <div v-if="showAssignmentForm" class="modal">
      <h3>{{ editingAssignment ? 'Редактировать задание' : 'Новое задание' }}</h3>
      <input v-model="newAssignment.title" placeholder="Название задания" />
      <textarea v-model="newAssignment.description" placeholder="Описание"></textarea>
      <input type="date" v-model="newAssignment.dueDate" :min="today"/>
      <div class="button-group">
        <button @click="saveAssignment">💾</button>
        <button @click="closeModal">❌</button>
      </div>
    </div>

  <div class="modal" v-if="showTestForm">
    <h3>{{ editingTest ? 'Редактировать тест' : 'Новый тест' }}</h3>
    <input v-model="newTest.title" placeholder="Название теста" />

    <div
      v-for="(question, qIndex) in newTest.questions"
      :key="qIndex"
      class="question-block"
    >
      <input v-model="question.text" placeholder="Текст вопроса" />
      
      <div
        v-for="(option, oIndex) in question.options"
        :key="oIndex"
        class="option-block"
      >
        <input
          v-model="option.text"
          placeholder="Вариант ответа"
        />
        <input type="checkbox" v-model="option.isCorrect" /> ✅ Правильный ответ
      </div>

      <button @click="addOption(qIndex)">➕ Добавить вариант</button>
    </div>

    <button @click="addQuestion">➕ Добавить вопрос</button>
    <button @click="saveTest">💾 Сохранить тест</button>
    <button @click="closeModal1">❌ Закрыть</button>
  </div>

  <div v-if="showLessonForm" class="overlay" @click="closeLessonForm"></div>
    <div v-if="showLessonForm" class="modal">
      <h3>Создание урока</h3>
      <input v-model="newLesson.title" placeholder="Название урока" />
      <textarea v-model="newLesson.content" placeholder="Содержание урока"></textarea>
      <input v-model="newLesson.videoUrl" @input="validateYoutubeUrl(newLesson.videoUrl)" placeholder="Ссылка на видео (необязательно)" />
      <p v-if="!isValidUrl" style="color: red;">Некорректная ссылка на YouTube</p>
      <div class="button-group">
        <button @click="createLesson" :disabled="!isValidUrl">💾 Сохранить</button>
        <button @click="closeLessonForm">❌</button>
      </div>
    </div>

  </div>
</template>


<script setup>
import { ref, onMounted } from 'vue'
import axios from '../services/axios'

const courses = ref([])
const error = ref('')
const showAssignmentForm = ref(false)
const editingAssignment = ref(null)
const currentLessonId = ref(null)
const newAssignment = ref({ title: '', description: '', dueDate: '', lessonId: '' })
const lessonAssignments = ref({})
const assignmentSubmissions = ref({})
const assignmentSubmissionsVisible = ref({})
const showTestForm = ref(false)
const editingTest = ref(false)
const newTest = ref({title: '', moduleId: '', questions: [] })
const today = new Date().toISOString().split('T')[0]

function startAddingTest(moduleId) {
  showTestForm.value = true
  editingTest.value = false
  newTest.value = {
    title: '',
    moduleId,
    testId: null,
    questions: []
  }
}

function startEditingTest(test, moduleId) {
  if (typeof test === 'string') {
    try {
      test = JSON.parse(test)
    } catch (e) {
      console.error('❌ Ошибка при парсинге теста:', e)
      alert('Ошибка при загрузке теста. Проверьте структуру данных.')
      return
    }
  }

  if (!test || !Array.isArray(test.Questions)) {
    alert('Тест не содержит корректных данных.')
    return
  }

  showTestForm.value = true
  editingTest.value = true

  newTest.value = {
    title: test.Title,
    moduleId,
    questions: test.Questions.map(q => ({
      text: q.Question,
      options: q.Options.map((opt, i) => ({
        text: opt,
        isCorrect: q.CorrectAnswers.includes(i)
      }))
    }))
  }
}

const addQuestion = () => {
  newTest.value.questions.push({
    text: '',
    options: []
  })
}

const addOption = (questionIndex) => {
  if (!newTest.value.questions[questionIndex].options) {
    newTest.value.questions[questionIndex].options = []
  }
  newTest.value.questions[questionIndex].options.push({
    text: '',
    isCorrect: false
  })
}

const closeModal1 = () => {
  showTestForm.value = false
}

const saveTest = async () => {
  try {
    const transformedQuestions = newTest.value.questions.map(q => ({
      question: q.text, 
      options: q.options.map(o => o.text || ''), 
      correctAnswers: q.options.reduce((acc, option, index) => {
        if (option.isCorrect) acc.push(index)
        return acc
      }, [])
    }))

    const payload = {
      title: newTest.value.title,
      moduleId: newTest.value.moduleId,
      questions: transformedQuestions
    }

      if (editingTest.value && newTest.value.testId) {
      await axios.put(`/modules/${newTest.value.moduleId}/test/${newTest.value.testId}`, payload)
    } else {
      await axios.post(`/modules/${newTest.value.moduleId}/test`, payload)
    }
    console.log("Ответ сервера:", response.data)
    alert("✅ Тест успешно сохранён!")
    closeModal()
  } catch (error) {
    console.error("Ошибка сохранения теста:", error)
  }
}

async function fetchCourses() {
  try {
    const res = await axios.get('/courses/my')
    courses.value = res.data
  } catch (e) {
    error.value = 'Ошибка загрузки курсов'
    console.error(e)
  }
}

function enforceGradeBounds(submission) {
  if (submission.grade > 10) submission.grade = 10
  if (submission.grade < 1) submission.grade = 1
}


function startAddingModule(courseId) {
  const course = courses.value.find(c => c.courseId === courseId);
  const maxIndex = course?.modules?.length
    ? Math.max(...course.modules.map(m => m.orderIndex || 0))
    : 0;

  const title = prompt('Введите название модуля');
  if (!title) return;

  axios
    .post('/modules', {
      courseId,
      title,
      orderIndex: maxIndex + 1
    })
    .then(fetchCourses)
    .catch(err => console.error('Ошибка при создании модуля:', err));
}


function editModule(module) {
  const title = prompt('Новое название модуля:', module.title)
  if (!title) return
  axios.put(`/modules/${module.moduleId}`, { ...module, title }).then(fetchCourses)
}

function deleteModule(moduleId) {
  if (confirm('Удалить модуль?')) {
    axios.delete(`/modules/${moduleId}`).then(fetchCourses)
  }
}

function editLesson(lesson) {
  const title = prompt('Новое название урока:', lesson.title)
  if (!title) return
  axios.put(`/lessons/${lesson.lessonId}`, { ...lesson, title }).then(fetchCourses)
}

function deleteLesson(lessonId) {
  if (confirm('Удалить урок?')) {
    axios.delete(`/lessons/${lessonId}`).then(fetchCourses)
  }
}

function startCreatingAssignment(lessonId) {
  currentLessonId.value = lessonId
  newAssignment.value = { title: '', description: '', dueDate: '', lessonId }
  editingAssignment.value = null
  showAssignmentForm.value = true
}

function startEditingAssignment(assignment) {
  newAssignment.value = {
    title: assignment.title,
    description: assignment.description,
    dueDate: assignment.dueDate ? assignment.dueDate.slice(0, 10) : '',
    lessonId: assignment.lessonId
  }
  editingAssignment.value = assignment
  showAssignmentForm.value = true
}

function closeModal() {
  showAssignmentForm.value = false
  editingAssignment.value = null
  newAssignment.value = { title: '', description: '', dueDate: '', lessonId: '' }
}

async function saveAssignment() {
  try {
    if (editingAssignment.value) {
      await axios.put(`/assignments/${editingAssignment.value.assignmentId}`, newAssignment.value)
    } else {
      await axios.post('/assignments', newAssignment.value)
    }
    await fetchAssignments(currentLessonId.value)
    closeModal()
  } catch (err) {
    console.error('Ошибка при сохранении задания:', err)
  }
}

async function deleteAssignment(id, lessonId) {
  if (confirm('Удалить задание?')) {
    try {
      await axios.delete(`/assignments/${id}`)
      await fetchAssignments(lessonId)
    } catch (err) {
      console.error('Ошибка при удалении задания:', err)
    }
  }
}

async function fetchAssignments(lessonId) {
  try {
    const res = await axios.get(`/assignments/lesson/${lessonId}`)
    lessonAssignments.value[lessonId] = res.data
  } catch (err) {
    console.error('Ошибка при загрузке заданий:', err)
  }
}

async function toggleSubmissions(assignmentId) {
  if (assignmentSubmissionsVisible.value[assignmentId]) {
    assignmentSubmissionsVisible.value[assignmentId] = false
  } else {
    if (!assignmentSubmissions.value[assignmentId]) {
      try {
        const res = await axios.get(`/submission/assignment/${assignmentId}`)
        assignmentSubmissions.value[assignmentId] = res.data
      } catch (err) {
        console.error(`Ошибка загрузки ответов для задания ${assignmentId}:`, err)
      }
    }
    assignmentSubmissionsVisible.value[assignmentId] = true
  }
}

function editSubmission(sub) {
  sub.original = { grade: sub.grade, feedback: sub.feedback }
  sub.editing = true
}

function cancelEdit(sub) {
  sub.grade = sub.original.grade
  sub.feedback = sub.original.feedback
  sub.editing = false
}

async function saveGrade(sub) {
  try {
    await axios.put(`/submission/${sub.submissionId}/grade`, {
      grade: sub.grade,
      feedback: sub.feedback
    })
    sub.editing = false
  } catch (err) {
    console.error('Ошибка сохранения оценки:', err)
  }
}

const fileMap = ref({})

function handleFileChange(event, lessonId) {
  fileMap.value[lessonId] = event.target.files[0]
}

async function uploadFile(lessonId) {
  const file = fileMap.value[lessonId]
  if (!file) return alert('Выберите файл!')

  const formData = new FormData()
  formData.append('file', file)
  formData.append('lessonId', lessonId)

  try {
    await axios.post('/files/upload', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
    alert('Файл успешно загружен')
    fileMap.value[lessonId] = null
  } catch (err) {
    console.error('Ошибка загрузки файла:', err)
    alert('Ошибка загрузки файла')
  }
}

const showLessonForm = ref(false)
const newLesson = ref({
  moduleId: null,
  title: '',
  content: '',
  videoUrl: ''
})


const isValidUrl = ref(true);

function validateYoutubeUrl(url) {
  const youtubeRegex = /^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/)[a-zA-Z0-9_-]+$/;
  isValidUrl.value = !url || youtubeRegex.test(url); 
}

function startAddingLesson(moduleId) {
  showLessonForm.value = true
  newLesson.value = {
    moduleId,
    title: '',
    content: '',
    videoUrl: ''
  }
}

function closeLessonForm() {
  showLessonForm.value = false
  newLesson.value = {
    moduleId: null,
    title: '',
    content: '',
    videoUrl: ''
  }
}

async function createLesson() {
  validateYoutubeUrl(newLesson.value.videoUrl);
  if (!isValidUrl.value) {
    alert('Некорректная ссылка на видео YouTube');
    return;
  }

  try {
    const course = courses.value.find(c =>
      c.modules.some(m => m.moduleId === newLesson.value.moduleId)
    );
    const module = course.modules.find(m => m.moduleId === newLesson.value.moduleId);

    const maxIndex = module?.lessons?.length
      ? Math.max(...module.lessons.map(l => l.orderIndex || 0))
      : 0;

    await axios.post('/lessons', {
      ...newLesson.value,
      orderIndex: maxIndex + 1
    });

    closeLessonForm();
    await fetchCourses();
  } catch (err) {
    console.error('Ошибка при создании урока:', err);
    alert('Ошибка при создании урока');
  }
}



onMounted(fetchCourses)
</script>

<style scoped>

.instructor-courses {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.course-card {
  background-color: #ffffff;
  padding: 20px;
  margin-bottom: 20px;
  border-radius: 12px;
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.course-card:hover {
  transform: translateY(-5px);
  box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
}

.module-block {
  background-color: #f8f8f8;
  padding: 15px;
  margin-top: 10px;
  border-radius: 8px;
  box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
}

h1, h3, h4 {
  font-family: 'Arial', sans-serif;
  color: #333;
}

ul {
  list-style: none;
  padding: 0;
}

li {
  padding: 8px 12px;
  border-bottom: 1px solid #ddd;
}

button {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  margin: 5px;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: #0056b3;
}

.modal {
  position: fixed;
  top: 50%;
  left: 50%;
  width: 420px;
  max-width: 90%;
  padding: 25px;
  transform: translate(-50%, -50%);
  background: #ffffff;
  box-shadow: 0px 10px 30px rgba(0, 0, 0, 0.3);
  border-radius: 12px;
  z-index: 1000;
  transition: opacity 0.3s ease, transform 0.3s ease;
  max-height: 80vh; 
  overflow-y: auto;
  padding: 20px;
}

.modal h3 {
  font-size: 22px;
  color: #333;
  margin-bottom: 15px;
}

.modal input,
.modal textarea,
.modal select {
  width: 100%;
  padding: 10px;
  margin: 10px 0;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 16px;
}

.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: rgba(0, 0, 0, 0.5);
  z-index: 999;
  transition: opacity 0.3s ease;
}

.course-header,
.module-header,
.lesson-header,
.assignment-header {
  display: flex;
  align-items: center;
  justify-content: space-between; 
  padding: 10px;
}

.button-group {
  display: flex;
  gap: 8px;
}

.upload-container {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  align-items: center;
}

.submission-info {
  padding: 5px;
  background-color: #fdfdfd;
  border: 1px solid #ddd;
  border-radius: 6px;
  margin-top: 5px;
}


.error {
  color: red;
  margin-top: 10px;
}

.submission-edit,
.submission-info {
  margin-top: 5px;
  padding: 5px;
  background-color: #fdfdfd;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.upload-form {
  margin-top: 10px;
  display: flex;
  align-items: center;
  gap: 10px;
}

</style>