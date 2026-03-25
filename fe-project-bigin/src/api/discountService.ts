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

export const discountService = {
  getAllDiscounts() {
    return axiosClient.get<ApiResponse<DiscountResponse[]>>('/discounts')
      .then(res => res.data)
  }
}
