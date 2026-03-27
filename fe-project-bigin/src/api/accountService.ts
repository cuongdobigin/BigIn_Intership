import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService'
import type { UserResponse } from './userService'

export interface AccountResponse {
  id: number
  username: string
  isActive: boolean
  user?: UserResponse
}

export interface PageResponse<T> {
  data: T[]
  page: number
  pageSize: number
  totalItems: number
  totalPages: number
  hasNext: boolean
  hasPrevious: boolean
}

export const accountService = {
  getAllAccounts(page: number = 1, pageSize: number = 10): Promise<ApiResponse<PageResponse<AccountResponse>>> {
    return axiosClient.get(`/api/account`, {
      params: {
        page,
        pageSize
      }
    })
  },

  deleteAccount(accountId: number): Promise<ApiResponse<string>> {
    return axiosClient.delete(`/api/account/${accountId}`)
  },

  toggleAccountStatus(accountId: number, isActive: boolean): Promise<ApiResponse<string>> {
    return axiosClient.put(`/api/account/${accountId}`, { isActive })
  }
}
