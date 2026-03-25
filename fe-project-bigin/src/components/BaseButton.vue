<script setup lang="ts">
defineProps<{
  isLoading?: boolean
  type?: 'button' | 'submit' | 'reset'
  variant?: 'primary' | 'secondary' | 'danger'
}>()
</script>

<template>
  <button 
    :type="type || 'button'" 
    class="btn" 
    :class="[`btn-${variant || 'primary'}`]"
    :disabled="isLoading"
  >
    <span v-if="isLoading" class="spinner"></span>
    <span v-else>
      <slot></slot>
    </span>
  </button>
</template>

<style scoped>
/* Spinner is scoped to the button component now for modularity */
.spinner {
  display: inline-block;
  width: 1.25rem;
  height: 1.25rem;
  border: 2px solid rgba(255,255,255,0.3);
  border-radius: 50%;
  border-top-color: #fff;
  animation: spin 1s ease-in-out infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>
