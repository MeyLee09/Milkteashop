# Dự án: Xây dựng website bán trà sữa ( ThanhMy MilkTea)

## 📝 Mô tả
Đây là website giới thiệu và đặt hàng các loại trà sữa của quán Meylee. Người dùng có thể xem thực đơn.

## 📁 Cấu trúc thư mục
ThanhMyMilkTea/
│
├── Connected Services/ # Dịch vụ được kết nối như Azure, Web API...
├── Dependencies/ # Các gói NuGet phụ thuộc
├── Properties/
│ └── launchSettings.json # Thiết lập khi chạy ứng dụng
│
├── wwwroot/ # Tài nguyên tĩnh phục vụ frontend
│ ├── css/ # Stylesheets
│ ├── images/ # Hình ảnh giao diện
│ ├── js/ # JavaScript
│ ├── lib/ # Thư viện frontend (Bootstrap, jQuery,...)
│ └── favicon.ico # Biểu tượng trang web
│
├── Controllers/ # Các controller điều khiển logic ứng dụng
│ ├── AccountController.cs
│ ├── AdminController.cs
│ ├── DonHangController.cs
│ ├── GioHangController.cs
│ ├── HomeController.cs
│ ├── NhanVienController.cs
│ └── SanPhamController.cs
│
├── Models/ # Các lớp mô hình dữ liệu
│
├── Views/ # Giao diện người dùng (Razor Views)
│
├── appsettings.json # Cấu hình ứng dụng (chuỗi kết nối CSDL,...)
├── Program.cs # Điểm khởi đầu của ứng dụng
└── SQL.sql # Tập tin chứa câu lệnh SQL để tạo/tạo mẫu CSDL


---

## 🚀 Hướng dẫn chạy dự án

### 1. Yêu cầu
- Visual Studio 2022 trở lên
- .NET 6.0 trở lên
- SQL Server

### 2. Các bước chạy
1. Clone hoặc mở project bằng Visual Studio.
2. Đảm bảo file `appsettings.json` đã cấu hình đúng chuỗi kết nối đến SQL Server.
3. Chạy script `SQL.sql` để tạo database (nếu có).
4. Nhấn `F5` hoặc chọn **IIS Express** để chạy ứng dụng.

---

## 📌 Thông tin bổ sung

- **Framework:** ASP.NET Core MVC
- **Cơ sở dữ liệu:** SQL Server
- **Tính năng chính:**
  - Quản lý người dùng (Admin, nhân viên)
  - Xem và đặt sản phẩm
  - Giỏ hàng và xử lý đơn hàng
  - Trang admin quản lý toàn hệ thống

---


## 👨‍💻 Tác giả
- **Họ tên:** Lê Thị Thanh My
- **Lớp:** DT23TTG10
- **Trường:** Đại học Trà Vinh
- **Email:** myltt260902@sv-onuni.edu.vn
- **Điện thoại:** 0373452348

## 🗓️ Ngày tạo
Tháng 8, 2025
