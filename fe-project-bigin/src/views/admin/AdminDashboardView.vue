<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { bookService } from '../../api/bookService'
import { accountService } from '../../api/accountService'
import { discountService } from '../../api/discountService'

const totalBooks = ref<number | string>('--')
const totalAccounts = ref<number | string>('--')
const totalVouchers = ref<number | string>('--')
const isLoading = ref(true)

const fetchStats = async () => {
  try {
    isLoading.value = true
    
    // Fetch books count
    const booksRes = await bookService.getBooksAdmin(1, 1)
    if (booksRes.isSuccess) {
      totalBooks.value = booksRes.data.totalItems
    }
    
    // Fetch accounts count
    const accountsRes = await accountService.getAllAccounts(1, 1)
    if (accountsRes.isSuccess) {
      totalAccounts.value = accountsRes.data.totalItems
    }

    // Fetch vouchers count
    const vouchersRes = await discountService.getAllDiscounts()
    if (vouchersRes.isSuccess) {
      totalVouchers.value = vouchersRes.data.length
    }
  } catch (error) {
    console.error('Failed to fetch dashboard stats:', error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchStats()
})
</script>

<template>
  <div class="admin-dashboard">
    <h1>📊 Bảng điều khiển Quản trị</h1>
    <p>Chào mừng bạn đến với hệ thống quản lý BigInBook.</p>
    
    <div class="stats-grid">
      <div class="stat-card">
        <h3>Tổng số sách</h3>
        <p class="stat-number">{{ totalBooks }}</p>
      </div>
      <router-link to="/admin/accounts" class="stat-card link-card">
        <h3>Tổng số tài khoản</h3>
        <p class="stat-number">{{ totalAccounts }}</p>
      </router-link>
      <div class="stat-card">
        <h3>Số lượng Voucher</h3>
        <p class="stat-number">{{ totalVouchers }}</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-top: 2rem;
}

.stat-card {
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: var(--radius-md);
  border: 1px solid var(--border);
  transition: transform 0.2s, box-shadow 0.2s;
  text-decoration: none;
  cursor: pointer;
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.05);
  background: #f1f5f9;
}

.stat-number {
  font-size: 2.5rem;
  font-weight: 700;
  color: var(--primary);
  margin-top: 0.5rem;
}

h3 {
  color: var(--text-muted);
  font-size: 1rem;
  font-weight: 500;
}
</style>
