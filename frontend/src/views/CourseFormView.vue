<template>
  <div class="course-form">
    <h1>{{ isEdit ? 'Редактировать курс' : 'Создать курс' }}</h1>

    <form @submit.prevent="handleSubmit">
      <div>
        <label>Название курса:</label>
        <input v-model="form.title" required />
      </div>

      <div>
        <label>Категории:</label>
        <select v-model="selectedCategoryIds" multiple required size="5">
          <option 
            v-for="cat in categories" 
            :key="cat.categoryId" 
            :value="cat.categoryId"
          >
            {{ cat.name }}
          </option>
        </select>
      </div>

      <div>
        <label>Инструктор:</label>
        <select v-model="form.instructorId" required>
          <option v-for="instr in instructors" :key="instr.userId" :value="instr.userId">
            {{ instr.firstName }} {{ instr.lastName }}
          </option>
        </select>
      </div>


      <div>
        <label>Описание:</label>
        <textarea v-model="form.description"></textarea>
      </div>

      <button type="submit" class="btn">
        {{ isEdit ? 'Сохранить изменения' : 'Создать курс' }}
      </button>
    </form>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from '../services/axios'
import { useAuthStore } from '../stores/authStore'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const isEdit = ref(route.params.id !== undefined)

const form = ref({
  title: '',
  description: '',
  categories: [],
  instructorId: null, 
})

const selectedCategoryIds = ref([])
const categories = ref([])
const instructors = ref([]) 
const errorMessage = ref('')

onMounted(async () => {
  try {
    const res = await axios.get('/course-categories')
    categories.value = res.data
  } catch {
    errorMessage.value = 'Ошибка загрузки категорий'
  }

  try {
    const res = await axios.get('/users/instructors')
    instructors.value = res.data
  } catch {
    errorMessage.value = 'Ошибка загрузки инструкторов'
  }

  if (isEdit.value) {
    try {
      const res = await axios.get(`/courses/${route.params.id}`)
      const course = res.data
      form.value.title = course.title
      form.value.description = course.description
      form.value.categories = course.categories
      form.value.instructorId = course.userId
      selectedCategoryIds.value = course.categories.map(c => c.categoryId)
    } catch (err) {
      errorMessage.value = 'Ошибка загрузки курса'
    }
  }
})

async function handleSubmit() {
  form.value.categories = selectedCategoryIds.value.map(id => {
    const category = categories.value.find(c => c.categoryId === id)
    return {
      categoryId: id,
      name: category?.name || ''
    }
  })

  try {
    if (isEdit.value) {
      await axios.put(`/courses/${route.params.id}`, form.value)
    } else {
      await axios.post('/courses', form.value)
    }
    router.push('/manager')
  } catch (err) {
    errorMessage.value = 'Ошибка сохранения курса'
  }
}
</script>


 <style scoped>
.course-form {
  max-width: 500px;
  margin: 40px auto;
  padding: 20px;
  background: #fff;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
  border-radius: 10px;
}

h1 {
  text-align: center;
  font-size: 24px;
  color: #333;
}

.form-group {
  margin-bottom: 15px;
}

select[multiple] {
  height: auto;
  min-height: 120px;
}
label {
  display: block;
  font-weight: bold;
  margin-bottom: 5px;
}

input, textarea, select {
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 16px;
}

select {
  height: 40px;
}

.btn {
  display: block;
  width: 100%;
  background: #007bff;
  color: white;
  padding: 10px;
  font-size: 18px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.btn:hover {
  background: #0056b3;
}

.error {
  text-align: center;
  color: red;
  font-size: 14px;
  margin-top: 10px;
}
</style>