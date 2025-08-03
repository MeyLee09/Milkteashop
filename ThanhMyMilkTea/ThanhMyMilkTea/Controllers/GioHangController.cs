using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;
using System.Text.Json;

namespace ThanhMyMilkTea.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;

        public GioHangController(ThanhMyMilkTeaContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var gioHangJson = HttpContext.Session.GetString("GioHang");
            var gioHang = new List<CartItem>();

            if (!string.IsNullOrEmpty(gioHangJson))
            {
                gioHang = JsonSerializer.Deserialize<List<CartItem>>(gioHangJson);
            }

            return View(gioHang);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string maSP, int soLuong = 1)
        {
            if (HttpContext.Session.GetString("Username") == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            var sanPham = await _context.SanPhams.FindAsync(maSP);
            if (sanPham == null || sanPham.TrangThai != true)
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

            var gioHangJson = HttpContext.Session.GetString("GioHang");
            var gioHang = new List<CartItem>();

            if (!string.IsNullOrEmpty(gioHangJson))
            {
                gioHang = JsonSerializer.Deserialize<List<CartItem>>(gioHangJson);
            }

            var existItem = gioHang.FirstOrDefault(x => x.MaSP == maSP);
            if (existItem != null)
            {
                existItem.SoLuong += soLuong;
            }
            else
            {
                gioHang.Add(new CartItem
                {
                    MaSP = sanPham.MaSp,
                    TenSP = sanPham.TenSp,
                    Gia = sanPham.Gia,
                    SoLuong = soLuong,
                    HinhAnh = sanPham.HinhAnh
                });
            }

            HttpContext.Session.SetString("GioHang", JsonSerializer.Serialize(gioHang));
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" });
        }

        [HttpPost]
        public IActionResult UpdateCart(string maSP, int soLuong)
        {
            var gioHangJson = HttpContext.Session.GetString("GioHang");
            if (!string.IsNullOrEmpty(gioHangJson))
            {
                var gioHang = JsonSerializer.Deserialize<List<CartItem>>(gioHangJson);
                var item = gioHang.FirstOrDefault(x => x.MaSP == maSP);
                if (item != null)
                {
                    if (soLuong <= 0)
                        gioHang.Remove(item);
                    else
                        item.SoLuong = soLuong;

                    HttpContext.Session.SetString("GioHang", JsonSerializer.Serialize(gioHang));
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(string maSP)
        {
            var gioHangJson = HttpContext.Session.GetString("GioHang");
            if (!string.IsNullOrEmpty(gioHangJson))
            {
                var gioHang = JsonSerializer.Deserialize<List<CartItem>>(gioHangJson);
                var item = gioHang.FirstOrDefault(x => x.MaSP == maSP);
                if (item != null)
                {
                    gioHang.Remove(item);
                    HttpContext.Session.SetString("GioHang", JsonSerializer.Serialize(gioHang));
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var gioHangJson = HttpContext.Session.GetString("GioHang");
            if (string.IsNullOrEmpty(gioHangJson))
                return RedirectToAction("Index");

            var gioHang = JsonSerializer.Deserialize<List<CartItem>>(gioHangJson);
            return View(gioHang);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string ghiChu, string hinhThucThanhToan = "Tiền mặt")
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var username = HttpContext.Session.GetString("Username");
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(x => x.Username == username || x.Sdt == username);

            var gioHangJson = HttpContext.Session.GetString("GioHang");
            if (string.IsNullOrEmpty(gioHangJson))
                return RedirectToAction("Index");

            var gioHang = JsonSerializer.Deserialize<List<CartItem>>(gioHangJson);
            var tongTien = gioHang.Sum(x => x.ThanhTien);

            var hoaDon = new HoaDon
            {
                MaHd = "HD" + DateTime.Now.Ticks.ToString().Substring(10),
                NgayLap = DateTime.Now,
                MaKh = khachHang?.MaKh,
                TongTien = tongTien,
                GiamGia = 0,
                ThanhTien = tongTien,
                HinhThucThanhToan = hinhThucThanhToan,
                TrangThai = true,
                GhiChu = ghiChu,
                TrangThaiDonHang = "Chờ xác nhận"
            };

            _context.HoaDons.Add(hoaDon);

            foreach (var item in gioHang)
            {
                var chiTiet = new ChiTietHoaDon
                {
                    MaHd = hoaDon.MaHd,
                    MaSp = item.MaSP,
                    SoLuong = item.SoLuong,
                    DonGia = item.Gia,
                    ThanhTien = item.ThanhTien
                };
                _context.ChiTietHoaDons.Add(chiTiet);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("GioHang");

            return RedirectToAction("OrderSuccess", new { maHD = hoaDon.MaHd });
        }

        public IActionResult OrderSuccess(string maHD)
        {
            ViewBag.MaHD = maHD;
            return View();
        }
    }
}