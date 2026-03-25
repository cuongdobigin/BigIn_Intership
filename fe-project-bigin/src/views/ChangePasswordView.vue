<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import BaseInput from '../components/BaseInput.vue'
import BaseButton from '../components/BaseButton.vue'
import { authService } from '../api/authService'
import { useToastStore } from '../stores/toast'

const router = useRouter()
const toast = useToastStore()
const email = ref('')
const oldPassword = ref('')
const newPassword = ref('')
const confirmNewPassword = ref('')
const isLoading = ref(false)
const error = ref('')

const handleChangePassword = async () => {
  if (newPassword.value !== confirmNewPassword.value) {
    error.value = 'Mật khẩu mới xác nhận không khớp.'
    return
  }

  try {
    isLoading.value = true
    error.value = ''
    
    // Call backend API via Axios
    await authService.changePassword({
      username: email.value,
      oldPassword: oldPassword.value,
      newPassword: newPassword.value
    })
    
    toast.show('Đổi mật khẩu thành công! Vui lòng đăng nhập lại.', 'success')
    router.push('/login')
  } catch (err: any) {
    error.value = err?.response?.data?.message || err?.response?.data?.Message || 'Gặp lỗi trong quá trình đổi mật khẩu.'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="view-container">
    <div class="header">
      <h2>Đổi mật khẩu</h2>
      <p>Cập nhật lại mật khẩu an toàn hơn cho tài khoản</p>
    </div>

    <form @submit.prevent="handleChangePassword" class="auth-form">
      <div v-if="error" class="alert alert-error">
        {{ error }}
      </div>

      <BaseInput
        id="email"
        label="Email"
        v-model="email"
        type="email"
        placeholder="Email đã đăng ký"
        required
      />

      <BaseInput
        id="oldPassword"
        label="Mật khẩu hiện tại"
        v-model="oldPassword"
        type="password"
        placeholder="••••••••"
        required
      />

      <BaseInput
        id="newPassword"
        label="Mật khẩu mới"
        v-model="newPassword"
        type="password"
        placeholder="Mật khẩu mới ít nhất 8 ký tự"
        minlength="8"
        required
      />

      <BaseInput
        id="confirmNewPassword"
        label="Xác nhận mật khẩu mới"
        v-model="confirmNewPassword"
        type="password"
        placeholder="Nhập lại mật khẩu mới"
        required
      />

      <BaseButton type="submit" :isLoading="isLoading" class="btn-margin">
        Cập nhật mật khẩu
      </BaseButton>

      <div class="auth-footer">
        Quay lại 
        <RouterLink to="/login" class="link-primary">Đăng nhập</RouterLink>
      </div>
    </form>
  </div>
</template>

<style scoped>
.view-container {
  width: 100%;
  max-width: 400px;
  margin: 0 auto;
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

.auth-form {
  display: flex;
  flex-direction: column;
}

.alert {
  padding: 0.75rem 1rem;
  border-radius: var(--radius-md);
  margin-bottom: 1rem;
  font-size: 0.875rem;
  text-align: center;
}

.alert-error {
  background-color: var(--danger);
  color: white;
}

.alert-success {
  background-color: var(--secondary);
  color: white;
}

.btn-margin {
  margin-top: 0.5rem;
}

.auth-footer {
  margin-top: 1.5rem;
  text-align: center;
  font-size: 0.875rem;
  color: var(--text-muted);
}

.link-primary {
  font-weight: 600;
  color: var(--primary);
  margin-left: 0.25rem;
}

.link-primary:hover {
  text-decoration: underline;
}
</style>
