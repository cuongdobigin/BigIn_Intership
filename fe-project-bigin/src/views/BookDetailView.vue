<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { bookService, type Book } from '../api/bookService'
import { reviewService, type ReviewResponse } from '../api/reviewService'
import { shoppingCartService } from '../api/shoppingCartService'
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

const quantity = ref(1)
const addingToCart = ref(false)

const handleAddToCart = async () => {
  if (!authStore.isLoggedIn) {
    toastStore.show('Vui lòng đăng nhập để mua hàng.', 'warning')
    return
  }
  addingToCart.value = true
  try {
    const res = await shoppingCartService.addToCart({ bookId, amount: quantity.value })
    if (res.isSuccess) {
      toastStore.show('Đã thêm vào giỏ hàng thành công!', 'success')
    } else {
      toastStore.show(res.message, 'error')
    }
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    toastStore.show(error.response?.data?.message || 'Có lỗi xảy ra', 'error')
  } finally {
    addingToCart.value = false
  }
}

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
  } catch (err: unknown) {
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

const editingReviewId = ref<number | null>(null)
const editReviewMessage = ref('')
const submittingEdit = ref(false)

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
        const review = reviews.value[index]
        if (review) {
          review.message = editReviewMessage.value
          review.updatedAt = res.data?.updatedAt || new Date().toISOString()
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
  carouselInterval = window.setInterval(nextImage, 3500)
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
onUnmounted(stopCarousel)
</script>

<template>
  <div class="book-detail-container">
    <BaseLoader v-if="loading" />
    <BaseError v-else-if="errorMsg" :message="errorMsg" />

    <div v-else-if="book" class="content-wrapper animate-fade-in">
      <div class="grid-layout">
        <!-- Sidebar: Image Gallery -->
        <div class="image-section">
          <div class="main-image-card glass" @mouseenter="stopCarousel" @mouseleave="startCarousel">
            <button v-if="allImages.length > 1" class="img-nav-btn prev" @click.stop="manualPrev" title="Ảnh trước">
              <svg viewBox="0 0 24 24" width="24" height="24" fill="none" stroke="currentColor" stroke-width="3">
                <polyline points="15 18 9 12 15 6"></polyline>
              </svg>
            </button>
            <img :src="allImages[activeImageIndex]" :alt="book.name" class="main-img" />
            <button v-if="allImages.length > 1" class="img-nav-btn next" @click.stop="manualNext" title="Ảnh tiếp theo">
              <svg viewBox="0 0 24 24" width="24" height="24" fill="none" stroke="currentColor" stroke-width="3">
                <polyline points="9 18 15 12 9 6"></polyline>
              </svg>
            </button>
          </div>
          <div v-if="allImages.length > 1" class="thumbnail-list">
            <div
              v-for="(img, idx) in allImages"
              :key="idx"
              class="thumb-item glass"
              :class="{ active: activeImageIndex === idx }"
              @click="selectImage(idx)"
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
                <span v-if="book.stock" class="qty-badge">({{ book.quantity }} sản phẩm)</span>
              </span>
            </div>

            <div class="description-box">
              <h3>Mô tả sản phẩm</h3>
              <p>{{ book.description || 'Chưa có mô tả cho cuốn sách này.' }}</p>
            </div>

            <div class="action-buttons">
              <div class="quantity-selector">
                <button @click="quantity > 1 && quantity--" class="qty-btn">-</button>
                <input type="number" v-model.number="quantity" min="1" class="qty-input" />
                <button @click="quantity++" class="qty-btn">+</button>
              </div>
              <button class="btn-primary ripple" :disabled="!book.stock || addingToCart" @click="handleAddToCart">
                 <span v-if="!addingToCart">Thêm vào giỏ</span>
                 <span v-else class="loader-tiny" style="border-top-color: white"></span>
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Reviews Section -->
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
                placeholder="Chia sẻ cảm nhận của bạn..."
                rows="1"
                class="glass-input"
                @keydown.enter.prevent="handleAddReview"
              ></textarea>
              <button
                @click="handleAddReview"
                :disabled="submittingReview || !newReviewMessage.trim()"
                class="btn-send"
              >
                <svg v-if="!submittingReview" viewBox="0 0 24 24" width="20" height="20" fill="currentColor">
                  <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"></path>
                </svg>
                <span v-else class="loader-tiny"></span>
              </button>
            </div>
          </div>
          <div v-else class="login-prompt glass">
            Hãy <router-link to="/login">đăng nhập</router-link> để để lại đánh giá.
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
                    <button v-if="authStore.username === review.accountUsername && editingReviewId !== review.id" @click="handleEditClick(review)" class="btn-action">
                      <svg viewBox="0 0 24 24" width="14" height="14" stroke="currentColor" stroke-width="2" fill="none">
                        <path d="M12 20h9M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                      </svg>
                    </button>
                    <button v-if="authStore.username === review.accountUsername" @click="handleDeleteReview(review.id)" class="btn-action btn-delete">
                      <svg viewBox="0 0 24 24" width="14" height="14" stroke="currentColor" stroke-width="2" fill="none">
                        <polyline points="3 6 5 6 21 6"></polyline>
                        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                      </svg>
                    </button>
                  </div>
                </div>
                
                <div v-if="editingReviewId === review.id" class="edit-review-form">
                  <textarea v-model="editReviewMessage" rows="2" class="glass-input edit-textarea"></textarea>
                  <div class="edit-actions">
                    <button @click="handleSaveEdit(review.id)" :disabled="submittingEdit" class="btn-save btn-small">Lưu</button>
                    <button @click="handleCancelEdit" class="btn-cancel btn-small">Hủy</button>
                  </div>
                </div>
                <p v-else class="review-text">
                  {{ review.message }}
                  <span v-if="review.updatedAt && review.updatedAt !== review.createdAt" class="edited-mark">(Đã sửa)</span>
                </p>
              </div>
            </div>
            <p v-if="reviews.length === 0" class="empty-reviews">Chưa có đánh giá nào.</p>
            <div v-if="totalReviewPages > 1" class="review-pagination">
              <BasePagination :currentPage="reviewPage" :totalPages="totalReviewPages" :hasNext="hasNextReviews" :hasPrevious="hasPreviousReviews" @change="handleReviewPageChange" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.book-detail-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem 1rem;
}

.content-wrapper {
  animation: fadeIn 0.6s ease-out;
}

.grid-layout {
  display: grid;
  grid-template-columns: 400px 1fr;
  gap: 2.5rem;
}

@media (max-width: 992px) {
  .grid-layout {
    grid-template-columns: 1fr;
  }
}

.glass {
  background: rgba(255, 255, 255, 0.7);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 20px;
  box-shadow: 0 8px 32px rgba(31, 38, 135, 0.05);
}

.main-image-card {
  position: relative;
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
  background: rgba(255, 255, 255, 0.4);
  backdrop-filter: blur(4px);
  color: var(--text-dark);
  border: 1px solid rgba(255, 255, 255, 0.5);
  width: 44px;
  height: 44px;
  border-radius: 50%;
  cursor: pointer;
  z-index: 10;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}

.img-nav-btn:hover {
  background: white;
  transform: translateY(-50%) scale(1.1);
  color: var(--primary);
  box-shadow: 0 8px 24px rgba(0,0,0,0.15);
}

.img-nav-btn.prev {
  left: 1rem;
}

.img-nav-btn.next {
  right: 1rem;
}

.main-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s;
}

