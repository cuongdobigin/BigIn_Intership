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
  bookImage?: string;
}

export const shoppingCartService = {
  addToCart(request: ShoppingCartRequest): Promise<ApiResponse<ShoppingCartResponse>> {
    return axiosClient.post('/api/cart', request)
  },
 
  getMyCart(): Promise<ApiResponse<ShoppingCartResponse[]>> {
    return axiosClient.get('/api/cart')
  },
 
  updateCartItem(id: number, request: ShoppingCartRequest): Promise<ApiResponse<ShoppingCartResponse>> {
    return axiosClient.put(`/api/cart/${id}`, request)
  },
 
  removeFromCart(id: number): Promise<ApiResponse<string>> {
    return axiosClient.delete(`/api/cart/${id}`)
  }
}
