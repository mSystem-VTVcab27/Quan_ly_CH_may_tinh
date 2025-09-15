using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL
{
    public class ChiTietPhieuNhapDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        // Lấy danh sách tất cả chi tiết phiếu nhập
        public async Task<List<ChiTietPhieuNhap>> GetAllChiTietPhieuNhapAsync()
        {
            List<ChiTietPhieuNhap> chiTietPhieuNhaps = new List<ChiTietPhieuNhap>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaChiTietPhieuNhap, MaPhieuNhap, MaSanPham, SoLuong, GiaNhap, GhiChu 
                                FROM ChiTietPhieuNhap";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            chiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                            {
                                MaChiTietPhieuNhap = reader["MaChiTietPhieuNhap"].ToString(),
                                MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                MaSanPham = reader["MaSanPham"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                GiaNhap = Convert.ToDecimal(reader["GiaNhap"]),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }

            return chiTietPhieuNhaps;
        }

        // Lấy chi tiết phiếu nhập theo mã
        public async Task<ChiTietPhieuNhap> GetChiTietPhieuNhapByIdAsync(string maChiTietPhieuNhap)
        {
            ChiTietPhieuNhap chiTietPhieuNhap = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaChiTietPhieuNhap, MaPhieuNhap, MaSanPham, SoLuong, GiaNhap, GhiChu 
                                FROM ChiTietPhieuNhap WHERE MaChiTietPhieuNhap = @MaChiTietPhieuNhap";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietPhieuNhap", maChiTietPhieuNhap);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            chiTietPhieuNhap = new ChiTietPhieuNhap
                            {
                                MaChiTietPhieuNhap = reader["MaChiTietPhieuNhap"].ToString(),
                                MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                MaSanPham = reader["MaSanPham"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                GiaNhap = Convert.ToDecimal(reader["GiaNhap"]),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }

            return chiTietPhieuNhap;
        }

        // Kiểm tra xem MaPhieuNhap có tồn tại trong bảng PhieuNhap hay không
        private async Task<bool> IsValidMaPhieuNhapAsync(string maPhieuNhap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                    await conn.OpenAsync();
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        // Kiểm tra xem MaSanPham có tồn tại trong bảng SanPham hay không
        private async Task<bool> IsValidMaSanPhamAsync(string maSanPham)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM SanPham WHERE MaSanPham = @MaSanPham";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSanPham", maSanPham);
                    await conn.OpenAsync();
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        // Thêm chi tiết phiếu nhập mới
        public async Task<bool> AddChiTietPhieuNhapAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            // Kiểm tra MaPhieuNhap hợp lệ
            if (!await IsValidMaPhieuNhapAsync(chiTietPhieuNhap.MaPhieuNhap))
            {
                throw new ArgumentException("Mã phiếu nhập không tồn tại.");
            }

            // Kiểm tra MaSanPham hợp lệ
            if (!await IsValidMaSanPhamAsync(chiTietPhieuNhap.MaSanPham))
            {
                throw new ArgumentException("Mã sản phẩm không tồn tại.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ChiTietPhieuNhap (MaChiTietPhieuNhap, MaPhieuNhap, MaSanPham, SoLuong, GiaNhap, GhiChu)
                                VALUES (@MaChiTietPhieuNhap, @MaPhieuNhap, @MaSanPham, @SoLuong, @GiaNhap, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietPhieuNhap", chiTietPhieuNhap.MaChiTietPhieuNhap);
                    cmd.Parameters.AddWithValue("@MaPhieuNhap", chiTietPhieuNhap.MaPhieuNhap);
                    cmd.Parameters.AddWithValue("@MaSanPham", chiTietPhieuNhap.MaSanPham);
                    cmd.Parameters.AddWithValue("@SoLuong", chiTietPhieuNhap.SoLuong);
                    cmd.Parameters.AddWithValue("@GiaNhap", chiTietPhieuNhap.GiaNhap);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)chiTietPhieuNhap.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Cập nhật chi tiết phiếu nhập
        public async Task<bool> UpdateChiTietPhieuNhapAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            // Kiểm tra MaPhieuNhap hợp lệ
            if (!await IsValidMaPhieuNhapAsync(chiTietPhieuNhap.MaPhieuNhap))
            {
                throw new ArgumentException("Mã phiếu nhập không tồn tại.");
            }

            // Kiểm tra MaSanPham hợp lệ
            if (!await IsValidMaSanPhamAsync(chiTietPhieuNhap.MaSanPham))
            {
                throw new ArgumentException("Mã sản phẩm không tồn tại.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE ChiTietPhieuNhap 
                                SET MaPhieuNhap = @MaPhieuNhap, MaSanPham = @MaSanPham, SoLuong = @SoLuong, 
                                    GiaNhap = @GiaNhap, GhiChu = @GhiChu
                                WHERE MaChiTietPhieuNhap = @MaChiTietPhieuNhap";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietPhieuNhap", chiTietPhieuNhap.MaChiTietPhieuNhap);
                    cmd.Parameters.AddWithValue("@MaPhieuNhap", chiTietPhieuNhap.MaPhieuNhap);
                    cmd.Parameters.AddWithValue("@MaSanPham", chiTietPhieuNhap.MaSanPham);
                    cmd.Parameters.AddWithValue("@SoLuong", chiTietPhieuNhap.SoLuong);
                    cmd.Parameters.AddWithValue("@GiaNhap", chiTietPhieuNhap.GiaNhap);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)chiTietPhieuNhap.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Xóa chi tiết phiếu nhập
        public async Task<bool> DeleteChiTietPhieuNhapAsync(string maChiTietPhieuNhap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM ChiTietPhieuNhap WHERE MaChiTietPhieuNhap = @MaChiTietPhieuNhap";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietPhieuNhap", maChiTietPhieuNhap);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

    // Lớp model cho ChiTietPhieuNhap
    public class ChiTietPhieuNhap
    {
        public string MaChiTietPhieuNhap { get; set; }
        public string MaPhieuNhap { get; set; }
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
        public string GhiChu { get; set; }
    }
}