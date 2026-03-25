<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { shoppingCartService, type ShoppingCartResponse } from '../api/shoppingCartService'
import { orderService, type OrderResponse } from '../api/orderService'
import { discountService, type DiscountResponse } from '../api/discountService'
import { paymentService } from '../api/paymentService'
import { useToastStore } from '../stores/toast'
import BaseLoader from '../components/BaseLoader.vue'

const cartItems = ref<ShoppingCartResponse[]>([])
const loading = ref(true)
const checkingOut = ref(false)
const createdOrder = ref<OrderResponse | null>(null)
const startingPayment = ref(false)
const toast = useToastStore()

const selectedItemIds = ref<number[]>([])
const allDiscounts = ref<DiscountResponse[]>([])
const selectedDiscountId = ref<number | null>(null)

const fetchCart = async () => {
  loading.value = true
  try {
    const res = await shoppingCartService.getMyCart()
    if (res.isSuccess) {
      console.log('Cart Items Data:', res.data)
      cartItems.value = res.data
      selectedItemIds.value = cartItems.value.map(i => i.id)
    }
  } catch (err: unknown) {
    console.error('Failed to fetch cart:', err)
    toast.show('Không thể tải giỏ hàng', 'error')
  } finally {
    loading.value = false
  }
}

const fetchDiscounts = async () => {
  try {
    const res = await discountService.getAllDiscounts()
    if (res.isSuccess) {
      allDiscounts.value = res.data.filter(d => d.isActive)
    }
  } catch (err) {
    console.error('Failed to fetch discounts:', err)
  }
}

const toggleSelectAll = () => {
  if (selectedItemIds.value.length === cartItems.value.length && cartItems.value.length > 0) {
    selectedItemIds.value = []
  } else {
    selectedItemIds.value = cartItems.value.map(i => i.id)
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
      selectedItemIds.value = selectedItemIds.value.filter(sid => sid !== id)
      toast.show('Đã xóa sản phẩm', 'success')
    }
  } catch (err: unknown) {
    console.error('Remove item error:', err)
    toast.show('Lỗi khi xóa sản phẩm', 'error')
  }
}

const selectedTotalPrice = computed(() => {
  return cartItems.value
    .filter(item => selectedItemIds.value.includes(item.id))
    .reduce((sum, item) => sum + (item.bookPrice * item.amount), 0)
})

const tax = computed(() => selectedTotalPrice.value * 0.1)

const applicableDiscounts = computed(() => {
  const total = selectedTotalPrice.value
  return allDiscounts.value.filter(d => {
    const now = new Date()
    const start = new Date(d.startDate)
    const end = new Date(d.endDate)
    return total >= d.minApply && (d.maxApply === 0 || total <= d.maxApply) && now >= start && now <= end
  })
})

const selectedDiscount = computed(() => {
  return applicableDiscounts.value.find(d => d.id === selectedDiscountId.value) || null
})

const discountAmount = computed(() => {
  if (!selectedDiscount.value) return 0
  return (selectedTotalPrice.value + tax.value) * (selectedDiscount.value.percent / 100)
})

const finalTotalAfterDiscount = computed(() => {
  return selectedTotalPrice.value + tax.value - discountAmount.value
})

const handleCheckout = async () => {
  if (selectedItemIds.value.length === 0) {
    toast.show('Vui lòng chọn ít nhất một sản phẩm để thanh toán', 'warning')
    return
  }
  
  checkingOut.value = true
  try {
    const res = await orderService.createOrder({
      idShoppingCart: selectedItemIds.value,
      discountId: selectedDiscountId.value || undefined
    })
    
    if (res.isSuccess) {
      toast.show('Đơn hàng đã được tạo! Vui lòng kiểm tra lại trước khi thanh toán.', 'success')
      createdOrder.value = res.data
      cartItems.value = cartItems.value.filter(i => !selectedItemIds.value.includes(i.id))
      selectedItemIds.value = []
      selectedDiscountId.value = null
    } else {
      toast.show(res.message || 'Lỗi khi tạo đơn hàng', 'error')
    }
  } catch (err: unknown) {
    console.error('Checkout error:', err)
    toast.show('Có lỗi xảy ra trong quá trình tạo đơn hàng', 'error')
  } finally {
    checkingOut.value = false
  }
}

