<template>
  <div class="course-list">
    <input
      type="text"
      v-model="searchQuery"
      placeholder="Поиск по названию курса..."
      class="search-box"
    />
    <h1>Курсы</h1>

    <div v-if="courseStore.loading" class="loading">Загрузка...</div>
    <div v-else-if="courseStore.error" class="error">{{ courseStore.error }}</div>
    <div v-else>
      <div v-if="groupedCourses.length === 0">
        <p class="no-courses">Нет доступных курсов.</p>
      </div>
      <div v-else>
        <div v-for="(courses, categoryName) in groupedCourses" :key="categoryName" class="category-block">
          <h2>{{ categoryName }}</h2>
          <ul>
            <li v-for="course in courses" :key="course.courseId" class="course-item">
              <router-link :to="`/courses/${course.courseId}`">
                {{ course.title }}
              </router-link>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useCourseStore } from '../stores/courseStore';

const courseStore = useCourseStore();
const searchQuery = ref('');

onMounted(async () => {
  await courseStore.fetchCourses();
});

const groupedCourses = computed(() => {
  if (!courseStore.courses || courseStore.courses.length === 0) return {};

  const filtered = courseStore.courses.filter(course =>
    course.title.toLowerCase().includes(searchQuery.value.trim().toLowerCase())
  );

  const groups = {};
  filtered.forEach(course => {
    if (!course.categories || course.categories.length === 0) {
      if (!groups['Без категории']) groups['Без категории'] = [];
      groups['Без категории'].push(course);
    } else {
      course.categories.forEach(cat => {
        if (!groups[cat.name]) groups[cat.name] = [];
        groups[cat.name].push(course);
      });
    }
  });

  return groups;
});
</script>

<style scoped>
.course-list {
  width: 90%;
  max-width: 1400px; 
  margin: 40px auto;
  padding: 40px 20px;
  background: linear-gradient(135deg, #f8f9fa, #ffffff);
  border-radius: 15px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
}


.course-list h1 {
  font-size: 2.8rem;
  font-weight: 800;
  color: #2c3e50;
  text-align: center;
  margin-bottom: 2rem;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.loading, .error, .no-courses {
  font-size: 1.2rem;
  color: #7f8c8d;
  text-align: center;
  margin: 2rem 0;
  font-style: italic;
}

.category-block {
  margin-bottom: 3rem;
  padding: 15px;
  background: #ecf0f1;
  border-radius: 10px;
}

.category-block h2 {
  font-size: 1.8rem;
  font-weight: 700;
  color: #007aff;
  border-bottom: 3px solid #dcdde1;
  padding-bottom: 8px;
  margin-bottom: 15px;
}

ul {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); 
  gap: 24px; 
  padding: 0;
  list-style: none;
}

.course-item {
  background: #ffffff;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  text-align: center;
}

.course-item:hover {
  transform: scale(1.05);
  box-shadow: 0 12px 30px rgba(0, 0, 0, 0.15);
}

.course-item a {
  font-size: 1.3rem;
  font-weight: bold;
  color: #007aff;
  text-decoration: none;
  display: block;
  transition: color 0.3s ease;
}

.course-item a:hover {
  color: #0056a3;
}

.search-box {
  width: 100%;
  max-width: 500px;
  margin: 0 auto 2rem;
  display: block;
  padding: 12px 16px;
  border-radius: 8px;
  border: 1px solid #ccc;
  font-size: 1.1rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
}


@media (max-width: 768px) {
  .course-list {
    padding: 20px;
    margin: 20px;
  }

  .course-list h1 {
    font-size: 2.2rem;
  }

  .category-block h2 {
    font-size: 1.6rem;
  }

  ul {
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  }
}
</style>
