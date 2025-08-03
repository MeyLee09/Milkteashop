namespace ThanhMyMilkTea.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public string MaNcc { get; set; } = null!;
        public string TenNcc { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string? Email { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}