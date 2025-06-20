<template>
  <div class="admin-panel">
    <h1>Админ-панель</h1>

    <h2>Пользователи</h2>
    <table>
      <thead>
        <tr>
          <th @click="sortBy('firstName')" class="sortable">Имя</th>
          <th @click="sortBy('email')" class="sortable">Email</th>
          <th @click="sortBy('role')" class="sortable">Роль</th>
          <th>Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.userId">
          <td>{{ user.firstName }} {{ user.lastName }}</td>
          <td>{{ user.email }}</td>
          <td>
            <select v-model="user.role" @change="updateUserRole(user)">
              <option value="student">Студент</option>
              <option value="instructor">Инструктор</option>
              <option value="manager">Менеджер</option>
              <option value="admin">Админ</option>
            </select>
          </td>
          <td>
            <button @click="deleteUser(user.userId)">Удалить</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from '../services/axios'

const users = ref([])
const errorMessage = ref('')
const sortColumn = ref('') 
const sortDirection = ref('asc')

async function fetchUsers() {
  try {
    const response = await axios.get('/users')
    users.value = response.data
  } catch (error) {
    console.error('Ошибка при загрузке пользователей', error)
    errorMessage.value = 'Не удалось загрузить пользователей'
  }
}

function sortBy(column) {
  if (sortColumn.value === column) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
  } else {
    sortColumn.value = column
    sortDirection.value = 'asc'
  }

  users.value.sort((a, b) => {
    const valA = a[column]?.toLowerCase?.() || a[column]
    const valB = b[column]?.toLowerCase?.() || b[column]
    if (valA < valB) return sortDirection.value === 'asc' ? -1 : 1
    if (valA > valB) return sortDirection.value === 'asc' ? 1 : -1
    return 0
  })
}

async function deleteUser(id) {
  if (confirm('Удалить пользователя?')) {
    try {
      await axios.delete(`/users/${id}`)
      users.value = users.value.filter(u => u.userId !== id)
    } catch (error) {
      console.error('Ошибка при удалении пользователя', error)
      errorMessage.value = 'Не удалось удалить пользователя'
    }
  }
}

async function updateUserRole(user) {
  try {
    await axios.put(`/users/${user.userId}/role`, { role: user.role });
    alert('Роль успешно обновлена');
  } catch (error) {
    console.error('Ошибка при обновлении роли', error)
    errorMessage.value = 'Не удалось обновить роль пользователя'
  }
}

onMounted(() => {
  fetchUsers()
})
</script>

<style scoped>
.admin-panel {
  max-width: 900px;
  margin: 40px auto;
  padding: 24px;
  background-color: #fdfdfd;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
}

.admin-panel h1 {
  text-align: center;
  margin-bottom: 20px;
  font-size: 26px;
  color: #333;
}

.admin-panel h2 {
  font-size: 20px;
  margin-bottom: 12px;
  color: #444;
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

th {
  padding: 12px;
  text-align: left;
  font-weight: 600;
  font-size: 15px;
  color: #555;
  border-bottom: 1px solid #ddd;
  user-select: none;
  transition: color 0.2s ease;
}

th.sortable {
  cursor: pointer;
}

th.sortable:hover {
  color: #1e88e5;
  text-decoration: underline;
}


td {
  padding: 10px;
  border-bottom: 1px solid #eee;
  font-size: 15px;
  color: #333;
}

select {
  padding: 6px 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  background-color: #fff;
}

button {
  padding: 6px 12px;
  background-color: #dc3545;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

button:hover {
  background-color: #c82333;
}

.error {
  color: #d32f2f;
  margin-top: 20px;
  font-weight: bold;
  text-align: center;
}

</style>
