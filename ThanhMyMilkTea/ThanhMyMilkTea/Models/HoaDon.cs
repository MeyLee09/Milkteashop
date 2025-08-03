namespace ThanhMyMilkTea.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public string MaHd { get; set; } = null!;
        public DateTime? NgayLap { get; set; }
        public string? MaKh { get; set; }
        public string? MaNv { get; set; }
        public decimal? TongTien { get; set; }
        public decimal? GiamGia { get; set; }
        public decimal? ThanhTien { get; set; }
        public string? HinhThucThanhToan { get; set; }
        public bool? TrangThai { get; set; }
        public string? GhiChu { get; set; }

        public string TrangThaiDonHang { get; set; }
        public virtual KhachHang? MaKhNavigation { get; set; }
        public virtual NhanVien? MaNvNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
