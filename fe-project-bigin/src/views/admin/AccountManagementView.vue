<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { accountService, type AccountResponse } from '../../api/accountService'
import { useToastStore } from '../../stores/toast'

const toast = useToastStore()

// State
const accounts = ref<AccountResponse[]>([])
const isLoading = ref(true)
const currentPage = ref(1)
const pageSize = ref(10)
const totalItems = ref(0)
const totalPages = ref(1)
const searchQuery = ref('')

// Fetch Data
const fetchData = async () => {
  try {
    isLoading.value = true
    const res = await accountService.getAllAccounts(currentPage.value, pageSize.value)
    if (res.isSuccess) {
      accounts.value = res.data.data
      totalItems.value = res.data.totalItems
      totalPages.value = res.data.totalPages
    }
  } catch (err) {
    console.error('Fetch accounts error:', err)
    toast.show('Không thể tải danh sách tài khoản.', 'error')
  } finally {
    isLoading.value = false
  }
}

// Delete Action
const handleDeleteAccount = async (id: number) => {
  if (!confirm('Bạn có chắc chắn muốn xóa tài khoản này không?')) return
  
  try {
    const res = await accountService.deleteAccount(id)
    if (res.isSuccess) {
      toast.show('Xóa tài khoản thành công!', 'success')
      fetchData()
    } else {
      toast.show('Không thể xóa tài khoản.', 'error')
    }
  } catch (err) {
    console.error('Delete account error:', err)
    toast.show('Có lỗi xảy ra khi xóa tài khoản.', 'error')
  }
}

// Handle Page Change
const handlePageChange = (page: number) => {
  if (page < 1 || page > totalPages.value) return
  currentPage.value = page
  fetchData()
}

// Toggle Status Action
const handleToggleStatus = async (acc: AccountResponse) => {
  const newStatus = !acc.isActive
  const actionText = newStatus ? 'mở khóa' : 'khóa'
  
  if (!confirm(`Bạn có chắc chắn muốn ${actionText} tài khoản "${acc.username}" không?`)) return
  
  try {
    const res = await accountService.toggleAccountStatus(acc.id, newStatus)
    if (res.isSuccess) {
      toast.show(`${newStatus ? 'Mở khóa' : 'Khóa'} tài khoản thành công!`, 'success')
      fetchData()
    } else {
      toast.show('Không thể cập nhật trạng thái tài khoản.', 'error')
    }
  } catch (err) {
    console.error('Toggle status error:', err)
    toast.show('Có lỗi xảy ra khi cập nhật trạng thái.', 'error')
  }
}

// Watchers for search
const filteredAccounts = computed(() => {
  const query = searchQuery.value.toLowerCase().trim()
  if (!query) return accounts.value
  
  return accounts.value.filter(acc => 
    acc.username.toLowerCase().includes(query) || 
    acc.user?.name.toLowerCase().includes(query) || 
    acc.user?.phone.includes(query)
  )
})

onMounted(() => {
  fetchData()
})
</script>

