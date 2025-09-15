using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL; // Giả sử các lớp DAL nằm trong namespace DAL
using BCrypt.Net; // Sử dụng thư viện BCrypt.Net-Next

namespace BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDAL;
        private readonly NhanVienDAL _nhanVienDAL;

        public TaiKhoanBLL()
        {
            _taiKhoanDAL = new TaiKhoanDAL();
            _nhanVienDAL = new NhanVienDAL();
        }

        // Lấy danh sách tất cả tài khoản
        public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
        {
            return await _taiKhoanDAL.GetAllTaiKhoanAsync();
        }

        // Lấy tài khoản theo tên tài khoản
        public async Task<TaiKhoan> GetTaiKhoanByIdAsync(string tenTaiKhoan)
        {
            if (string.IsNullOrEmpty(tenTaiKhoan))
                throw new ArgumentException("Tên tài khoản không được để trống.");

            var taiKhoan = await _taiKhoanDAL.GetTaiKhoanByIdAsync(tenTaiKhoan);
            if (taiKhoan == null)
                throw new Exception("Tài khoản không tồn tại.");

            return taiKhoan;
        }

        // Thêm tài khoản mới
        public async Task<bool> AddTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            // Kiểm tra dữ liệu đầu vào
            if (taiKhoan == null)
                throw new ArgumentNullException(nameof(taiKhoan));
            if (string.IsNullOrEmpty(taiKhoan.TenTaiKhoan) || string.IsNullOrEmpty(taiKhoan.MatKhau) || string.IsNullOrEmpty(taiKhoan.VaiTro))
                throw new ArgumentException("Tên tài khoản, mật khẩu và vai trò không được để trống.");

            // Kiểm tra độ dài mật khẩu (tối thiểu 6 ký tự)
            if (taiKhoan.MatKhau.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");

            // Kiểm tra vai trò hợp lệ (giả sử chỉ có các vai trò: Admin, Staff)
            if (taiKhoan.VaiTro != "admin" && taiKhoan.VaiTro != "staff")
                throw new ArgumentException("Vai trò chỉ có thể là 'Admin' hoặc 'Staff'.");

            // Kiểm tra mã nhân viên (nếu có)
            if (!string.IsNullOrEmpty(taiKhoan.MaNhanVien))
            {
                var nhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(taiKhoan.MaNhanVien);
                if (nhanVien == null)
                    throw new ArgumentException("Mã nhân viên không tồn tại.");
            }

            // Kiểm tra trùng tên tài khoản
            var existingTaiKhoan = await _taiKhoanDAL.GetTaiKhoanByIdAsync(taiKhoan.TenTaiKhoan);
            if (existingTaiKhoan != null)
                throw new ArgumentException("Tên tài khoản đã tồn tại.");

            // Mã hóa mật khẩu với BCrypt
            try
            {
                taiKhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(taiKhoan.MatKhau, 8);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi mã hóa mật khẩu: " + ex.Message);
            }

            return await _taiKhoanDAL.AddTaiKhoanAsync(taiKhoan);
        }

        // Cập nhật tài khoản
        public async Task<bool> UpdateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            // Kiểm tra dữ liệu đầu vào
            if (taiKhoan == null)
                throw new ArgumentNullException(nameof(taiKhoan));
            if (string.IsNullOrEmpty(taiKhoan.TenTaiKhoan) || string.IsNullOrEmpty(taiKhoan.MatKhau) || string.IsNullOrEmpty(taiKhoan.VaiTro))
                throw new ArgumentException("Tên tài khoản, mật khẩu và vai trò không được để trống.");

            // Kiểm tra vai trò hợp lệ
            if (taiKhoan.VaiTro != "admin" && taiKhoan.VaiTro != "staff")
                throw new ArgumentException("Vai trò chỉ có thể là 'Admin' hoặc 'Staff'.");

            // Kiểm tra mã nhân viên (nếu có)
            if (!string.IsNullOrEmpty(taiKhoan.MaNhanVien))
            {
                var nhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(taiKhoan.MaNhanVien);
                if (nhanVien == null)
                    throw new ArgumentException("Mã nhân viên không tồn tại.");
            }

            // Kiểm tra sự tồn tại của tài khoản
            var existingTaiKhoan = await _taiKhoanDAL.GetTaiKhoanByIdAsync(taiKhoan.TenTaiKhoan);
            if (existingTaiKhoan == null)
                throw new Exception("Tài khoản không tồn tại.");

            // Kiểm tra xem mật khẩu đã thay đổi chưa
            try
            {
                // Nếu mật khẩu không phải là chuỗi băm hợp lệ (bắt đầu bằng $2), mã hóa lại
                if (!taiKhoan.MatKhau.StartsWith("$2"))
                {
                    taiKhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(taiKhoan.MatKhau, 12);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi mã hóa mật khẩu: " + ex.Message);
            }

            return await _taiKhoanDAL.UpdateTaiKhoanAsync(taiKhoan);
        }

        // Xóa tài khoản
        public async Task<bool> DeleteTaiKhoanAsync(string tenTaiKhoan)
        {
            if (string.IsNullOrEmpty(tenTaiKhoan))
                throw new ArgumentException("Tên tài khoản không được để trống.");

            // Kiểm tra sự tồn tại của tài khoản
            var taiKhoan = await _taiKhoanDAL.GetTaiKhoanByIdAsync(tenTaiKhoan);
            if (taiKhoan == null)
                throw new Exception("Tài khoản không tồn tại.");

            return await _taiKhoanDAL.DeleteTaiKhoanAsync(tenTaiKhoan);
        }

        // Xác thực tài khoản
        public async Task<bool> AuthenticateAsync(string tenTaiKhoan, string matKhau)
        {
            if (string.IsNullOrEmpty(tenTaiKhoan) || string.IsNullOrEmpty(matKhau))
                throw new ArgumentException("Tên tài khoản và mật khẩu không được để trống.");

            var taiKhoan = await _taiKhoanDAL.GetTaiKhoanByIdAsync(tenTaiKhoan);
            if (taiKhoan == null)
                return false;

            try
            {
                // Kiểm tra xem mật khẩu trong cơ sở dữ liệu có đúng định dạng băm BCrypt không
                if (!taiKhoan.MatKhau.StartsWith("$2"))
                {
                    throw new Exception("Mật khẩu trong cơ sở dữ liệu không đúng định dạng băm BCrypt.");
                }

                return BCrypt.Net.BCrypt.Verify(matKhau, taiKhoan.MatKhau);
            }
            catch (SaltParseException ex)
            {
                throw new Exception("Lỗi xác thực: Định dạng salt không hợp lệ. Vui lòng kiểm tra dữ liệu mật khẩu.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xác thực mật khẩu: " + ex.Message, ex);
            }
        }
    }
}