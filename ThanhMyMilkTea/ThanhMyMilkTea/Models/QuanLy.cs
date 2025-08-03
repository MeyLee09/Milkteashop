namespace ThanhMyMilkTea.Models
{
    public partial class QuanLy
    {
        public QuanLy()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        public string MaQl { get; set; } = null!;
        public string TenQl { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}