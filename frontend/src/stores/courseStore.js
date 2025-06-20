    import { defineStore } from 'pinia';
    import axios from 'axios';

    export const useCourseStore = defineStore('course', {
        state: () => ({
            courses: [],
            loading: false,
            error: null,
        }),

        actions: {
            async fetchCourses() {
                this.loading = true;
                this.error = null;
                try {
                    const response = await axios.get('https://localhost:7054/api/courses');
                    this.courses = response.data;
                } catch (err) {
                    this.error = 'Ошибка загрузки курсов';
                } finally {
                    this.loading = false;
                }
            }
        }
    });
