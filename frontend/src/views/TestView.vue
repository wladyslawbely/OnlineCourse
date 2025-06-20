<template>
  <div class="test" v-if="test">
    <h2>{{ test.title }}</h2>

    <div
      class="question-block"
      v-for="(question, index) in test.questions"
      :key="question.questionId"
    >
      <p class="question-text">{{ question.question }}</p>
      <div class="options">
        <label
          class="option-label"
          v-for="(option, i) in question.options"
          :key="i"
        >
          <input
            type="checkbox"
            :value="option"
            v-model="userAnswers[question.questionId]"
          />
          <span class="option-text">{{ option }}</span>
        </label>
      </div>
    </div>

    <button class="submit-btn" @click="submitTest">
      📊 Завершить тест
    </button>
    <p class="result" v-if="result">
      Ваш результат: <strong>{{ result.score }}</strong> из
      <strong>{{ result.total }}</strong>
    </p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from '../services/axios';

const route = useRoute();
const moduleId = route.params.moduleId;
const test = ref(null);
const userAnswers = ref({});
const result = ref(null);

async function fetchTest() {
  try {
    const response = await axios.get(`/modules/${moduleId}/test`);
    test.value = response.data;

    test.value.questions.forEach(q => {
      userAnswers.value[q.questionId] = [];
    });
  } catch (error) {
    console.error("Ошибка при загрузке теста:", error);
  }
}

async function submitTest() {
  try {
    const submissionData = {
      moduleId: parseInt(moduleId),
      answers: Object.entries(userAnswers.value).map(
        ([questionId, selectedOptions]) => ({
          questionId: parseInt(questionId),
          selectedAnswers: selectedOptions
        })
      )
    };

    const response = await axios.post('/tests/submit', submissionData);
    result.value = response.data;
  } catch (error) {
    console.error("Ошибка отправки теста:", error);
  }
}

onMounted(fetchTest);
</script>

<style scoped>
.test {
  max-width: 700px;
  margin: 40px auto;
  padding: 30px;
  background: #ffffff;
  border-radius: 10px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

h2 {
  text-align: center;
  color: #333;
  font-size: 26px;
  font-weight: 600;
  margin-bottom: 30px;
}

.question-block {
  background: #f6f8fa;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 20px;
  border: 1px solid #e1e4e8;
}

.question-text {
  margin-bottom: 15px;
  font-size: 18px;
  font-weight: 500;
  color: #24292e;
}

.options {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.option-label {
  display: flex;
  align-items: center;
  padding: 8px 12px;
  background: #fff;
  border: 1px solid #d1d5da;
  border-radius: 6px;
  cursor: pointer;
}

.option-label:hover {
  background: #f1f3f5;
  border-color: #c6cbd1;
}

input[type="checkbox"] {
  margin-right: 10px;
  transform: scale(1.2);
}

.option-text {
  font-size: 16px;
  color: #586069;
}

.submit-btn {
  width: 100%;
  padding: 14px;
  font-size: 18px;
  font-weight: 600;
  color: #fff;
  background: linear-gradient(45deg, #4CAF50, #43A047);
  border: none;
  border-radius: 8px;
  cursor: pointer;
  margin-top: 20px;
}

.submit-btn:hover {
  background: linear-gradient(45deg, #43A047, #388E3C);
  transform: translateY(-2px);
}

.result {
  margin-top: 30px;
  text-align: center;
  font-size: 20px;
  font-weight: 500;
  color: #333;
}
</style>
