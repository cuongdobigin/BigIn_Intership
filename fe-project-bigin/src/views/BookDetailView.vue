<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { bookService, type Book } from '../api/bookService'
import { reviewService, type ReviewResponse } from '../api/reviewService'
import { useAuthStore } from '../stores/auth'
import { useToastStore } from '../stores/toast'
import BaseLoader from '../components/BaseLoader.vue'
import BaseError from '../components/BaseError.vue'

import BasePagination from '../components/BasePagination.vue'

const route = useRoute()
const authStore = useAuthStore()
const toastStore = useToastStore()

const bookId = Number(route.params.id)
const book = ref<Book | null>(null)
const reviews = ref<ReviewResponse[]>([])
const loading = ref(true)
const errorMsg = ref('')
const activeImageIndex = ref(0)

// Review Pagination
const reviewPage = ref(1)
const totalReviewPages = ref(1)
const hasNextReviews = ref(false)
const hasPreviousReviews = ref(false)
const reviewPageSize = 3

// Review Form
const newReviewMessage = ref('')
const submittingReview = ref(false)

// Review Edit
const editingReviewId = ref<number | null>(null)
const editReviewMessage = ref('')
const submittingEdit = ref(false)

const fetchReviews = async (page: number) => {
  try {
    const res = await reviewService.getReviewsByBookId(bookId, page, reviewPageSize)
    if (res.isSuccess) {
      reviews.value = res.data.data
      totalReviewPages.value = res.data.totalPages
      reviewPage.value = res.data.page
      hasNextReviews.value = res.data.hasNext
      hasPreviousReviews.value = res.data.hasPrevious
    }
  } catch (err: any) {
    console.error('Lỗi khi tải đánh giá:', err)
  }
}

const fetchBookData = async () => {
  loading.value = true
  try {
    const bookRes = await bookService.getBookById(bookId)
    if (bookRes.isSuccess) {
      book.value = bookRes.data
    } else {
      errorMsg.value = bookRes.message
    }
    await fetchReviews(1)
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    errorMsg.value = error.response?.data?.message || 'Không thể tải thông tin sách.'
  } finally {
    loading.value = false
  }
}

const handleAddReview = async () => {
  if (!authStore.isLoggedIn) {
    toastStore.show('Bạn cần đăng nhập để viết đánh giá.', 'warning')
    return
  }
  if (!newReviewMessage.value.trim()) return

  submittingReview.value = true
  try {
    const res = await reviewService.createReview(bookId, newReviewMessage.value)
    if (res.isSuccess) {
      toastStore.show('Đã thêm đánh giá của bạn!', 'success')
      newReviewMessage.value = ''
      await fetchReviews(1)
    } else {
      toastStore.show(res.message, 'error')
    }
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    toastStore.show(error.response?.data?.message || 'Lỗi khi gửi đánh giá.', 'error')
  } finally {
    submittingReview.value = false
  }
}

const handleReviewPageChange = (page: number) => {
  fetchReviews(page)
  // Scroll to reviews section
  const el = document.getElementById('reviews-anchor')
  if (el) el.scrollIntoView({ behavior: 'smooth' })
}

const handleDeleteReview = async (id: number) => {
  if (!confirm('Bạn có chắc chắn muốn xóa đánh giá này?')) return

  try {
    const res = await reviewService.deleteReview(id)
    if (res.isSuccess) {
      toastStore.show('Đã xóa đánh giá.', 'success')
      reviews.value = reviews.value.filter(r => r.id !== id)
    }
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    toastStore.show(error.response?.data?.message || 'Lỗi khi xóa đánh giá.', 'error')
  }
}

const handleEditClick = (review: ReviewResponse) => {
  editingReviewId.value = review.id
  editReviewMessage.value = review.message
}

const handleCancelEdit = () => {
  editingReviewId.value = null
  editReviewMessage.value = ''
}

