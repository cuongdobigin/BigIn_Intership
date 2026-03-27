import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

function decodeJWT(token: string) {
  try {
    const parts = token.split('.')
    if (parts.length !== 3) return null

    const base64Url = parts[1]
    if (!base64Url) return null
    
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        })
        .join(''),
    )
    return JSON.parse(jsonPayload)
  } catch (e) {
    return null
  }
}

export const useAuthStore = defineStore('auth', () => {
  const username = ref(localStorage.getItem('username') || '')
  const token = ref(localStorage.getItem('accessToken') || '')
  
  // Decoding role from token on initialization if possible
  const getInitialRole = () => {
    const savedToken = localStorage.getItem('accessToken')
    if (savedToken) {
      const decoded = decodeJWT(savedToken)
      return decoded?.role || 
             decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
             localStorage.getItem('userRole') || 
             ''
    }
    return ''
  }

  const role = ref(getInitialRole())

  const isLoggedIn = computed(() => !!token.value)
  const isUser = computed(() => role.value?.toLowerCase() === 'user')
  const isAdmin = computed(() => role.value?.toLowerCase() === 'admin')

  const setUser = (name: string, authToken: string) => {
    username.value = name
    token.value = authToken

    const decoded = decodeJWT(authToken)
    console.log('Decoded Token:', decoded)

    const userRole =
      decoded?.role ||
      decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
      'user'

    role.value = userRole

    localStorage.setItem('username', name)
    localStorage.setItem('accessToken', authToken)
    localStorage.setItem('userRole', userRole)
  }

  const logout = () => {
    username.value = ''
    token.value = ''
    role.value = ''
    localStorage.removeItem('username')
    localStorage.removeItem('accessToken')
    localStorage.removeItem('userRole')
  }

  return {
    username,
    token,
    role,
    isLoggedIn,
    isUser,
    isAdmin,
    setUser,
    logout,
  }
})
