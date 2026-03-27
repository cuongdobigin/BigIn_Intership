<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { bookService, type Book } from '../api/bookService'
import BookCard from '../components/BookCard.vue'
import BaseLoader from '../components/BaseLoader.vue'
import BasePagination from '../components/BasePagination.vue'
import BaseError from '../components/BaseError.vue'

import AppSectionHeader from '../components/AppSectionHeader.vue'

const route = useRoute()
const router = useRouter()

const books = ref<Book[]>([])
const loading = ref(false)
const errorMsg = ref('')
const currentPage = ref(Number(route.query.page) || 1)
const pageSize = 9
const totalPages = ref(1)
const hasNext = ref(false)
const hasPrevious = ref(false)

const pageTitle = computed(() => {
  return route.query.typeName ? `Sách: ${route.query.typeName}` : 'Tất Cả Sách'
})

const fetchBooks = async () => {
  loading.value = true
  errorMsg.value = ''
  const typeId = route.query.typeId ? Number(route.query.typeId) : undefined
  
  try {
    const response = await bookService.getBooks(currentPage.value, pageSize, typeId)
    if (response.isSuccess) {
      if (response.data && response.data.data) {
        books.value = response.data.data
        totalPages.value = response.data.totalPages || 1
        hasNext.value = response.data.hasNext || false
        hasPrevious.value = response.data.hasPrevious || false
      } else {
        errorMsg.value = 'Dữ liệu trả về không đúng cấu trúc.'
      }
    } else {
      errorMsg.value = response.message || 'Lấy danh sách sách thất bại.'
    }
  } catch (err: unknown) {
    if (err instanceof Error) {
      errorMsg.value = err.message
    } else {
      errorMsg.value = 'Có lỗi xảy ra khi kết nối máy chủ.'
    }
  } finally {
    loading.value = false
  }
}

watch(() => route.query.typeId, () => {
  currentPage.value = 1
  fetchBooks()
})

watch(() => route.query.page, (newVal) => {
  currentPage.value = Number(newVal) || 1
  fetchBooks()
})

onMounted(() => {
  fetchBooks()
})

const handlePageChange = (page: number) => {
  router.push({
    query: {
      ...route.query,
      page
    }
  })
  window.scrollTo({ top: 0, behavior: 'smooth' })
}
</script>

<template>
  <div class="books-view">
    <AppSectionHeader :title="pageTitle" />

    <BaseError v-if="errorMsg" :message="errorMsg" @retry="fetchBooks" />

    <template v-else>
      <div v-if="loading" class="loader-container">
        <BaseLoader message="Đang tải danh sách sách..." />
      </div>

      <div v-else-if="books.length === 0" class="no-results">
        <p>Không có sách nào trong chuyên mục này.</p>
      </div>

      <div v-else class="books-grid">
        <BookCard v-for="book in books" :key="book.id" :book="book" />
      </div>

      <div class="pagination-container" v-if="totalPages > 1">
        <BasePagination 
          :current-page="currentPage" 
          :total-pages="totalPages"
          @change="handlePageChange"
        />
      </div>
    </template>
  </div>
</template>

<style scoped>
.books-view {
  width: 100%;
}

.books-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 2rem;
  margin-bottom: 3rem;
}

.loader-container, .no-results {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 200px;
}

.pagination-container {
  display: flex;
  justify-content: center;
  margin-top: 1rem;
}

@media (max-width: 640px) {
  .books-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
}
</style>
