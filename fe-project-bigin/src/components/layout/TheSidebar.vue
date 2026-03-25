<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { bookService, type BookType } from '../../api/bookService'

const bookTypes = ref<BookType[]>([])

const fetchBookTypes = async () => {
  try {
    const response = await bookService.getTypeBooks()
    if (response.isSuccess) {
      bookTypes.value = response.data
    }
  } catch (error) {
    console.error('Failed to fetch book types:', error)
  }
}

onMounted(() => {
  fetchBookTypes()
})
</script>

<template>
  <aside class="sidebar">
    <h3>📚 Danh Mục Sách</h3>
    <ul class="category-list">
      <li v-for="type in bookTypes" :key="type.typeId">
        <RouterLink :to="{ path: '/books', query: { typeId: type.typeId, typeName: type.name } }">
          {{ type.name }}
        </RouterLink>
      </li>
      <li v-if="bookTypes.length === 0">Đang tải...</li>
    </ul>
  </aside>
</template>

<style scoped>
.sidebar {
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

.sidebar h3 {
  font-size: 1.125rem;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid var(--border);
}

.category-list {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.category-list a {
  display: block;
  padding: 0.5rem;
  color: var(--text-main);
  border-radius: 4px;
  font-weight: 400;
  transition: all 0.2s;
}

.category-list a:hover {
  background-color: #f3f4f6;
  color: var(--primary);
  padding-left: 0.75rem;
}
</style>
