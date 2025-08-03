using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhMyMilkTea.Models;

namespace ThanhMyMilkTea.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ThanhMyMilkTeaContext _context;

        public SanPhamController(ThanhMyMilkTeaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, string search = "", string category = "")
        {
            int pageSize = 6;
            var query = _context.SanPhams
                .Include(x => x.MaDanhMucNavigation)
                .Where(x => x.TrangThai == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.TenSp.Contains(search));
                ViewBag.Search = search;
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.MaDanhMuc == category);
                ViewBag.Category = category;
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var sanPhams = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.DanhMucs = await _context.DanhMucSps.Where(x => x.TrangThai == true).ToListAsync();

            return View(sanPhams);
        }

        public async Task<IActionResult> Details(string id)
        {
            var sanPham = await _context.SanPhams
                .Include(x => x.MaDanhMucNavigation)
                .Include(x => x.MaNccNavigation)
                .FirstOrDefaultAsync(x => x.MaSp == id && x.TrangThai == true);

            if (sanPham == null)
                return NotFound();

            return View(sanPham);
        }
    }
}