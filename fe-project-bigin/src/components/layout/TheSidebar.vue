<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { bookService, type BookType } from '../../api/bookService'

const categories = ref<BookType[]>([])
const isLoading = ref(true)

const fetchCategories = async () => {
  try {
    isLoading.value = true
    const response = await bookService.getTypeBooks()
    if (response.isSuccess) {
      categories.value = response.data
    }
  } catch (error) {
    console.error('Failed to fetch categories:', error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchCategories()
})
</script>

<template>
  <aside class="sidebar card">
    <div class="sidebar-header">
      <h2 class="sidebar-title">Danh mục sách</h2>
    </div>
    
    <div v-if="isLoading" class="sidebar-loading">
      Đang tải danh mục...
    </div>
    
    <nav v-else class="category-nav">
      <RouterLink 
        to="/books" 
        class="category-item"
        active-class="active"
      >
        Tất cả sách
      </RouterLink>
      
      <RouterLink 
        v-for="cat in categories" 
        :key="cat.typeId"
        :to="{ path: '/books', query: { typeId: cat.typeId, typeName: cat.name }}"
        class="category-item"
        active-class="active"
      >
        {{ cat.name }}
      </RouterLink>
    </nav>
  </aside>
</template>

<style scoped>
.sidebar {
  width: 250px;
  background: white;
  border-radius: var(--radius-md);
  padding: 1.5rem;
  border: 1px solid var(--border);
  height: fit-content;
}

.sidebar-header {
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid var(--primary-light);
}

.sidebar-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: var(--text-main);
  margin: 0;
}

.category-nav {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  max-height: 450px; /* Chiều cao tối đa trước khi xuất hiện thanh cuộn */
  overflow-y: auto;
  padding-right: 8px; /* Khoảng trống cho thanh cuộn */
}

/* Tùy chỉnh thanh cuộn cho đẹp hơn */
.category-nav::-webkit-scrollbar {
  width: 6px;
}

.category-nav::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

.category-nav::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 10px;
}

.category-nav::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}

.category-item {
  padding: 0.75rem 1rem;
  border-radius: 8px;
  text-decoration: none;
  color: var(--text-muted);
  font-weight: 500;
  transition: all 0.2s;
  font-size: 0.95rem;
}

.category-item:hover {
  background: var(--bg-body);
  color: var(--primary);
  padding-left: 1.25rem;
}

.category-item.active {
  background: var(--primary-light);
  color: var(--primary);
  font-weight: 600;
}

.sidebar-loading {
  padding: 1rem;
  text-align: center;
  color: var(--text-muted);
  font-size: 0.9rem;
}

@media (max-width: 768px) {
  .sidebar {
    width: 100%;
    margin-bottom: 2rem;
  }
}
</style>
