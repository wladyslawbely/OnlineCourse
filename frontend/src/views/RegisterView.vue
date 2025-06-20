<template>
    <div class="register-page">
      <h1>Регистрация</h1>
  
      <form @submit.prevent="handleRegister">
        <div>
          <label>Имя:</label>
          <input v-model="form.firstName" required />
        </div>
  
        <div>
          <label>Фамилия:</label>
          <input v-model="form.lastName" required />
        </div>
  
        <div>
          <label>Email:</label>
          <input type="email" v-model="form.email" required />
        </div>
  
        <div>
          <label>Пароль:</label>
          <input type="password" v-model="form.password" required minlength="6" />
        </div>
  
        <div>
          <label>Подтверждение пароля:</label>
          <input type="password" v-model="form.confirmPassword" required minlength="6" />
        </div>
  
        <button type="submit">Зарегистрироваться</button>
      </form>
  
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      <p v-if="successMessage" class="success">{{ successMessage }}</p>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  import axios from '../services/axios'
  
  const router = useRouter()
  
  const form = ref({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: ''
  })
  
  const errorMessage = ref('')
  const successMessage = ref('')
  
  async function handleRegister() {
    errorMessage.value = ''
    successMessage.value = ''
  
    if (form.value.password !== form.value.confirmPassword) {
      errorMessage.value = 'Пароли не совпадают'
      return
    }
  
    try {
      await axios.post('/users/register', {
        firstName: form.value.firstName,
        lastName: form.value.lastName,
        email: form.value.email,
        password: form.value.password
      })
  
      successMessage.value = 'Регистрация успешна! Перенаправляем на вход...'
      setTimeout(() => router.push('/login'), 1500)
    } catch (err) {
      errorMessage.value = err.response?.data || 'Ошибка регистрации'
    }
  }
  </script>
  
  <style scoped>
  .register-page {
  max-width: 500px;
  margin: 40px auto; 
  padding: 20px;
  background: linear-gradient(135deg, #f9f9f9, #ffffff); 
  border-radius: 10px;
  box-shadow: 0px 8px 20px rgba(0, 0, 0, 0.1); 
}

h1 {
  font-size: 24px;
  color: #333;
  text-align: center;
  margin-bottom: 20px;
  font-weight: bold;
}

form div {
  margin-bottom: 15px; 
}

label {
  display: block;
  margin-bottom: 5px;
  font-size: 14px;
  color: #555; 
}

input {
  width: 100%;
  padding: 10px; 
  box-sizing: border-box;
  border: 1px solid #ccc; 
  border-radius: 5px; 
  transition: border-color 0.3s; 
}

input:focus {
  border-color: #007bff; 
  outline: none; 
}

button {
  width: 100%;
  padding: 12px;
  background: linear-gradient(135deg, #007bff, #0056b3); 
  color: white;
  font-weight: bold;
  border: none;
  border-radius: 6px;
  cursor: pointer;
 transition: background-color 0.5s ease-in-out 0.2s;
}

button:hover {
  background: linear-gradient(135deg, #ffffff, #004080); 
}

button:active {
  transform: scale(0.98); 
}

.error {
  color: #e74c3c;
  text-align: center;
  margin-top: 10px;
}

.success {
  color: #2ecc71; 
  text-align: center;
  margin-top: 10px;
}

@media (max-width: 768px) {
  .register-page {
    padding: 15px;
  }

  h1 {
    font-size: 20px; 
  }
}

</style>  