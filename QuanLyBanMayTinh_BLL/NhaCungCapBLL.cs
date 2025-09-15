using System;

using System.Collections.Generic;

using System.Text.RegularExpressions;

using System.Threading.Tasks;

using DAL; // Giả sử các lớp DAL nằm trong namespace DAL



namespace BLL

{

    public class NhaCungCapBLL

    {

        private readonly NhaCungCapDAL _nhaCungCapDAL;

        private readonly PhieuNhapDAL _phieuNhapDAL;



        public NhaCungCapBLL()

        {

            _nhaCungCapDAL = new NhaCungCapDAL();

            _phieuNhapDAL = new PhieuNhapDAL();

        }



        // Lấy danh sách tất cả nhà cung cấp

        public async Task<List<NhaCungCap>> GetAllNhaCungCapAsync()

        {

            return await _nhaCungCapDAL.GetAllNhaCungCapAsync();

        }



        // Lấy nhà cung cấp theo mã

        public async Task<NhaCungCap> GetNhaCungCapByIdAsync(string maNhaCungCap)

        {

            if (string.IsNullOrEmpty(maNhaCungCap))

                throw new ArgumentException("Mã nhà cung cấp không được để trống.");



            var nhaCungCap = await _nhaCungCapDAL.GetNhaCungCapByIdAsync(maNhaCungCap);

            if (nhaCungCap == null)

                throw new Exception("Nhà cung cấp không tồn tại.");



            return nhaCungCap;

        }



        // Thêm nhà cung cấp mới

        public async Task<bool> AddNhaCungCapAsync(NhaCungCap nhaCungCap)

        {

            // Kiểm tra dữ liệu đầu vào

            if (nhaCungCap == null)

                throw new ArgumentNullException(nameof(nhaCungCap));

            if (string.IsNullOrEmpty(nhaCungCap.MaNhaCungCap) || string.IsNullOrEmpty(nhaCungCap.TenNhaCungCap) ||

                string.IsNullOrEmpty(nhaCungCap.SoDienThoai) || string.IsNullOrEmpty(nhaCungCap.DiaChi) ||

                string.IsNullOrEmpty(nhaCungCap.Email))

                throw new ArgumentException("Mã nhà cung cấp, tên nhà cung cấp, số điện thoại, địa chỉ và email không được để trống.");



            // Kiểm tra định dạng số điện thoại (ví dụ: 10 chữ số, bắt đầu bằng 0)

            if (!Regex.IsMatch(nhaCungCap.SoDienThoai, @"^0\d{9}$"))

                throw new ArgumentException("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0.");



            // Kiểm tra định dạng email

            if (!Regex.IsMatch(nhaCungCap.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))

                throw new ArgumentException("Định dạng email không hợp lệ.");



            // Kiểm tra trùng mã nhà cung cấp

            var existingNhaCungCap = await _nhaCungCapDAL.GetNhaCungCapByIdAsync(nhaCungCap.MaNhaCungCap);

            if (existingNhaCungCap != null)

                throw new ArgumentException("Mã nhà cung cấp đã tồn tại.");



            return await _nhaCungCapDAL.AddNhaCungCapAsync(nhaCungCap);

        }



        // Cập nhật nhà cung cấp

        public async Task<bool> UpdateNhaCungCapAsync(NhaCungCap nhaCungCap)

        {

            // Kiểm tra dữ liệu đầu vào

            if (nhaCungCap == null)

                throw new ArgumentNullException(nameof(nhaCungCap));

            if (string.IsNullOrEmpty(nhaCungCap.MaNhaCungCap) || string.IsNullOrEmpty(nhaCungCap.TenNhaCungCap) ||

                string.IsNullOrEmpty(nhaCungCap.SoDienThoai) || string.IsNullOrEmpty(nhaCungCap.DiaChi) ||

                string.IsNullOrEmpty(nhaCungCap.Email))

                throw new ArgumentException("Mã nhà cung cấp, tên nhà cung cấp, số điện thoại, địa chỉ và email không được để trống.");



            // Kiểm tra định dạng số điện thoại

            if (!Regex.IsMatch(nhaCungCap.SoDienThoai, @"^0\d{9}$"))

                throw new ArgumentException("Số điện thoại phải có 10 chữ số và bắt đầu bằng 0.");



            // Kiểm tra định dạng email

            if (!Regex.IsMatch(nhaCungCap.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))

                throw new ArgumentException("Định dạng email không hợp lệ.");



            // Kiểm tra sự tồn tại của nhà cung cấp

            var existingNhaCungCap = await _nhaCungCapDAL.GetNhaCungCapByIdAsync(nhaCungCap.MaNhaCungCap);

            if (existingNhaCungCap == null)

                throw new Exception("Nhà cung cấp không tồn tại.");



            return await _nhaCungCapDAL.UpdateNhaCungCapAsync(nhaCungCap);

        }



        // Xóa nhà cung cấp

        public async Task<bool> DeleteNhaCungCapAsync(string maNhaCungCap)

        {

            if (string.IsNullOrEmpty(maNhaCungCap))

                throw new ArgumentException("Mã nhà cung cấp không được để trống.");



            // Kiểm tra sự tồn tại của nhà cung cấp

            var nhaCungCap = await _nhaCungCapDAL.GetNhaCungCapByIdAsync(maNhaCungCap);

            if (nhaCungCap == null)

                throw new Exception("Nhà cung cấp không tồn tại.");



            // Kiểm tra xem nhà cung cấp có đang được tham chiếu trong bảng PhieuNhap

            var phieuNhaps = await _phieuNhapDAL.GetAllPhieuNhapAsync();

            if (phieuNhaps.Any(pn => pn.MaNhaCungCap == maNhaCungCap))

                throw new InvalidOperationException("Không thể xóa nhà cung cấp vì đang được tham chiếu trong phiếu nhập.");



            return await _nhaCungCapDAL.DeleteNhaCungCapAsync(maNhaCungCap);

        }

    }

}