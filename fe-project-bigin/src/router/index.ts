import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import BooksView from '../views/BooksView.vue'
import { useAuthStore } from '../stores/auth'
import { useToastStore } from '../stores/toast'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    // 1. Admin Routes (Most specific prefix)
    {
      path: '/admin',
      component: () => import('../layouts/AdminLayout.vue'),
      meta: { requiresAuth: true, requiresAdmin: true },
      children: [
        {
          path: '',
          name: 'admin-dashboard',
          component: () => import('../views/admin/AdminDashboardView.vue')
        },
        {
          path: 'categories',
          name: 'admin-categories',
          component: () => import('../views/admin/CategoryManagementView.vue')
        },
        {
          path: 'books',
          name: 'admin-books',
          component: () => import('../views/admin/BookManagementView.vue')
        },
        {
          path: 'vouchers',
          name: 'admin-vouchers',
          component: () => import('../views/admin/VoucherManagementView.vue')
        },
        {
          path: 'accounts',
          name: 'admin-accounts',
          component: () => import('../views/admin/AccountManagementView.vue')
        }
      ]
    },
    // 2. Client Routes (Main Layout) - Move this UP so it catches "/" home
    {
      path: '/',
      component: () => import('../layouts/MainLayout.vue'),
      children: [
        {
          path: '',
          name: 'home',
          component: HomeView
        },
        {
          path: 'books',
          name: 'books',
          component: BooksView,
        },
        {
          path: 'books/:id',
          name: 'book-detail',
          component: () => import('../views/BookDetailView.vue'),
          props: true
        },
        {
          path: 'cart',
          name: 'cart',
          component: () => import('../views/CartView.vue'),
          meta: { requiresAuth: true }
        },
      ]
    },
    // 3. Auth Routes (Auth Layout)
    {
      path: '/',
      component: () => import('../layouts/AuthLayout.vue'),
      children: [
        {
          path: 'login',
          name: 'login',
          component: () => import('../views/LoginView.vue'),
        },
        {
          path: 'register',
          name: 'register',
          component: () => import('../views/RegisterView.vue'),
        },
        {
          path: 'change-password',
          name: 'change-password',
          component: () => import('../views/ChangePasswordView.vue'),
        },
        {
          path: 'profile-setup',
          name: 'profile-setup',
          component: () => import('../views/ProfileSetupView.vue'),
          meta: { requiresAuth: true }
        },
      ]
    }
  ],
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  const toast = useToastStore()

  const isLoggedIn = auth.isLoggedIn
  const isAdmin = auth.isAdmin

  // Yêu cầu Đăng nhập chung
  if (to.meta.requiresAuth && !isLoggedIn) {
    toast.show('Vui lòng đăng nhập để tiếp tục.', 'warning')
    return next('/login')
  }

  // Chặn User vào Admin
  if (to.meta.requiresAdmin && !isAdmin) {
    toast.show('Bạn không có quyền truy cập khu vực Quản trị.', 'error')
    return next('/')
  }

  next()
})

export default router
