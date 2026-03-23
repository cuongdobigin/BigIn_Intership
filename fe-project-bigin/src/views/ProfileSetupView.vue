<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import BaseInput from '../components/BaseInput.vue'
import BaseButton from '../components/BaseButton.vue'
import { userService } from '../api/userService'
import { useToastStore } from '../stores/toast'

const router = useRouter()
const toast = useToastStore()
const name = ref('')
const phone = ref('')
const address = ref('')
const isLoading = ref(false)
const error = ref('')

const handleSubmit = async () => {
  try {
    isLoading.value = true
    error.value = ''
    
    await userService.createUserProfile({
      name: name.value,
      phone: phone.value,
      address: address.value
    })
    
    toast.show('Cập nhật thông tin thành công!', 'success')
    router.push('/')
  } catch (err: any) {
    error.value = err?.response?.data?.message || 'Có lỗi xảy ra khi cập nhật thông tin.'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="view-container">
    <div class="header">
      <h2>Thiết lập hồ sơ</h2>
      <p>Chào mừng bạn! Vui lòng hoàn tất thông tin để tiếp tục.</p>
    </div>

    <form @submit.prevent="handleSubmit" class="profile-form">
      <div v-if="error" class="alert-error">
        {{ error }}
      </div>

      <BaseInput
        id="name"
        label="Họ tên"
        v-model="name"
        placeholder="Nhập họ và tên"
        required
      />

      <BaseInput
        id="phone"
        label="Số điện thoại"
        v-model="phone"
        placeholder="Nhập số điện thoại"
        required
      />

      <BaseInput
        id="address"
        label="Địa chỉ"
        v-model="address"
        placeholder="Nhập địa chỉ của bạn"
        required
      />

      <div class="form-actions">
        <BaseButton type="submit" :isLoading="isLoading">
          Hoàn tất
        </BaseButton>
      </div>
    </form>
  </div>
</template>

<style scoped>
.view-container {
  width: 100%;
  max-width: 500px;
  margin: 2rem auto;
  padding: 2rem;
  background: white;
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-md);
}

.header {
  margin-bottom: 2rem;
  text-align: center;
}

.header h2 {
  font-size: 1.875rem;
  color: var(--text-main);
  margin-bottom: 0.5rem;
  font-weight: 700;
}

.header p {
  color: var(--text-muted);
  font-size: 0.95rem;
}

.profile-form {
  display: flex;
  flex-direction: column;
}

.alert-error {
  background-color: var(--danger);
  color: white;
  padding: 0.75rem 1rem;
  border-radius: var(--radius-md);
  margin-bottom: 1rem;
  font-size: 0.875rem;
  text-align: center;
}

.form-actions {
  margin-top: 1.5rem;
}
</style>
