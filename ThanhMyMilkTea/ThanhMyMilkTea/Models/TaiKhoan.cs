namespace ThanhMyMilkTea.Models
{
    public partial class TaiKhoan
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? LoaiTaiKhoan { get; set; }
        public bool? TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }

    }
}