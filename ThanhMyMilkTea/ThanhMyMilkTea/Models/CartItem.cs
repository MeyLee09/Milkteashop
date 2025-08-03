namespace ThanhMyMilkTea.Models
{
    public class CartItem
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => Gia * SoLuong;
        public string HinhAnh { get; set; }
    }
}