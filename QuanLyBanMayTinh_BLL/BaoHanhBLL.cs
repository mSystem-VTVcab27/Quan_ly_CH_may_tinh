using DAL;

namespace BLL
{
    public class BaoHanhBLL
    {
        private readonly BaoHanhDAL _baoHanhDAL;

        public BaoHanhBLL()
        {
            _baoHanhDAL = new BaoHanhDAL();
        }

        /// <summary>
        /// Lấy danh sách tất cả các phiếu bảo hành.
        /// </summary>
        /// <returns>Danh sách các đối tượng BaoHanh.</returns>
        public async Task<List<BaoHanh>> GetAllBaoHanhAsync()
        {
            return await _baoHanhDAL.GetAllBaoHanhAsync();
        }

        /// <summary>
        /// Lấy thông tin bảo hành theo mã.
        /// </summary>
        /// <param name="maBaoHanh">Mã bảo hành cần lấy.</param>
        /// <returns>Đối tượng BaoHanh hoặc null nếu không tìm thấy.</returns>
        public async Task<BaoHanh> GetBaoHanhByIdAsync(string maBaoHanh)
        {
            if (string.IsNullOrWhiteSpace(maBaoHanh))
            {
                throw new ArgumentException("Mã bảo hành không được để trống.");
            }
            return await _baoHanhDAL.GetBaoHanhByIdAsync(maBaoHanh);
        }

        /// <summary>
        /// Thêm một phiếu bảo hành mới.
        /// </summary>
        /// <param name="baoHanh">Đối tượng BaoHanh cần thêm.</param>
        /// <returns>True nếu thêm thành công, False nếu thất bại.</returns>
        public async Task<bool> AddBaoHanhAsync(BaoHanh baoHanh)
        {
            if (baoHanh == null)
            {
                throw new ArgumentNullException(nameof(baoHanh), "Thông tin bảo hành không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(baoHanh.MaBaoHanh) || string.IsNullOrWhiteSpace(baoHanh.MaHoaDon) || string.IsNullOrWhiteSpace(baoHanh.MaSanPham))
            {
                throw new ArgumentException("Mã bảo hành, Mã hóa đơn và Mã sản phẩm không được để trống.");
            }

            if (baoHanh.NgayBatDau >= baoHanh.NgayKetThuc)
            {
                throw new ArgumentException("Ngày bắt đầu phải sớm hơn ngày kết thúc.");
            }

            // Kiểm tra sự tồn tại của mã bảo hành
            var existingWarranty = await GetBaoHanhByIdAsync(baoHanh.MaBaoHanh);
            if (existingWarranty != null)
            {
                throw new ArgumentException("Mã bảo hành đã tồn tại.");
            }

            return await _baoHanhDAL.AddBaoHanhAsync(baoHanh);
        }

        /// <summary>
        /// Cập nhật thông tin một phiếu bảo hành.
        /// </summary>
        /// <param name="baoHanh">Đối tượng BaoHanh cần cập nhật.</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        public async Task<bool> UpdateBaoHanhAsync(BaoHanh baoHanh)
        {
            if (baoHanh == null)
            {
                throw new ArgumentNullException(nameof(baoHanh), "Thông tin bảo hành không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(baoHanh.MaBaoHanh) || string.IsNullOrWhiteSpace(baoHanh.MaHoaDon) || string.IsNullOrWhiteSpace(baoHanh.MaSanPham))
            {
                throw new ArgumentException("Mã bảo hành, Mã hóa đơn và Mã sản phẩm không được để trống.");
            }

            if (baoHanh.NgayBatDau >= baoHanh.NgayKetThuc)
            {
                throw new ArgumentException("Ngày bắt đầu phải sớm hơn ngày kết thúc.");
            }

            // Kiểm tra sự tồn tại của bảo hành trước khi cập nhật
            var existingWarranty = await GetBaoHanhByIdAsync(baoHanh.MaBaoHanh);
            if (existingWarranty == null)
            {
                throw new ArgumentException("Không tìm thấy bảo hành để cập nhật.");
            }

            return await _baoHanhDAL.UpdateBaoHanhAsync(baoHanh);
        }

        /// <summary>
        /// Xóa một phiếu bảo hành.
        /// </summary>
        /// <param name="maBaoHanh">Mã bảo hành cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại.</returns>
        public async Task<bool> DeleteBaoHanhAsync(string maBaoHanh)
        {
            if (string.IsNullOrWhiteSpace(maBaoHanh))
            {
                throw new ArgumentException("Mã bảo hành không được để trống.");
            }
            return await _baoHanhDAL.DeleteBaoHanhAsync(maBaoHanh);
        }
    }
}