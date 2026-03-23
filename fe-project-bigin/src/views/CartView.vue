<template>
  <div class="cart-view">
    <div class="empty-cart">
      <h2>🛒 Giỏ hàng của bạn trống</h2>
      <p>Chưa có sản phẩm nào trong giỏ hàng. Hãy chọn sách bạn yêu thích nhé!</p>
      <RouterLink to="/" class="btn btn-outline" style="width: auto; margin-top: 1rem; display: inline-flex;">Tiếp tục mua sắm</RouterLink>
    </div>
  </div>
</template>

<style scoped>
.cart-view {
  background: var(--bg-surface);
  border: 1px solid var(--border);
  padding: 3rem 2rem;
  border-radius: var(--radius-md);
  text-align: center;
  min-height: 400px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.empty-cart h2 {
  margin-bottom: 1rem;
  font-size: 1.5rem;
}

.empty-cart p {
  color: var(--text-muted);
}

.btn-outline {
  background: transparent;
  color: var(--primary);
  border: 1px solid var(--primary);
}

.btn-outline:hover {
  background: var(--bg-body);
}
</style>
