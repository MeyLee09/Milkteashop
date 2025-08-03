using System.ComponentModel.DataAnnotations;

namespace ThanhMyMilkTea.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public string? MaDanhMuc { get; set; }
        public string? MaNcc { get; set; }
        public string? XuatXu { get; set; }
        public decimal Gia { get; set; }
        public int? SoLuongTon { get; set; }
        public string? DonViTinh { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public bool? TrangThai { get; set; }

        public virtual DanhMucSp? MaDanhMucNavigation { get; set; }
        public virtual NhaCungCap? MaNccNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}