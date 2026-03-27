import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService'

export interface DiscountResponse {
  id: number;
  minApply: number;
  maxApply: number;
  percent: number;
  isActive: boolean;
  startDate: string; // ISO Date string
  endDate: string; // ISO Date string
}

export interface DiscountRequest {
  minApply: number;
  maxApply: number;
  percent: number;
  startDate: string;
  endDate: string;
}

export const discountService = {
  getAllDiscounts(): Promise<ApiResponse<DiscountResponse[]>> {
    return axiosClient.get('/api/discounts')
  },
  
  getDiscountById(id: number): Promise<ApiResponse<DiscountResponse>> {
    return axiosClient.get(`/api/discounts/${id}`)
  },

  createDiscount(data: DiscountRequest): Promise<ApiResponse<DiscountResponse>> {
    return axiosClient.post('/api/discounts', data)
  },

  updateDiscount(id: number, data: DiscountRequest): Promise<ApiResponse<DiscountResponse>> {
    return axiosClient.put(`/api/discounts/${id}`, data)
  },

  deleteDiscount(id: number): Promise<ApiResponse<string>> {
    return axiosClient.delete(`/api/discounts/${id}`)
  }
}
