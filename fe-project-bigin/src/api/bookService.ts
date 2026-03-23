import axiosClient from './axiosClient'

export interface BookType {
  typeId: number
  name: string
  description: string
  isActive: boolean
}

export interface Book {
  id: number
  name: string
  price: number
  description: string
  stock: boolean
  author: string
  avatar: string
}

export interface PaginatedResponse<T> {
  data: T[]
  page: number
  pageSize: number
  totalItems: number
  totalPages: number
  hasNext: boolean
  hasPrevious: boolean
}

export interface ApiResponse<T> {
  isSuccess: boolean
  message: string
  code: number
  data: T
}

export const bookService = {
  getTypeBooks(): Promise<ApiResponse<BookType[]>> {
    return axiosClient.get('/api/type-books')
  },

  getBooks(page: number, pageSize: number, typeId?: number): Promise<ApiResponse<PaginatedResponse<Book>>> {
    let url = `/api/books?page=${page}&pageSize=${pageSize}`
    
    // Nếu có typeId, sử dụng endpoint chuyên biệt cho danh mục
    if (typeId) {
      url = `/api/books/type-books/${typeId}?page=${page}&pageSize=${pageSize}`
    }
    
    console.log('Calling API:', url)
    return axiosClient.get(url)
  },
}
