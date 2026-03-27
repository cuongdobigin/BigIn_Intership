<script setup lang="ts">
import { RouterLink } from 'vue-router'

const menuItems = [
  { name: '📊 Bảng điều khiển', path: '/admin' },
  { name: '📁 Quản lý danh mục', path: '/admin/categories' },
  { name: '📚 Quản lý sách', path: '/admin/books' },
  { name: '🎫 Quản lý voucher', path: '/admin/vouchers' },
  { name: '👥 Quản lý tài khoản', path: '/admin/accounts' },
]
</script>

<template>
  <aside class="admin-sidebar">
    <div class="sidebar-header">
      <h3>⚙️ Quản lý</h3>
    </div>
    <ul class="admin-menu">
      <li v-for="item in menuItems" :key="item.path">
        <RouterLink :to="item.path" exact-active-class="active">
          {{ item.name }}
        </RouterLink>
      </li>
    </ul>
  </aside>
</template>

<style scoped>
.admin-sidebar {
  width: 250px;
  flex-shrink: 0;
  background: #ffffff;
  border: 1px solid var(--border);
  border-radius: var(--radius-md);
  padding: 1.5rem;
  height: fit-content;
  position: sticky;
  top: 90px;
}

.sidebar-header h3 {
  font-size: 1.125rem;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid var(--primary);
  color: var(--text-main);
}

.admin-menu {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.admin-menu a {
  display: block;
  padding: 0.75rem 1rem;
  color: var(--text-main);
  border-radius: var(--radius-sm);
  font-weight: 500;
  transition: all 0.2s;
  text-decoration: none;
}

.admin-menu a:hover {
  background-color: #f3f4f6;
  color: var(--primary);
  transform: translateX(5px);
}

.admin-menu a.active {
  background-color: var(--primary);
  color: white;
}
</style>
