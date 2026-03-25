<template>
  <footer class="app-footer">
    <div class="footer-container">
      <div class="footer-col">
        <h4>Về BigInBook</h4>
        <p>Nơi cung cấp đa dạng các thể loại sách phục vụ việc học tập, giải trí và kiến thức vô tận.</p>
      </div>
      <div class="footer-col">
        <h4>Liên hệ</h4>
        <ul>
          <li>📍 Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM</li>
          <li>📞 CSHK: 1900 1234 (8:00 - 22:00)</li>
          <li>📧 Email: support@biginbook.vn</li>
        </ul>
      </div>
      <div class="footer-col">
        <h4>Hỗ trợ khách hàng</h4>
        <ul>
          <li><a href="#">Chính sách đổi trả</a></li>
          <li><a href="#">Chính sách bảo mật</a></li>
          <li><a href="#">Câu hỏi thường gặp</a></li>
        </ul>
      </div>
    </div>
    <div class="footer-bottom">
      &copy; 2026 BigInBook Online Store. All rights reserved.
    </div>
  </footer>
</template>

<style scoped>
.app-footer {
  background-color: #ffffff;
  border-top: 1px solid var(--border);
  margin-top: 2rem;
  padding: 3rem 0 1rem;
}

.footer-container {
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 2rem;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
  margin-bottom: 2rem;
}

.footer-col h4 {
  font-size: 1.125rem;
  margin-bottom: 1rem;
}

.footer-col ul {
  list-style: none;
}

.footer-col ul li, .footer-col p {
  color: var(--text-muted);
  margin-bottom: 0.5rem;
  line-height: 1.5;
}

.footer-col a {
  font-weight: 400;
  color: var(--text-muted);
}
.footer-col a:hover {
  color: var(--primary);
}

.footer-bottom {
  text-align: center;
  color: var(--text-muted);
  font-size: 0.875rem;
  padding-top: 1rem;
  border-top: 1px solid var(--border);
}
</style>
