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
    const error = err as { response?: { data?: { message?: string } } }
    errorMsg.value = error?.response?.data?.message || 'Có lỗi xảy ra khi kết nối đến máy chủ.'
  } finally {
    loading.value = false
  }
}

const changePage = (page: number) => {
  router.push({
    query: { ...route.query, page: page.toString() }
  })
}

onMounted(() => fetchBooks())
watch(() => route.query, () => {
  currentPage.value = Number(route.query.page) || 1
  fetchBooks()
}, { deep: true })
</script>

<template>
  <div class="books-view">
    <AppSectionHeader :title="pageTitle" />

    <BaseLoader v-if="loading" />

    <BaseError v-else-if="errorMsg" :message="errorMsg" />

    <template v-else>
      <div v-if="books.length > 0" class="book-grid">
        <BookCard v-for="book in books" :key="book.id" :book="book" />
      </div>

      <div v-else class="no-books">
        Không tìm thấy cuốn sách nào.
      </div>

      <BasePagination 
        :currentPage="currentPage"
        :totalPages="totalPages"
        :hasNext="hasNext"
        :hasPrevious="hasPrevious"
        @change="changePage"
      />
    </template>
  </div>
</template>

<style scoped>
.books-view {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  padding-bottom: 3rem;
}

.section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 2px solid var(--primary);
  padding-bottom: 0.5rem;
}

.book-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 2rem;
}

.no-books {
  text-align: center;
  padding: 4rem;
  color: var(--text-muted);
  font-size: 1.125rem;
}
</style>