.thumbnail-list {
  display: flex;
  gap: 0.5rem;
  overflow-x: auto;
}

.thumb-item {
  flex: 0 0 60px;
  height: 60px;
  cursor: pointer;
}

.thumb-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 8px;
}

.thumb-item.active {
  border-color: var(--primary);
}

.info-card {
  padding: 2rem;
}

.book-title {
  font-size: 1.6rem;
  font-weight: 800;
  margin-bottom: 0.5rem;
}

.author {
  color: #64748b;
  margin-bottom: 1.2rem;
}

.author span {
  color: var(--primary);
  font-weight: 600;
}

.price-badge {
  display: inline-block;
  font-size: 1.5rem;
  font-weight: 700;
  color: #ff4757;
  background: rgba(255, 71, 87, 0.05);
  padding: 0.4rem 1.2rem;
  border-radius: 50px;
  margin-bottom: 1.2rem;
}

.tag {
  padding: 0.3rem 0.8rem;
  border-radius: 50px;
  font-size: 0.8rem;
  font-weight: 600;
}

.in-stock { background: #dcfce7; color: #166534; }
.out-of-stock { background: #fee2e2; color: #991b1b; }

.qty-badge {
  font-size: 0.75rem;
  opacity: 0.8;
  margin-left: 0.2rem;
  font-weight: 500;
}

.description-box {
  margin-top: 1.5rem;
}

.description-box h3 {
  font-size: 1.1rem;
  margin-bottom: 0.5rem;
}

.description-box p {
  color: #475569;
  line-height: 1.6;
}

.action-buttons {
  margin-top: 2rem;
  display: flex;
  gap: 1rem;
  align-items: center;
}

.quantity-selector {
  display: flex;
  background: #f1f5f9;
  border-radius: 10px;
  overflow: hidden;
  height: 42px;
}

.qty-btn {
  width: 36px;
  border: none;
  background: transparent;
  cursor: pointer;
  font-size: 1.2rem;
}

.qty-input {
  width: 45px;
  border: none;
  background: transparent;
  text-align: center;
  font-weight: 700;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0 1.5rem;
  height: 42px;
  border-radius: 10px;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(118, 75, 162, 0.2);
}

/* Reviews Section Styles */
.reviews-wrapper {
  margin-top: 3rem;
  max-width: 800px;
  margin-left: auto;
  margin-right: auto;
  animation: slideUp 0.6s ease-out;
}

.reviews-section {
  padding: 1.5rem;
}

.review-form {
  margin-bottom: 1.5rem;
}

.review-input-wrapper {
  position: relative;
}

.glass-input {
  width: 100%;
  padding: 0.8rem 3rem 0.8rem 1rem;
  border-radius: 12px;
  border: 1px solid #e2e8f0;
  background: white;
  resize: none;
}

.btn-send {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  border: none;
  background: transparent;
  color: var(--primary);
  cursor: pointer;
}

.review-item {
  display: flex;
  gap: 1rem;
  padding: 0.75rem 1.25rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.02);
  transition: all 0.3s;
  margin-bottom: 1rem;
}

.review-item:hover {
  transform: translateX(5px);
}

.review-avatar {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: #e2e8f0;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 200;
  font-size: 0.6rem;
  flex-shrink: 0;
}

.review-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-width: 0; /* Prevents overflow */
}

.review-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;
  width: 100%;
}

.meta-right {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.user-name {
  font-weight: 200;
  color: #000000;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.6rem;
  flex-grow: 1;
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

.loader-tiny {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(0,0,0,0.1);
  border-top-color: currentColor;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

.review-pagination {
  margin-top: 2rem;
  display: flex;
  justify-content: center;
}

.edit-review-form {
  margin-top: 0.5rem;
  width: 100%;
}

.edit-textarea {
  font-size: 0.75rem !important;
  margin-bottom: 0.5rem;
  min-height: 60px;
}

.edit-actions {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-end;
}

.btn-small {
  padding: 0.3rem 0.8rem;
  font-size: 0.7rem;
  border-radius: 6px;
  cursor: pointer;
  border: none;
  font-weight: 600;
  transition: all 0.2s;
}

.btn-save {
  background: var(--primary);
  color: white;
}

.btn-save:hover {
  filter: brightness(1.1);
  transform: translateY(-1px);
}

.btn-cancel {
  background: #f1f5f9;
  color: #64748b;
  border: 1px solid #e2e8f0;
}

.btn-cancel:hover {
  background: #e2e8f0;
}

@keyframes spin { to { transform: rotate(360deg); } }
@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
</style>
