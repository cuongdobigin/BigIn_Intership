<script setup lang="ts">
defineProps<{
  currentPage: number
  totalPages: number
  hasNext: boolean
  hasPrevious: boolean
}>()

const emit = defineEmits<{
  (e: 'change', page: number): void
}>()

const changePage = (page: number) => {
  emit('change', page)
}
</script>

<template>
  <div v-if="totalPages > 1" class="pagination">
    <button 
      class="page-btn" 
      :disabled="!hasPrevious" 
      @click="changePage(currentPage - 1)"
    >
      Trước
    </button>
    
    <div class="page-numbers">
      <button 
        v-for="p in totalPages" 
        :key="p" 
        class="page-num" 
        :class="{ active: p === currentPage }"
        @click="changePage(p)"
      >
        {{ p }}
      </button>
    </div>

    <button 
      class="page-btn" 
      :disabled="!hasNext" 
      @click="changePage(currentPage + 1)"
    >
      Sau
    </button>
  </div>
</template>

<style scoped>
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1.5rem;
  margin-top: 2rem;
}

.page-numbers {
  display: flex;
  gap: 0.5rem;
}

.page-num {
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid var(--border);
  border-radius: 50%;
  background: white;
  cursor: pointer;
  transition: all 0.2s;
  font-weight: 500;
}

.page-num.active {
  background-color: var(--primary);
  color: white;
  border-color: var(--primary);
}

.page-num:hover:not(.active) {
  border-color: var(--primary);
  color: var(--primary);
}

.page-btn {
  padding: 0.5rem 1.25rem;
  border: 1px solid var(--border);
  background: white;
  border-radius: var(--radius-md);
  cursor: pointer;
  transition: all 0.2s;
}

.page-btn:hover:not(:disabled) {
  border-color: var(--primary);
  color: var(--primary);
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
