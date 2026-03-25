import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService'

export interface MoMoResponse {
  url: string;
}

export const paymentService = {
  createMoMoPayment(orderId: number): Promise<ApiResponse<MoMoResponse>> {
    return axiosClient.post(`/api/payment/create/${orderId}`)
  }
}
