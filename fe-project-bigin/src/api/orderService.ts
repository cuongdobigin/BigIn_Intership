import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService'
import type { ShoppingCartResponse } from './shoppingCartService'

export interface OrderRequest {
  idShoppingCart: number[];
  discountId?: number;
}

export interface OrderResponse {
  id: number;
  tax: number;
  subTotal: number;
  paymentTotal: number;
  paymentStatus: string;
  discountPercent: number;
  isActive: boolean;
  shoppingCarts: ShoppingCartResponse[];
}

export const orderService = {
  createOrder(request: OrderRequest): Promise<ApiResponse<OrderResponse>> {
    return axiosClient.post('/api/orders', request)
  },

  getMyOrders(): Promise<ApiResponse<OrderResponse[]>> {
    return axiosClient.get('/api/orders')
  },

  getOrderById(id: number): Promise<ApiResponse<OrderResponse>> {
    return axiosClient.get(`/api/orders/${id}`)
  }
}
