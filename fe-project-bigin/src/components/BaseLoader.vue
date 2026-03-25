<script setup lang="ts">
defineProps<{
  message?: string
}>()
</script>

<template>
  <div class="loader-container">
    <div class="spinner"></div>
    <span class="loader-text">{{ message || 'Đang tải...' }}</span>
  </div>
</template>

<style scoped>
.loader-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  padding: 2rem;
  width: 100%;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid var(--primary);
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

.loader-text {
  color: var(--primary);
  font-weight: 500;
  font-size: 0.875rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
