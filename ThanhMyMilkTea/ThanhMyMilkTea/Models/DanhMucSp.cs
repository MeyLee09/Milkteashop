namespace ThanhMyMilkTea.Models
{
    public partial class DanhMucSp
    {
        public DanhMucSp()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public string MaDanhMuc { get; set; } = null!;
        public string TenDanhMuc { get; set; } = null!;
        public string? MoTa { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}