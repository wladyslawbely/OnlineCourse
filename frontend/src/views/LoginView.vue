<template>
  <div class="login">
    <h2>Вход</h2>
    <form @submit.prevent="login">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Пароль" required />
      <button type="submit">Войти</button>
    </form>
    <p v-if="errorMessage">{{ errorMessage }}</p>
  </div>
</template>


<script setup>
import { ref } from "vue";
import { useAuthStore } from "../stores/authStore";
import { useRouter } from "vue-router";

const authStore = useAuthStore();
const router = useRouter();

const email = ref("");
const password = ref("");
const errorMessage = ref("");

const login = async () => {
  try {
    await authStore.login(email.value, password.value);
    router.push("/profile"); 
  } catch (error) {
    errorMessage.value = error.message;
  }
};
</script>

<style>
.login {
  max-width: 400px;
  margin: 60px auto; 
  padding: 20px;
  background: linear-gradient(135deg, #f9f9f9, #ffffff); 
  border-radius: 10px; 
  box-shadow: 0px 8px 20px rgba(0, 0, 0, 0.1); 
}

h2 {
  font-size: 22px; 
  color: #333;
  text-align: center; 
  margin-bottom: 20px; 
}

form {
  display: flex;
  flex-direction: column; 
  gap: 15px; 
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
}

button:hover {
  background: linear-gradient(135deg, #0056b3, #004080); 
}

button:active {
  transform: scale(0.98); 
}

@media (max-width: 768px) {
  .login {
    padding: 15px; 
  }

  h2 {
    font-size: 18px; 
  }
}
</style>