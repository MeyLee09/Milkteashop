using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;

namespace ThanhMyMilkTea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;

        public HomeController(ThanhMyMilkTeaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sanPhamNoiBat = await _context.SanPhams
                .Include(x => x.MaDanhMucNavigation)
                .Where(x => x.TrangThai == true)
                .Take(6)
                .ToListAsync();

            return View(sanPhamNoiBat);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}