const handleSaveEdit = async (id: number) => {
  if (!editReviewMessage.value.trim()) return

  submittingEdit.value = true
  try {
    const res = await reviewService.updateReview(id, editReviewMessage.value)
    if (res.isSuccess) {
      toastStore.show('Đã cập nhật đánh giá!', 'success')
      const index = reviews.value.findIndex(r => r.id === id)
      if (index !== -1) {
        reviews.value[index].message = editReviewMessage.value
        // Optionally update the updatedAt timestamp if the backend returns it
        if (res.data && res.data.updatedAt) {
          reviews.value[index].updatedAt = res.data.updatedAt
        } else {
          reviews.value[index].updatedAt = new Date().toISOString()
        }
      }
      handleCancelEdit()
    } else {
      toastStore.show(res.message, 'error')
    }
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    toastStore.show(error.response?.data?.message || 'Lỗi khi cập nhật đánh giá.', 'error')
  } finally {
    submittingEdit.value = false
  }
}

const allImages = computed(() => {
  if (!book.value) return []
  const images = book.value.images.map(i => i.link)
  // If no specific images, can use a placeholder or avatar if exists
  return images.length > 0 ? images : ['https://via.placeholder.com/600x800?text=No+Image']
})

let carouselInterval: number | undefined

const nextImage = () => {
  if (allImages.value.length > 1) {
    activeImageIndex.value = (activeImageIndex.value + 1) % allImages.value.length
  }
}

const prevImage = () => {
  if (allImages.value.length > 1) {
    activeImageIndex.value = (activeImageIndex.value - 1 + allImages.value.length) % allImages.value.length
  }
}

const startCarousel = () => {
  if (carouselInterval) return
  carouselInterval = window.setInterval(nextImage, 2500)
}

const stopCarousel = () => {
  if (carouselInterval) {
    clearInterval(carouselInterval)
    carouselInterval = undefined
  }
}

const selectImage = (idx: number) => {
  activeImageIndex.value = idx
  stopCarousel()
  startCarousel()
}

const manualNext = () => {
  nextImage()
  stopCarousel()
  startCarousel()
}

const manualPrev = () => {
  prevImage()
  stopCarousel()
  startCarousel()
}

onMounted(() => {
  fetchBookData()
  startCarousel()
})

onUnmounted(() => {
  stopCarousel()
})
</script>

