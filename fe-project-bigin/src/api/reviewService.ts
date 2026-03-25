import axiosClient from './axiosClient'
import type { ApiResponse, PaginatedResponse } from './bookService'

export interface ReviewResponse {
  id: number
  message: string
  isActive: boolean
  createdAt: string
  updatedAt: string
  accountUsername: string
  bookId: number
}

export const reviewService = {
  getReviewsByBookId(bookId: number, page: number = 1, pageSize: number = 5): Promise<ApiResponse<PaginatedResponse<ReviewResponse>>> {
    return axiosClient.get(`/api/reviews/book/${bookId}?page=${page}&pageSize=${pageSize}`)
  },

  createReview(bookId: number, message: string): Promise<ApiResponse<ReviewResponse>> {
    return axiosClient.post(`/api/reviews/${bookId}`, { message })
  },

  updateReview(id: number, message: string): Promise<ApiResponse<ReviewResponse>> {
    return axiosClient.put(`/api/reviews/${id}`, { message })
  },

  deleteReview(id: number): Promise<ApiResponse<string>> {
    return axiosClient.delete(`/api/reviews/${id}`)
  }
}
