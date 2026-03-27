import axiosClient from './axiosClient'
import type { ApiResponse, ImageResponse } from './bookService'

export interface ImageRequest {
  bookId: number
  link: string
}

export interface BulkImageRequest {
  bookId: number
  links: string[]
}

export const imageService = {
  createImage(data: ImageRequest): Promise<ApiResponse<ImageResponse>> {
    return axiosClient.post('/api/images', data)
  },

  createImages(data: BulkImageRequest): Promise<ApiResponse<ImageResponse[]>> {
    return axiosClient.post('/api/images/bulk', data)
  },

  getImagesByBookId(bookId: number): Promise<ApiResponse<ImageResponse[]>> {
    return axiosClient.get(`/api/images/book/${bookId}`)
  },

  deleteImage(id: number): Promise<ApiResponse<string>> {
    return axiosClient.delete(`/api/images/${id}`)
  }
}
