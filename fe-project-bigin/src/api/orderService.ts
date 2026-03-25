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
  createOrder(request: OrderRequest) {
    return axiosClient.post<ApiResponse<OrderResponse>>('/orders', request)
      .then(res => res.data)
  },

  getMyOrders() {
    return axiosClient.get<ApiResponse<OrderResponse[]>>('/orders')
      .then(res => res.data)
  },

  getOrderById(id: number) {
    return axiosClient.get<ApiResponse<OrderResponse>>(`/orders/${id}`)
      .then(res => res.data)
  }
}
