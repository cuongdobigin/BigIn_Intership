<script setup lang="ts">
defineProps<{
  modelValue: string | number
  label?: string
  id?: string
  type?: string
  placeholder?: string
  required?: boolean
  minlength?: string | number
}>()

defineEmits<{
  (e: 'update:modelValue', value: string): void
}>()
</script>

<template>
  <div class="form-group">
    <label v-if="label" class="form-label" :for="id">{{ label }}</label>
    <input
      :id="id"
      :type="type || 'text'"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
      class="form-input"
      :placeholder="placeholder"
      :required="required"
      :minlength="minlength"
    />
  </div>
</template>

<style scoped>
/* Inherits .form-group, .form-label, .form-input from base/assets CSS */
</style>
