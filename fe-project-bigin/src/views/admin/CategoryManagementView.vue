<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { bookService, type BookType } from '../../api/bookService'
import { useToastStore } from '../../stores/toast'

interface CategoryWithCount extends BookType {
  bookCount: number | string
}

const categories = ref<CategoryWithCount[]>([])
const isLoading = ref(true)
const toast = useToastStore()

const showModal = ref(false)
const isEditing = ref(false)
const editId = ref<number | null>(null)
const categoryForm = ref({
  name: '',
  description: '',
  isActive: true
})
const isSubmitting = ref(false)

const fetchCategories = async () => {
  try {
    isLoading.value = true
    const res = await bookService.getTypeBooksAdmin()
    if (res.isSuccess) {
      categories.value = res.data.map(cat => ({ ...cat, bookCount: '...' }))
      fetchBookCounts()
    }
  } catch (error) {
    console.error('Fetch categories error:', error)
    toast.show('Không thể tải danh sách danh mục.', 'error')
  } finally {
    isLoading.value = false
  }
}

const fetchBookCounts = async () => {
  for (const cat of categories.value) {
    try {
      const res = await bookService.getBooksAdmin(1, 1, cat.typeId)
      if (res.isSuccess) {
        cat.bookCount = res.data.totalItems
      } else {
        cat.bookCount = 0
      }
    } catch (e) {
      console.error('Fetch count error:', e)
      cat.bookCount = 'N/A'
    }
  }
}

const openAddModal = () => {
  isEditing.value = false
  editId.value = null
  categoryForm.value = { name: '', description: '', isActive: true }
  showModal.value = true
}

const openEditModal = (cat: CategoryWithCount) => {
  isEditing.value = true
  editId.value = cat.typeId
  categoryForm.value = {
    name: cat.name,
    description: cat.description,
    isActive: cat.isActive
  }
  showModal.value = true
}

const handleSaveCategory = async () => {
  if (!categoryForm.value.name) return

  try {
    isSubmitting.value = true
    let res;
    if (isEditing.value && editId.value !== null) {
      res = await bookService.updateTypeBook(editId.value, categoryForm.value)
    } else {
      res = await bookService.createTypeBook(categoryForm.value)
    }

    if (res.isSuccess) {
      toast.show(isEditing.value ? 'Cập nhật thành công!' : 'Thêm danh mục thành công!', 'success')
      showModal.value = false
      fetchCategories() // Refresh list
    }
  } catch (error) {
    console.error('Save category error:', error)
    toast.show(isEditing.value ? 'Cập nhật thất bại.' : 'Thêm danh mục thất bại.', 'error')
  } finally {
    isSubmitting.value = false
  }
}

onMounted(() => {
  fetchCategories()
})
</script>

<template>
  <div class="management-view">
    <div class="view-header">
      <h1>Quản lý Danh mục Sách</h1>
      <div class="header-actions">
        <button class="btn btn-primary" @click="openAddModal">Thêm danh mục</button>
      </div>
    </div>

    <!-- Category Modal -->
    <div v-if="showModal" class="modal-overlay">
      <div class="modal-content">
        <h2>{{ isEditing ? 'Cập nhật Danh mục' : 'Thêm Danh mục Mới' }}</h2>
        <form @submit.prevent="handleSaveCategory">
          <div class="form-group">
            <label class="form-label">Tên danh mục</label>
            <input v-model="categoryForm.name" type="text" class="form-input" placeholder="Nhập tên danh mục..." required />
          </div>
          <div class="form-group">
            <label class="form-label">Mô tả</label>
            <textarea v-model="categoryForm.description" class="form-input" style="height: 100px;" placeholder="Nhập mô tả..."></textarea>
          </div>
          <div class="form-group" v-if="isEditing">
            <label class="checkbox-label">
              <input type="checkbox" v-model="categoryForm.isActive" /> Trạng thái hoạt động
            </label>
          </div>
          <div class="modal-actions">
            <button type="button" @click="showModal = false" class="btn-secondary">Hủy</button>
            <button type="submit" class="btn-primary" :disabled="isSubmitting">
              {{ isSubmitting ? 'Đang lưu...' : 'Lưu lại' }}
            </button>
          </div>
        </form>
      </div>
    </div>

    <div class="table-container card">
      <table class="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Tên danh mục</th>
            <th>Mô tả</th>
            <th>Số lượng sách</th>
            <th>Trạng thái</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cat in categories" :key="cat.typeId">
            <td>#{{ cat.typeId }}</td>
            <td><strong>{{ cat.name }}</strong></td>
            <td>{{ cat.description }}</td>
            <td><span class="badge">{{ cat.bookCount }}</span></td>
            <td>
              <span :class="['status-badge', cat.isActive ? 'active' : 'inactive']">
                {{ cat.isActive ? 'Hoạt động' : 'Ẩn' }}
              </span>
            </td>
            <td>
              <button class="btn-icon" @click="openEditModal(cat)">✏️</button>
            </td>
          </tr>
          <tr v-if="isLoading">
            <td colspan="6" class="text-center">Đang tải dữ liệu...</td>
          </tr>
          <tr v-if="!isLoading && categories.length === 0">
            <td colspan="6" class="text-center">Chưa có danh mục nào.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
.view-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.5rem;
  margin-bottom: 2.5rem;
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

.header-actions {
  width: 100%;
  display: flex;
  justify-content: flex-end;
}

.card {
  background: white;
  border-radius: var(--radius-md);
  border: 1px solid var(--border);
  overflow: hidden;
}

.admin-table {
  width: 100%;
  border-collapse: collapse;
}

.admin-table th, .admin-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid var(--border);
}

.admin-table th {
  background: #f8fafc;
  font-weight: 600;
  color: var(--text-muted);
  font-size: 0.875rem;
}

.badge {
  background: #e0f2fe;
  color: #0369a1;
  padding: 0.25rem 0.6rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 600;
}

.status-badge {
  padding: 0.25rem 0.5rem;
  border-radius: 10%;
  font-size: 0.75rem;
  font-weight: 600;
  display: inline-block;
}

.status-badge.active {
  background-color: #dcfce7;
  color: #166534;
}
.status-badge.inactive {
  background-color: #fee2e2;
  color: #991b1b;
}

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
  width: auto;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(118, 75, 162, 0.3);
}

.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  transform: none;
}

.text-center { text-align: center; }

/* Modal Overlay Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2000;
  backdrop-filter: blur(4px);
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: var(--radius-lg);
  width: 90%;
  max-width: 500px;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
}

.modal-content h2 {
  margin-top: 0;
  margin-bottom: 1.5rem;
  color: var(--text-main);
}

/* Form Styles */
.form-group {
  margin-bottom: 1.25rem;
}

.form-label {
  display: block;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: var(--text-main);
  font-size: 0.9rem;
}

.form-input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid var(--border);
  border-radius: 8px;
  font-size: 0.95rem;
  outline: none;
  transition: border-color 0.2s;
}

.form-input:focus {
  border-color: var(--primary);
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.95rem;
  cursor: pointer;
  user-select: none;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid var(--border);
}

.btn-secondary {
  padding: 0.7rem 1.5rem;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
  background: white;
  font-weight: 600;
  cursor: pointer;
  color: #64748b;
  transition: all 0.2s;
}

.btn-secondary:hover {
  background: #f8fafc;
}
</style>
