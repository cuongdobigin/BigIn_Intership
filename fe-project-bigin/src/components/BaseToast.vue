<script setup lang="ts">
import { useToastStore } from '../stores/toast'

const toastStore = useToastStore()
</script>

<template>
  <div class="toast-container">
    <TransitionGroup name="toast">
      <div 
        v-for="toast in toastStore.toasts" 
        :key="toast.id" 
        class="toast-item"
        :class="`toast-${toast.type}`"
      >
        <div class="toast-content">
          <span v-if="toast.type === 'success'" class="icon">✅</span>
          <span v-else-if="toast.type === 'error'" class="icon">❌</span>
          <span v-else-if="toast.type === 'warning'" class="icon">⚠️</span>
          <span v-else class="icon">ℹ️</span>
          <p>{{ toast.message }}</p>
        </div>
      </div>
    </TransitionGroup>
  </div>
</template>

<style scoped>
.toast-container {
  position: fixed;
  bottom: 2rem;
  right: 2rem;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  pointer-events: none;
}

.toast-item {
  min-width: 280px;
  max-width: 400px;
  padding: 1rem 1.25rem;
  border-radius: var(--radius-lg);
  background: white;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
  pointer-events: auto;
  border-left: 4px solid #ddd;
  overflow: hidden;
}

.toast-content {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.toast-content p {
  margin: 0;
  font-size: 0.95rem;
  font-weight: 500;
  color: var(--text-main);
}

.toast-success { border-left-color: #10b981; }
.toast-error { border-left-color: #ef4444; }
.toast-warning { border-left-color: #f59e0b; }
.toast-info { border-left-color: #3b82f6; }

/* Transitions */
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.toast-enter-from {
  opacity: 0;
  transform: translateX(30px) scale(0.9);
}

.toast-leave-to {
  opacity: 0;
  transform: translateX(30px);
}
</style>
