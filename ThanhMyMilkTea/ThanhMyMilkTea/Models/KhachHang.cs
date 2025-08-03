namespace ThanhMyMilkTea.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public string MaKh { get; set; } = null!;
        public string TenKh { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public int? DiemTichLuy { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}