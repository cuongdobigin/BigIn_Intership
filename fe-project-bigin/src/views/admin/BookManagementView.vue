<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { bookService, type Book, type BookType } from '../../api/bookService'
import { imageService } from '../../api/imageService'
import { useToastStore } from '../../stores/toast'

const toast = useToastStore()

// State
const books = ref<Book[]>([])
const categories = ref<BookType[]>([])
const isLoading = ref(true)
const totalItems = ref(0)
const currentPage = ref(1)
const pageSize = ref(10)
const filterCategoryId = ref<number | undefined>(undefined)

// Modal State
const showModal = ref(false)
const isEditing = ref(false)
const editId = ref<number | null>(null)
const isSubmitting = ref(false)
const initialImages = ref<any[]>([])

const bookForm = ref({
  name: '',
  author: '',
  price: 0,
  quantity: 0,
  description: '',
  typeBookId: 0,
  imageLinks: [''], // Array of string URLs
  isActive: true
})

// Fetch Data
const fetchData = async () => {
  try {
    isLoading.value = true
    const [booksRes, categoriesRes] = await Promise.all([
      bookService.getBooksAdmin(currentPage.value, pageSize.value, filterCategoryId.value),
      bookService.getTypeBooks()
    ])

    if (booksRes.isSuccess) {
      books.value = booksRes.data.data
      totalItems.value = booksRes.data.totalItems
    }
    if (categoriesRes.isSuccess) {
      categories.value = categoriesRes.data
    }
  } catch (err) {
    console.error('Fetch data error:', err)
    toast.show('Không thể tải dữ liệu sách.', 'error')
  } finally {
    isLoading.value = false
  }
}

const handlePageChange = (page: number) => {
  currentPage.value = page
  fetchData()
}

const handleFilterChange = () => {
  currentPage.value = 1
  fetchData()
}

// Modal Actions
const openAddModal = () => {
  isEditing.value = false
  editId.value = null
  bookForm.value = {
    name: '',
    author: '',
    price: 0,
    quantity: 0,
    description: '',
    typeBookId: categories.value[0]?.typeId || 0,
    imageLinks: [''],
    isActive: true
  }
  showModal.value = true
}

const openEditModal = async (book: Book) => {
  isEditing.value = true
  editId.value = book.id

  // Prepare form with book data
  initialImages.value = [...book.images]
  const links = book.images.map(img => img.link)

  bookForm.value = {
    name: book.name,
    author: book.author,
    price: book.price,
    quantity: book.quantity,
    description: book.description,
    typeBookId: book.typeBookId || 0,
    imageLinks: links.length > 0 ? [...links] : [''],
    isActive: book.isActive
  }

  showModal.value = true
}

const addImageLink = () => {
  bookForm.value.imageLinks.push('')
}

const removeImageLink = (index: number) => {
  bookForm.value.imageLinks.splice(index, 1)
  if (bookForm.value.imageLinks.length === 0) {
    bookForm.value.imageLinks = ['']
  }
}

const handleSaveBook = async () => {
  if (!bookForm.value.name || !bookForm.value.typeBookId) {
    toast.show('Vui lòng điền đầy đủ thông tin bắt buộc.', 'warning')
    return
  }

  try {
    isSubmitting.value = true

    const bookData = {
      name: bookForm.value.name,
      author: bookForm.value.author,
      price: bookForm.value.price,
      quantity: bookForm.value.quantity,
      description: bookForm.value.description,
      typeBookId: bookForm.value.typeBookId,
      isActive: bookForm.value.isActive
    }

    let bookId: number;
    let success = false;

    if (isEditing.value && editId.value) {
      const res = await bookService.updateBook(editId.value, bookData)
      if (res.isSuccess) {
        bookId = editId.value
        success = true
      }
    } else {
      const res = await bookService.createBook(bookData)
      if (res.isSuccess) {
        bookId = res.data.id
        success = true
      }
    }

    if (success) {
      const currentLinks = [...new Set(bookForm.value.imageLinks.map(l => l.trim()).filter(l => l !== ''))]

      if (isEditing.value) {
        const initialLinks = initialImages.value.map(i => i.link)
        // Find new links to add
        const newLinks = currentLinks.filter(link => !initialLinks.includes(link))
        if (newLinks.length > 0) {
          // @ts-expect-error - bookId is assigned if success is true
          await imageService.createImages({ bookId: bookId, links: newLinks })
        }

        // Find removed images to delete
        const imagesToDelete = initialImages.value.filter(img => !currentLinks.includes(img.link))
        for (const img of imagesToDelete) {
          try {
            await imageService.deleteImage(img.id)
          } catch (e) {
            console.warn(`Failed to delete image ${img.id}:`, e)
          }
        }
      } else {
        // For new books, just add all valid links
        if (currentLinks.length > 0) {
          // @ts-expect-error - bookId is assigned if success is true
          await imageService.createImages({ bookId: bookId, links: currentLinks })
        }
      }

      toast.show(isEditing.value ? 'Cập nhật sách thành công!' : 'Thêm sách mới thành công!', 'success')
      showModal.value = false
      fetchData()
    }
  } catch (error) {
    console.error('Save book error:', error)
    toast.show('Có lỗi xảy ra khi lưu thông tin sách.', 'error')
  } finally {
    isSubmitting.value = false
  }
}

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
}

