using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL; // Giả sử các lớp DAL nằm trong namespace DAL

namespace BLL
{
    public class PhieuNhapBLL
    {
        private readonly PhieuNhapDAL _phieuNhapDAL;
        private readonly NhaCungCapDAL _nhaCungCapDAL;
        private readonly NhanVienDAL _nhanVienDAL;
        private readonly ChiTietPhieuNhapDAL _chiTietPhieuNhapDAL;

        public PhieuNhapBLL()
        {
            _phieuNhapDAL = new PhieuNhapDAL();
            _nhaCungCapDAL = new NhaCungCapDAL();
            _nhanVienDAL = new NhanVienDAL();
            _chiTietPhieuNhapDAL = new ChiTietPhieuNhapDAL();
        }

        // Lấy danh sách tất cả phiếu nhập
        public async Task<List<PhieuNhap>> GetAllPhieuNhapAsync()
        {
            return await _phieuNhapDAL.GetAllPhieuNhapAsync();
        }

        // Lấy phiếu nhập theo mã
        public async Task<PhieuNhap> GetPhieuNhapByIdAsync(string maPhieuNhap)
        {
            if (string.IsNullOrEmpty(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");

            var phieuNhap = await _phieuNhapDAL.GetPhieuNhapByIdAsync(maPhieuNhap);
            if (phieuNhap == null)
                throw new Exception("Phiếu nhập không tồn tại.");

            return phieuNhap;
        }

        // Thêm phiếu nhập mới
        public async Task<bool> AddPhieuNhapAsync(PhieuNhap phieuNhap)
        {
            // Kiểm tra dữ liệu đầu vào
            if (phieuNhap == null)
                throw new ArgumentNullException(nameof(phieuNhap));
            if (string.IsNullOrEmpty(phieuNhap.MaPhieuNhap) || string.IsNullOrEmpty(phieuNhap.MaNhaCungCap) ||
                string.IsNullOrEmpty(phieuNhap.MaNhanVien))
                throw new ArgumentException("Mã phiếu nhập, mã nhà cung cấp và mã nhân viên không được để trống.");

            // Kiểm tra ngày nhập hợp lệ
            if (phieuNhap.NgayNhap > DateTime.Now)
                throw new ArgumentException("Ngày nhập không được là ngày trong tương lai.");

            // Kiểm tra mã nhà cung cấp
            var nhaCungCap = await _nhaCungCapDAL.GetNhaCungCapByIdAsync(phieuNhap.MaNhaCungCap);
            if (nhaCungCap == null)
                throw new ArgumentException("Mã nhà cung cấp không tồn tại.");

            // Kiểm tra mã nhân viên
            var nhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(phieuNhap.MaNhanVien);
            if (nhanVien == null)
                throw new ArgumentException("Mã nhân viên không tồn tại.");

            // Kiểm tra trùng mã phiếu nhập
            var existingPhieuNhap = await _phieuNhapDAL.GetPhieuNhapByIdAsync(phieuNhap.MaPhieuNhap);
            if (existingPhieuNhap != null)
                throw new ArgumentException("Mã phiếu nhập đã tồn tại.");

            return await _phieuNhapDAL.AddPhieuNhapAsync(phieuNhap);
        }

        // Cập nhật phiếu nhập
        public async Task<bool> UpdatePhieuNhapAsync(PhieuNhap phieuNhap)
        {
            // Kiểm tra dữ liệu đầu vào
            if (phieuNhap == null)
                throw new ArgumentNullException(nameof(phieuNhap));
            if (string.IsNullOrEmpty(phieuNhap.MaPhieuNhap) || string.IsNullOrEmpty(phieuNhap.MaNhaCungCap) ||
                string.IsNullOrEmpty(phieuNhap.MaNhanVien))
                throw new ArgumentException("Mã phiếu nhập, mã nhà cung cấp và mã nhân viên không được để trống.");

            // Kiểm tra ngày nhập hợp lệ
            if (phieuNhap.NgayNhap > DateTime.Now)
                throw new ArgumentException("Ngày nhập không được là ngày trong tương lai.");

            // Kiểm tra sự tồn tại của phiếu nhập
            var existingPhieuNhap = await _phieuNhapDAL.GetPhieuNhapByIdAsync(phieuNhap.MaPhieuNhap);
            if (existingPhieuNhap == null)
                throw new Exception("Phiếu nhập không tồn tại.");

            // Kiểm tra mã nhà cung cấp
            var nhaCungCap = await _nhaCungCapDAL.GetNhaCungCapByIdAsync(phieuNhap.MaNhaCungCap);
            if (nhaCungCap == null)
                throw new ArgumentException("Mã nhà cung cấp không tồn tại.");

            // Kiểm tra mã nhân viên
            var nhanVien = await _nhanVienDAL.GetNhanVienByIdAsync(phieuNhap.MaNhanVien);
            if (nhanVien == null)
                throw new ArgumentException("Mã nhân viên không tồn tại.");

            return await _phieuNhapDAL.UpdatePhieuNhapAsync(phieuNhap);
        }

        // Xóa phiếu nhập
        public async Task<bool> DeletePhieuNhapAsync(string maPhieuNhap)
        {
            if (string.IsNullOrEmpty(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");

            // Kiểm tra sự tồn tại của phiếu nhập
            var phieuNhap = await _phieuNhapDAL.GetPhieuNhapByIdAsync(maPhieuNhap);
            if (phieuNhap == null)
                throw new Exception("Phiếu nhập không tồn tại.");

            // Kiểm tra xem phiếu nhập có đang được tham chiếu trong bảng ChiTietPhieuNhap
            var chiTietPhieuNhaps = await _chiTietPhieuNhapDAL.GetAllChiTietPhieuNhapAsync();
            if (chiTietPhieuNhaps.Any(ct => ct.MaPhieuNhap == maPhieuNhap))
                throw new InvalidOperationException("Không thể xóa phiếu nhập vì đang được tham chiếu trong chi tiết phiếu nhập.");

            return await _phieuNhapDAL.DeletePhieuNhapAsync(maPhieuNhap);
        }
    }
}