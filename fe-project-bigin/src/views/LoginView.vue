<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import BaseInput from '../components/BaseInput.vue'
import BaseButton from '../components/BaseButton.vue'
import { authService } from '../api/authService'
import { useToastStore } from '../stores/toast'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const toast = useToastStore()
const auth = useAuthStore()
const email = ref('')
const password = ref('')
const isLoading = ref(false)
const error = ref('')

const handleLogin = async () => {
  try {
    isLoading.value = true
    error.value = ''
    
    // Call the actual backend API via Axios
    const response: any = await authService.login({ username: email.value, password: password.value })
    const loginData = response.data
    
    // Xử lý token từ BE
    if (loginData?.token) {
      auth.setUser(email.value, loginData.token)
    }

    if (loginData?.isFirstTime) {
      toast.show('Đăng nhập thành công! Vui lòng hoàn thiện thông tin hồ sơ.', 'success')
      router.push('/profile-setup')
    } else {
      toast.show('Đăng nhập thành công!', 'success')
      router.push('/') // Chuyển hướng về trang chủ
    }
  } catch (err: any) {
    error.value = err?.response?.data?.message || err?.response?.data?.Message || 'Email hoặc mật khẩu không chính xác.'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="view-container">
    <div class="header">
      <h2>Chào mừng trở lại</h2>
      <p>Vui lòng đăng nhập vào tài khoản của bạn</p>
    </div>

    <form @submit.prevent="handleLogin" class="auth-form">
      <div v-if="error" class="alert-error">
        {{ error }}
      </div>

      <BaseInput
        id="email"
        label="Email"
        v-model="email"
        type="email"
        placeholder="nhapemail@example.com"
        required
      />

      <BaseInput
        id="password"
        label="Mật khẩu"
        v-model="password"
        type="password"
        placeholder="••••••••"
        required
      />

      <div class="forgot-password">
        <RouterLink to="/change-password">Đổi mật khẩu</RouterLink>
      </div>

      <BaseButton type="submit" :isLoading="isLoading">
        Đăng nhập
      </BaseButton>

      <div class="auth-footer">
        Chưa có tài khoản? 
        <RouterLink to="/register" class="link-primary">Tạo tài khoản</RouterLink>
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

.alert-error {
  background-color: var(--danger);
  color: white;
  padding: 0.75rem 1rem;
  border-radius: var(--radius-md);
  margin-bottom: 1rem;
  font-size: 0.875rem;
  text-align: center;
}

.forgot-password {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 1.5rem;
  font-size: 0.875rem;
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
