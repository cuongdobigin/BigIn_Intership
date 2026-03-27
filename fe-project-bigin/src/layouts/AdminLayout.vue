<script setup lang="ts">
import { RouterView } from 'vue-router'
import TheAdminHeader from '../components/layout/TheAdminHeader.vue'
import TheAdminSidebar from '../components/layout/TheAdminSidebar.vue'
import TheFooter from '../components/layout/TheFooter.vue'
import AISupport from '../components/chat/AISupport.vue'
</script>

<template>
  <div class="admin-layout-wrapper">
    <TheAdminHeader />

    <div class="content-wrapper">
      <TheAdminSidebar />

      <main class="main-content">
        <RouterView />
      </main>
    </div>

    <!-- AI Support Chatbox -->
    <AISupport />

    <TheFooter />
  </div>
</template>

<style scoped>
.admin-layout-wrapper {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: var(--bg-body);
}

.content-wrapper {
  flex: 1;
  display: flex;
  max-width: 1280px;
  width: 100%;
  margin: 70px auto 0;
  padding: 2rem;
  gap: 2rem;
}

.main-content {
  flex: 1;
  min-width: 0;
  background: white;
  padding: 2rem;
  border-radius: var(--radius-md);
  border: 1px solid var(--border);
}

@media (max-width: 768px) {
  .content-wrapper {
    flex-direction: column;
    padding: 1rem;
  }
}
</style>
