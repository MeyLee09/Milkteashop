using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;

namespace ThanhMyMilkTea.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;

        public NhanVienController(ThanhMyMilkTeaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "NHANVIEN")
                return RedirectToAction("Index", "Home");

            return View();
        }

        // Xem danh sách sản phẩm
        public async Task<IActionResult> SanPham(int page = 1, string search = "")
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "NHANVIEN")
                return RedirectToAction("Index", "Home");

            int pageSize = 10;
            var query = _context.SanPhams
                .Include(x => x.MaDanhMucNavigation)
                .Include(x => x.MaNccNavigation)
                .Where(x => x.TrangThai == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.TenSp.Contains(search));
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

        // Xem danh sách đơn hàng cần giao
        public async Task<IActionResult> DonHang(int page = 1)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "NHANVIEN")
                return RedirectToAction("Index", "Home");

            int pageSize = 10;
            var query = _context.HoaDons
                .Include(x => x.MaKhNavigation)
                .Where(x => x.TrangThaiDonHang == "Đang giao" || x.TrangThaiDonHang == "Đã xác nhận")
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

        // Xác nhận đã giao đơn hàng
        [HttpPost]
        public async Task<IActionResult> XacNhanGiaoDon(string maHD)
        {
            if (HttpContext.Session.GetString("LoaiTaiKhoan") != "NHANVIEN")
                return RedirectToAction("Index", "Home");

            var hoaDon = await _context.HoaDons.FindAsync(maHD);
            if (hoaDon != null)
            {
                hoaDon.TrangThaiDonHang = "Đã giao";

                // Gán nhân viên giao hàng
                var username = HttpContext.Session.GetString("Username");
                var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(x => x.Email == username || x.Sdt == username);
                if (nhanVien != null)
                {
                    hoaDon.MaNv = nhanVien.MaNv;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("DonHang");
        }
    }
}