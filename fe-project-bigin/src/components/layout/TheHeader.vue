<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'

const router = useRouter()
const auth = useAuthStore()

const handleLogout = () => {
  auth.logout()
  router.push('/login')
}
</script>

<template>
  <header class="app-header">
    <div class="header-container">
      <div class="logo">
        <RouterLink to="/">📚 BigInBook</RouterLink>
      </div>
      
      <div class="search-bar">
        <input type="text" placeholder="Tìm kiếm sách, tác giả..." class="search-input" />
        <button class="search-btn">Tìm</button>
      </div>

      <div class="header-actions">
        <template v-if="auth.isLoggedIn">
          <RouterLink to="/profile" class="user-info">
            👋 {{ auth.username || 'User' }}
          </RouterLink>
          <RouterLink to="/cart" class="cart-link">🛒 Giỏ hàng</RouterLink>
          <button @click="handleLogout" class="logout-btn">Đăng xuất</button>
        </template>
        <template v-else>
          <RouterLink to="/login" class="login-link">Đăng nhập</RouterLink>
          <RouterLink to="/register" class="register-btn">Đăng ký</RouterLink>
        </template>
      </div>
    </div>
  </header>
</template>

<style scoped>
.app-header {
  background: white;
  height: 70px;
  border-bottom: 1px solid var(--border);
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 1000;
  display: flex;
  align-items: center;
}

.header-container {
  max-width: 1280px;
  width: 100%;
  margin: 0 auto;
  padding: 0 2rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.logo a {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--primary);
  text-decoration: none;
}

.search-bar {
  flex: 1;
  max-width: 500px;
  margin: 0 2rem;
  display: flex;
  background: var(--bg-body);
  border-radius: 8px;
  padding: 4px;
}

.search-input {
  flex: 1;
  border: none;
  background: transparent;
  padding: 0.6rem 1rem;
  font-size: 0.9rem;
  outline: none;
}

.search-btn {
  background: var(--primary);
  color: white;
  border: none;
  padding: 0.6rem 1.2rem;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.login-link {
  color: var(--text-main);
  text-decoration: none;
  font-weight: 500;
}

.register-btn {
  background: var(--primary);
  color: white;
  padding: 0.6rem 1.2rem;
  border-radius: 8px;
  text-decoration: none;
  font-weight: 600;
}

.user-info {
  text-decoration: none;
  color: var(--text-main);
  font-weight: 600;
}

.cart-link {
  text-decoration: none;
  color: var(--text-main);
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.3rem;
}

.cart-link:hover {
  color: var(--primary);
}

.logout-btn {
  background: none;
  border: 1px solid var(--border);
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
}

@media (max-width: 768px) {
  .search-bar {
    display: none;
  }
}
</style>
