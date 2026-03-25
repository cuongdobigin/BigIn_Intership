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
        <RouterLink to="/cart" class="action-btn">
          🛒 Giỏ hàng
        </RouterLink>
        <div class="user-info">
          <span class="avatar">👤</span>
          <span v-if="auth.isLoggedIn">{{ auth.username }}</span>
          <span v-else>Tài khoản</span>
        </div>
        
        <button v-if="auth.isLoggedIn" @click="handleLogout" class="logout-btn">Thoát</button>
        <RouterLink v-else to="/login" class="login-link">Đăng nhập</RouterLink>
      </div>
    </div>
  </header>
</template>

<style scoped>
.app-header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 70px;
  background-color: #ffffff;
  border-bottom: 1px solid var(--border);
  z-index: 100;
  display: flex;
  align-items: center;
  box-shadow: 0 1px 2px rgba(0,0,0,0.05);
}

.header-container {
  width: 100%;
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 2rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 2rem;
}

.logo a {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--primary);
}

.search-bar {
  flex: 1;
  max-width: 500px;
  display: flex;
}

.search-input {
  flex: 1;
  padding: 0.6rem 1rem;
  border: 1px solid var(--border);
  border-right: none;
  border-radius: var(--radius-md) 0 0 var(--radius-md);
  outline: none;
}
.search-input:focus {
  border-color: var(--primary);
}

.search-btn {
  padding: 0.6rem 1.25rem;
  background-color: var(--primary);
  color: #fff;
  border: 1px solid var(--primary);
  border-radius: 0 var(--radius-md) var(--radius-md) 0;
  cursor: pointer;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.action-btn, .user-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: var(--text-main);
  font-weight: 500;
  cursor: pointer;
}

.logout-btn {
  background: none;
  border: none;
  color: var(--danger);
  font-size: 0.875rem;
  cursor: pointer;
  font-weight: 500;
  padding: 0;
}

.logout-btn:hover {
  text-decoration: underline;
}

.login-link {
  color: var(--primary);
  font-size: 0.875rem;
  font-weight: 600;
}
</style>
