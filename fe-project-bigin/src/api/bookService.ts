import axiosClient from './axiosClient'

export interface BookType {
  typeId: number;
  name: string;
  description: string;
  isActive: boolean;
}

export interface BookImage {
  id: number;
  link: string;
}

export interface Book {
  id: number;
  name: string;
  author: string;
  price: number;
  quantity: number;
  description: string;
  typeBookId?: number;
  images: BookImage[];
  isActive: boolean;
}

export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  message: string;
  data: T;
}

export const bookService = {
  getBooks(page: number, pageSize: number, typeId?: number): Promise<ApiResponse<PaginatedResponse<Book>>> {
    let url = `/api/books?page=${page}&pageSize=${pageSize}`

    if (typeId) {
      url = `/api/books/type-books/${typeId}?page=${page}&pageSize=${pageSize}`
    }

    return axiosClient.get(url)
  },

  getBooksAdmin(page: number, pageSize: number, typeId?: number): Promise<ApiResponse<PaginatedResponse<Book>>> {
    if (typeId) {
      return axiosClient.get(`/api/books/type-books/admin/${typeId}?page=${page}&pageSize=${pageSize}`)
    }
    return this.getBooks(page, pageSize)
  },

  getBookById(id: number): Promise<ApiResponse<Book>> {
    return axiosClient.get(`/api/books/${id}`)
  },

  getTypeBooks(): Promise<ApiResponse<BookType[]>> {
    return axiosClient.get('/api/type-books')
  },

  getTypeBooksAdmin(): Promise<ApiResponse<BookType[]>> {
    // Note: The backend endpoint has a leading slash overriding the controller route
    return axiosClient.get('/admin')
  },

  createBook(data: any): Promise<ApiResponse<Book>> {
    return axiosClient.post('/api/books', data)
  },

  updateBook(id: number, data: any): Promise<ApiResponse<Book>> {
    return axiosClient.put(`/api/books/${id}`, data)
  },

  createTypeBook(data: any): Promise<ApiResponse<BookType>> {
    return axiosClient.post('/api/type-books', data)
  },

  updateTypeBook(id: number, data: any): Promise<ApiResponse<BookType>> {
    return axiosClient.put(`/api/type-books/${id}`, data)
  }
}
