using DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class DanhMucSanPhamBLL
    {
        private readonly DanhMucSanPhamDAL _danhMucSanPhamDAL;

        public DanhMucSanPhamBLL()
        {
            _danhMucSanPhamDAL = new DanhMucSanPhamDAL();
        }

        /// <summary>
        /// Lấy danh sách tất cả danh mục sản phẩm.
        /// </summary>
        /// <returns>Danh sách các danh mục sản phẩm.</returns>
        public async Task<List<DanhMucSanPham>> GetAllDanhMucSanPhamAsync()
        {
            return await _danhMucSanPhamDAL.GetAllDanhMucSanPhamAsync();
        }

        /// <summary>
        /// Lấy thông tin danh mục sản phẩm theo mã.
        /// </summary>
        /// <param name="maDanhMuc">Mã danh mục cần lấy.</param>
        /// <returns>Đối tượng DanhMucSanPham hoặc null nếu không tìm thấy.</returns>
        public async Task<DanhMucSanPham> GetDanhMucSanPhamByIdAsync(string maDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(maDanhMuc))
            {
                throw new ArgumentException("Mã danh mục không được để trống.");
            }
            return await _danhMucSanPhamDAL.GetDanhMucSanPhamByIdAsync(maDanhMuc);
        }

        /// <summary>
        /// Thêm một danh mục sản phẩm mới.
        /// </summary>
        /// <param name="danhMucSanPham">Đối tượng DanhMucSanPham cần thêm.</param>
        /// <returns>True nếu thêm thành công, False nếu thất bại.</returns>
        public async Task<bool> AddDanhMucSanPhamAsync(DanhMucSanPham danhMucSanPham)
        {
            if (danhMucSanPham == null)
            {
                throw new ArgumentNullException(nameof(danhMucSanPham), "Thông tin danh mục không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(danhMucSanPham.MaDanhMuc) || string.IsNullOrWhiteSpace(danhMucSanPham.TenDanhMuc))
            {
                throw new ArgumentException("Mã và Tên danh mục không được để trống.");
            }

            // Kiểm tra xem mã danh mục đã tồn tại chưa
            var existingCategory = await GetDanhMucSanPhamByIdAsync(danhMucSanPham.MaDanhMuc);
            if (existingCategory != null)
            {
                throw new ArgumentException("Mã danh mục đã tồn tại.");
            }

            return await _danhMucSanPhamDAL.AddDanhMucSanPhamAsync(danhMucSanPham);
        }

        /// <summary>
        /// Cập nhật thông tin một danh mục sản phẩm.
        /// </summary>
        /// <param name="danhMucSanPham">Đối tượng DanhMucSanPham cần cập nhật.</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        public async Task<bool> UpdateDanhMucSanPhamAsync(DanhMucSanPham danhMucSanPham)
        {
            if (danhMucSanPham == null)
            {
                throw new ArgumentNullException(nameof(danhMucSanPham), "Thông tin danh mục không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(danhMucSanPham.MaDanhMuc) || string.IsNullOrWhiteSpace(danhMucSanPham.TenDanhMuc))
            {
                throw new ArgumentException("Mã và Tên danh mục không được để trống.");
            }

            // Kiểm tra sự tồn tại của danh mục trước khi cập nhật
            var existingCategory = await GetDanhMucSanPhamByIdAsync(danhMucSanPham.MaDanhMuc);
            if (existingCategory == null)
            {
                throw new ArgumentException("Không tìm thấy danh mục để cập nhật.");
            }

            return await _danhMucSanPhamDAL.UpdateDanhMucSanPhamAsync(danhMucSanPham);
        }

        /// <summary>
        /// Xóa một danh mục sản phẩm.
        /// </summary>
        /// <param name="maDanhMuc">Mã danh mục cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại.</returns>
        public async Task<bool> DeleteDanhMucSanPhamAsync(string maDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(maDanhMuc))
            {
                throw new ArgumentException("Mã danh mục không được để trống.");
            }

            // (Tùy chọn) Thêm logic kiểm tra xem danh mục có đang được sử dụng ở bảng nào khác không trước khi xóa.
            // Ví dụ: Kiểm tra xem có sản phẩm nào thuộc danh mục này không.

            return await _danhMucSanPhamDAL.DeleteDanhMucSanPhamAsync(maDanhMuc);
        }
    }
}