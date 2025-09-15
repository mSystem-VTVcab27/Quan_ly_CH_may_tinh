using DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ChiTietHoaDonBanBLL
    {
        private readonly ChiTietHoaDonBanDAL _chiTietHoaDonBanDAL;

        public ChiTietHoaDonBanBLL()
        {
            _chiTietHoaDonBanDAL = new ChiTietHoaDonBanDAL();
        }

        /// <summary>
        /// Lấy tất cả các chi tiết hóa đơn bán.
        /// </summary>
        /// <returns>Danh sách chi tiết hóa đơn.</returns>
        public async Task<List<ChiTietHoaDonBan>> GetAllChiTietHoaDonBanAsync()
        {
            return await _chiTietHoaDonBanDAL.GetAllChiTietHoaDonBanAsync();
        }

        /// <summary>
        /// Lấy chi tiết hóa đơn bán theo mã.
        /// </summary>
        /// <param name="maChiTietHoaDon">Mã chi tiết hóa đơn.</param>
        /// <returns>Đối tượng ChiTietHoaDonBan hoặc null.</returns>
        public async Task<ChiTietHoaDonBan> GetChiTietHoaDonBanByIdAsync(string maChiTietHoaDon)
        {
            if (string.IsNullOrWhiteSpace(maChiTietHoaDon))
            {
                throw new ArgumentException("Mã chi tiết hóa đơn không được để trống.");
            }
            return await _chiTietHoaDonBanDAL.GetChiTietHoaDonBanByIdAsync(maChiTietHoaDon);
        }

        /// <summary>
        /// Thêm mới một chi tiết hóa đơn bán.
        /// </summary>
        /// <param name="chiTietHoaDonBan">Thông tin chi tiết hóa đơn cần thêm.</param>
        /// <returns>True nếu thành công, False nếu thất bại.</returns>
        public async Task<bool> AddChiTietHoaDonBanAsync(ChiTietHoaDonBan chiTietHoaDonBan)
        {
            if (chiTietHoaDonBan == null)
            {
                throw new ArgumentNullException(nameof(chiTietHoaDonBan), "Thông tin chi tiết hóa đơn không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(chiTietHoaDonBan.MaChiTietHoaDon) ||
                string.IsNullOrWhiteSpace(chiTietHoaDonBan.MaHoaDon) ||
                string.IsNullOrWhiteSpace(chiTietHoaDonBan.MaSanPham))
            {
                throw new ArgumentException("Mã chi tiết, mã hóa đơn và mã sản phẩm không được để trống.");
            }

            if (chiTietHoaDonBan.SoLuong <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.");
            }

            if (chiTietHoaDonBan.DonGia < 0)
            {
                throw new ArgumentException("Đơn giá không được âm.");
            }

            // Kiểm tra sự tồn tại của mã chi tiết
            var existingDetail = await GetChiTietHoaDonBanByIdAsync(chiTietHoaDonBan.MaChiTietHoaDon);
            if (existingDetail != null)
            {
                throw new ArgumentException("Mã chi tiết hóa đơn đã tồn tại.");
            }

            return await _chiTietHoaDonBanDAL.AddChiTietHoaDonBanAsync(chiTietHoaDonBan);
        }

        /// <summary>
        /// Cập nhật một chi tiết hóa đơn bán.
        /// </summary>
        /// <param name="chiTietHoaDonBan">Thông tin chi tiết hóa đơn cần cập nhật.</param>
        /// <returns>True nếu thành công, False nếu thất bại.</returns>
        public async Task<bool> UpdateChiTietHoaDonBanAsync(ChiTietHoaDonBan chiTietHoaDonBan)
        {
            if (chiTietHoaDonBan == null)
            {
                throw new ArgumentNullException(nameof(chiTietHoaDonBan), "Thông tin chi tiết hóa đơn không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(chiTietHoaDonBan.MaChiTietHoaDon) ||
                string.IsNullOrWhiteSpace(chiTietHoaDonBan.MaHoaDon) ||
                string.IsNullOrWhiteSpace(chiTietHoaDonBan.MaSanPham))
            {
                throw new ArgumentException("Mã chi tiết, mã hóa đơn và mã sản phẩm không được để trống.");
            }

            if (chiTietHoaDonBan.SoLuong <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.");
            }

            if (chiTietHoaDonBan.DonGia < 0)
            {
                throw new ArgumentException("Đơn giá không được âm.");
            }

            // Kiểm tra sự tồn tại của chi tiết hóa đơn trước khi cập nhật
            var existingDetail = await GetChiTietHoaDonBanByIdAsync(chiTietHoaDonBan.MaChiTietHoaDon);
            if (existingDetail == null)
            {
                throw new ArgumentException("Không tìm thấy chi tiết hóa đơn để cập nhật.");
            }

            return await _chiTietHoaDonBanDAL.UpdateChiTietHoaDonBanAsync(chiTietHoaDonBan);
        }

        /// <summary>
        /// Xóa một chi tiết hóa đơn bán.
        /// </summary>
        /// <param name="maChiTietHoaDon">Mã chi tiết cần xóa.</param>
        /// <returns>True nếu thành công, False nếu thất bại.</returns>
        public async Task<bool> DeleteChiTietHoaDonBanAsync(string maChiTietHoaDon)
        {
            if (string.IsNullOrWhiteSpace(maChiTietHoaDon))
            {
                throw new ArgumentException("Mã chi tiết hóa đơn không được để trống.");
            }

            // (Tùy chọn) Kiểm tra xem chi tiết này có liên quan đến bảo hành không.
            if (await _chiTietHoaDonBanDAL.HasWarrantyAsync(maChiTietHoaDon))
            {
                throw new InvalidOperationException("Không thể xóa chi tiết hóa đơn này vì đã có phiếu bảo hành liên quan.");
            }

            return await _chiTietHoaDonBanDAL.DeleteChiTietHoaDonBanAsync(maChiTietHoaDon);
        }
    }
}