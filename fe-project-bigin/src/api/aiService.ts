import axiosClient from './axiosClient'
import type { ApiResponse } from './bookService'

export interface MessageDto {
  role: 'user' | 'assistant'
  content: string
}

export interface ChatHistoryRequest {
  currentMessage: string
  history: MessageDto[]
}

export interface ChatResponse {
  message: string
}

export const aiService = {
  chatAsync(payload: ChatHistoryRequest): Promise<ApiResponse<ChatResponse>> {
    return axiosClient.post('/api/chat', payload)
  }
}
