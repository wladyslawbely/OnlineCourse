import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import CoursesView from '../views/CoursesView.vue'
import CourseDetailsView from '../views/CourseDetailsView.vue'
import LoginView from '../views/LoginView.vue'
import ModulesView from '../views/ModulesView.vue'
import { useAuthStore } from '../stores/authStore'
import Profile from '../views/Profile.vue'
import ManagerPanelView from '../views/ManagerPanelView.vue'
import CourseFormView from '../views/CourseFormView.vue'
import CourseModulesView from '../views/CourseModulesView.vue'
import InstructorCoursesView from '../views/InstructorCoursesView.vue'
import TestView from '../views/TestView.vue'

const routes = [
  { path: '/', component: HomeView },
  { path: '/login', component: LoginView },
  { path: '/courses', component: CoursesView },
  { path: '/courses/:id', component: CourseDetailsView, props: true },
  { path: '/courses/:id/modules', component: ModulesView, props: true },
  { path: '/profile',component: Profile, meta: { requiresAuth: true } },
  { path: '/lessons/:lessonId', component: () => import('../views/LessonDetailsView.vue'), props: true },
  { path: '/admin', component: () => import('../views/AdminPanelView.vue'), meta: { requiresAuth: true, requiresAdmin: true } },
  { path: '/manager',component:ManagerPanelView,meta: { requiresAuth: true, requiresManager: true }  },
  { path: '/manager/courses/create', component: CourseFormView, meta: { requiresAuth: true, requiresManager: true } },
  { path: '/manager/courses/:id/edit', component:CourseFormView, props: true, meta: { requiresAuth: true, requiresManager: true }},
  { path: '/manager/courses/:id/manage', component: CourseModulesView, meta: { requiresAuth: true, requiresManager: true }},  
  { path: '/manager/courses/:id/modules', component: CourseModulesView, props: true, meta: { requiresAuth: true, requiresManager: true }},
  { path: '/instructor/courses',component: InstructorCoursesView ,meta: { requiresAuth: true, requiresInstructor: true }},
  { path: '/categories', name: 'CourseCategories', component: () => import('../views/CourseCategoriesView.vue'), meta: { requiresAuth: true, roles: ['admin', 'instructor','manager'] }},
  { path: '/register', component: () => import('../views/RegisterView.vue'),  meta: { requiresAuth: false }},
  { path: '/modules/:moduleId/test', component: TestView}
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()

  if (authStore.token && !authStore.user) {
    await authStore.getUser();
  }

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
  }
  
  else if (to.meta.roles && !to.meta.roles.includes(authStore.user?.role)) {
    next('/') 
  }
  
  
  else {
    next()
  }
})

export default router
