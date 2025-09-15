using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL; // Giả sử các lớp DAL nằm trong namespace DAL

namespace BLL
{
    public class ChiTietPhieuNhapBLL
    {
        private readonly ChiTietPhieuNhapDAL _chiTietPhieuNhapDAL;
        private readonly PhieuNhapDAL _phieuNhapDAL;
        private readonly SanPhamDAL _sanPhamDAL;

        public ChiTietPhieuNhapBLL()
        {
            _chiTietPhieuNhapDAL = new ChiTietPhieuNhapDAL();
            _phieuNhapDAL = new PhieuNhapDAL();
            _sanPhamDAL = new SanPhamDAL();
        }

        // Lấy danh sách tất cả chi tiết phiếu nhập
        public async Task<List<ChiTietPhieuNhap>> GetAllChiTietPhieuNhapAsync()
        {
            return await _chiTietPhieuNhapDAL.GetAllChiTietPhieuNhapAsync();
        }

        // Lấy chi tiết phiếu nhập theo mã
        public async Task<ChiTietPhieuNhap> GetChiTietPhieuNhapByIdAsync(string maChiTietPhieuNhap)
        {
            if (string.IsNullOrEmpty(maChiTietPhieuNhap))
                throw new ArgumentException("Mã chi tiết phiếu nhập không được để trống.");

            var chiTietPhieuNhap = await _chiTietPhieuNhapDAL.GetChiTietPhieuNhapByIdAsync(maChiTietPhieuNhap);
            if (chiTietPhieuNhap == null)
                throw new Exception("Chi tiết phiếu nhập không tồn tại.");

            return chiTietPhieuNhap;
        }

        // Thêm chi tiết phiếu nhập mới
        public async Task<bool> AddChiTietPhieuNhapAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            // Kiểm tra dữ liệu đầu vào
            if (chiTietPhieuNhap == null)
                throw new ArgumentNullException(nameof(chiTietPhieuNhap));
            if (string.IsNullOrEmpty(chiTietPhieuNhap.MaChiTietPhieuNhap) ||
                string.IsNullOrEmpty(chiTietPhieuNhap.MaPhieuNhap) ||
                string.IsNullOrEmpty(chiTietPhieuNhap.MaSanPham))
                throw new ArgumentException("Mã chi tiết phiếu nhập, mã phiếu nhập và mã sản phẩm không được để trống.");
            if (chiTietPhieuNhap.SoLuong <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0.");
            if (chiTietPhieuNhap.GiaNhap <= 0)
                throw new ArgumentException("Giá nhập phải lớn hơn 0.");

            // Kiểm tra mã phiếu nhập
            var phieuNhap = await _phieuNhapDAL.GetPhieuNhapByIdAsync(chiTietPhieuNhap.MaPhieuNhap);
            if (phieuNhap == null)
                throw new ArgumentException("Mã phiếu nhập không tồn tại.");

            // Kiểm tra mã sản phẩm
            var sanPham = await _sanPhamDAL.GetSanPhamByIdAsync(chiTietPhieuNhap.MaSanPham);
            if (sanPham == null)
                throw new ArgumentException("Mã sản phẩm không tồn tại.");

            // Kiểm tra trùng mã chi tiết phiếu nhập
            var existingChiTietPhieuNhap = await _chiTietPhieuNhapDAL.GetChiTietPhieuNhapByIdAsync(chiTietPhieuNhap.MaChiTietPhieuNhap);
            if (existingChiTietPhieuNhap != null)
                throw new ArgumentException("Mã chi tiết phiếu nhập đã tồn tại.");

            return await _chiTietPhieuNhapDAL.AddChiTietPhieuNhapAsync(chiTietPhieuNhap);
        }

        // Cập nhật chi tiết phiếu nhập
        public async Task<bool> UpdateChiTietPhieuNhapAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            // Kiểm tra dữ liệu đầu vào
            if (chiTietPhieuNhap == null)
                throw new ArgumentNullException(nameof(chiTietPhieuNhap));
            if (string.IsNullOrEmpty(chiTietPhieuNhap.MaChiTietPhieuNhap) ||
                string.IsNullOrEmpty(chiTietPhieuNhap.MaPhieuNhap) ||
                string.IsNullOrEmpty(chiTietPhieuNhap.MaSanPham))
                throw new ArgumentException("Mã chi tiết phiếu nhập, mã phiếu nhập và mã sản phẩm không được để trống.");
            if (chiTietPhieuNhap.SoLuong <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0.");
            if (chiTietPhieuNhap.GiaNhap <= 0)
                throw new ArgumentException("Giá nhập phải lớn hơn 0.");

            // Kiểm tra sự tồn tại của chi tiết phiếu nhập
            var existingChiTietPhieuNhap = await _chiTietPhieuNhapDAL.GetChiTietPhieuNhapByIdAsync(chiTietPhieuNhap.MaChiTietPhieuNhap);
            if (existingChiTietPhieuNhap == null)
                throw new Exception("Chi tiết phiếu nhập không tồn tại.");

            // Kiểm tra mã phiếu nhập
            var phieuNhap = await _phieuNhapDAL.GetPhieuNhapByIdAsync(chiTietPhieuNhap.MaPhieuNhap);
            if (phieuNhap == null)
                throw new ArgumentException("Mã phiếu nhập không tồn tại.");

            // Kiểm tra mã sản phẩm
            var sanPham = await _sanPhamDAL.GetSanPhamByIdAsync(chiTietPhieuNhap.MaSanPham);
            if (sanPham == null)
                throw new ArgumentException("Mã sản phẩm không tồn tại.");

            return await _chiTietPhieuNhapDAL.UpdateChiTietPhieuNhapAsync(chiTietPhieuNhap);
        }

        // Xóa chi tiết phiếu nhập
        public async Task<bool> DeleteChiTietPhieuNhapAsync(string maChiTietPhieuNhap)
        {
            if (string.IsNullOrEmpty(maChiTietPhieuNhap))
                throw new ArgumentException("Mã chi tiết phiếu nhập không được để trống.");

            // Kiểm tra sự tồn tại của chi tiết phiếu nhập
            var chiTietPhieuNhap = await _chiTietPhieuNhapDAL.GetChiTietPhieuNhapByIdAsync(maChiTietPhieuNhap);
            if (chiTietPhieuNhap == null)
                throw new Exception("Chi tiết phiếu nhập không tồn tại.");

            return await _chiTietPhieuNhapDAL.DeleteChiTietPhieuNhapAsync(maChiTietPhieuNhap);
        }
    }
}