using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL; // Giả sử các lớp DAL nằm trong namespace DAL

namespace BLL
{
    public class KhachHangBLL
    {
        private readonly KhachHangDAL _khachHangDAL;
        private readonly HoaDonBanDAL _hoaDonBanDAL;

        public KhachHangBLL()
        {
            _khachHangDAL = new KhachHangDAL();
            _hoaDonBanDAL = new HoaDonBanDAL();
        }

        // Lấy danh sách tất cả khách hàng
        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            return await _khachHangDAL.GetAllKhachHangAsync();
        }

        // Lấy khách hàng theo mã
        public async Task<KhachHang> GetKhachHangByIdAsync(string maKhachHang)
        {
            if (string.IsNullOrEmpty(maKhachHang))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            var khachHang = await _khachHangDAL.GetKhachHangByIdAsync(maKhachHang);
            if (khachHang == null)
                throw new Exception("Khách hàng không tồn tại.");

            return khachHang;
        }

        // Thêm khách hàng mới
        public async Task<bool> AddKhachHangAsync(KhachHang khachHang)
        {
            // Kiểm tra dữ liệu đầu vào
            if (khachHang == null)
                throw new ArgumentNullException(nameof(khachHang));
            if (string.IsNullOrEmpty(khachHang.MaKhachHang) || string.IsNullOrEmpty(khachHang.HoTen) ||
                string.IsNullOrEmpty(khachHang.SoDienThoai) || string.IsNullOrEmpty(khachHang.DiaChi))
                throw new ArgumentException("Mã khách hàng, họ tên, số điện thoại và địa chỉ không được để trống.");

            // Kiểm tra định dạng số điện thoại (ví dụ: 10 chữ số, bắt đầu bằng 0)
            if (!Regex.IsMatch(khachHang.SoDienThoai, @"^0\d{9}$"))
                throw new ArgumentException("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0.");

            // Kiểm tra trùng mã khách hàng
            var existingKhachHang = await _khachHangDAL.GetKhachHangByIdAsync(khachHang.MaKhachHang);
            if (existingKhachHang != null)
                throw new ArgumentException("Mã khách hàng đã tồn tại.");

            return await _khachHangDAL.AddKhachHangAsync(khachHang);
        }

        // Cập nhật khách hàng
        public async Task<bool> UpdateKhachHangAsync(KhachHang khachHang)
        {
            // Kiểm tra dữ liệu đầu vào
            if (khachHang == null)
                throw new ArgumentNullException(nameof(khachHang));
            if (string.IsNullOrEmpty(khachHang.MaKhachHang) || string.IsNullOrEmpty(khachHang.HoTen) ||
                string.IsNullOrEmpty(khachHang.SoDienThoai) || string.IsNullOrEmpty(khachHang.DiaChi))
                throw new ArgumentException("Mã khách hàng, họ tên, số điện thoại và địa chỉ không được để trống.");

            // Kiểm tra định dạng số điện thoại
            if (!Regex.IsMatch(khachHang.SoDienThoai, @"^0\d{9}$"))
                throw new ArgumentException("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0.");

            // Kiểm tra sự tồn tại của khách hàng
            var existingKhachHang = await _khachHangDAL.GetKhachHangByIdAsync(khachHang.MaKhachHang);
            if (existingKhachHang == null)
                throw new Exception("Khách hàng không tồn tại.");

            return await _khachHangDAL.UpdateKhachHangAsync(khachHang);
        }

        // Xóa khách hàng
        public async Task<bool> DeleteKhachHangAsync(string maKhachHang)
        {
            if (string.IsNullOrEmpty(maKhachHang))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            // Kiểm tra sự tồn tại của khách hàng
            var khachHang = await _khachHangDAL.GetKhachHangByIdAsync(maKhachHang);
            if (khachHang == null)
                throw new Exception("Khách hàng không tồn tại.");

            // Kiểm tra xem khách hàng có đang được tham chiếu trong bảng HoaDonBan
            var hoaDonBans = await _hoaDonBanDAL.GetAllHoaDonBanAsync();
            if (hoaDonBans.Any(hd => hd.MaKhachHang == maKhachHang))
                throw new InvalidOperationException("Không thể xóa khách hàng vì đang được tham chiếu trong hóa đơn bán.");

            return await _khachHangDAL.DeleteKhachHangAsync(maKhachHang);
        }
    }
}