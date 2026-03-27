<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { discountService, type DiscountResponse, type DiscountRequest } from '../../api/discountService'
import { useToastStore } from '../../stores/toast'
import BaseLoader from '../../components/BaseLoader.vue'

const toast = useToastStore()
const discounts = ref<DiscountResponse[]>([])
const loading = ref(true)
const showModal = ref(false)
const isEditing = ref(false)
const editId = ref<number | null>(null)
const isSubmitting = ref(false)

const discountForm = ref<DiscountRequest>({
  minApply: 0,
  maxApply: 0,
  percent: 0,
  startDate: new Date().toISOString().substring(0, 10),
  endDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().substring(0, 10)
})

const fetchDiscounts = async () => {
  loading.value = true
  try {
    const res = await discountService.getAllDiscounts()
    if (res.isSuccess) {
      discounts.value = res.data
    }
  } catch (error) {
    console.error('Lỗi khi tải danh sách voucher:', error)
    toast.show('Không thể tải danh sách voucher', 'error')
  } finally {
    loading.value = false
  }
}

const openAddModal = () => {
  isEditing.value = false
  editId.value = null
  discountForm.value = {
    minApply: 0,
    maxApply: 0,
    percent: 0,
    startDate: new Date().toISOString().substring(0, 10),
    endDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().substring(0, 10)
  }
  showModal.value = true
}

const openEditModal = (discount: DiscountResponse) => {
  isEditing.value = true
  editId.value = discount.id
  discountForm.value = {
    minApply: discount.minApply,
    maxApply: discount.maxApply,
    percent: discount.percent * 100, // Show as percentage 0-100
    startDate: discount.startDate ? discount.startDate.substring(0, 10) : new Date().toISOString().substring(0, 10),
    endDate: discount.endDate ? discount.endDate.substring(0, 10) : new Date().toISOString().substring(0, 10)
  }
  showModal.value = true
}

const handleSaveDiscount = async () => {
  if (discountForm.value.minApply < 0 || discountForm.value.maxApply < 0) {
    toast.show('Giá trị áp dụng không được âm', 'warning')
    return
  }
  if (discountForm.value.percent <= 0 || discountForm.value.percent > 100) {
    toast.show('Phần trăm giảm giá phải từ 1 đến 100', 'warning')
    return
  }

  isSubmitting.value = true
  try {
    const payload = {
      ...discountForm.value,
      percent: discountForm.value.percent / 100 // Convert back to 0-1 range
    }

    let res
    if (isEditing.value && editId.value) {
      res = await discountService.updateDiscount(editId.value, payload)
    } else {
      res = await discountService.createDiscount(payload)
    }

    if (res.isSuccess) {
      toast.show(isEditing.value ? 'Cập nhật voucher thành công!' : 'Thêm voucher mới thành công!', 'success')
      showModal.value = false
      fetchDiscounts()
    } else {
      toast.show(res.message, 'error')
    }
  } catch (error: unknown) {
    const err = error as any
    const msg = err.response?.data?.message || 'Có lỗi xảy ra'
    toast.show(msg, 'error')
  } finally {
    isSubmitting.value = false
  }
}

const handleDeleteDiscount = async (id: number) => {
  if (!confirm('Bạn có chắc chắn muốn xóa voucher này?')) return
  
  try {
    const res = await discountService.deleteDiscount(id)
    if (res.isSuccess) {
      toast.show('Đã xóa voucher thành công', 'success')
      fetchDiscounts()
    }
  } catch (error) {
    toast.show('Lỗi khi xóa voucher', 'error')
  }
}

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
}

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('vi-VN')
}

const isExpired = (endDate: string) => {
  return new Date(endDate) < new Date()
}

onMounted(fetchDiscounts)
</script>

