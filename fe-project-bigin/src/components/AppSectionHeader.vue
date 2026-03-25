<script setup lang="ts">
defineProps<{
  title: string
  showViewAll?: boolean
  viewAllLink?: string
}>()
</script>

<template>
  <div class="section-header">
    <h2>{{ title }}</h2>
    <RouterLink v-if="showViewAll && viewAllLink" :to="viewAllLink" class="view-all">
      Xem tất cả →
    </RouterLink>
    <slot name="extra"></slot>
  </div>
</template>

<style scoped>
.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  border-bottom: 2px solid var(--primary);
  padding-bottom: 0.5rem;
}

.section-header h2 {
  font-size: 1.5rem;
  margin: 0;
}

.view-all {
  color: var(--primary);
  font-size: 0.875rem;
  text-decoration: none;
  font-weight: 500;
}

.view-all:hover {
  text-decoration: underline;
}
</style>
