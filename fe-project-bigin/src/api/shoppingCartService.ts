import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService' // assumes ApiResponse is exported or can be created here

export interface ShoppingCartRequest {
  bookId: number;
  amount: number;
}

export interface ShoppingCartResponse {
  id: number;
  amount: number;
  isActive: boolean;
  bookId: number;
  bookName: string;
  bookPrice: number;
}

export const shoppingCartService = {
  addToCart(request: ShoppingCartRequest) {
    return axiosClient.post<ApiResponse<ShoppingCartResponse>>('/cart', request)
      .then(res => res.data)
  },

  getMyCart() {
    return axiosClient.get<ApiResponse<ShoppingCartResponse[]>>('/cart')
      .then(res => res.data)
  },

  updateCartItem(id: number, request: ShoppingCartRequest) {
    return axiosClient.put<ApiResponse<ShoppingCartResponse>>(`/cart/${id}`, request)
      .then(res => res.data)
  },

  removeFromCart(id: number) {
    return axiosClient.delete<ApiResponse<string>>(`/cart/${id}`)
      .then(res => res.data)
  }
}
