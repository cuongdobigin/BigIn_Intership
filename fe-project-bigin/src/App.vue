<script setup lang="ts">
import { useRoute } from 'vue-router'
import AuthLayout from './layouts/AuthLayout.vue'
import MainLayout from './layouts/MainLayout.vue'
import BaseToast from './components/BaseToast.vue'
import { computed } from 'vue'

const route = useRoute()
// Optionally, you can add dynamic layouts if there are more than just auth pages.
// Here we assume everything right now uses AuthLayout.
const isAuthRoute = computed(() => ['login', 'register', 'change-password', 'profile-setup'].includes(route.name as string))
</script>

<template>
  <AuthLayout v-if="isAuthRoute" />
  <MainLayout v-else />
  <BaseToast />
</template>

<style>
/* App specific global resets or fonts can be added here or in main.css */
</style>
