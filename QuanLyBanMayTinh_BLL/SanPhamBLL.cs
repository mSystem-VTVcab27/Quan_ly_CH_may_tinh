using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL; // Giả sử các lớp DAL nằm trong namespace DAL

namespace BLL
{
    public class SanPhamBLL
    {
        private readonly SanPhamDAL _sanPhamDAL;
        private readonly DanhMucSanPhamDAL _danhMucSanPhamDAL;

        public SanPhamBLL()
        {
            _sanPhamDAL = new SanPhamDAL();
            _danhMucSanPhamDAL = new DanhMucSanPhamDAL();
        }

        // Lấy danh sách tất cả sản phẩm
        public async Task<List<SanPham>> GetAllSanPhamAsync()
        {
            return await _sanPhamDAL.GetAllSanPhamAsync();
        }

        // Lấy sản phẩm theo mã
        public async Task<SanPham> GetSanPhamByIdAsync(string maSanPham)
        {
            if (string.IsNullOrEmpty(maSanPham))
                throw new ArgumentException("Mã sản phẩm không được để trống.");

            var sanPham = await _sanPhamDAL.GetSanPhamByIdAsync(maSanPham);
            if (sanPham == null)
                throw new Exception("Sản phẩm không tồn tại.");

            return sanPham;
        }

        // Thêm sản phẩm mới
        public async Task<bool> AddSanPhamAsync(SanPham sanPham)
        {
            // Kiểm tra dữ liệu đầu vào
            if (sanPham == null)
                throw new ArgumentNullException(nameof(sanPham));
            if (string.IsNullOrEmpty(sanPham.MaSanPham) || string.IsNullOrEmpty(sanPham.TenSanPham) || string.IsNullOrEmpty(sanPham.MaDanhMuc))
                throw new ArgumentException("Mã sản phẩm, tên sản phẩm và mã danh mục không được để trống.");
            if (sanPham.GiaNhap <= 0 || sanPham.GiaBan <= 0)
                throw new ArgumentException("Giá nhập và giá bán phải lớn hơn 0.");
            if (sanPham.BaoHanh < 0)
                throw new ArgumentException("Thời gian bảo hành không được âm.");

            // Kiểm tra trùng mã sản phẩm
            var existingSanPham = await _sanPhamDAL.GetSanPhamByIdAsync(sanPham.MaSanPham);
            if (existingSanPham != null)
                throw new ArgumentException("Mã sản phẩm đã tồn tại.");

            // Kiểm tra mã danh mục
            var danhMuc = await _danhMucSanPhamDAL.GetDanhMucSanPhamByIdAsync(sanPham.MaDanhMuc);
            if (danhMuc == null)
                throw new ArgumentException("Mã danh mục không tồn tại.");

            return await _sanPhamDAL.AddSanPhamAsync(sanPham);
        }

        // Cập nhật sản phẩm
        public async Task<bool> UpdateSanPhamAsync(SanPham sanPham)
        {
            // Kiểm tra dữ liệu đầu vào
            if (sanPham == null)
                throw new ArgumentNullException(nameof(sanPham));
            if (string.IsNullOrEmpty(sanPham.MaSanPham) || string.IsNullOrEmpty(sanPham.TenSanPham) || string.IsNullOrEmpty(sanPham.MaDanhMuc))
                throw new ArgumentException("Mã sản phẩm, tên sản phẩm và mã danh mục không được để trống.");
            if (sanPham.GiaNhap <= 0 || sanPham.GiaBan <= 0)
                throw new ArgumentException("Giá nhập và giá bán phải lớn hơn 0.");
            if (sanPham.BaoHanh < 0)
                throw new ArgumentException("Thời gian bảo hành không được âm.");

            // Kiểm tra sự tồn tại của sản phẩm
            var existingSanPham = await _sanPhamDAL.GetSanPhamByIdAsync(sanPham.MaSanPham);
            if (existingSanPham == null)
                throw new Exception("Sản phẩm không tồn tại.");

            // Kiểm tra mã danh mục
            var danhMuc = await _danhMucSanPhamDAL.GetDanhMucSanPhamByIdAsync(sanPham.MaDanhMuc);
            if (danhMuc == null)
                throw new ArgumentException("Mã danh mục không tồn tại.");

            return await _sanPhamDAL.UpdateSanPhamAsync(sanPham);
        }

        // Xóa sản phẩm
        public async Task<bool> DeleteSanPhamAsync(string maSanPham)
        {
            if (string.IsNullOrEmpty(maSanPham))
                throw new ArgumentException("Mã sản phẩm không được để trống.");

            // Kiểm tra sự tồn tại của sản phẩm
            var sanPham = await _sanPhamDAL.GetSanPhamByIdAsync(maSanPham);
            if (sanPham == null)
                throw new Exception("Sản phẩm không tồn tại.");

            // Kiểm tra xem sản phẩm có đang được tham chiếu trong các bảng khác
            // Ví dụ: ChiTietPhieuNhap, ChiTietHoaDonBan, BaoHanh
            // (Có thể thêm logic kiểm tra nếu cần)

            return await _sanPhamDAL.DeleteSanPhamAsync(maSanPham);
        }
    }
}