<template>
  <div class="book-detail-container">
    <BaseLoader v-if="loading" />
    <BaseError v-else-if="errorMsg" :message="errorMsg" />

    <div v-else-if="book" class="content-wrapper animate-fade-in">
      <div class="grid-layout">
        <!-- Sidebar: Image Gallery -->
        <div class="image-section">
          <div 
            class="main-image-card glass"
            @mouseenter="stopCarousel"
            @mouseleave="startCarousel"
          >
            <!-- Navigation Buttons -->
            <button v-if="allImages.length > 1" class="img-nav-btn prev" @click.stop="manualPrev">&lt;</button>
            
            <img :src="allImages[activeImageIndex]" :alt="book.name" class="main-img" />
            
            <button v-if="allImages.length > 1" class="img-nav-btn next" @click.stop="manualNext">&gt;</button>
          </div>
          <div v-if="allImages.length > 1" class="thumbnail-list">
            <div
              v-for="(img, idx) in allImages"
              :key="idx"
              class="thumb-item glass"
              :class="{ active: activeImageIndex === idx }"
              @click="selectImage(idx)"
              @mouseenter="stopCarousel"
              @mouseleave="startCarousel"
            >
              <img :src="img" alt="Thumbnail" />
            </div>
          </div>
        </div>

        <!-- Main Info -->
        <div class="info-section">
          <div class="glass info-card">
            <h1 class="book-title">{{ book.name }}</h1>
            <p class="author">Tác giả: <span>{{ book.author }}</span></p>

            <div class="price-badge">
              {{ book.price.toLocaleString('vi-VN') }} VNĐ
            </div>

            <div class="status-tags">
              <span :class="['tag', book.stock ? 'in-stock' : 'out-of-stock']">
                {{ book.stock ? 'Còn hàng' : 'Hết hàng' }}
              </span>
            </div>

            <div class="description-box">
              <h3>Mô tả sản phẩm</h3>
              <p>{{ book.description || 'Chưa có mô tả cho cuốn sách này.' }}</p>
            </div>

            <div class="action-buttons">
              <button class="btn-primary ripple" :disabled="!book.stock">
                Thêm vào giỏ hàng
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Reviews Section (Full Width Below) -->
      <div id="reviews-anchor" class="reviews-wrapper animate-slide-up">
        <div class="reviews-section glass">
          <div class="review-header">
            <h2>Đánh giá từ độc giả</h2>
          </div>

          <!-- New Review Form -->
          <div v-if="authStore.isLoggedIn" class="review-form">
            <div class="review-input-wrapper">
              <textarea
                v-model="newReviewMessage"
                placeholder="Chia sẻ cảm nhận của bạn về cuốn sách này..."
                rows="1"
                class="glass-input"
                @keydown.enter.prevent="handleAddReview"
              ></textarea>
              <button
                @click="handleAddReview"
                :disabled="submittingReview || !newReviewMessage.trim()"
                class="btn-send"
                title="Gửi đánh giá"
              >
                <svg v-if="!submittingReview" viewBox="0 0 24 24" width="20" height="20" fill="currentColor">
                  <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"></path>
                </svg>
                <span v-else class="loader-tiny"></span>
              </button>
            </div>
          </div>
          <div v-else class="login-prompt glass">
            Hãy <router-link to="/login">đăng nhập</router-link> để để lại đánh giá của bạn.
          </div>

          <!-- Review List -->
          <div class="review-list">
            <div v-for="review in reviews" :key="review.id" class="review-item animate-slide-up">
              <div class="review-avatar">
                {{ review.accountUsername.charAt(0).toUpperCase() }}
              </div>
              <div class="review-content">
                <div class="review-meta">
                  <span class="user-name">
                    {{ review.accountUsername }}
                    <span v-if="authStore.username === review.accountUsername" class="own-badge">Bạn</span>
                  </span>
                  <div class="meta-right">
                    <span class="date">{{ new Date(review.createdAt).toLocaleDateString('vi-VN') }}</span>
                    <!-- Nút Sửa -->
                    <button v-if="authStore.username === review.accountUsername && editingReviewId !== review.id" @click="handleEditClick(review)" class="btn-action" title="Sửa đánh giá">
                      <svg viewBox="0 0 24 24" width="14" height="14" stroke="currentColor" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M12 20h9"></path>
                        <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                      </svg>
                    </button>
                    <!-- Nút Xóa -->
                    <button v-if="authStore.username === review.accountUsername" @click="handleDeleteReview(review.id)" class="btn-action btn-delete" title="Xóa đánh giá">
                      <svg viewBox="0 0 24 24" width="14" height="14" stroke="currentColor" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <polyline points="3 6 5 6 21 6"></polyline>
                        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                      </svg>
                    </button>
                  </div>
                </div>
                
                <div v-if="editingReviewId === review.id" class="edit-review-form">
                  <textarea
                    v-model="editReviewMessage"
                    rows="2"
                    class="glass-input edit-textarea"
                    @keydown.enter.prevent="handleSaveEdit(review.id)"
                  ></textarea>
                  <div class="edit-actions">
                    <button @click="handleSaveEdit(review.id)" :disabled="submittingEdit || !editReviewMessage.trim()" class="btn-save btn-small">
                      <span v-if="!submittingEdit">Lưu</span>
                      <span v-else class="loader-tiny"></span>
                    </button>
                    <button @click="handleCancelEdit" :disabled="submittingEdit" class="btn-cancel btn-small">Hủy</button>
                  </div>
                </div>
                <p v-else class="review-text">
                  {{ review.message }}
                  <span v-if="review.updatedAt && review.updatedAt !== review.createdAt" class="edited-mark">(Đã chỉnh sửa)</span>
                </p>
              </div>
            </div>

            <p v-if="reviews.length === 0" class="empty-reviews">Chưa có đánh giá nào cho cuốn sách này. Hãy là người đầu tiên!</p>

            <!-- Review Pagination -->
            <div v-if="totalReviewPages > 1" class="review-pagination">
              <BasePagination
                :currentPage="reviewPage"
                :totalPages="totalReviewPages"
                :hasNext="hasNextReviews"
                :hasPrevious="hasPreviousReviews"
                @change="handleReviewPageChange"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.book-detail-container {
  max-width: 1300px;
  margin: 0 auto;
  padding: 2rem 1rem;
  min-height: 10vh;
}

