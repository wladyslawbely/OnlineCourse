<template>
  <div class="lesson-details">
    <h1>{{ lesson.title }}</h1>

    <p v-html="lesson.content"></p>

    <div v-if="lesson.videoUrl" class="video-wrapper">
      <iframe
        :src="formatVideoUrl(lesson.videoUrl)"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
        allowfullscreen
      ></iframe>
    </div>


    <div v-if="lesson.files && lesson.files.length">
      <h2>Файлы, прикреплённые к уроку:</h2>
      <ul>
        <li v-for="file in lesson.files" :key="file.fileId">
          <a :href="`https://localhost:7054${file.fileUrl}`" target="_blank">
            {{ file.fileName }}
          </a>
        </li>
      </ul>
    </div>

    <div v-if="assignments.length">
      <h2>Задания</h2>
      <ul>
        <li v-for="assignment in assignments" :key="assignment.assignmentId">
          <strong>{{ assignment.title }}</strong>
          <p>{{ assignment.description }}</p>
          <p v-if="assignment.dueDate">Срок сдачи: {{ formatDate(assignment.dueDate) }}</p>

          <div v-if="submissions[assignment.assignmentId]">
            <p>✅ Отправлено</p>
            <p>Оценка: {{ submissions[assignment.assignmentId].grade ?? 'Ожидает проверки' }}</p>
            <p>Комментарий: {{ submissions[assignment.assignmentId].feedback || '—' }}</p>
            <p>Ответ: {{ submissions[assignment.assignmentId].content }}</p>

            <button @click="startEditing(submissions[assignment.assignmentId])">✏️ Редактировать</button>
            <button @click="deleteSubmission(submissions[assignment.assignmentId].submissionId)">🗑️ Удалить</button>

            <div
              v-if="editingSubmission?.submissionId === submissions[assignment.assignmentId].submissionId"
              class="edit-form"
            >
              <textarea v-model="editingSubmission.content" placeholder="Измените ответ..."></textarea>
              <button @click="saveEditedSubmission">💾 Сохранить</button>
              <button @click="cancelEditing">❌ Отмена</button>
            </div>
          </div>

          <div v-else>
            <form @submit.prevent="submitAssignment(assignment.assignmentId)">
              <textarea
                v-model="submissionText[assignment.assignmentId]"
                placeholder="Ваш ответ..."
                @input="autoResize($event)"
              ></textarea>
              <button type="submit">Сдать</button>
            </form>
          </div>
        </li>
      </ul>
    </div>


    <button v-if="canCompleteLesson" @click="completeLesson" class="btn-done">
      Завершить урок
    </button>
    <p v-else class="hint">
      Завершение станет доступно после проверки всех заданий преподавателем.
    </p>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import axios from '../services/axios';
import { useAuthStore } from '../stores/authStore';

const authStore = useAuthStore();
const route = useRoute();
const lesson = ref({});
const errorMessage = ref('');
const assignments = ref([]);
const submissions = ref({});
const submissionText = ref({});
const editingSubmission = ref(null)

function startEditing(submission) {
  editingSubmission.value = {
    submissionId: submission.submissionId,
    content: submission.content
  }
}

function cancelEditing() {
  editingSubmission.value = null
}

async function saveEditedSubmission() {
  try {
    await axios.put(`/submission/${editingSubmission.value.submissionId}`, {
      ...submissions.value[editingSubmission.value.submissionId],
      content: editingSubmission.value.content
    })
    alert("Ответ обновлён!")
    editingSubmission.value = null
    await fetchSubmissions()
  } catch (err) {
    console.error("Ошибка сохранения:", err)
  }
}

async function fetchSubmissions() {
  try {
    const res = await axios.get(`/submission/user/${authStore.user.userId}`);
    for (const sub of res.data) {
      submissions.value[sub.assignmentId] = sub;
    }
  } catch (err) {
    console.error("Ошибка загрузки отправок:", err);
  }
}

function autoResize(e) {
  e.target.style.height = 'auto'
  e.target.style.height = e.target.scrollHeight + 'px'
}

async function submitAssignment(assignmentId) {
  try {
    const content = submissionText.value[assignmentId];
    if (!content) return alert("Введите ответ!");

    const payload = {
      assignmentId,
      userId: authStore.user.userId,
      content: content
    };

    const existing = submissions.value[assignmentId];

    if (existing) {
      await axios.put(`/submission/${existing.submissionId}`, payload, {
        headers: { "Content-Type": "application/json" }
      });
      alert("Отправка обновлена!");
    } else {
      await axios.post("/submission", payload, {
        headers: { "Content-Type": "application/json" }
      });
      alert("Задание отправлено!");
    }

    await fetchSubmissions();
  } catch (err) {
    console.error("Ошибка при отправке:", err);
  }
}

async function editSubmission(submission) {
  const newText = prompt("Введите новое содержимое:", submission.content);
  if (!newText) return;

  try {
    const payload = {
      ...submission,
      content: newText
    };

    await axios.put(`/submission/${submission.submissionId}`, payload, {
      headers: { "Content-Type": "application/json" }
    });
    alert("Отправка обновлена");
    fetchSubmissions();
  } catch (error) {
    console.error("Ошибка редактирования:", error);
  }
}

