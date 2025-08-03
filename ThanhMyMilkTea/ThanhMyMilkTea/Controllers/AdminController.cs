using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;

namespace ThanhMyMilkTea.Controllers
{
    public class AdminController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ThanhMyMilkTeaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            ViewBag.TotalSanPham = await _context.SanPhams.CountAsync();
            ViewBag.TotalDonHang = await _context.HoaDons.CountAsync();
            ViewBag.TotalKhachHang = await _context.KhachHangs.CountAsync();
            ViewBag.DoanhThu = await _context.HoaDons.SumAsync(x => x.ThanhTien ?? 0);

            return View();
        }

        #region Quản lý Sản phẩm
        public async Task<IActionResult> SanPham(int page = 1, string search = "")
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            int pageSize = 5;
            var query = _context.SanPhams
                .Include(x => x.MaDanhMucNavigation)
                .Include(x => x.MaNccNavigation);

            if (!string.IsNullOrEmpty(search))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<SanPham, NhaCungCap?>)query.Where(x => x.TenSp.Contains(search));
                ViewBag.Search = search;
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var sanPhams = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(sanPhams);
        }

        public async Task<IActionResult> CreateSanPham()
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            ViewBag.DanhMucs = await _context.DanhMucSps.Where(x => x.TrangThai == true).ToListAsync();
            ViewBag.NhaCungCaps = await _context.NhaCungCaps.Where(x => x.TrangThai == true).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSanPham(SanPham sanPham, IFormFile? hinhAnh)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            sanPham.MaSp = "SP" + DateTime.Now.Ticks.ToString().Substring(10);
            sanPham.TrangThai = true;

            // Xử lý upload hình ảnh
            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                string fileName = await SaveImageAsync(hinhAnh);
                sanPham.HinhAnh = fileName;
            }

            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction("SanPham");
        }

        public async Task<IActionResult> EditSanPham(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null) return NotFound();

            ViewBag.DanhMucs = await _context.DanhMucSps.Where(x => x.TrangThai == true).ToListAsync();
            ViewBag.NhaCungCaps = await _context.NhaCungCaps.Where(x => x.TrangThai == true).ToListAsync();
            return View(sanPham);
        }

        [HttpPost]
        public async Task<IActionResult> EditSanPham(SanPham sanPham, IFormFile? hinhAnh)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var existingProduct = await _context.SanPhams.FindAsync(sanPham.MaSp);
            if (existingProduct == null) return NotFound();

            // Xử lý upload hình ảnh mới
            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                // Xóa hình cũ nếu có
                if (!string.IsNullOrEmpty(existingProduct.HinhAnh))
                {
                    DeleteImage(existingProduct.HinhAnh);
                }

                // Lưu hình mới
                string fileName = await SaveImageAsync(hinhAnh);
                sanPham.HinhAnh = fileName;
            }
            else
            {
                // Giữ nguyên hình cũ nếu không upload hình mới
                sanPham.HinhAnh = existingProduct.HinhAnh;
            }
            sanPham.TrangThai = true;
            _context.Entry(existingProduct).CurrentValues.SetValues(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction("SanPham");
        }
        // Method xử lý lưu hình ảnh
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file unique
            string fileExtension = Path.GetExtension(imageFile.FileName);
            string fileName = $"{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Lưu file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return fileName;
        }

        // Method xóa hình ảnh
        private void DeleteImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
        public async Task<IActionResult> ToggleSanPham(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                sanPham.TrangThai = !sanPham.TrangThai;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("SanPham");
        }
        #endregion

        #region Quản lý Danh mục
        public async Task<IActionResult> DanhMuc(int page = 1)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            int pageSize = 5;
            var query = _context.DanhMucSps.OrderBy(x => x.TenDanhMuc);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var danhMucs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(danhMucs);
        }

        public IActionResult CreateDanhMuc()
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDanhMuc(DanhMucSp danhMuc)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            danhMuc.MaDanhMuc = "DM" + DateTime.Now.Ticks.ToString().Substring(10);
            danhMuc.TrangThai = true;

            _context.DanhMucSps.Add(danhMuc);
            await _context.SaveChangesAsync();

            return RedirectToAction("DanhMuc");
        }

        public async Task<IActionResult> EditDanhMuc(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var danhMuc = await _context.DanhMucSps.FindAsync(id);
            if (danhMuc == null) return NotFound();
            return View(danhMuc);
        }

        [HttpPost]
        public async Task<IActionResult> EditDanhMuc(DanhMucSp danhMuc)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");
            danhMuc.TrangThai = true;
            _context.DanhMucSps.Update(danhMuc);
            await _context.SaveChangesAsync();

            return RedirectToAction("DanhMuc");
        }

        public async Task<IActionResult> ToggleDanhMuc(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var danhMuc = await _context.DanhMucSps.FindAsync(id);
            if (danhMuc != null)
            {
                danhMuc.TrangThai = !danhMuc.TrangThai;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DanhMuc");
        }
        #endregion

        #region Quản lý Nhà cung cấp
        public async Task<IActionResult> NhaCungCap(int page = 1)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            int pageSize = 6;
            var query = _context.NhaCungCaps.OrderBy(x => x.TenNcc);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var nhaCungCaps = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(nhaCungCaps);
        }

        public IActionResult CreateNhaCungCap()
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNhaCungCap(NhaCungCap nhaCungCap)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            nhaCungCap.MaNcc = "NCC" + DateTime.Now.Ticks.ToString().Substring(10);
            nhaCungCap.TrangThai = true;

            _context.NhaCungCaps.Add(nhaCungCap);
            await _context.SaveChangesAsync();

            return RedirectToAction("NhaCungCap");
        }

        public async Task<IActionResult> EditNhaCungCap(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null) return NotFound();
            return View(nhaCungCap);
        }

        [HttpPost]
        public async Task<IActionResult> EditNhaCungCap(NhaCungCap nhaCungCap)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");
            nhaCungCap.TrangThai = true;
            _context.NhaCungCaps.Update(nhaCungCap);
            await _context.SaveChangesAsync();

            return RedirectToAction("NhaCungCap");
        }

        public async Task<IActionResult> ToggleNhaCungCap(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap != null)
            {
                nhaCungCap.TrangThai = !nhaCungCap.TrangThai;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("NhaCungCap");
        }
        #endregion

        #region Quản lý Nhân viên
        public async Task<IActionResult> NhanVien(int page = 1)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            int pageSize = 5;
            var query = _context.NhanViens
                .Include(x => x.MaQlNavigation)
                .OrderBy(x => x.TenNv);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var nhanViens = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(nhanViens);
        }

        public async Task<IActionResult> CreateNhanVien()
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            ViewBag.QuanLys = await _context.QuanLies
                .Where(x => x.TrangThai == true)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNhanVien(string username, string password, string TenNv, string Email, string Sdt, string DiaChi, string ChucVu, decimal? Luong, DateTime? NgayVaoLam, string MaQl)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            // Validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(TenNv))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin bắt buộc (Username, Password, Tên nhân viên)!";
                ViewBag.QuanLys = await _context.QuanLies.Where(x => x.TrangThai == true).ToListAsync();
                return View();
            }

            if (password.Length < 6)
            {
                TempData["Error"] = "Mật khẩu phải có ít nhất 6 ký tự!";
                ViewBag.QuanLys = await _context.QuanLies.Where(x => x.TrangThai == true).ToListAsync();
                return View();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra username đã tồn tại
                var existUser = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.Username == username);
                if (existUser != null)
                {
                    TempData["Error"] = "Tên đăng nhập đã tồn tại!";
                    ViewBag.QuanLys = await _context.QuanLies.Where(x => x.TrangThai == true).ToListAsync();
                    return View();
                }

                // Kiểm tra email đã tồn tại (nếu có)
                if (!string.IsNullOrEmpty(Email))
                {
                    var existEmail = await _context.NhanViens.FirstOrDefaultAsync(x => x.Email == Email);
                    if (existEmail != null)
                    {
                        TempData["Error"] = "Email đã được sử dụng!";
                        ViewBag.QuanLys = await _context.QuanLies.Where(x => x.TrangThai == true).ToListAsync();
                        return View();
                    }
                }

                // Bước 1: Tạo TaiKhoan
                var taiKhoan = new TaiKhoan
                {
                    Username = username,
                    Password = password, // Trong thực tế nên hash password
                    LoaiTaiKhoan = "NHANVIEN",
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync(); // Lưu TaiKhoan trước

                // Bước 2: Tạo NhanVien
                var nhanVien = new NhanVien
                {
                    MaNv = "NV" + DateTime.Now.Ticks.ToString().Substring(10),
                    TenNv = TenNv,
                    Email = Email,
                    Sdt = Sdt,
                    DiaChi = DiaChi,
                    ChucVu = ChucVu,
                    Luong = Luong,
                    NgayVaoLam = NgayVaoLam ?? DateTime.Now,
                    MaQl = string.IsNullOrEmpty(MaQl) ? null : MaQl,
                    Username = username, // Liên kết với TaiKhoan
                    TrangThai = true
                };

                _context.NhanViens.Add(nhanVien);
                await _context.SaveChangesAsync(); // Lưu NhanVien

                await transaction.CommitAsync();
                TempData["Success"] = "Thêm nhân viên thành công!";
                return RedirectToAction("NhanVien");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.QuanLys = await _context.QuanLies.Where(x => x.TrangThai == true).ToListAsync();
                return View();
            }
        }

        public async Task<IActionResult> EditNhanVien(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null) return NotFound();

            ViewBag.QuanLys = await _context.QuanLies.Where(x => x.TrangThai == true).ToListAsync();
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> EditNhanVien(NhanVien nhanVien)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");
            nhanVien.TrangThai = true;
            _context.NhanViens.Update(nhanVien);
            await _context.SaveChangesAsync();

            return RedirectToAction("NhanVien");
        }

        public async Task<IActionResult> ToggleNhanVien(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                nhanVien.TrangThai = !nhanVien.TrangThai;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("NhanVien");
        }
        #endregion

        #region Quản lý Đơn hàng
        public async Task<IActionResult> DonHang(int page = 1)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            int pageSize = 5;
            var query = _context.HoaDons
                .Include(x => x.MaKhNavigation)
                .Include(x => x.MaNvNavigation)
                .OrderByDescending(x => x.NgayLap);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var donHangs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(donHangs);
        }

        public async Task<IActionResult> DonHangDetails(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var hoaDon = await _context.HoaDons
                .Include(x => x.MaKhNavigation)
                .Include(x => x.MaNvNavigation)
                .Include(x => x.ChiTietHoaDons)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefaultAsync(x => x.MaHd == id);

            return View(hoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDonHang(string maHD, string trangThai)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var hoaDon = await _context.HoaDons.FindAsync(maHD);
            if (hoaDon != null)
            {
                hoaDon.TrangThaiDonHang = trangThai;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("DonHang");
        }
        #endregion

        #region Quản lý Khách hàng
        public async Task<IActionResult> KhachHang(int page = 1)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            int pageSize = 5;
            var query = _context.KhachHangs.OrderBy(x => x.TenKh);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var khachHangs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(khachHangs);
        }

        public async Task<IActionResult> ToggleKhachHang(string id)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "ADMIN")
                return RedirectToAction("Index", "Home");

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                khachHang.TrangThai = !khachHang.TrangThai;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("KhachHang");
        }
        #endregion
    }
}