import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService'

export interface CreateUserRequest {
  name: string
  phone: string
  address: string
}

export interface UserResponse {
  id: number
  name: string
  phone: string
  address: string
  usernameAccount: string
}

export const userService = {
  createUserProfile(payload: CreateUserRequest) {
    return axiosClient.post('/api/users', payload)
  },
  
  findAllUsers(): Promise<ApiResponse<UserResponse[]>> {
    return axiosClient.get('/api/users')
  },

  deleteAccount(accountId: number): Promise<ApiResponse<string>> {
    return axiosClient.delete(`/api/account/${accountId}`)
  }
}
