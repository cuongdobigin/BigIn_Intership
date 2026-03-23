import axiosClient from './axiosClient'

export interface LoginPayload {
  username: string
  password: string
}

export interface RegisterPayload {
  username: string
  password: string
}

export interface ChangePasswordPayload {
  username: string
  oldPassword: string
  newPassword: string
}

export interface LoginResponse {
  token: string
  isFirstTime: boolean
}

export const authService = {
  login(payload: LoginPayload) {
    return axiosClient.post('/api/auth/login', payload) as Promise<any>
  },

  register(payload: RegisterPayload) {
    return axiosClient.post('/api/account', payload)
  },

  changePassword(payload: ChangePasswordPayload) {
    return axiosClient.post('/api/account/change-password', payload)
  },

  refreshToken() {
    return axiosClient.post<{ data: { token: string } }>('/api/auth/refresh-token')
  },
}
