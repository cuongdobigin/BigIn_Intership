<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { aiService, type MessageDto } from '../../api/aiService'
import { useAuthStore } from '../../stores/auth'

const auth = useAuthStore()
const isOpen = ref(false)
const inputMessage = ref('')
const isLoading = ref(false)
const chatHistory = ref<MessageDto[]>([
  { role: 'assistant', content: 'Xin chào! Tôi là trợ lý AI của BigInBook. Tôi có thể giúp gì cho bạn hôm nay?' }
])
const scrollRef = ref<HTMLElement | null>(null)

const toggleChat = () => {
  if (!auth.isLoggedIn) {
     alert('Vui lòng đăng nhập để sử dụng trợ lý AI.')
     return
  }
  isOpen.value = !isOpen.value
  if (isOpen.value) {
    scrollToBottom()
  }
}

const scrollToBottom = async () => {
  await nextTick()
  if (scrollRef.value) {
    scrollRef.value.scrollTop = scrollRef.value.scrollHeight
  }
}

const handleSendMessage = async () => {
  if (!inputMessage.value.trim() || isLoading.value) return

  const userMessage = inputMessage.value.trim()
  chatHistory.value.push({ role: 'user', content: userMessage })
  inputMessage.value = ''
  isLoading.value = true
  scrollToBottom()

  try {
    // Backend expects MessageDto[] History and string CurrentMessage
    // MessageDto role is "user" or "model" (assistant)
    const historyForBackend = chatHistory.value.slice(0, -1).map(m => ({
      role: (m.role === 'assistant' ? 'assistant' : 'user') as 'user' | 'assistant',
      content: m.content
    }))

    const response = await aiService.chatAsync({
      currentMessage: userMessage,
      history: historyForBackend
    })

    if (response.isSuccess) {
      chatHistory.value.push({ role: 'assistant', content: response.data.message })
    } else {
      chatHistory.value.push({ role: 'assistant', content: 'Rất tiếc, đã có lỗi xảy ra. Vui lòng thử lại sau.' })
    }
  } catch (error) {
    console.error('Chat error:', error)
    chatHistory.value.push({ role: 'assistant', content: 'Kết nối máy chủ thất bại.' })
  } finally {
    isLoading.value = false
    scrollToBottom()
  }
}
</script>

<template>
  <div class="ai-support-container" v-if="auth.isLoggedIn">
    <!-- Floating Icon -->
    <button 
      class="ai-fab" 
      @click="toggleChat"
      :class="{ 'is-open': isOpen }"
      title="Hỗ trợ AI"
    >
      <span class="ai-icon">🤖</span>
      <span class="ai-badge" v-if="!isOpen">AI</span>
    </button>

    <!-- Chatbox Window -->
    <Transition name="slide-fade">
      <div class="chat-window" v-if="isOpen">
        <div class="chat-header">
          <div class="header-info">
            <span class="header-bot-icon">🤖</span>
            <div>
              <h4>BigIn AI Support</h4>
              <span class="status-online">Đang trực tuyến</span>
            </div>
          </div>
          <button class="btn-close" @click="isOpen = false">✕</button>
        </div>

        <div class="chat-messages" ref="scrollRef">
          <div 
            v-for="(msg, index) in chatHistory" 
            :key="index"
            :class="['message-bubble', msg.role]"
          >
            <div class="bubble-content">
              {{ msg.content }}
            </div>
          </div>

          <div v-if="isLoading" class="message-bubble assistant">
            <div class="bubble-content loading-dots">
              <span class="dot"></span>
              <span class="dot"></span>
              <span class="dot"></span>
            </div>
          </div>
        </div>

        <div class="chat-footer">
          <form @submit.prevent="handleSendMessage">
            <input 
              v-model="inputMessage" 
              type="text" 
              placeholder="Nhập tin nhắn..." 
              :disabled="isLoading"
            />
            <button type="submit" :disabled="!inputMessage.trim() || isLoading">
              <span v-if="!isLoading">✈️</span>
              <span v-else class="spinner"></span>
            </button>
          </form>
        </div>
      </div>
    </Transition>
  </div>
</template>

<style scoped>
.ai-support-container {
  position: fixed;
  bottom: 2rem;
  right: 2rem;
  z-index: 9999;
}

/* Floating Action Button (FAB) */
.ai-fab {
  width: 65px;
  height: 65px;
  border-radius: 50%;
  background: linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%);
  border: none;
  box-shadow: 0 4px 15px rgba(59, 130, 246, 0.4);
  cursor: pointer;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  animation: floating 3s ease-in-out infinite;
}

