# Dự án: Xây dựng website bán trà sữa ( ThanhMy MilkTea)

## 📝 Mô tả
Đây là website giới thiệu và đặt hàng các loại trà sữa của quán Thanh My MilkTea. Người dùng có thể xem thực đơn.

## 📁 Cấu trúc thư mục

ThanhMyMilkTea/
│
├── 📦 Connected Services/ # Dịch vụ tích hợp ngoài (Web APIs, Azure,...)
├── 📦 Dependencies/ # Các gói NuGet sử dụng
│
├── 🏷️ Properties/
│ └── 🧾 launchSettings.json # Cấu hình khi chạy ứng dụng
│
├── 🌐 wwwroot/ # Tài nguyên tĩnh phục vụ phía client
│ ├── 🎨 css/ # Giao diện CSS
│ ├── 🖼️ images/ # Hình ảnh
│ ├── 📜 js/ # File JavaScript
│ ├── 📚 lib/ # Thư viện frontend (Bootstrap, jQuery,...)
│ └── 🌟 favicon.ico # Biểu tượng trình duyệt
│
├── 🎮 Controllers/ # Xử lý điều hướng, logic backend
│ ├── 👤 AccountController.cs # Quản lý đăng nhập/đăng ký
│ ├── 🛡️ AdminController.cs # Giao diện & quyền của Admin
│ ├── 📦 DonHangController.cs # Quản lý đơn hàng
│ ├── 🛒 GioHangController.cs # Xử lý giỏ hàng
│ ├── 🏠 HomeController.cs # Trang chủ & trang tĩnh
│ ├── 👨‍💼 NhanVienController.cs # Quản lý nhân viên
│ └── 🧃 SanPhamController.cs # Quản lý sản phẩm
│
├─── 📦 Models/                           # Các lớp dữ liệu (Models)
│   ├── 🛒 CartItem.cs                  # Mục trong giỏ hàng
│   ├── 🧾 ChiTietHoaDon.cs             # Chi tiết hóa đơn
│   ├── 📂 DanhMucSp.cs                 # Danh mục sản phẩm
│   ├── ⚠️ ErrorViewModel.cs            # Model lỗi (dùng cho trang lỗi)
│   ├── 🧾 HoaDon.cs                    # Hóa đơn
│   ├── 👥 KhachHang.cs                 # Khách hàng
│   ├── 🏭 NhaCungCap.cs                # Nhà cung cấp
│   ├── 👨‍💼 NhanVien.cs                  # Nhân viên
│   ├── 👔 QuanLy.cs                    # Quản lý (admin hoặc user quản lý)
│   ├── 🧃 SanPham.cs                   # Sản phẩm
│   ├── 👤 TaiKhoan.cs                  # Tài khoản người dùng
│   └── 🧠 ThanhMyMilkTeaContext.cs    # DbContext – quản lý database
│
├── 🖼️ Views/                          # Giao diện Razor (.cshtml)
│   ├── 👤 Account/                    # View đăng nhập, đăng ký
│   ├── 🛡️ Admin/                      # View dành cho quản trị viên
│   ├── 📦 DonHang/                    # View quản lý đơn hàng
│   ├── 🛒 GioHang/                    # View giỏ hàng
│   ├── 🏠 Home/                      # View trang chủ và trang tĩnh
│   ├── 👨‍💼 NhanVien/                  # View quản lý nhân viên
│   ├── 🧃 SanPham/                   # View quản lý sản phẩm
│   ├── 📚 Shared/                    # Layouts và partial views dùng chung
│   ├── 🔧 _ViewImports.cshtml        # File import chung các namespace
│   └── 🔧 ViewStart.cshtml            # File cấu hình layout chung
│
├── 🛠️ appsettings.json # Cấu hình toàn cục (ConnectionString,...)
├── 🚀 Program.cs # Entry point khởi chạy ứng dụng
└── 🗂️ SQL.sql # Script tạo database và bảng mẫu
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
