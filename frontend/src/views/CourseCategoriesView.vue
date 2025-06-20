<template>
    <div class="category-admin">
      <h1>Категории курсов</h1>
  
      <form @submit.prevent="createCategory">
        <input v-model="newCategory" placeholder="Новая категория" />
        <button type="submit">Создать</button>
      </form>
  
      <ul>
        <li v-for="category in categories" :key="category.categoryId" class="category-item">
            <span class="category-name">{{ category.name }}</span>
            <div class="button-group">
              <button @click="editCategory(category)">✏️</button>
              <button @click="deleteCategory(category.categoryId)">🗑️</button>
            </div>
        </li>
      </ul>

      <div v-if="editingCategory" class="modal">
        <h3>Редактирование категории</h3>
        <input v-model="editingCategory.name" />
        <button @click="updateCategory">Сохранить</button>
        <button @click="editingCategory = null">Отмена</button>
      </div>
  
      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </template>
  
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import axios from '../services/axios'
  
  const categories = ref([])
  const newCategory = ref('')
  const editingCategory = ref(null)
  const error = ref('')
  
  async function loadCategories() {
    try {
      const res = await axios.get('/course-categories')
      categories.value = res.data
    } catch (err) {
      error.value = 'Ошибка загрузки категорий'
    }
  }
  
  async function createCategory() {
    if (!newCategory.value.trim()) return
    try {
      await axios.post('/course-categories', { name: newCategory.value })
      newCategory.value = ''
      loadCategories()
    } catch (err) {
      error.value = 'Ошибка при создании категории'
    }
  }
  
  function editCategory(category) {
    editingCategory.value = { ...category }
  }
  
  async function updateCategory() {
    try {
      await axios.put(`/course-categories/${editingCategory.value.categoryId}`, editingCategory.value)
      editingCategory.value = null
      loadCategories()
    } catch (err) {
      error.value = 'Ошибка при редактировании'
    }
  }
  
  async function deleteCategory(id) {
    if (!confirm('Удалить категорию?')) return
    try {
      await axios.delete(`/course-categories/${id}`)
      loadCategories()
    } catch (err) {
      error.value = 'Ошибка при удалении'
    }
  }
  
  onMounted(() => loadCategories())
  </script>
  
  <style scoped>
  .category-admin {
  max-width: 600px;
  margin: 40px auto;
  padding: 20px;
  background-color: #f8f9fa;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.08);
}

.category-admin h1 {
  text-align: center;
  margin-bottom: 20px;
  font-size: 24px;
  color: #333;
}

form {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}

form input {
  flex: 1;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 16px;
}

form button {
  padding: 10px 16px;
  background-color: #1e88e5;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

form button:hover {
  background-color: #1565c0;
}

ul {
  list-style: none;
  padding: 0;
}

li {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px;
  margin-bottom: 10px;
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 6px;
}

li span {
  flex: 1;
  font-size: 16px;
  color: #333;
}

li button {
  background: none;
  border: none;
  font-size: 18px;
  cursor: pointer;
  padding: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
}

li button:hover {
  transform: scale(1.1);
}

.modal {
  background-color: #eef2f5;
  padding: 15px;
  border-radius: 8px;
  margin-top: 20px;
  border: 1px solid #ccc;
}

.modal h3 {
  margin-bottom: 10px;
}

.modal input {
  padding: 8px;
  font-size: 15px;
  width: 100%;
  margin-bottom: 10px;
  border-radius: 5px;
  border: 1px solid #bbb;
}

.modal button {
  margin-right: 10px;
  padding: 8px 14px;
  font-size: 15px;
  background-color: #42a5f5;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.modal button:last-child {
  background-color: #b0bec5;
}

.error {
  color: #d32f2f;
  margin-top: 10px;
  text-align: center;
  font-weight: bold;
}

.category-item {
  display: flex;
  justify-content: space-between; 
  align-items: center;
  padding: 12px;
  margin-bottom: 10px;
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 6px;
}

.category-name {
  font-size: 16px;
  color: #333;
  flex: 1; 
}

.button-group {
  display: flex;
  gap: 8px;
}
</style>