.content-wrapper {
  animation: fadeIn 0.6s ease-out;
}

.grid-layout {
  display: grid;
  grid-template-columns: 480px 1fr;
  gap: 3rem;
}

/* Glassmorphism background */
.glass {
  background: rgba(255, 255, 255, 0.7);
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 20px;
  box-shadow: 0 8px 32px rgba(31, 38, 135, 0.1);
}

/* Image Section */
.image-section {
  position: sticky;
  top: 2rem;
}

.main-image-card {
  position: relative;
  width: 100%;
  aspect-ratio: 3/4;
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1rem;
}

.img-nav-btn {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: rgba(0, 0, 0, 0.2);
  color: white;
  border: none;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  font-size: 1.5rem;
  cursor: pointer;
  transition: all 0.3s;
  z-index: 10;
  display: flex;
  align-items: center;
  justify-content: center;
}

.img-nav-btn:hover {
  background: rgba(0, 0, 0, 0.5);
}

.img-nav-btn.prev {
  left: 10px;
}

.img-nav-btn.next {
  right: 10px;
}

.main-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}

.main-img:hover {
  transform: scale(1.05);
}

.thumbnail-list {
  display: flex;
  gap: 0.75rem;
  overflow-x: auto;
  padding-bottom: 0.5rem;
}

.thumb-item {
  flex: 0 0 80px;
  height: 80px;
  cursor: pointer;
  overflow: hidden;
  transition: all 0.3s ease;
}

.thumb-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.thumb-item.active {
  border-color: var(--primary);
  box-shadow: 0 0 10px rgba(var(--primary-rgb), 0.5);
  transform: translateY(-4px);
}

/* Info Section */
.info-card {
  padding: 2.5rem;
  height: 100%;
}

