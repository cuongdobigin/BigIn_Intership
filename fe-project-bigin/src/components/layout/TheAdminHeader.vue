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
  <header class="admin-header glass">
    <div class="header-container">
      <div class="header-left">
        <RouterLink to="/" class="logo">📚 BigInAdmin</RouterLink>
        <span class="divider">|</span>
        <span class="panel-title">Hệ thống quản trị</span>
      </div>

      <div class="header-right">
        <RouterLink to="/" class="store-link">
          <svg viewBox="0 0 24 24" width="18" height="18" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
            <polyline points="9 22 9 12 15 12 15 22"></polyline>
          </svg>
          Xem cửa hàng
        </RouterLink>

        <div class="user-profile">
          <div class="avatar">Admin</div>
          <div class="user-info">
            <span class="username">{{ auth.username }}</span>
            <span class="role">Quản trị viên</span>
          </div>
        </div>

        <button @click="handleLogout" class="btn-logout" title="Đăng xuất">
          <svg viewBox="0 0 24 24" width="20" height="20" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
            <polyline points="16 17 21 12 16 7"></polyline>
            <line x1="21" y1="12" x2="9" y2="12"></line>
          </svg>
        </button>
      </div>
    </div>
  </header>
</template>

<style scoped>
.admin-header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 70px;
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid rgba(226, 232, 240, 0.8);
  z-index: 1000;
  display: flex;
  align-items: center;
}

.header-container {
  width: 100%;
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 2rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.logo {
  font-size: 1.4rem;
  font-weight: 800;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
  text-decoration: none;
}

.divider {
  color: #e2e8f0;
  font-weight: 200;
}

.panel-title {
  font-weight: 600;
  color: #64748b;
  font-size: 0.95rem;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 2rem;
}

.store-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #64748b;
  text-decoration: none;
  font-size: 0.9rem;
  font-weight: 500;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  transition: all 0.2s;
}

.store-link:hover {
  background: #f1f5f9;
  color: #1e293b;
}

.user-profile {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding-left: 1rem;
  border-left: 1px solid #e2e8f0;
}

.avatar {
  width: 36px;
  height: 36px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 0.7rem;
  font-weight: 700;
}

.user-info {
  display: flex;
  flex-direction: column;
}

.username {
  font-weight: 700;
  color: #1e293b;
  font-size: 0.9rem;
}

.role {
  font-size: 0.75rem;
  color: #94a3b8;
}

.btn-logout {
  background: none;
  border: none;
  color: #94a3b8;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 8px;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-logout:hover {
  background: #fef2f2;
  color: #ef4444;
}
</style>
