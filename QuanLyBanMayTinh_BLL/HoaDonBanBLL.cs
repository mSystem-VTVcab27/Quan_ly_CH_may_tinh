using DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class HoaDonBanBLL
    {
        private readonly HoaDonBanDAL _hoaDonBanDAL;
        private readonly ChiTietHoaDonBanDAL _chiTietHoaDonBanDAL; // Cần thiết cho việc xóa

        public HoaDonBanBLL()
        {
            _hoaDonBanDAL = new HoaDonBanDAL();
            _chiTietHoaDonBanDAL = new ChiTietHoaDonBanDAL();
        }

        /// <summary>
        /// Lấy danh sách tất cả hóa đơn bán.
        /// </summary>
        /// <returns>Danh sách các hóa đơn bán.</returns>
        public async Task<List<HoaDonBan>> GetAllHoaDonBanAsync()
        {
            return await _hoaDonBanDAL.GetAllHoaDonBanAsync();
        }

        /// <summary>
        /// Lấy thông tin hóa đơn bán theo mã.
        /// </summary>
        /// <param name="maHoaDon">Mã hóa đơn cần lấy.</param>
        /// <returns>Đối tượng HoaDonBan hoặc null nếu không tìm thấy.</returns>
        public async Task<HoaDonBan> GetHoaDonBanByIdAsync(string maHoaDon)
        {
            if (string.IsNullOrWhiteSpace(maHoaDon))
            {
                throw new ArgumentException("Mã hóa đơn không được để trống.");
            }
            return await _hoaDonBanDAL.GetHoaDonBanByIdAsync(maHoaDon);
        }

        /// <summary>
        /// Thêm một hóa đơn bán mới.
        /// </summary>
        /// <param name="hoaDonBan">Đối tượng HoaDonBan cần thêm.</param>
        /// <returns>True nếu thêm thành công, False nếu thất bại.</returns>
        public async Task<bool> AddHoaDonBanAsync(HoaDonBan hoaDonBan)
        {
            if (hoaDonBan == null)
            {
                throw new ArgumentNullException(nameof(hoaDonBan), "Thông tin hóa đơn không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(hoaDonBan.MaHoaDon) || string.IsNullOrWhiteSpace(hoaDonBan.MaNhanVien))
            {
                throw new ArgumentException("Mã hóa đơn và Mã nhân viên không được để trống.");
            }
            if (hoaDonBan.TongTien < 0)
            {
                throw new ArgumentException("Tổng tiền không được là số âm.");
            }

            // Kiểm tra sự tồn tại của mã hóa đơn
            var existingInvoice = await GetHoaDonBanByIdAsync(hoaDonBan.MaHoaDon);
            if (existingInvoice != null)
            {
                throw new ArgumentException("Mã hóa đơn đã tồn tại.");
            }

            return await _hoaDonBanDAL.AddHoaDonBanAsync(hoaDonBan);
        }

        /// <summary>
        /// Cập nhật thông tin một hóa đơn bán.
        /// </summary>
        /// <param name="hoaDonBan">Đối tượng HoaDonBan cần cập nhật.</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        public async Task<bool> UpdateHoaDonBanAsync(HoaDonBan hoaDonBan)
        {
            if (hoaDonBan == null)
            {
                throw new ArgumentNullException(nameof(hoaDonBan), "Thông tin hóa đơn không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(hoaDonBan.MaHoaDon) || string.IsNullOrWhiteSpace(hoaDonBan.MaNhanVien))
            {
                throw new ArgumentException("Mã hóa đơn và Mã nhân viên không được để trống.");
            }
            if (hoaDonBan.TongTien < 0)
            {
                throw new ArgumentException("Tổng tiền không được là số âm.");
            }

            // Kiểm tra sự tồn tại của hóa đơn trước khi cập nhật
            var existingInvoice = await GetHoaDonBanByIdAsync(hoaDonBan.MaHoaDon);
            if (existingInvoice == null)
            {
                throw new ArgumentException("Không tìm thấy hóa đơn để cập nhật.");
            }

            return await _hoaDonBanDAL.UpdateHoaDonBanAsync(hoaDonBan);
        }

        /// <summary>
        /// Xóa một hóa đơn bán và tất cả các chi tiết liên quan.
        /// </summary>
        /// <param name="maHoaDon">Mã hóa đơn cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại.</returns>
        public async Task<bool> DeleteHoaDonBanAsync(string maHoaDon)
        {
            if (string.IsNullOrWhiteSpace(maHoaDon))
            {
                throw new ArgumentException("Mã hóa đơn không được để trống.");
            }

            // Xóa tất cả các chi tiết hóa đơn liên quan trước
            await _chiTietHoaDonBanDAL.DeleteByMaHoaDonAsync(maHoaDon);

            // Sau đó xóa hóa đơn chính
            return await _hoaDonBanDAL.DeleteHoaDonBanAsync(maHoaDon);
        }
    }
}