.book-title {
  font-size: 1.8rem;
  font-weight: 800;
  background: linear-gradient(45deg, #1a1a1a, #4a4a4a);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  margin-bottom: 0.5rem;
}

.author {
  font-size: 1.1rem;
  color: #666;
  margin-bottom: 1.5rem;
}

.author span {
  color: var(--primary);
  font-weight: 600;
}

.price-badge {
  display: inline-block;
  font-size: 1.75rem;
  font-weight: 700;
  color: #ff4757;
  background: rgba(255, 71, 87, 0.1);
  padding: 0.5rem 1.5rem;
  border-radius: 50px;
  margin-bottom: 1.5rem;
}

.status-tags {
  margin-bottom: 2rem;
}

.tag {
  padding: 0.4rem 1rem;
  border-radius: 50px;
  font-weight: 600;
  font-size: 0.9rem;
}

.in-stock { background: #e3f9e5; color: #1f9d55; }
.out-of-stock { background: #fee2e2; color: #dc2626; }

.description-box h3 {
  font-size: 1.25rem;
  margin-bottom: 1rem;
}

.description-box p {
  line-height: 1.8;
  color: #555;
}

.action-buttons {
  margin-top: 2.5rem;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 1rem 2.5rem;
  font-size: 1.1rem;
  font-weight: 600;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.3s ease;
  width: 100%;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(118, 75, 162, 0.3);
}

.btn-primary:active:not(:disabled) {
  transform: translateY(0);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Reviews Section */
.reviews-section {
  padding: 1.5rem 2rem;
}

.review-header h2 {
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
}

.review-form {
  margin-bottom: 2rem;
}

.review-input-wrapper {
  position: relative;
  width: 100%;
}

.glass-input {
  width: 100%;
  background: rgba(255, 255, 255, 0.5);
  border: 1px solid rgba(0, 0, 0, 0.1);
  border-radius: 20px;
  padding: 0.6rem 3rem 0.6rem 1rem;
  resize: none;
  font-family: inherit;
  transition: all 0.3s;
  font-size: 0.9rem;
  min-height: 45px;
  line-height: 1.5;
}

.glass-input:focus {
  outline: none;
  background: white;
  border-color: var(--primary);
  box-shadow: 0 0 15px rgba(var(--primary-rgb), 0.1);
}

.btn-send {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  background: transparent;
  color: var(--primary);
  border: none;
  width: 36px;
  height: 36px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-send:hover:not(:disabled) {
  transform: translateY(-50%) scale(1.1) rotate(-10deg);
  color: var(--primary-hover);
}

.btn-send:disabled {
  color: #94a3b8;
  cursor: not-allowed;
}

.loader-tiny {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(0,0,0,0.1);
  border-top-color: var(--primary);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
.btn-submit:disabled { opacity: 0.5; }

.login-prompt {
  padding: 1.5rem;
  text-align: center;
  margin-bottom: 2rem;
}

.review-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.review-item {
  display: flex;
  gap: 1rem;
  padding: 0.75rem 1.25rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.02);
  transition: all 0.3s;
}

.review-item:hover {
  transform: translateX(5px);
  box-shadow: 0 6px 15px rgba(0, 0, 0, 0.05);
}

.review-avatar {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: linear-gradient(45deg, var(--primary), #a78bfa);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 300;
  font-size: 0.6rem;
  flex-shrink: 0;
}

.review-content {
  flex: 1;
}

.review-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;
}

.user-name {
  font-weight: 200;
  color: #000000;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.6rem;
}

.own-badge {
  background: #f3f4f6;
  color: #666;
  font-size: 0.65rem;
  padding: 0.1rem 0.4rem;
  border-radius: 50px;
  border: 1px solid #e5e7eb;
}

.date {
  font-size: 0.75rem;
  color: #999;
}

.review-text {
  color: #555;
  line-height: 1;
  font-size: 0.7rem;
}

.meta-right {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-action {
  background: transparent;
  border: none;
  color: #666;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
  opacity: 0.6;
}

.btn-action:hover {
  background: rgba(100, 100, 100, 0.1);
  opacity: 1;
}

.btn-delete {
  color: #ff4757;
}

.btn-delete:hover {
  background: rgba(255, 71, 87, 0.1);
}

.edit-review-form {
  margin-top: 0.5rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.edit-textarea {
  min-height: 60px;
  padding: 0.5rem;
  font-size: 0.85rem;
}

.edit-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
}

.btn-small {
  padding: 0.4rem 1rem;
  border: none;
  border-radius: 6px;
  font-size: 0.75rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-save {
  background: var(--primary);
  color: white;
}

.btn-save:hover:not(:disabled) {
  background: var(--primary-hover, #5a67d8);
}

.btn-save:disabled {
  background: #cbd5e1;
  color: #94a3b8;
  cursor: not-allowed;
}

.btn-cancel {
  background: #f1f5f9;
  color: #64748b;
}

.btn-cancel:hover:not(:disabled) {
  background: #e2e8f0;
}

.edited-mark {
  font-size: 0.65rem;
  color: #94a3b8;
  font-style: italic;
  margin-left: 0.25rem;
}

.empty-reviews {
  text-align: center;
  color: #999;
  padding: 2rem;
}

.reviews-wrapper {
  margin-top: 3rem;
  max-width: 800px;
  margin-left: auto;
  margin-right: auto;
  animation: slideUp 0.6s ease-out;
}

.review-pagination {
  margin-top: 2rem;
  display: flex;
  justify-content: center;
}

/* Animations */
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

.animate-fade-in { animation: fadeIn 0.8s ease-out; }

.animate-slide-up {
  animation: slideUp 0.5s ease-out forwards;
}

@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

@media (max-width: 992px) {
  .grid-layout {
    grid-template-columns: 1fr;
  }
  .image-section {
    position: static;
  }
}
</style>
