<template>
    <div class="category-list">
      <h3>Категории курсов</h3>
      <ul>
        <li v-for="category in categories" :key="category.categoryId">
          {{ category.name }}
        </li>
      </ul>
      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import axios from '../services/axios'
  
  const categories = ref([])
  const error = ref('')
  
  async function fetchCategories() {
    try {
      const res = await axios.get('/course-categories')
      categories.value = res.data
    } catch (err) {
      error.value = 'Не удалось загрузить категории.'
      console.error(err)
    }
  }
  
  onMounted(fetchCategories)
  </script>
  
  <style scoped>
.category-list {
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #fafafa;
  max-width: 500px;
  margin: 0 auto;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.category-list h3 {
  margin-bottom: 15px;
  font-size: 20px;
  color: #333;
  text-align: center;
}

.category-list ul {
  list-style: none;
  padding: 0;
}

.category-list li {
  padding: 10px 14px;
  margin-bottom: 8px;
  background-color: #fff;
  border: 1px solid #e1e1e1;
  border-radius: 6px;
  transition: background-color 0.2s ease;
  cursor: pointer;
}

.category-list li:hover {
  background-color: #f0f8ff;
}

.error {
  margin-top: 10px;
  color: #c0392b;
  text-align: center;
  font-weight: bold;
}
</style>
  