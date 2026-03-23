<script setup lang="ts">
import type { Book } from '../api/bookService'

defineProps<{
  book: Book
}>()

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price * 1000)
}
</script>

<template>
  <div class="book-card">
    <div class="book-cover">
      <img :src="book.avatar" :alt="book.name" class="cover-img" />
    </div>
    <div class="book-info">
      <h3 class="book-title" :title="book.name">{{ book.name }}</h3>
      <p class="book-author">Tác giả: {{ book.author }}</p>
      <p class="book-desc">{{ book.description }}</p>
      <div class="book-footer">
        <span class="book-price">{{ formatPrice(book.price) }}</span>
        <button class="add-to-cart" :disabled="!book.stock">
          {{ book.stock ? 'Thêm vào giỏ' : 'Hết hàng' }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.book-card {
  background: #ffffff;
  border: 1px solid var(--border);
  border-radius: var(--radius-lg);
  overflow: hidden;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  display: flex;
  flex-direction: column;
  height: 100%;
}

.book-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-lg);
  border-color: var(--primary-light);
}

.book-cover {
  height: 250px;
  overflow: hidden;
  background-color: #f8fafc;
  display: flex;
  align-items: center;
  justify-content: center;
}

.cover-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s;
}

.book-card:hover .cover-img {
  transform: scale(1.05);
}

.book-info {
  padding: 1.25rem;
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.book-title {
  font-size: 1.125rem;
  font-weight: 700;
  color: var(--text-dark);
  line-height: 1.4;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  line-clamp: 2;
  overflow: hidden;
  height: 3.1rem;
}

.book-author {
  font-size: 0.875rem;
  color: var(--text-muted);
}

.book-desc {
  font-size: 0.875rem;
  color: var(--text-muted);
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  line-clamp: 2;
  overflow: hidden;
  margin-bottom: 1rem;
}

.book-footer {
  margin-top: auto;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.book-price {
  font-size: 1.25rem;
  font-weight: 800;
  color: var(--danger);
}

.add-to-cart {
  padding: 0.6rem 1rem;
  background-color: var(--primary);
  color: white;
  border: none;
  border-radius: var(--radius-md);
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
  white-space: nowrap;
}

.add-to-cart:hover:not(:disabled) {
  background-color: var(--primary-hover);
}

.add-to-cart:disabled {
  background-color: #cbd5e1;
  cursor: not-allowed;
}
</style>