async function deleteSubmission(submissionId) {
  if (!confirm("Удалить отправку?")) return;
  try {
    await axios.delete(`/submission/${submissionId}`);
    alert("Удалено!");
    fetchSubmissions();
  } catch (error) {
    console.error("Ошибка удаления:", error);
  }
}

async function fetchAssignments() {
  try {
    const res = await axios.get(`/assignments/lesson/${route.params.lessonId}`);
    assignments.value = res.data;
  } catch (error) {
    console.error("Ошибка загрузки заданий:", error);
  }
}

function formatDate(dateStr) {
  return new Date(dateStr).toLocaleDateString("ru-RU");
}

async function completeLesson() {
  try {
    await axios.post("/lesson-progress", {
      lessonId: route.params.lessonId,
      userId: authStore.user.userId
    });
    alert("Прогресс сохранён!");
  } catch (error) {
    console.error("Ошибка при отправке прогресса:", error);
  }
}

async function fetchLesson() {
  try {
    const response = await axios.get(`/lessons/${route.params.lessonId}`);
    lesson.value = response.data;
    const fileResponse = await axios.get(`/files/lesson/${route.params.lessonId}`);
    lesson.value.files = fileResponse.data;
  } catch (error) {
    errorMessage.value = "Не удалось загрузить урок.";
    console.error(error);
  }
}

function formatVideoUrl(url) {
  try {
    const parsed = new URL(url);
    if (parsed.hostname.includes("youtube.com") && parsed.searchParams.get("v")) {
      return `https://www.youtube.com/embed/${parsed.searchParams.get("v")}`;
    }
    if (parsed.hostname.includes("youtu.be")) {
      return `https://www.youtube.com/embed/${parsed.pathname.slice(1)}`;
    }
    return url;
  } catch {
    return url;
  }
}

const canCompleteLesson = computed(() => {
  return assignments.value.length > 0 &&
    assignments.value.every(a => {
      const submission = submissions.value[a.assignmentId];
      return submission && submission.grade !== null;
    });
});

onMounted(() => {
  fetchLesson();
  fetchAssignments();
  fetchSubmissions();
});
</script>

<style scoped>
.lesson-details {
  max-width: 800px;
  margin: 40px auto;
  padding: 24px;
  background: #fdfdfd;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.lesson-details h1 {
  font-size: 24px;
  font-weight: bold;
  margin-bottom: 20px;
  text-align: center;
  color: #333;
}

.lesson-details h2 {
  font-size: 18px;
  margin: 30px 0 12px;
  border-bottom: 1px solid #e0e0e0;
  padding-bottom: 6px;
  color: #444;
  letter-spacing: 0.4px;
}

.video-wrapper {
  width: 100%;
  position: relative;
  padding-bottom: 56.25%;
  height: 0;
  overflow: hidden;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  margin-top: 20px;
}

.video-wrapper iframe {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border: none;
}

ul {
  list-style: none;
  padding-left: 0;
}

li {
  margin-bottom: 12px;
}

li a {
  text-decoration: none;
  color: #1e88e5;
}

li a:hover {
  text-decoration: underline;
}

.assignment-wrapper {
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 16px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.03);
}

.assignment-wrapper p {
  margin: 6px 0;
  font-size: 15px;
  color: #333;
}

textarea {
  background-color: #fdfdfd;
  border: 1px solid #ccc;
  border-radius: 10px;
  padding: 12px 14px;
  font-size: 15px;
  line-height: 1.4;
  box-shadow: inset 0 1px 3px rgba(0,0,0,0.05);
  transition: border 0.2s ease;
}

textarea:focus {
  border-color: #1e88e5;
  outline: none;
  background-color: #fff;
}


form button {
  margin-top: 8px;
  padding: 8px 14px;
  background-color: #1e88e5;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
}

form button:hover {
  background-color: #1565c0;
}

.btn-done {
  background-color: #28a745;
  color: white;
  padding: 10px 20px;
  margin-top: 24px;
  border: none;
  border-radius: 6px;
  font-size: 15px;
  cursor: pointer;
}

.btn-done:hover {
  background-color: #218838;
}

.error {
  background-color: #fdecea;
  color: #d32f2f;
  padding: 10px 14px;
  border-radius: 6px;
  margin-top: 20px;
  text-align: center;
  font-weight: bold;
}

.hint {
  color: #777;
  font-style: italic;
  margin-top: 16px;
  text-align: center;
}

button {
  margin-right: 6px;
  padding: 6px 12px;
  border-radius: 5px;
  font-size: 14px;
  border: none;
  background-color: #f0f0f0;
  cursor: pointer;
}

button:hover {
  background-color: #e0e0e0;
}

.edit-form {
  background-color: #f1f5f9;
  padding: 14px;
  margin-top: 10px;
  border: 1px solid #ccc;
  border-radius: 8px;
}

.edit-form textarea {
  width: 100%;
  min-height: 80px;
  padding: 10px;
  font-size: 15px;
  border: 1px solid #bbb;
  border-radius: 6px;
  margin-bottom: 10px;
  resize: vertical;
}


</style>