.ai-fab:hover {
  transform: scale(1.1);
  box-shadow: 0 6px 20px rgba(59, 130, 246, 0.6);
}

.ai-fab.is-open {
  transform: rotate(360deg);
  background: #ef4444;
}

.ai-icon {
  font-size: 2rem;
}

.ai-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  background: #ef4444;
  color: white;
  font-size: 0.65rem;
  font-weight: 700;
  padding: 2px 6px;
  border-radius: 10px;
  border: 2px solid white;
}

/* Chat Window */
.chat-window {
  position: absolute;
  bottom: 80px;
  right: 0;
  width: 380px;
  height: 550px;
  background: white;
  border-radius: 1.25rem;
  box-shadow: 0 10px 40px rgba(0,0,0,0.15);
  display: flex;
  flex-direction: column;
  overflow: hidden;
  border: 1px solid rgba(0,0,0,0.05);
}

.chat-header {
  padding: 1.25rem;
  background: linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%);
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.header-bot-icon {
  font-size: 1.75rem;
}

.header-info h4 {
  margin: 0;
  font-size: 1rem;
}

.status-online {
  font-size: 0.75rem;
  opacity: 0.9;
  display: flex;
  align-items: center;
  gap: 4px;
}

.status-online::before {
  content: '';
  width: 8px;
  height: 8px;
  background: #22c55e;
  border-radius: 50%;
}

.btn-close {
  background: none;
  border: none;
  color: white;
  font-size: 1.2rem;
  cursor: pointer;
  opacity: 0.7;
}

.btn-close:hover { opacity: 1; }

.chat-messages {
  flex: 1;
  padding: 1.5rem;
  overflow-y: auto;
  background: #f8fafc;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.message-bubble {
  max-width: 85%;
  display: flex;
}

.message-bubble.user {
  align-self: flex-end;
}

.message-bubble.assistant {
  align-self: flex-start;
}

.bubble-content {
  padding: 0.75rem 1rem;
  border-radius: 1rem;
  font-size: 0.95rem;
  line-height: 1.4;
}

.user .bubble-content {
  background: #3b82f6;
  color: white;
  border-bottom-right-radius: 0.25rem;
}

.assistant .bubble-content {
  background: white;
  color: #1e293b;
  border-bottom-left-radius: 0.25rem;
  box-shadow: 0 2px 5px rgba(0,0,0,0.05);
}

.chat-footer {
  padding: 1rem;
  background: white;
  border-top: 1px solid #f1f5f9;
}

.chat-footer form {
  display: flex;
  gap: 0.5rem;
}

.chat-footer input {
  flex: 1;
  padding: 0.75rem 1rem;
  border: 1px solid #e2e8f0;
  border-radius: 2rem;
  outline: none;
  transition: border-color 0.2s;
}

.chat-footer input:focus {
  border-color: #3b82f6;
}

.chat-footer button {
  width: 45px;
  height: 45px;
  border-radius: 50%;
  background: #3b82f6;
  color: white;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s;
}

.chat-footer button:disabled {
  background: #cbd5e1;
  cursor: not-allowed;
}

/* Animations */
@keyframes floating {
  0% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
  100% { transform: translateY(0); }
}

.slide-fade-enter-active {
  transition: all 0.3s ease-out;
}
.slide-fade-leave-active {
  transition: all 0.2s cubic-bezier(1, 0.5, 0.8, 1);
}
.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateY(20px);
  opacity: 0;
}

/* Loading Dots */
.loading-dots {
  display: flex;
  gap: 4px;
  padding: 0.5rem 1rem;
}
.dot {
  width: 6px;
  height: 6px;
  background: #94a3b8;
  border-radius: 50%;
  animation: dot-pulse 1.5s infinite linear;
}
.dot:nth-child(2) { animation-delay: 0.2s; }
.dot:nth-child(3) { animation-delay: 0.4s; }

@keyframes dot-pulse {
  0%, 100% { transform: scale(1); opacity: 0.4; }
  50% { transform: scale(1.3); opacity: 1; }
}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid white;
  border-top: 2px solid rgba(255,255,255,0.3);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

@media (max-width: 480px) {
  .chat-window {
    width: calc(100vw - 2rem);
    height: 70vh;
    right: -1rem;
  }
}
</style>
