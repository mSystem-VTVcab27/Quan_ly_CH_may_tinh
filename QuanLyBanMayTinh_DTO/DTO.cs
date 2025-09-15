namespace QuanLyBanMayTinh_DTO
{
    namespace DTO
    {
        public class DanhMucSanPhamDTO
        {
            public string MaDanhMuc { get; set; }
            public string TenDanhMuc { get; set; }
            public string MoTa { get; set; }
            public string GhiChu { get; set; }
        }

        public class SanPhamDTO
        {
            public string MaSanPham { get; set; }
            public string TenSanPham { get; set; }
            public string MaDanhMuc { get; set; }
            public decimal GiaNhap { get; set; }
            public decimal GiaBan { get; set; }
            public int BaoHanh { get; set; }
            public string MoTa { get; set; }
            public string GhiChu { get; set; }
        }

        public class KhachHangDTO
        {
            public string MaKhachHang { get; set; }
            public string HoTen { get; set; }
            public string SoDienThoai { get; set; }
            public string DiaChi { get; set; }
            public string GhiChu { get; set; }
        }

        public class NhaCungCapDTO
        {
            public string MaNhaCungCap { get; set; }
            public string TenNhaCungCap { get; set; }
            public string SoDienThoai { get; set; }
            public string DiaChi { get; set; }
            public string Email { get; set; }
            public string GhiChu { get; set; }
        }

        public class NhanVienDTO
        {
            public string MaNhanVien { get; set; }
            public string HoTen { get; set; }
            public string GioiTinh { get; set; }
            public DateTime? NgayVaoLam { get; set; }
            public string SoDienThoai { get; set; }
            public string Email { get; set; }
            public string DiaChi { get; set; }
            public string GhiChu { get; set; }
        }

        public class TaiKhoanDTO
        {
            public string TenTaiKhoan { get; set; }
            public string MatKhau { get; set; }
            public string VaiTro { get; set; }
            public string MaNhanVien { get; set; }
            public string GhiChu { get; set; }
        }

        public class PhieuNhapDTO
        {
            public string MaPhieuNhap { get; set; }
            public DateTime NgayNhap { get; set; }
            public string MaNhaCungCap { get; set; }
            public string MaNhanVien { get; set; }
            public string GhiChu { get; set; }
        }

        public class ChiTietPhieuNhapDTO
        {
            public string MaChiTietPhieuNhap { get; set; }
            public string MaPhieuNhap { get; set; }
            public string MaSanPham { get; set; }
            public int SoLuong { get; set; }
            public decimal GiaNhap { get; set; }
            public string GhiChu { get; set; }
        }

        public class HoaDonBanDTO
        {
            public string MaHoaDon { get; set; }
            public DateTime NgayBan { get; set; }
            public string MaKhachHang { get; set; }
            public string MaNhanVien { get; set; }
            public decimal TongTien { get; set; }
            public string GhiChu { get; set; }
        }

        public class ChiTietHoaDonBanDTO
        {
            public string MaChiTietHoaDon { get; set; }
            public string MaHoaDon { get; set; }
            public string MaSanPham { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal GiamGia { get; set; }
            public string GhiChu { get; set; }
        }

        public class BaoHanhDTO
        {
            public string MaBaoHanh { get; set; }
            public string MaHoaDon { get; set; }
            public string MaSanPham { get; set; }
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public string TinhTrang { get; set; }
            public string GhiChu { get; set; }
        }
    }
}
