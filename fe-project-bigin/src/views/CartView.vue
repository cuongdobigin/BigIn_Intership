<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { shoppingCartService, type ShoppingCartResponse } from '../api/shoppingCartService'
import { orderService } from '../api/orderService'
import { useToastStore } from '../stores/toast'
import { useRouter } from 'vue-router'
import BaseLoader from '../components/BaseLoader.vue'

const router = useRouter()
const cartItems = ref<ShoppingCartResponse[]>([])
const loading = ref(true)
const checkingOut = ref(false)
const toast = useToastStore()

const fetchCart = async () => {
  loading.value = true
  try {
    const res = await shoppingCartService.getMyCart()
    if (res.isSuccess) {
      cartItems.value = res.data
    }
  } catch (err: unknown) {
    console.error('Failed to fetch cart:', err)
    toast.show('Không thể tải giỏ hàng', 'error')
  } finally {
    loading.value = false
  }
}

const updateQuantity = async (item: ShoppingCartResponse, delta: number) => {
  const newAmount = item.amount + delta
  if (newAmount < 1) return
  
  try {
    const res = await shoppingCartService.updateCartItem(item.id, { 
      bookId: item.bookId, 
      amount: newAmount 
    })
    if (res.isSuccess) {
      item.amount = newAmount
      toast.show('Đã cập nhật số lượng', 'success')
    }
  } catch (err: unknown) {
    console.error('Update quantity error:', err)
    toast.show('Lỗi khi cập nhật số lượng', 'error')
  }
}

const removeItem = async (id: number) => {
  if (!confirm('Bạn có muốn xóa sản phẩm này khỏi giỏ hàng?')) return
  
  try {
    const res = await shoppingCartService.removeFromCart(id)
    if (res.isSuccess) {
      cartItems.value = cartItems.value.filter(i => i.id !== id)
      toast.show('Đã xóa sản phẩm', 'success')
    }
  } catch (err: unknown) {
    console.error('Remove item error:', err)
    toast.show('Lỗi khi xóa sản phẩm', 'error')
  }
}

const totalPrice = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + (item.bookPrice * item.amount), 0)
})

const handleCheckout = async () => {
  if (cartItems.value.length === 0) return
  
  if (!confirm('Bạn có xác nhận thanh toán đơn hàng này?')) return

  checkingOut.value = true
  try {
    const res = await orderService.createOrder({
      idShoppingCart: cartItems.value.map(item => item.id)
    })
    
    if (res.isSuccess) {
      toast.show('Thanh toán thành công! Đã tạo đơn hàng mới.', 'success')
      cartItems.value = [] // Clear local cart
      router.push('/') // Redirect to home or order history
    } else {
      toast.show(res.message || 'Lỗi khi tạo đơn hàng', 'error')
    }
  } catch (err: unknown) {
    console.error('Checkout error:', err)
    toast.show('Có lỗi xảy ra trong quá trình thanh toán', 'error')
  } finally {
    checkingOut.value = false
  }
}

onMounted(fetchCart)
</script>

