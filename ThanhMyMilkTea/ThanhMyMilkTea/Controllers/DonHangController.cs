using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;

namespace ThanhMyMilkTea.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;

        public DonHangController(ThanhMyMilkTeaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var username = HttpContext.Session.GetString("Username");
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(x => x.Username == username);

            var donHangs = await _context.HoaDons
                .Include(x => x.MaKhNavigation)
                .Include(x => x.MaNvNavigation)
                .Where(x => x.MaKh == khachHang.MaKh)
                .OrderByDescending(x => x.NgayLap)
                .ToListAsync();

            return View(donHangs);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var hoaDon = await _context.HoaDons
                .Include(x => x.MaKhNavigation)
                .Include(x => x.MaNvNavigation)
                .Include(x => x.ChiTietHoaDons)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefaultAsync(x => x.MaHd == id);

            if (hoaDon == null)
                return NotFound();

            return View(hoaDon);
        }
    }
}