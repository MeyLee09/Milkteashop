namespace ThanhMyMilkTea.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public string MaNv { get; set; } = null!;
        public string TenNv { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string? Email { get; set; }
        public string? ChucVu { get; set; }
        public decimal? Luong { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public string? MaQl { get; set; }
        public bool? TrangThai { get; set; }
        public string? Username { get; set; }
        public virtual QuanLy? MaQlNavigation { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}