<template>
  <div class="cart-view-container animate-fade-in">
    <div class="cart-header">
      <h1>Giỏ hàng của bạn</h1>
      <p v-if="cartItems.length > 0">Bạn có {{ cartItems.length }} sản phẩm trong giỏ</p>
    </div>

    <BaseLoader v-if="loading" />
    
    <div v-else-if="cartItems.length === 0" class="empty-state glass">
      <div class="empty-icon">🛒</div>
      <h2>Giỏ hàng đang trống</h2>
      <p>Có vẻ như bạn chưa chọn cuốn sách nào.</p>
      <RouterLink to="/books" class="btn-primary-link">Khám phá sách mới</RouterLink>
    </div>

    <div v-else class="cart-layout">
      <!-- Item List -->
      <div class="items-section">
        <div v-for="item in cartItems" :key="item.id" class="cart-item glass">
          <div class="item-details">
            <!-- Note: Backend response doesn't have image link directly, 
                 ideally we'd fetch it or have it in the response. 
                 Using a placeholder for now as per current DTO -->
            <div class="item-img-placeholder">📚</div>
            <div class="item-info">
              <h3 class="book-name">{{ item.bookName }}</h3>
              <p class="book-price-unit">{{ item.bookPrice.toLocaleString('vi-VN') }} VNĐ</p>
            </div>
          </div>

          <div class="item-actions">
            <div class="quantity-control">
              <button @click="updateQuantity(item, -1)" class="q-btn">-</button>
              <span class="q-val">{{ item.amount }}</span>
              <button @click="updateQuantity(item, 1)" class="q-btn">+</button>
            </div>
            <div class="item-total">
              {{ (item.bookPrice * item.amount).toLocaleString('vi-VN') }} VNĐ
            </div>
            <button @click="removeItem(item.id)" class="btn-remove" title="Xóa">
              <svg viewBox="0 0 24 24" width="20" height="20" fill="none" stroke="currentColor" stroke-width="2">
                <polyline points="3 6 5 6 21 6"></polyline>
                <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
              </svg>
            </button>
          </div>
        </div>
      </div>

      <!-- Summary Sidebar -->
      <div class="summary-section">
        <div class="summary-card glass">
          <h3>Tổng đơn hàng</h3>
          <div class="summary-row">
            <span>Tạm tính</span>
            <span>{{ totalPrice.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <div class="summary-row">
            <span>Phí vận chuyển</span>
            <span class="free">Miễn phí</span>
          </div>
          <hr />
          <div class="summary-row total">
            <span>Tổng cộng</span>
            <span class="total-price">{{ totalPrice.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <button class="btn-checkout ripple" :disabled="checkingOut || cartItems.length === 0" @click="handleCheckout">
            <span v-if="!checkingOut">Tiến hành thanh toán</span>
            <span v-else class="loader-tiny" style="border-top-color: white"></span>
          </button>
          <div class="secure-info">
            <svg viewBox="0 0 24 24" width="16" height="16" fill="currentColor">
              <path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4zm0 2.18l7 3.12v4.7c0 4.67-3.13 8.75-7 9.82-3.87-1.07-7-5.15-7-9.82V6.3l7-3.12z"></path>
            </svg>
            Thanh toán an toàn & bảo mật
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.cart-view-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.cart-header {
  margin-bottom: 2rem;
}

.cart-header h1 {
  font-size: 2rem;
  font-weight: 800;
  color: var(--text-dark);
}

.cart-header p {
  color: var(--text-muted);
}

/* Glassmorphism */
.glass {
  background: rgba(255, 255, 255, 0.7);
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 20px;
  box-shadow: 0 8px 32px rgba(31, 38, 135, 0.05);
}

.cart-layout {
  display: grid;
  grid-template-columns: 1fr 380px;
  gap: 2rem;
}

@media (max-width: 992px) {
  .cart-layout {
    grid-template-columns: 1fr;
  }
}

.items-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.cart-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.5rem;
  transition: transform 0.3s;
}

.cart-item:hover {
  transform: translateX(10px);
}

.item-details {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.item-img-placeholder {
  width: 70px;
  height: 90px;
  background: #f1f5f9;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
}

.book-name {
  font-size: 1.1rem;
  font-weight: 700;
  margin-bottom: 0.25rem;
}

.book-price-unit {
  color: var(--text-muted);
  font-size: 0.9rem;
}

.item-actions {
  display: flex;
  align-items: center;
  gap: 2rem;
}

.quantity-control {
  display: flex;
  align-items: center;
  background: #f8fafc;
  border-radius: 10px;
  padding: 0.25rem;
}

.q-btn {
  background: transparent;
  border: none;
  width: 30px;
  height: 30px;
  font-size: 1.2rem;
  cursor: pointer;
  color: var(--primary);
  display: flex;
  align-items: center;
  justify-content: center;
}

.q-val {
  width: 40px;
  text-align: center;
  font-weight: 700;
}

.item-total {
  font-weight: 800;
  font-size: 1.1rem;
  color: var(--danger);
  min-width: 120px;
  text-align: right;
}

.btn-remove {
  background: transparent;
  border: none;
  color: #94a3b8;
  cursor: pointer;
  transition: color 0.2s;
}

.btn-remove:hover {
  color: var(--danger);
}

/* Summary Sidebar */
.summary-card {
  padding: 2rem;
  position: sticky;
  top: 2rem;
}

.summary-card h3 {
  font-size: 1.25rem;
  margin-bottom: 1.5rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1rem;
  color: var(--text-muted);
}

.summary-row.total {
  color: var(--text-dark);
  font-weight: 800;
  font-size: 1.2rem;
  margin-top: 1rem;
}

.total-price {
  color: var(--danger);
}

.free {
  color: #10b981;
  font-weight: 600;
}

hr {
  border: none;
  border-top: 1px solid rgba(0,0,0,0.05);
  margin: 1.5rem 0;
}

.btn-checkout {
  width: 100%;
  padding: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-weight: 700;
  font-size: 1.1rem;
  cursor: pointer;
  margin-top: 2rem;
  box-shadow: 0 10px 20px rgba(118, 75, 162, 0.2);
}

.secure-info {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  font-size: 0.8rem;
  color: #94a3b8;
  margin-top: 1.5rem;
}

/* Empty State */
.empty-state {
  padding: 4rem 2rem;
  text-align: center;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.empty-state h2 {
  margin-bottom: 0.5rem;
}

.empty-state p {
  color: var(--text-muted);
  margin-bottom: 2rem;
}

.btn-primary-link {
  display: inline-block;
  padding: 0.8rem 2rem;
  background: var(--primary);
  color: white;
  text-decoration: none;
  border-radius: 12px;
  font-weight: 600;
}

.loader-tiny {
  display: inline-block;
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255,255,255,0.7);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>
