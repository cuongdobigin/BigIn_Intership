import axiosClient from './axiosClient'

export interface CreateUserRequest {
  name: string
  phone: string
  address: string
}

export const userService = {
  createUserProfile(payload: CreateUserRequest) {
    return axiosClient.post('/api/users', payload)
  },
}
