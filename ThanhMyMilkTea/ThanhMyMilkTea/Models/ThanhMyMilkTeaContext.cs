using Microsoft.EntityFrameworkCore;

namespace ThanhMyMilkTea.Models
{
    public partial class ThanhMyMilkTeaContext : DbContext
    {
        public ThanhMyMilkTeaContext()
        {
        }

        public ThanhMyMilkTeaContext(DbContextOptions<ThanhMyMilkTeaContext> options)
            : base(options)
        {
        }
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<DanhMucSp> DanhMucSps { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<QuanLy> QuanLies { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ThanhMyMilkTea;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => new { e.MaHd, e.MaSp });

                entity.ToTable("ChiTietHoaDon");

                entity.Property(e => e.MaHd).HasMaxLength(50);
                entity.Property(e => e.MaSp)
                    .HasMaxLength(50)
                    .HasColumnName("MaSP");
                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.GhiChu).HasMaxLength(255);
                entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.MaHdNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHd)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ChiTietHo__MaHD__5165187F");

                entity.HasOne(d => d.MaSpNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaSp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietHo__MaSP__52593CB8");
            });

            modelBuilder.Entity<DanhMucSp>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc);

                entity.ToTable("DanhMucSP");

                entity.Property(e => e.MaDanhMuc).HasMaxLength(50);
                entity.Property(e => e.MoTa).HasMaxLength(500);
                entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHd);

                entity.ToTable("HoaDon");

                entity.Property(e => e.MaHd)
                    .HasMaxLength(50)
                    .HasColumnName("MaHD");
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.GiamGia)
                    .HasDefaultValueSql("((0))")
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.HinhThucThanhToan)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Tiền mặt')");
                entity.Property(e => e.MaKh)
                    .HasMaxLength(50)
                    .HasColumnName("MaKH");
                entity.Property(e => e.MaNv)
                    .HasMaxLength(50)
                    .HasColumnName("MaNV");
                entity.Property(e => e.NgayLap)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ThanhTien)
                    .HasDefaultValueSql("((0))")
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TongTien)
                    .HasDefaultValueSql("((0))")
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaKh)
                    .HasConstraintName("FK__HoaDon__MaKH__4E88ABD4");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaNv)
                    .HasConstraintName("FK__HoaDon__MaNV__4F7CD00D");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKh);

                entity.ToTable("KhachHang");

                entity.Property(e => e.MaKh)
                    .HasMaxLength(50)
                    .HasColumnName("MaKH");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.DiemTichLuy).HasDefaultValueSql("((0))");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.NgayDangKy)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("date");
                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .HasColumnName("SDT");
                entity.Property(e => e.TenKh)
                    .HasMaxLength(100)
                    .HasColumnName("TenKH");
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNcc);

                entity.ToTable("NhaCungCap");

                entity.Property(e => e.MaNcc)
                    .HasMaxLength(50)
                    .HasColumnName("MaNCC");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .HasColumnName("SDT");
                entity.Property(e => e.TenNcc)
                    .HasMaxLength(255)
                    .HasColumnName("TenNCC");
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNv);

                entity.ToTable("NhanVien");

                entity.Property(e => e.MaNv)
                    .HasMaxLength(50)
                    .HasColumnName("MaNV");
                entity.Property(e => e.ChucVu).HasMaxLength(50);
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Luong).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.MaQl)
                    .HasMaxLength(50)
                    .HasColumnName("MaQL");
                entity.Property(e => e.NgayVaoLam).HasColumnType("date");
                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .HasColumnName("SDT");
                entity.Property(e => e.TenNv)
                    .HasMaxLength(100)
                    .HasColumnName("TenNV");
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaQlNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.MaQl)
                    .HasConstraintName("FK__NhanVien__MaQL__3C69FB99");
            });

            modelBuilder.Entity<QuanLy>(entity =>
            {
                entity.HasKey(e => e.MaQl);

                entity.ToTable("QuanLy");

                entity.Property(e => e.MaQl)
                    .HasMaxLength(50)
                    .HasColumnName("MaQL");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .HasColumnName("SDT");
                entity.Property(e => e.TenQl)
                    .HasMaxLength(100)
                    .HasColumnName("TenQL");
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSp);

                entity.ToTable("SanPham");

                entity.Property(e => e.MaSp)
                    .HasMaxLength(50)
                    .HasColumnName("MaSP");
                entity.Property(e => e.DonViTinh)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Ly')");
                entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.HinhAnh).HasMaxLength(255);
                entity.Property(e => e.MaDanhMuc).HasMaxLength(50);
                entity.Property(e => e.MaNcc)
                    .HasMaxLength(50)
                    .HasColumnName("MaNCC");
                entity.Property(e => e.MoTa).HasMaxLength(1000);
                entity.Property(e => e.SoLuongTon).HasDefaultValueSql("((0))");
                entity.Property(e => e.TenSp)
                    .HasMaxLength(255)
                    .HasColumnName("TenSP");
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
                entity.Property(e => e.XuatXu).HasMaxLength(100);

                entity.HasOne(d => d.MaDanhMucNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaDanhMuc)
                    .HasConstraintName("FK__SanPham__MaDanhM__44FF419A");

                entity.HasOne(d => d.MaNccNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaNcc)
                    .HasConstraintName("FK__SanPham__MaNCC__45F365D3");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.LoaiTaiKhoan)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('KHACHHANG')");
                entity.Property(e => e.NgayTao)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}