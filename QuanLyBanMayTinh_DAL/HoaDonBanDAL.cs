using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL
{
    public class HoaDonBanDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        // Lấy danh sách tất cả hóa đơn bán
        public async Task<List<HoaDonBan>> GetAllHoaDonBanAsync()
        {
            List<HoaDonBan> hoaDonBans = new List<HoaDonBan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaHoaDon, NgayBan, MaKhachHang, MaNhanVien, TongTien, GhiChu 
                                FROM HoaDonBan";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            hoaDonBans.Add(new HoaDonBan
                            {
                                MaHoaDon = reader["MaHoaDon"].ToString(),
                                NgayBan = Convert.ToDateTime(reader["NgayBan"]),
                                MaKhachHang = reader["MaKhachHang"]?.ToString(),
                                MaNhanVien = reader["MaNhanVien"].ToString(),
                                TongTien = Convert.ToDecimal(reader["TongTien"]),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }

            return hoaDonBans;
        }

        // Lấy hóa đơn bán theo mã
        public async Task<HoaDonBan> GetHoaDonBanByIdAsync(string maHoaDon)
        {
            HoaDonBan hoaDonBan = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaHoaDon, NgayBan, MaKhachHang, MaNhanVien, TongTien, GhiChu 
                                FROM HoaDonBan WHERE MaHoaDon = @MaHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            hoaDonBan = new HoaDonBan
                            {
                                MaHoaDon = reader["MaHoaDon"].ToString(),
                                NgayBan = Convert.ToDateTime(reader["NgayBan"]),
                                MaKhachHang = reader["MaKhachHang"]?.ToString(),
                                MaNhanVien = reader["MaNhanVien"].ToString(),
                                TongTien = Convert.ToDecimal(reader["TongTien"]),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }

            return hoaDonBan;
        }

        // Kiểm tra xem MaKhachHang có tồn tại trong bảng KhachHang hay không
        private async Task<bool> IsValidMaKhachHangAsync(string maKhachHang)
        {
            if (string.IsNullOrEmpty(maKhachHang))
                return true; // MaKhachHang có thể NULL

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM KhachHang WHERE MaKhachHang = @MaKhachHang";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    await conn.OpenAsync();
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        // Kiểm tra xem MaNhanVien có tồn tại trong bảng NhanVien hay không
        private async Task<bool> IsValidMaNhanVienAsync(string maNhanVien)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    await conn.OpenAsync();
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        // Thêm hóa đơn bán mới
        public async Task<bool> AddHoaDonBanAsync(HoaDonBan hoaDonBan)
        {
            // Kiểm tra MaKhachHang hợp lệ
            if (!await IsValidMaKhachHangAsync(hoaDonBan.MaKhachHang))
            {
                throw new ArgumentException("Mã khách hàng không tồn tại.");
            }

            // Kiểm tra MaNhanVien hợp lệ
            if (!await IsValidMaNhanVienAsync(hoaDonBan.MaNhanVien))
            {
                throw new ArgumentException("Mã nhân viên không tồn tại.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO HoaDonBan (MaHoaDon, NgayBan, MaKhachHang, MaNhanVien, TongTien, GhiChu)
                                VALUES (@MaHoaDon, @NgayBan, @MaKhachHang, @MaNhanVien, @TongTien, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", hoaDonBan.MaHoaDon);
                    cmd.Parameters.AddWithValue("@NgayBan", hoaDonBan.NgayBan);
                    cmd.Parameters.AddWithValue("@MaKhachHang", (object)hoaDonBan.MaKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaNhanVien", hoaDonBan.MaNhanVien);
                    cmd.Parameters.AddWithValue("@TongTien", hoaDonBan.TongTien);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)hoaDonBan.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Cập nhật hóa đơn bán
        public async Task<bool> UpdateHoaDonBanAsync(HoaDonBan hoaDonBan)
        {
            // Kiểm tra MaKhachHang hợp lệ
            if (!await IsValidMaKhachHangAsync(hoaDonBan.MaKhachHang))
            {
                throw new ArgumentException("Mã khách hàng không tồn tại.");
            }

            // Kiểm tra MaNhanVien hợp lệ
            if (!await IsValidMaNhanVienAsync(hoaDonBan.MaNhanVien))
            {
                throw new ArgumentException("Mã nhân viên không tồn tại.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE HoaDonBan 
                                SET NgayBan = @NgayBan, MaKhachHang = @MaKhachHang, MaNhanVien = @MaNhanVien, 
                                    TongTien = @TongTien, GhiChu = @GhiChu
                                WHERE MaHoaDon = @MaHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", hoaDonBan.MaHoaDon);
                    cmd.Parameters.AddWithValue("@NgayBan", hoaDonBan.NgayBan);
                    cmd.Parameters.AddWithValue("@MaKhachHang", (object)hoaDonBan.MaKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaNhanVien", hoaDonBan.MaNhanVien);
                    cmd.Parameters.AddWithValue("@TongTien", hoaDonBan.TongTien);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)hoaDonBan.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Xóa hóa đơn bán
        public async Task<bool> DeleteHoaDonBanAsync(string maHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM HoaDonBan WHERE MaHoaDon = @MaHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

    // Lớp model cho HoaDonBan
    public class HoaDonBan
    {
        public string MaHoaDon { get; set; }
        public DateTime NgayBan { get; set; }
        public string MaKhachHang { get; set; }
        public string MaNhanVien { get; set; }
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }
    }
}