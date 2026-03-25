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
const password = ref('')
const confirmPassword = ref('')
const isLoading = ref(false)
const error = ref('')

const handleRegister = async () => {
  if (password.value !== confirmPassword.value) {
    error.value = 'Mật khẩu xác nhận không khớp.'
    return
  }

  try {
    isLoading.value = true
    error.value = ''

    // Call backend API via Axios
    await authService.register({
      username: email.value,
      password: password.value,
    })

    toast.show('Đăng ký thành công! Vui lòng đăng nhập.', 'success')
    router.push('/login')
  } catch (err: any) {
    error.value =
      err?.response?.data?.message ||
      err?.response?.data?.Message ||
      'Đã có lỗi xảy ra. Vui lòng thử lại.'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="view-container">
    <div class="header">
      <h2>Tham gia BigInBook</h2>
      <p>Tạo tài khoản để bắt đầu hành trình đọc sách của bạn</p>
    </div>

    <form @submit.prevent="handleRegister" class="auth-form">
      <div v-if="error" class="alert-error">
        {{ error }}
      </div>

      <BaseInput
        id="email"
        label="Email (Username)"
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
        placeholder="Mật khẩu ít nhất 8 ký tự"
        minlength="8"
        required
      />

      <BaseInput
        id="confirmPassword"
        label="Xác nhận mật khẩu"
        v-model="confirmPassword"
        type="password"
        placeholder="Nhập lại mật khẩu"
        required
      />

      <BaseButton type="submit" :isLoading="isLoading"> Đăng ký </BaseButton>

      <div class="auth-footer">
        Đã có tài khoản?
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

.alert-error {
  background-color: var(--danger);
  color: white;
  padding: 0.75rem 1rem;
  border-radius: var(--radius-md);
  margin-bottom: 1rem;
  font-size: 0.875rem;
  text-align: center;
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
