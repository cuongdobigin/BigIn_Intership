<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { discountService, type DiscountResponse } from '../api/discountService'
import BaseLoader from './BaseLoader.vue'

defineProps<{
  show: boolean
}>()

const emit = defineEmits(['close'])

const discounts = ref<DiscountResponse[]>([])
const loading = ref(false)

const fetchDiscounts = async () => {
  loading.value = true
  try {
    const res = await discountService.getAllDiscounts()
    if (res.isSuccess) {
      discounts.value = res.data.filter(d => d.isActive)
    }
  } catch (err) {
    console.error('Lỗi khi tải ưu đãi:', err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchDiscounts()
})

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('vi-VN')
}
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div v-if="show" class="modal-overlay" @click.self="emit('close')">
        <div class="modal-content glass animate-scale-up">
          <div class="modal-header">
            <h2>🎁 Chương trình Ưu đãi</h2>
            <button class="close-btn" @click="emit('close')">&times;</button>
          </div>

          <div class="modal-body">
            <BaseLoader v-if="loading" />
            
            <div v-else-if="discounts.length === 0" class="empty-discounts">
              <p>Hiện chưa có chương trình ưu đãi nào mới. Hãy quay lại sau nhé!</p>
            </div>

            <div v-else class="discount-list">
              <div v-for="d in discounts" :key="d.id" class="discount-card">
                <div class="discount-badge">-{{ d.percent }}%</div>
                <div class="discount-info">
                  <h3>Giảm giá cực sốc</h3>
                  <p class="condition">
                    Áp dụng cho đơn hàng từ <strong>{{ d.minApply.toLocaleString('vi-VN') }} VNĐ</strong> 
                    đến <strong>{{ d.maxApply.toLocaleString('vi-VN') }} VNĐ</strong>
                  </p>
                  <p class="expiry">Hạn dùng: {{ formatDate(d.startDate) }} - {{ formatDate(d.endDate) }}</p>
                </div>
              </div>
            </div>
          </div>

          <div class="modal-footer">
            <button class="btn-close-bottom" @click="emit('close')">Đóng</button>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.4);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.modal-content {
  width: 90%;
  max-width: 500px;
  max-height: 80vh;
  padding: 2rem;
  display: flex;
  flex-direction: column;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.modal-header h2 {
  font-size: 1.5rem;
  margin: 0;
}

.close-btn {
  background: transparent;
  border: none;
  font-size: 2rem;
  line-height: 1;
  cursor: pointer;
  color: #94a3b8;
}

.modal-body {
  overflow-y: auto;
  padding-right: 0.5rem;
}

.discount-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.discount-card {
  display: flex;
  align-items: center;
  gap: 1rem;
  background: white;
  padding: 1rem;
  border-radius: 12px;
  border: 1px dashed var(--primary);
  position: relative;
}

.discount-badge {
  background: var(--danger);
  color: white;
  font-weight: 800;
  padding: 0.5rem;
  border-radius: 8px;
  min-width: 60px;
  text-align: center;
  font-size: 1.1rem;
}

.discount-info h3 {
  font-size: 1rem;
  margin: 0 0 0.25rem 0;
  color: var(--primary);
}

.condition {
  font-size: 0.85rem;
  margin: 0 0 0.25rem 0;
  color: #475569;
}

.expiry {
  font-size: 0.75rem;
  color: #94a3b8;
  margin: 0;
}

.modal-footer {
  margin-top: 1.5rem;
  text-align: center;
}

.btn-close-bottom {
  padding: 0.6rem 2rem;
  background: #f1f5f9;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 600;
}

.btn-close-bottom:hover {
  background: #e2e8f0;
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

.animate-scale-up {
  animation: scaleUp 0.3s ease-out;
}

@keyframes scaleUp {
  from { transform: scale(0.9); opacity: 0; }
  to { transform: scale(1); opacity: 1; }
}

.glass {
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 20px;
}
</style>
