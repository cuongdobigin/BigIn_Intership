<script setup lang="ts">
import { ref, onMounted } from 'vue'
import BaseButton from '../components/BaseButton.vue'
import AppSectionHeader from '../components/AppSectionHeader.vue'
import DiscountModal from '../components/DiscountModal.vue'
import BookCard from '../components/BookCard.vue'
import { bookService, type Book } from '../api/bookService'

const showDiscounts = ref(false)
const topBooks = ref<Book[]>([])
const loading = ref(true)

const fetchTopBooks = async () => {
  try {
    const res = await bookService.getBooks(1, 6)
    if (res.isSuccess) {
      topBooks.value = res.data.data
    }
  } catch (err) {
    console.error('Failed to fetch top books:', err)
  } finally {
    loading.value = false
  }
}

onMounted(fetchTopBooks)
</script>

<template>
  <div class="home-view">
    <!-- Intro Banner -->
    <section class="banner-section">
      <div class="banner-content">
        <h1>BigInBook - Tri Thức Vô Tận</h1>
        <p>Hàng ngàn đầu sách hay đang giảm giá đến 50% chỉ trong tuần này. BigInBook tự hào mang tri thức đến mọi nhà!</p>
        <BaseButton style="width: auto;" @click="showDiscounts = true">Xem Ưu Đãi Ngay</BaseButton>
      </div>
    </section>

    <!-- Discount Modal -->
    <DiscountModal :show="showDiscounts" @close="showDiscounts = false" />

    <!-- Top Selling Section -->
    <section class="top-selling">
      <AppSectionHeader 
        title="🔥 Top Sách Bán Chạy" 
        :showViewAll="true" 
        viewAllLink="/books" 
      />

      <div v-if="loading" class="loading-state">
        Đang tải sách...
      </div>
      <div v-else class="book-grid">
        <BookCard v-for="book in topBooks" :key="book.id" :book="book" />
      </div>
    </section>

    <!-- Info Section -->
    <section class="info-section">
      <h2>Về BigInBook</h2>
      <p>
        BigInBook được thành lập vào năm 2026 với sứ mệnh đưa tri thức và sách truyện lan tỏa đến từng con đường, khu phố. Chúng tôi cung cấp sách chất lượng, chuẩn bản quyền từ các NXB nổi tiếng trong và ngoài nước. Đặc biệt hỗ trợ vận chuyển siêu tốc chỉ trong 2H ở nội thành.
      </p>
    </section>
  </div>
</template>

<style scoped>
.home-view {
  display: flex;
  flex-direction: column;
  gap: 2.5rem;
}

/* Banner */
.banner-section {
  background: linear-gradient(135deg, #ebf8ff 0%, #dbeafe 100%);
  border: 1px solid var(--border);
  border-radius: var(--radius-lg);
  padding: 3rem 2rem;
  display: flex;
  align-items: center;
}

.banner-content {
  max-width: 600px;
}

.banner-content h1 {
  font-size: 2rem;
  margin-bottom: 1rem;
  color: var(--primary-hover);
}

.banner-content p {
  color: var(--text-muted);
  margin-bottom: 1.5rem;
  font-size: 1.125rem;
}

/* Grid */
.book-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.5rem;
}

.loading-state {
  text-align: center;
  padding: 3rem;
  color: var(--text-muted);
}

/* Info Section */
.info-section {
  background: #ffffff;
  border: 1px solid var(--border);
  border-radius: var(--radius-md);
  padding: 2rem;
}

.info-section h2 {
  margin-bottom: 1rem;
}

.info-section p {
  color: var(--text-muted);
  line-height: 1.6;
}

@media (max-width: 992px) {
  .book-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 640px) {
  .book-grid {
    grid-template-columns: 1fr;
  }
}
</style>