const handleImageError = (event: Event) => {
  const target = event.target as HTMLImageElement
  if (target) {
    target.style.display = 'none'
  }
}

onMounted(() => {
  fetchData()
})

const totalPages = computed(() => Math.ceil(totalItems.value / pageSize.value))
</script>

<template>
  <div class="management-view">
    <div class="view-header">
      <h1>Quản lý Sách</h1>
      <div class="header-actions">
        <button class="btn-primary" @click="openAddModal">Thêm sách mới</button>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters card">
      <div class="filter-group">
        <label>Lọc theo danh mục:</label>
        <select v-model="filterCategoryId" @change="handleFilterChange" class="form-input" style="width: 250px;">
          <option :value="undefined">Tất cả danh mục</option>
          <option v-for="cat in categories" :key="cat.typeId" :value="cat.typeId">
            {{ cat.name }}
          </option>
        </select>
      </div>
    </div>

    <!-- Book Modal -->
    <div v-if="showModal" class="modal-overlay">
      <div class="modal-content large">
        <h2>{{ isEditing ? 'Chỉnh sửa sách' : 'Thêm sách mới' }}</h2>
        <form @submit.prevent="handleSaveBook" class="book-form-grid">
          <div class="form-main">
            <div class="form-group">
              <label class="form-label">Tên sách *</label>
              <input v-model="bookForm.name" type="text" class="form-input" required />
            </div>

            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Tác giả</label>
                <input v-model="bookForm.author" type="text" class="form-input" />
              </div>
              <div class="form-group">
                <label class="form-label">Danh mục *</label>
                <select v-model.number="bookForm.typeBookId" class="form-input" required>
                  <option v-for="cat in categories" :key="cat.typeId" :value="cat.typeId">
                    {{ cat.name }}
                  </option>
                </select>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Giá (VND) *</label>
                <input v-model.number="bookForm.price" type="number" class="form-input" required min="0" />
              </div>
              <div class="form-group">
                <label class="form-label">Số lượng *</label>
                <input v-model.number="bookForm.quantity" type="number" class="form-input" required min="0" />
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">Mô tả</label>
              <textarea v-model="bookForm.description" class="form-input" style="height: 120px;"></textarea>
            </div>

            <div class="form-group">
              <label class="checkbox-label">
                <input type="checkbox" v-model="bookForm.isActive" /> Trạng thái hoạt động
              </label>
            </div>
          </div>

          <div class="form-side">
            <div class="form-group">
              <label class="form-label">Hình ảnh (URLs)</label>
              <div v-for="(link, index) in bookForm.imageLinks" :key="index" class="image-link-input">
                <input v-model="bookForm.imageLinks[index]" type="text" class="form-input" placeholder="https://..." />
                <button type="button" class="btn-remove" @click="removeImageLink(index)">❌</button>
              </div>
              <button type="button" class="btn-add-more" @click="addImageLink">+ Thêm ảnh</button>
            </div>

            <div class="image-previews">
               <div v-for="(link, index) in bookForm.imageLinks" :key="index" class="preview-box">
                  <img v-if="link" :src="link" alt="Preview" @error="handleImageError" />
                  <div v-else class="placeholder">No image</div>
               </div>
            </div>
          </div>

          <div class="modal-actions full-width">
            <button type="button" @click="showModal = false" class="btn-secondary">Hủy</button>
            <button type="submit" class="btn-primary" :disabled="isSubmitting">
              {{ isSubmitting ? 'Đang lưu...' : 'Lưu lại' }}
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Table -->
    <div class="table-container card">
      <table class="admin-table">
        <thead>
          <tr>
            <th>STT</th>
            <th>Ảnh</th>
            <th>Tên sách / Tác giả</th>
            <th>Giá</th>
            <th>Kho</th>
            <th>Trạng thái</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(book, index) in books" :key="book.id">
            <td>{{ (currentPage - 1) * pageSize + index + 1 }}</td>
            <td class="td-img">
              <img :src="book.images[0]?.link || 'https://via.placeholder.com/50x70?text=No+Img'" alt="book" class="table-book-img" />
            </td>
            <td>
              <div class="book-info">
                <strong>{{ book.name }}</strong>
                <span class="text-muted">{{ book.author }}</span>
              </div>
            </td>
            <td class="text-primary font-bold">{{ formatPrice(book.price) }}</td>
            <td>
              <span :class="['stock-badge', book.quantity > 0 ? 'in-stock' : 'out-of-stock']">
                {{ book.quantity > 0 ? `Còn ${book.quantity}` : 'Hết hàng' }}
              </span>
            </td>
            <td>
              <span :class="['status-badge', book.isActive ? 'active' : 'inactive']">
                {{ book.isActive ? 'Hoạt động' : 'Ẩn' }}
              </span>
            </td>
            <td>
              <button class="btn-icon" @click="openEditModal(book)">✏️</button>
            </td>
          </tr>
          <tr v-if="isLoading">
            <td colspan="5" class="text-center">Đang tải dữ liệu...</td>
          </tr>
          <tr v-if="!isLoading && books.length === 0">
            <td colspan="5" class="text-center">Không tìm thấy sách nào.</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <div class="pagination" v-if="totalPages > 1">
      <button :disabled="currentPage === 1" @click="handlePageChange(currentPage - 1)">«</button>
      <span class="page-info">{{ currentPage }} / {{ totalPages }}</span>
      <button :disabled="currentPage === totalPages" @click="handlePageChange(currentPage + 1)">»</button>
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
  padding: 1rem;
  margin-bottom: 1.5rem;
}

