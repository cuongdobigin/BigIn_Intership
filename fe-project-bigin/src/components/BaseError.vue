<script setup lang="ts">
defineProps<{
  message: string
}>()
</script>

<template>
  <div class="error-banner">
    ⚠️ {{ message }}
  </div>
</template>

<style scoped>
.error-banner {
  background-color: #fee2e2;
  border: 1px solid #fecaca;
  color: #b91c1c;
  padding: 1rem;
  border-radius: var(--radius-md);
  text-align: center;
  font-weight: 600;
}
</style>