<template>
  <div class="management-view">
    <div class="view-header">
      <h1>Quản lý Voucher</h1>
      <p class="subtitle">Quản lý các chương trình khuyến mãi và mã giảm giá</p>
      <div class="header-actions">
        <button class="btn-primary" @click="openAddModal">
          Tạo voucher mới
        </button>
      </div>
    </div>

    <!-- Stats summary (Optional but nice) -->
    <div class="stats-cards">
      <div class="stat-card glass">
        <div class="stat-icon purple">🎫</div>
        <div class="stat-info">
          <span class="stat-label">Tổng voucher</span>
          <span class="stat-value">{{ discounts.length }}</span>
        </div>
      </div>
      <div class="stat-card glass">
        <div class="stat-icon green">✅</div>
        <div class="stat-info">
          <span class="stat-label">Đang hoạt động</span>
          <span class="stat-value">{{ discounts.filter(d => !isExpired(d.endDate) && d.isActive).length }}</span>
        </div>
      </div>
    </div>
    
    <div class="table-container glass">
      <BaseLoader v-if="loading" />
      <table v-else class="data-table">
        <thead>
          <tr>
            <th>STT</th>
            <th>Min Apply</th>
            <th>Max Apply</th>
            <th>Giảm giá (%)</th>
            <th>Ngày bắt đầu</th>
            <th>Ngày kết thúc</th>
            <th>Trạng thái</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(discount, index) in discounts" :key="discount.id">
            <td>{{ index + 1 }}</td>
            <td>{{ formatPrice(discount.minApply) }}</td>
            <td>{{ formatPrice(discount.maxApply) }}</td>
            <td>
              <span class="percent-badge">{{ (discount.percent * 100).toFixed(0) }}%</span>
            </td>
            <td>{{ formatDate(discount.startDate) }}</td>
            <td>{{ formatDate(discount.endDate) }}</td>
            <td>
              <span :class="['status-pill', isExpired(discount.endDate) ? 'expired' : (discount.isActive ? 'active' : 'inactive')]">
                {{ isExpired(discount.endDate) ? 'Hết hạn' : (discount.isActive ? 'Hoạt động' : 'Tạm dừng') }}
              </span>
            </td>
            <td class="actions">
              <button class="btn-icon" @click="openEditModal(discount)" title="Sửa">✏️</button>
              <button class="btn-icon" @click="handleDeleteDiscount(discount.id)" title="Xóa">🗑️</button>
            </td>
          </tr>
          <tr v-if="discounts.length === 0">
            <td colspan="8" class="empty-state">Chưa có mã giảm giá nào</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Add/Edit Modal -->
    <div v-if="showModal" class="modal-overlay" @click.self="showModal = false">
      <div class="modal-content glass animate-scale-up">
        <div class="modal-header">
          <h2>{{ isEditing ? 'Cập nhật Voucher' : 'Tạo Voucher mới' }}</h2>
          <button class="btn-close" @click="showModal = false">&times;</button>
        </div>
        
        <form @submit.prevent="handleSaveDiscount" class="voucher-form">
          <div class="form-grid">
            <div class="form-group">
              <label>Giá trị áp dụng tối thiểu (VNĐ)</label>
              <input type="number" v-model.number="discountForm.minApply" required min="0" />
            </div>
            <div class="form-group">
              <label>Giá trị giảm tối đa (VNĐ)</label>
              <input type="number" v-model.number="discountForm.maxApply" required min="0" />
            </div>
            <div class="form-group">
              <label>Phần trăm giảm giá (%)</label>
              <input type="number" v-model.number="discountForm.percent" required min="1" max="100" />
            </div>
            <div class="form-group empty"></div> <!-- Spacer -->
            
            <div class="form-group">
              <label>Ngày bắt đầu</label>
              <input type="date" v-model="discountForm.startDate" required />
            </div>
            <div class="form-group">
              <label>Ngày kết thúc</label>
              <input type="date" v-model="discountForm.endDate" required />
            </div>
          </div>
          
          <div class="modal-actions">
            <button type="button" class="btn-secondary" @click="showModal = false">Hủy</button>
            <button type="submit" class="btn-primary" :disabled="isSubmitting">
              {{ isSubmitting ? 'Đang lưu...' : (isEditing ? 'Cập nhật' : 'Tạo mới') }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<style scoped>
.management-view {
  padding: 1rem;
}

.view-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 2.5rem;
  gap: 0.5rem;
}

.view-header h1 {
  margin: 0;
  font-size: 2.22rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
  text-align: center;
  font-weight: 800;
}

.subtitle {
  text-align: center;
  color: #64748b;
  font-size: 1rem;
  margin-bottom: 1rem;
}

.header-actions {
  width: 100%;
  display: flex;
  justify-content: flex-end;
}

.stats-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  display: flex;
  align-items: center;
  padding: 1.5rem;
  gap: 1rem;
}

.stat-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
}

.stat-icon.purple { background: rgba(118, 75, 162, 0.1); }
.stat-icon.green { background: rgba(34, 197, 94, 0.1); }

.stat-info .stat-label {
  display: block;
  font-size: 0.8rem;
  color: #64748b;
}

.stat-info .stat-value {
  display: block;
  font-size: 1.5rem;
  font-weight: 700;
  color: #1e293b;
}

.glass {
  background: rgba(255, 255, 255, 0.7);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 16px;
  box-shadow: 0 8px 32px rgba(31, 38, 135, 0.05);
}

.table-container {
  overflow: hidden;
  padding: 1rem;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.data-table th {
  padding: 1rem;
  background: rgba(248, 250, 252, 0.5);
  color: #64748b;
  font-weight: 600;
  font-size: 0.85rem;
  text-transform: uppercase;
}

.data-table td {
  padding: 1rem;
  border-bottom: 1px solid rgba(226, 232, 240, 0.5);
  color: #334155;
  font-size: 0.9rem;
}

.percent-badge {
  background: #fdf2f8;
  color: #db2777;
  padding: 0.2rem 0.6rem;
  border-radius: 6px;
  font-weight: 700;
}

.status-pill {
  padding: 0.25rem 0.75rem;
  border-radius: 50px;
  font-size: 0.75rem;
  font-weight: 600;
  white-space: nowrap;
}

.status-pill.active { background: #dcfce7; color: #166534; }
.status-pill.inactive { background: #f1f5f9; color: #64748b; }
.status-pill.expired { background: #fee2e2; color: #991b1b; }

.actions {
  display: flex;
  gap: 0.5rem;
}

.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.1rem;
  padding: 0.25rem;
  border-radius: 8px;
  transition: background 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-icon:hover { background: #f1f5f9; }

.btn-primary {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.9rem;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(118, 75, 162, 0.3);
}

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.2);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  width: 100%;
  max-width: 600px;
  background: white;
  padding: 2rem;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.btn-close {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #64748b;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.25rem;
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  font-size: 0.85rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: #475569;
}

.form-group input {
  width: 100%;
  padding: 0.75rem;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
  background: #f8fafc;
  outline: none;
  transition: all 0.2s;
}

.form-group input:focus {
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
}

.btn-secondary {
  padding: 0.7rem 1.5rem;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
  background: white;
  font-weight: 600;
  cursor: pointer;
}

.animate-scale-up {
  animation: scaleUp 0.3s ease-out;
}

@keyframes scaleUp {
  from { transform: scale(0.95); opacity: 0; }
  to { transform: scale(1); opacity: 1; }
}

.empty-state {
  text-align: center;
  padding: 3rem !important;
  color: #94a3b8;
  font-style: italic;
}
</style>