<template>
  <div class="management-view">
    <div class="view-header">
      <h1>Quản lý Tài khoản</h1>
    </div>

    <!-- Filters & Search -->
    <div class="filters card">
      <div class="search-group">
        <label>Tìm kiếm tài khoản:</label>
        <div class="search-input-wrapper">
          <input 
            v-model="searchQuery" 
            type="text" 
            placeholder="Nhập username, họ tên hoặc SĐT..." 
            class="form-input"
          />
          <span class="search-icon">🔍</span>
        </div>
      </div>
      <div class="stats-summary">
        Tổng số: <strong>{{ totalItems }}</strong> tài khoản
      </div>
    </div>

    <!-- Accounts Table -->
    <div class="table-container card">
      <table class="admin-table">
        <thead>
          <tr>
            <th>STT</th>
            <th>Tên đăng nhập</th>
            <th>Họ và Tên</th>
            <th>Số điện thoại</th>
            <th>Trạng thái</th>
            <th class="text-center">Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(acc, index) in filteredAccounts" :key="acc.username">
            <td>{{ (currentPage - 1) * pageSize + index + 1 }}</td>
            <td>
              <span class="username-tag">{{ acc.username }}</span>
            </td>
            <td>{{ acc.user?.name || '---' }}</td>
            <td>{{ acc.user?.phone || '---' }}</td>
            <td>
              <span :class="['status-badge', acc.isActive ? 'active' : 'inactive']">
                {{ acc.isActive ? 'Hoạt động' : 'Đã khóa' }}
              </span>
            </td>
            <td class="text-center">
              <div class="action-buttons">
                <button 
                  class="btn-icon toggle-status" 
                  :class="{ 'text-danger': acc.isActive, 'text-success': !acc.isActive }"
                  @click="handleToggleStatus(acc)"
                  :title="acc.isActive ? 'Khóa tài khoản' : 'Mở khóa tài khoản'"
                >
                  {{ acc.isActive ? '🔒' : '🔓' }}
                </button>
                <button 
                  class="btn-icon delete" 
                  @click="handleDeleteAccount(acc.id)"
                  title="Xóa tài khoản"
                >
                  🗑️
                </button>
              </div>
            </td>
          </tr>
          
          <tr v-if="isLoading">
            <td colspan="6" class="text-center py-4">
              <div class="loading-spinner"></div>
              Đang tải dữ liệu...
            </td>
          </tr>
          
          <tr v-if="!isLoading && filteredAccounts.length === 0">
            <td colspan="6" class="text-center py-4">
              Không tìm thấy tài khoản nào phù hợp.
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <div class="pagination" v-if="totalPages > 1">
      <button 
        :disabled="currentPage === 1" 
        @click="handlePageChange(currentPage - 1)"
        class="page-btn"
      >
        « Trước
      </button>
      
      <div class="page-numbers">
        <button 
          v-for="p in totalPages" 
          :key="p"
          :class="['page-num', { active: p === currentPage }]"
          @click="handlePageChange(p)"
        >
          {{ p }}
        </button>
      </div>

      <button 
        :disabled="currentPage === totalPages" 
        @click="handlePageChange(currentPage + 1)"
        class="page-btn"
      >
        Sau »
      </button>
    </div>
  </div>
</template>

<style scoped>
.view-header {
  margin-bottom: 2rem;
}

.view-header h1 {
  margin: 0;
  font-size: 2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 800;
}

.card {
  background: white;
  border-radius: var(--radius-md);
  border: 1px solid var(--border);
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.02);
}

.filters {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 2rem;
}

.search-group {
  flex: 1;
  display: flex;
  align-items: center;
  gap: 1rem;
}

.search-input-wrapper {
  position: relative;
  flex: 1;
  max-width: 500px;
}

.search-icon {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #94a3b8;
}

.stats-summary {
  color: #64748b;
  font-size: 0.95rem;
}

.admin-table {
  width: 100%;
  border-collapse: collapse;
}

.admin-table th {
  background: #f8fafc;
  padding: 1rem;
  text-align: left;
  font-weight: 600;
  color: #475569;
  border-bottom: 2px solid var(--border);
}

.admin-table td {
  padding: 1rem;
  border-bottom: 1px solid var(--border);
  color: #1e293b;
}

.username-tag {
  background: #f1f5f9;
  color: #475569;
  padding: 0.2rem 0.6rem;
  border-radius: 4px;
  font-family: monospace;
  font-weight: 600;
}

.status-badge {
  padding: 0.25rem 0.6rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
  white-space: nowrap;
}

.status-badge.active { background: #dcfce7; color: #166534; }
.status-badge.inactive { background: #fee2e2; color: #991b1b; }

.text-center { text-align: center; }
.py-4 { padding-top: 2rem; padding-bottom: 2rem; }

.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.25rem;
  padding: 0.5rem;
  border-radius: 8px;
  transition: all 0.2s;
}

.btn-icon:hover { background: #f1f5f9; }
.btn-icon.delete:hover { background: #fee2e2; transform: scale(1.1); }

.action-buttons {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
}

.text-danger { color: #ef4444; }
.text-success { color: #22c55e; }

.toggle-status:hover {
  background: #f1f5f9;
  transform: scale(1.1);
}

/* Pagination Styles */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 2rem;
}

.page-btn {
  padding: 0.5rem 1rem;
  border: 1px solid var(--border);
  background: white;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
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
  background: white;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.page-num.active {
  background: var(--primary);
  color: white;
  border-color: var(--primary);
}

.page-num:hover:not(.active) {
  border-color: var(--primary);
  color: var(--primary);
}

.loading-spinner {
  width: 30px;
  height: 30px;
  border: 3px solid #f3f3f3;
  border-top: 3px solid var(--primary);
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

@media (max-width: 768px) {
  .filters {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
