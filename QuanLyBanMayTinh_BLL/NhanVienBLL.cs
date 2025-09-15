using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL; // Giả sử các lớp DAL nằm trong namespace DAL

namespace BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL _nhanVienDAL;
        private readonly TaiKhoanDAL _taiKhoanDAL;
        private readonly PhieuNhapDAL _phieuNhapDAL;
        private readonly HoaDonBanDAL _hoaDonBanDAL;

        public NhanVienBLL()
        {
            _nhanVienDAL = new NhanVienDAL();
            _taiKhoanDAL = new TaiKhoanDAL();
            _phieuNhapDAL = new PhieuNhapDAL();
            _hoaDonBanDAL = new HoaDonBanDAL();
        }

        // Lấy danh sách tất cả nhân viên
        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            return await _nhanVienDAL.GetAllNhanVienAsync();
        }

        // Lấy nhân viên theo mã
        public async Task<NhanVien> GetNhanVienByIdAsync(string maNhanVien)
        {
            if (string.IsNullOrEmpty(maNhanVien))
                throw new ArgumentException("Mã nhân viên không được để trống.");

            var nhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(maNhanVien);
            if (nhanVien == null)
                throw new Exception("Nhân viên không tồn tại.");

            return nhanVien;
        }

        // Thêm nhân viên mới
        public async Task<bool> AddNhanVienAsync(NhanVien nhanVien)
        {
            // Kiểm tra dữ liệu đầu vào
            if (nhanVien == null)
                throw new ArgumentNullException(nameof(nhanVien));
            if (string.IsNullOrEmpty(nhanVien.MaNhanVien) || string.IsNullOrEmpty(nhanVien.HoTen) ||
                string.IsNullOrEmpty(nhanVien.SoDienThoai) || string.IsNullOrEmpty(nhanVien.Email) ||
                string.IsNullOrEmpty(nhanVien.DiaChi))
                throw new ArgumentException("Mã nhân viên, họ tên, số điện thoại, email và địa chỉ không được để trống.");

            // Kiểm tra định dạng số điện thoại (ví dụ: 10 chữ số, bắt đầu bằng 0)
            if (!Regex.IsMatch(nhanVien.SoDienThoai, @"^0\d{9}$"))
                throw new ArgumentException("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0.");

            // Kiểm tra định dạng email
            if (!Regex.IsMatch(nhanVien.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                throw new ArgumentException("Định dạng email không hợp lệ.");

            // Kiểm tra ngày vào làm (nếu có, không được là ngày trong tương lai)
            if (nhanVien.NgayVaoLam.HasValue && nhanVien.NgayVaoLam.Value > DateTime.Now)
                throw new ArgumentException("Ngày vào làm không được là ngày trong tương lai.");

            // Kiểm tra trùng mã nhân viên
            var existingNhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(nhanVien.MaNhanVien);
            if (existingNhanVien != null)
                throw new ArgumentException("Mã nhân viên đã tồn tại.");

            return await _nhanVienDAL.AddNhanVienAsync(nhanVien);
        }

        // Cập nhật nhân viên
        public async Task<bool> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            // Kiểm tra dữ liệu đầu vào
            if (nhanVien == null)
                throw new ArgumentNullException(nameof(nhanVien));
            if (string.IsNullOrEmpty(nhanVien.MaNhanVien) || string.IsNullOrEmpty(nhanVien.HoTen) ||
                string.IsNullOrEmpty(nhanVien.SoDienThoai) || string.IsNullOrEmpty(nhanVien.Email) ||
                string.IsNullOrEmpty(nhanVien.DiaChi))
                throw new ArgumentException("Mã nhân viên, họ tên, số điện thoại, email và địa chỉ không được để trống.");

            // Kiểm tra định dạng số điện thoại
            if (!Regex.IsMatch(nhanVien.SoDienThoai, @"^0\d{9}$"))
                throw new ArgumentException("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0.");

            // Kiểm tra định dạng email
            if (!Regex.IsMatch(nhanVien.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                throw new ArgumentException("Định dạng email không hợp lệ.");

            // Kiểm tra ngày vào làm
            if (nhanVien.NgayVaoLam.HasValue && nhanVien.NgayVaoLam.Value > DateTime.Now)
                throw new ArgumentException("Ngày vào làm không được là ngày trong tương lai.");

            // Kiểm tra sự tồn tại của nhân viên
            var existingNhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(nhanVien.MaNhanVien);
            if (existingNhanVien == null)
                throw new Exception("Nhân viên không tồn tại.");

            return await _nhanVienDAL.UpdateNhanVienAsync(nhanVien);
        }

        // Xóa nhân viên
        public async Task<bool> DeleteNhanVienAsync(string maNhanVien)
        {
            if (string.IsNullOrEmpty(maNhanVien))
                throw new ArgumentException("Mã nhân viên không được để trống.");

            // Kiểm tra sự tồn tại của nhân viên
            var nhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(maNhanVien);
            if (nhanVien == null)
                throw new Exception("Nhân viên không tồn tại.");

            // Kiểm tra xem nhân viên có đang được tham chiếu trong bảng TaiKhoan
            var taiKhoans = await _taiKhoanDAL.GetAllTaiKhoanAsync();
            if (taiKhoans.Any(tk => tk.MaNhanVien == maNhanVien))
                throw new InvalidOperationException("Không thể xóa nhân viên vì đang được tham chiếu trong tài khoản.");

            // Kiểm tra xem nhân viên có đang được tham chiếu trong bảng PhieuNhap
            var phieuNhaps = await _phieuNhapDAL.GetAllPhieuNhapAsync();
            if (phieuNhaps.Any(pn => pn.MaNhanVien == maNhanVien))
                throw new InvalidOperationException("Không thể xóa nhân viên vì đang được tham chiếu trong phiếu nhập.");

            // Kiểm tra xem nhân viên có đang được tham chiếu trong bảng HoaDonBan
            var hoaDonBans = await _hoaDonBanDAL.GetAllHoaDonBanAsync();
            if (hoaDonBans.Any(hd => hd.MaNhanVien == maNhanVien))
                throw new InvalidOperationException("Không thể xóa nhân viên vì đang được tham chiếu trong hóa đơn bán.");

            return await _nhanVienDAL.DeleteNhanVienAsync(maNhanVien);
        }
    }
}