using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;

namespace ThanhMyMilkTea.Controllers
{
    public class AccountController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;

        public AccountController(ThanhMyMilkTeaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _context.TaiKhoans
                    .FirstOrDefaultAsync(x => x.Username == username &&
                                            x.Password == password &&
                                            x.TrangThai == true);

                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("LoaiTaiKhoan", user.LoaiTaiKhoan);

                    if (user.LoaiTaiKhoan == "ADMIN")
                        return RedirectToAction("Index", "Admin");
                    else if (user.LoaiTaiKhoan == "NHANVIEN")
                        return RedirectToAction("Index", "NhanVien");
                    else
                        return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra trong quá trình đăng nhập!";
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string tenKH, string email, string sdt, string diaChi)
        {
            // Validation input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(tenKH) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin bắt buộc!";
                return View();
            }

            // Validation email format
            if (!IsValidEmail(email))
            {
                ViewBag.Error = "Email không đúng định dạng!";
                return View();
            }

            // Validation password length
            if (password.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự!";
                return View();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra username đã tồn tại
                var existUser = await _context.TaiKhoans
                    .FirstOrDefaultAsync(x => x.Username == username);

                if (existUser != null)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                    return View();
                }

                // Kiểm tra email đã tồn tại
                var existEmail = await _context.KhachHangs
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (existEmail != null)
                {
                    ViewBag.Error = "Email đã được sử dụng!";
                    return View();
                }

                // Bước 1: Tạo và lưu TaiKhoan trước
                var taiKhoan = new TaiKhoan
                {
                    Username = username,
                    Password = password, // Trong thực tế nên hash password
                    LoaiTaiKhoan = "KHACHHANG",
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync(); // Lưu TaiKhoan trước

                // Bước 2: Tạo KhachHang sau khi TaiKhoan đã được lưu
                var khachHang = new KhachHang
                {
                    MaKh = "KH" + DateTime.Now.Ticks.ToString().Substring(10),
                    TenKh = tenKH,
                    Email = email,
                    Sdt = sdt ?? "", // Đảm bảo không null
                    DiaChi = diaChi ?? "", // Đảm bảo không null
                    NgayDangKy = DateTime.Now,
                    DiemTichLuy = 0,
                    Username = username, // Liên kết với TaiKhoan đã tồn tại
                    TrangThai = true
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync(); // Lưu KhachHang

                // Commit transaction
                await transaction.CommitAsync();

                ViewBag.Success = "Đăng ký thành công! Vui lòng đăng nhập.";
                return View("Login");
            }
            catch (Exception ex)
            {
                // Rollback transaction nếu có lỗi
                await transaction.RollbackAsync();

                ViewBag.Error = "Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Helper method để validate email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Alternative approach - Sử dụng một SaveChanges duy nhất với đúng thứ tự
        [HttpPost]
        public async Task<IActionResult> RegisterAlternative(string username, string password, string tenKH, string email, string sdt, string diaChi)
        {
            // Validation tương tự...
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(tenKH) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin bắt buộc!";
                return View("Register");
            }

            try
            {
                // Kiểm tra username đã tồn tại
                var existUser = await _context.TaiKhoans
                    .FirstOrDefaultAsync(x => x.Username == username);

                if (existUser != null)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                    return View("Register");
                }

                // Tạo TaiKhoan
                var taiKhoan = new TaiKhoan
                {
                    Username = username,
                    Password = password,
                    LoaiTaiKhoan = "KHACHHANG",
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };

                // Tạo KhachHang với navigation property
                var khachHang = new KhachHang
                {
                    MaKh = "KH" + DateTime.Now.Ticks.ToString().Substring(10),
                    TenKh = tenKH,
                    Email = email,
                    Sdt = sdt ?? "",
                    DiaChi = diaChi ?? "",
                    NgayDangKy = DateTime.Now,
                    DiemTichLuy = 0,
                    Username = username,
                    TrangThai = true,
                };

                // Thêm cả hai vào context
                _context.TaiKhoans.Add(taiKhoan);
                _context.KhachHangs.Add(khachHang);

                // Lưu một lần duy nhất - EF sẽ xử lý thứ tự
                await _context.SaveChangesAsync();

                ViewBag.Success = "Đăng ký thành công! Vui lòng đăng nhập.";
                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại!";
                return View("Register");
            }
        }
    }
}