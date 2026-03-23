import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import BooksView from '../views/BooksView.vue'
import { useAuthStore } from '../stores/auth'
import { useToastStore } from '../stores/toast'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/books',
      name: 'books',
      component: BooksView,
    },
    {
      path: '/cart',
      name: 'cart',
      component: () => import('../views/CartView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue'),
    },
    {
      path: '/change-password',
      name: 'change-password',
      component: () => import('../views/ChangePasswordView.vue'),
    },
    {
      path: '/profile-setup',
      name: 'profile-setup',
      component: () => import('../views/ProfileSetupView.vue'),
      meta: { requiresAuth: true }
    },
  ],
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  const toast = useToastStore()

  // Chặn Admin truy cập các trang user
  const isAdmin = auth.role?.toLowerCase() === 'admin'

  if (isAdmin && to.path !== '/login') {
    auth.logout()
    toast.show('Tài khoản Admin không có quyền truy cập khu vực này.', 'error')
    return next('/login')
  }

  if (to.meta.requiresAuth && !auth.isLoggedIn) {
    toast.show('Vui lòng đăng nhập để tiếp tục.', 'warning')
    return next('/login')
  }

  next()
})

export default router