.filters {
  display: flex;
  align-items: center;
}

.filter-group {
  display: flex;
  align-items: center;
  gap: 1rem;
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
}

.table-book-img {
  width: 45px;
  height: 60px;
  object-fit: cover;
  border-radius: 4px;
}

.book-info {
  display: flex;
  flex-direction: column;
}

.text-muted { font-size: 0.85rem; color: #64748b; }
.text-primary { color: var(--primary); }
.font-bold { font-weight: 700; }

.stock-badge {
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 600;
}
.in-stock { background: #dcfce7; color: #166534; }
.out-of-stock { background: #fee2e2; color: #991b1b; }

/* Modal large */
.modal-content.large {
  max-width: 900px;
}

.book-form-grid {
  display: grid;
  grid-template-columns: 1fr 300px;
  gap: 2rem;
  margin-top: 1.5rem;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.image-link-input {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
}

.btn-remove {
  background: none;
  border: none;
  cursor: pointer;
}

.btn-add-more {
  background: #f1f5f9;
  border: 1px dashed #cbd5e1;
  width: 100%;
  padding: 0.5rem;
  border-radius: 4px;
  cursor: pointer;
  color: #475569;
}

.image-previews {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(60px, 1fr));
  gap: 0.5rem;
  margin-top: 1rem;
}

.preview-box {
  width: 60px;
  height: 80px;
  border: 1px solid var(--border);
  border-radius: 4px;
  overflow: hidden;
  background: #f8fafc;
  display: flex;
  align-items: center;
  justify-content: center;
}

.preview-box img { width: 100%; height: 100%; object-fit: cover; }
.placeholder { font-size: 0.6rem; color: #94a3b8; text-align: center; }

.modal-actions.full-width {
  grid-column: span 2;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1.5rem;
  border-top: 1px solid var(--border);
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 2rem;
}

.pagination button {
  padding: 0.5rem 1rem;
  border: 1px solid var(--border);
  background: white;
  border-radius: 4px;
  cursor: pointer;
}

.pagination button:disabled { opacity: 0.5; cursor: not-allowed; }

.page-info { font-weight: 600; }

.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.25rem;
  padding: 0.25rem;
  border-radius: var(--radius-sm);
  transition: background 0.2s;
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

.btn-secondary {
  padding: 0.7rem 1.5rem;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
  background: white;
  font-weight: 600;
  cursor: pointer;
  color: #64748b;
}

/* Modal default overlay */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: var(--radius-lg);
  width: 90%;
}

.status-badge {
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
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

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.95rem;
  cursor: pointer;
  user-select: none;
  color: var(--text-main);
}

.checkbox-label input {
  width: 18px;
  height: 18px;
  cursor: pointer;
}
</style>
