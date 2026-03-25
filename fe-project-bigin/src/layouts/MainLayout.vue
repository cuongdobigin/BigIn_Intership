<script setup lang="ts">
import { RouterView } from 'vue-router'
import TheHeader from '../components/layout/TheHeader.vue'
import TheSidebar from '../components/layout/TheSidebar.vue'
import TheFooter from '../components/layout/TheFooter.vue'
</script>

<template>
  <div class="main-layout-wrapper">
    <!-- Header đã được tách ra component riêng -->
    <TheHeader />

    <div class="content-wrapper">
      <!-- Sidebar đã được tách ra component riêng -->
      <TheSidebar />

      <!-- Main Content Area -->
      <main class="main-content">
        <RouterView />
      </main>
    </div>

    <!-- Footer đã được tách ra component riêng -->
    <TheFooter />
  </div>
</template>

<style scoped>
/* Layout Architecture */
.main-layout-wrapper {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: var(--bg-body);
}

/* Content Area */
.content-wrapper {
  flex: 1;
  display: flex;
  max-width: 1280px;
  width: 100%;
  margin: 70px auto 0; /* Clear fixed header */
  padding: 2rem;
  gap: 2rem;
}

/* Main Content */
.main-content {
  flex: 1;
  min-width: 0;
}

/* Responsive adjustments if needed */
@media (max-width: 768px) {
  .content-wrapper {
    flex-direction: column;
    padding: 1rem;
  }
}
</style>
