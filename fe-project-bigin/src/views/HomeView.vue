<script setup lang="ts">
import { ref } from 'vue'
import BaseButton from '../components/BaseButton.vue'

import AppSectionHeader from '../components/AppSectionHeader.vue'

// Dummy Data
const topBooks = ref([
  { id: 1, title: 'Thám tử lừng danh Conan D100', author: 'Gosho Aoyama', price: '25.000đ', image: '📘' },
  { id: 2, title: 'Toán Học Đại Cương - Tập 1', author: 'NXB Giáo Dục', price: '120.000đ', image: '📐' },
  { id: 3, title: 'Hoá Hữu Cơ Cơ Bản', author: 'Trần Văn A', price: '95.000đ', image: '🧪' },
  { id: 4, title: 'Truyện Kiều', author: 'Nguyễn Du', price: '45.000đ', image: '📜' },
  { id: 5, title: 'Đắc Nhân Tâm', author: 'Dale Carnegie', price: '88.000đ', image: '📖' },
  { id: 6, title: 'Tâm Lý Học Tội Pham', author: 'Tác Giả Vô Danh', price: '150.000đ', image: '🧠' },
])
</script>

<template>
  <div class="home-view">
    <!-- Intro Banner -->
    <section class="banner-section">
      <div class="banner-content">
        <h1>BigInBook - Tri Thức Vô Tận</h1>
        <p>Hàng ngàn đầu sách hay đang giảm giá đến 50% chỉ trong tuần này. BigInBook tự hào mang tri thức đến mọi nhà!</p>
        <BaseButton style="width: auto;">Xem Ưu Đãi Ngay</BaseButton>
      </div>
    </section>

    <!-- Top Selling Section -->
    <section class="top-selling">
      <AppSectionHeader 
        title="🔥 Top Sách Bán Chạy" 
        :showViewAll="true" 
        viewAllLink="/books" 
      />

      <div class="book-grid">
        <div v-for="book in topBooks" :key="book.id" class="book-card">
          <div class="book-cover">
            <span class="cover-icon">{{ book.image }}</span>
          </div>
          <div class="book-info">
            <h3 class="book-title">{{ book.title }}</h3>
            <p class="book-author">{{ book.author }}</p>
            <div class="book-price">{{ book.price }}</div>
            <button class="add-to-cart">Thêm vào giỏ</button>
          </div>
        </div>
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

.book-card {
  background: #ffffff;
  border: 1px solid var(--border);
  border-radius: var(--radius-md);
  padding: 1rem;
  display: flex;
  flex-direction: column;
  transition: transform 0.2s, box-shadow 0.2s;
}

.book-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1);
}

.book-cover {
  height: 200px;
  background-color: #f3f4f6;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 5rem;
  margin-bottom: 1rem;
}

.book-title {
  font-size: 1rem;
  margin-bottom: 0.25rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.book-author {
  font-size: 0.75rem;
  color: var(--text-muted);
  margin-bottom: 0.5rem;
}

.book-price {
  font-weight: 700;
  color: var(--danger);
  font-size: 1.125rem;
  margin-bottom: 1rem;
  margin-top: auto;
}

.add-to-cart {
  width: 100%;
  padding: 0.5rem;
  background-color: #ffffff;
  border: 1px solid var(--primary);
  color: var(--primary);
  border-radius: var(--radius-md);
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.add-to-cart:hover {
  background-color: var(--primary);
  color: #fff;
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
</style>