const handlePayNow = async () => {
  if (!createdOrder.value) return
  
  startingPayment.value = true
  try {
    const res = await paymentService.createMoMoPayment(createdOrder.value.id)
    if (res.isSuccess && res.data.url) {
      window.location.href = res.data.url
    } else {
      toast.show(res.message || 'Lỗi khi lấy link thanh toán', 'error')
    }
  } catch (err: unknown) {
    console.error('Payment error:', err)
    toast.show('Lỗi kết nối tới cổng thanh toán', 'error')
  } finally {
    startingPayment.value = false
  }
}

onMounted(() => {
  fetchCart()
  fetchDiscounts()
})
</script>

<template>
  <div class="cart-view-container animate-fade-in">
    <div class="cart-header">
      <h1 v-if="!createdOrder">Giỏ hàng của bạn</h1>
      <h1 v-else>Xác nhận thanh toán #{{ createdOrder.id }}</h1>
      <p v-if="!createdOrder && cartItems.length > 0">Bạn có {{ cartItems.length }} sản phẩm trong giỏ</p>
      <p v-else-if="createdOrder">Vui lòng kiểm tra lại thông tin đơn hàng</p>
    </div>

    <BaseLoader v-if="loading" />
    
    <!-- Empty Cart -->
    <div v-else-if="cartItems.length === 0 && !createdOrder" class="empty-state glass">
      <div class="empty-icon">🛒</div>
      <h2>Giỏ hàng đang trống</h2>
      <p>Có vẻ như bạn chưa chọn cuốn sách nào.</p>
      <RouterLink to="/books" class="btn-primary-link">Khám phá sách mới</RouterLink>
    </div>

    <!-- Order Created / Confirmation -->
    <div v-else-if="createdOrder" class="order-confirmation-layout">
      <div class="confirmation-card glass">
        <div class="success-checkmark">✅</div>
        <h2>Đơn hàng đã sẵn sàng!</h2>
        
        <div class="order-details-summary">
          <div class="detail-row">
            <span>Mã đơn hàng:</span>
            <strong>#{{ createdOrder.id }}</strong>
          </div>
          <div class="detail-row">
            <span>Tạm tính:</span>
            <span>{{ createdOrder.subTotal.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <div class="detail-row">
            <span>Thuế (10%):</span>
            <span>{{ createdOrder.tax.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <div v-if="createdOrder.discountPercent > 0" class="detail-row discount">
            <span>Giảm giá ({{ createdOrder.discountPercent }}%):</span>
            <span>-{{ ((createdOrder.subTotal + createdOrder.tax) * createdOrder.discountPercent / 100).toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <hr />
          <div class="detail-row total">
            <span>Tổng thanh toán:</span>
            <span class="total-amount">{{ createdOrder.paymentTotal.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
        </div>

        <div class="confirmation-actions">
          <button class="btn-momo ripple" :disabled="startingPayment" @click="handlePayNow">
            <template v-if="!startingPayment">
              <img src="https://developers.momo.vn/v3/vi/assets/images/logo-custom-657754b9d09c314fd599-4d6cb879d724f8c44b360e227a92211f.png" alt="MoMo" class="momo-logo" />
              Thanh toán ngay qua MoMo
            </template>
            <span v-else class="loader-tiny" style="border-top-color: white"></span>
          </button>
          <button class="btn-secondary" @click="createdOrder = null" :disabled="startingPayment">Quay lại</button>
        </div>
      </div>
    </div>

    <!-- Normal Cart Layout -->
    <div v-else class="cart-layout">
      <!-- Item List -->
      <div class="items-section">
        <div class="selection-header glass">
          <label class="checkbox-container">
            <input type="checkbox" :checked="selectedItemIds.length === cartItems.length && cartItems.length > 0" @change="toggleSelectAll" />
            <span class="checkmark"></span>
            <span class="label-text">Chọn tất cả ({{ cartItems.length }})</span>
          </label>
        </div>

        <div v-for="item in cartItems" :key="item.id" class="cart-item glass" :class="{ selected: selectedItemIds.includes(item.id) }">
          <div class="item-selection">
            <label class="checkbox-container">
              <input type="checkbox" :value="item.id" v-model="selectedItemIds" />
              <span class="checkmark"></span>
            </label>
          </div>
          <div class="item-details">
            <div class="item-img glass">
              <img v-if="item.bookImage || (item as any).BookImage" :src="item.bookImage || (item as any).BookImage" :alt="item.bookName" class="cart-book-img" />
              <div v-else class="item-img-placeholder">
                <svg viewBox="0 0 24 24" width="24" height="24" fill="none" stroke="#94a3b8" stroke-width="2">
                  <path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"></path>
                  <path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"></path>
                </svg>
              </div>
            </div>
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
          <h3>Tổng đơn đặt hàng</h3>
          
          <div class="discount-selector">
            <label>Mã giảm giá</label>
            <div class="select-wrapper">
              <select v-model="selectedDiscountId" class="glass-select" :disabled="applicableDiscounts.length === 0">
                <option :value="null">-- Chọn mã giảm giá --</option>
                <option v-for="d in applicableDiscounts" :key="d.id" :value="d.id">
                  Giảm {{ d.percent }}% (HĐ từ {{ d.minApply.toLocaleString('vi-VN') }}đ)
                </option>
              </select>
            </div>
            <p v-if="applicableDiscounts.length === 0 && selectedTotalPrice > 0" class="no-discount">Không có mã phù hợp cho đơn hàng này</p>
            <p v-else-if="selectedDiscount" class="discount-hint">Đã áp dụng mã giảm {{ selectedDiscount.percent }}%</p>
          </div>

          <div class="summary-row">
            <span>Tạm tính ({{ selectedItemIds.length }} SP)</span>
            <span>{{ selectedTotalPrice.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <div class="summary-row">
            <span>Thuế (10%)</span>
            <span>{{ tax.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <div v-if="selectedDiscount" class="summary-row discount-row">
            <span>Giảm giá</span>
            <span>-{{ discountAmount.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <div class="summary-row">
            <span>Phí vận chuyển</span>
            <span class="free">Miễn phí</span>
          </div>
          <hr />
          <div class="summary-row total">
            <span>Tổng cộng</span>
            <span class="total-price">{{ finalTotalAfterDiscount.toLocaleString('vi-VN') }} VNĐ</span>
          </div>
          <button class="btn-checkout ripple" :disabled="checkingOut || selectedItemIds.length === 0" @click="handleCheckout">
            <span v-if="!checkingOut">Xác nhận thanh toán</span>
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
  padding: 1rem 1rem;
}

.cart-header {
  margin-bottom: 1.5rem;
}

.cart-header h1 {
  font-size: 1.8rem;
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

.selection-header {
  padding: 1rem 1.5rem;
  display: flex;
  align-items: center;
}

.cart-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.25rem 1.5rem;
  transition: all 0.3s;
  border: 1px solid transparent;
}

.cart-item.selected {
  border-color: var(--primary-light);
  background: rgba(255, 255, 255, 0.85);
}

.item-selection {
  margin-right: 1rem;
}

.item-details {
  flex: 1;
  display: flex;
  align-items: center;
  gap: 1.25rem;
}

.item-img {
  width: 60px;
  height: 80px;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 10px rgba(0,0,0,0.1);
}

.cart-book-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.item-img-placeholder {
  width: 100%;
  height: 100%;
  background: #f1f5f9;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
}

.book-name {
  font-size: 1.05rem;
  font-weight: 700;
  margin-bottom: 0.25rem;
}

.book-price-unit {
  color: var(--text-muted);
  font-size: 0.85rem;
}

.item-actions {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.quantity-control {
  display: flex;
  align-items: center;
  background: #f1f5f9;
  border-radius: 8px;
  padding: 0.15rem;
}

.q-btn {
  background: transparent;
  border: none;
  width: 28px;
  height: 28px;
  font-size: 1.1rem;
  cursor: pointer;
  color: var(--primary);
  display: flex;
  align-items: center;
  justify-content: center;
}

.q-val {
  width: 35px;
  text-align: center;
  font-weight: 700;
  font-size: 0.9rem;
}

.item-total {
  font-weight: 800;
  font-size: 1rem;
  color: var(--danger);
  min-width: 110px;
  text-align: right;
}

.btn-remove {
  background: transparent;
  border: none;
  color: #cbd5e1;
  cursor: pointer;
  transition: color 0.2s;
}

.btn-remove:hover {
  color: var(--danger);
}

/* Checkbox Styles */
.checkbox-container {
  display: flex;
  align-items: center;
  position: relative;
  cursor: pointer;
  user-select: none;
}

.checkbox-container input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
  height: 0;
  width: 0;
}

.checkmark {
  height: 20px;
  width: 20px;
  background-color: #fff;
  border: 2px solid #cbd5e1;
  border-radius: 6px;
  margin-right: 0.75rem;
  display: inline-block;
  transition: all 0.2s;
}

.checkbox-container:hover input ~ .checkmark {
  border-color: var(--primary);
}

.checkbox-container input:checked ~ .checkmark {
  background-color: var(--primary);
  border-color: var(--primary);
}

.checkmark:after {
  content: "";
  position: absolute;
  display: none;
  left: 7px;
  top: 3px;
  width: 6px;
  height: 11px;
  border: solid white;
  border-width: 0 2.5px 2.5px 0;
  transform: rotate(45deg);
}

.checkbox-container input:checked ~ .checkmark:after {
  display: block;
}

.label-text {
  font-weight: 600;
  color: var(--text-dark);
}

/* Discount Selector */
.discount-selector {
  margin-bottom: 1.5rem;
  background: rgba(var(--primary-rgb), 0.05);
  padding: 1rem;
  border-radius: 12px;
}

.discount-selector label {
  display: block;
  font-size: 0.85rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  color: var(--primary);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.glass-select {
  width: 100%;
  padding: 0.6rem;
  border: 1px solid rgba(0,0,0,0.1);
  border-radius: 8px;
  background: white;
  font-size: 0.9rem;
  cursor: pointer;
  outline: none;
}

.no-discount {
  font-size: 0.75rem;
  color: var(--text-muted);
  margin-top: 0.5rem;
  font-style: italic;
}

.discount-hint {
  font-size: 0.75rem;
  color: #10b981;
  margin-top: 0.5rem;
  font-weight: 600;
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
  font-weight: 800;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.8rem;
  color: var(--text-muted);
  font-size: 0.95rem;
}

.discount-row {
  color: #10b981;
  font-weight: 600;
}

.summary-row.total {
  color: var(--text-dark);
  font-weight: 800;
  font-size: 1.25rem;
  margin-top: 1.2rem;
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
  margin: 1.2rem 0;
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
  margin-top: 1.5rem;
  box-shadow: 0 10px 20px rgba(118, 75, 162, 0.2);
  transition: all 0.3s;
}

.btn-checkout:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 12px 24px rgba(118, 75, 162, 0.3);
}

.btn-checkout:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  background: #94a3b8;
  box-shadow: none;
}

/* Order Confirmation */
.order-confirmation-layout {
  display: flex;
  justify-content: center;
  padding: 2rem 0;
}

.confirmation-card {
  max-width: 500px;
  width: 100%;
  padding: 3rem 2rem;
  text-align: center;
}

.success-checkmark {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.order-details-summary {
  background: rgba(248, 250, 252, 0.5);
  border-radius: 12px;
  padding: 1.5rem;
  margin: 2rem 0;
  text-align: left;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.75rem;
  font-size: 0.95rem;
}

.detail-row.total {
  margin-top: 1rem;
  font-weight: 800;
  font-size: 1.15rem;
}

.total-amount {
  color: var(--danger);
}

.confirmation-actions {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.btn-momo {
  background: #ae2070;
  color: white;
  border: none;
  padding: 1rem;
  border-radius: 12px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  cursor: pointer;
  transition: transform 0.2s;
}

.btn-momo:hover:not(:disabled) {
  transform: translateY(-2px);
  background: #90165c;
}

.momo-logo {
  height: 24px;
}

.btn-secondary {
  background: transparent;
  border: 1px solid #cbd5e1;
  padding: 0.8rem;
  border-radius: 12px;
  cursor: pointer;
  font-weight: 600;
}

.btn-secondary:hover {
  background: #f1f5f9;
}

/* Base Styles */
.secure-info {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  font-size: 0.8rem;
  color: #94a3b8;
  margin-top: 1.5rem;
}

.empty-state {
  padding: 4rem 2rem;
  text-align: center;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
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
