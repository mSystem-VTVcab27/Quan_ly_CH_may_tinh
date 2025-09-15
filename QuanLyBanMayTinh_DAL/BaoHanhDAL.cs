using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
namespace DAL
{
    public class BaoHanhDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        // Lấy danh sách tất cả bảo hành
        public async Task<List<BaoHanh>> GetAllBaoHanhAsync()
        {
            List<BaoHanh> baoHanhs = new List<BaoHanh>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaBaoHanh, MaHoaDon, MaSanPham, NgayBatDau, NgayKetThuc, TinhTrang, GhiChu 
                                FROM BaoHanh";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            baoHanhs.Add(new BaoHanh
                            {
                                MaBaoHanh = reader["MaBaoHanh"].ToString(),
                                MaHoaDon = reader["MaHoaDon"].ToString(),
                                MaSanPham = reader["MaSanPham"].ToString(),
                                NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                                NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]),
                                TinhTrang = reader["TinhTrang"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }

            return baoHanhs;
        }

        // Lấy bảo hành theo mã
        public async Task<BaoHanh> GetBaoHanhByIdAsync(string maBaoHanh)
        {
            BaoHanh baoHanh = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaBaoHanh, MaHoaDon, MaSanPham, NgayBatDau, NgayKetThuc, TinhTrang, GhiChu 
                                FROM BaoHanh WHERE MaBaoHanh = @MaBaoHanh";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBaoHanh", maBaoHanh);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            baoHanh = new BaoHanh
                            {
                                MaBaoHanh = reader["MaBaoHanh"].ToString(),
                                MaHoaDon = reader["MaHoaDon"].ToString(),
                                MaSanPham = reader["MaSanPham"].ToString(),
                                NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                                NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]),
                                TinhTrang = reader["TinhTrang"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }

            return baoHanh;
        }

        // Kiểm tra xem MaHoaDon có tồn tại trong bảng HoaDonBan hay không
        private async Task<bool> IsValidMaHoaDonAsync(string maHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM HoaDonBan WHERE MaHoaDon = @MaHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
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

        // Thêm bảo hành mới
        public async Task<bool> AddBaoHanhAsync(BaoHanh baoHanh)
        {
            // Kiểm tra MaHoaDon hợp lệ
            if (!await IsValidMaHoaDonAsync(baoHanh.MaHoaDon))
            {
                throw new ArgumentException("Mã hóa đơn không tồn tại.");
            }

            // Kiểm tra MaSanPham hợp lệ
            if (!await IsValidMaSanPhamAsync(baoHanh.MaSanPham))
            {
                throw new ArgumentException("Mã sản phẩm không tồn tại.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO BaoHanh (MaBaoHanh, MaHoaDon, MaSanPham, NgayBatDau, NgayKetThuc, TinhTrang, GhiChu)
                                VALUES (@MaBaoHanh, @MaHoaDon, @MaSanPham, @NgayBatDau, @NgayKetThuc, @TinhTrang, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBaoHanh", baoHanh.MaBaoHanh);
                    cmd.Parameters.AddWithValue("@MaHoaDon", baoHanh.MaHoaDon);
                    cmd.Parameters.AddWithValue("@MaSanPham", baoHanh.MaSanPham);
                    cmd.Parameters.AddWithValue("@NgayBatDau", baoHanh.NgayBatDau);
                    cmd.Parameters.AddWithValue("@NgayKetThuc", baoHanh.NgayKetThuc);
                    cmd.Parameters.AddWithValue("@TinhTrang", baoHanh.TinhTrang);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)baoHanh.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Cập nhật bảo hành
        public async Task<bool> UpdateBaoHanhAsync(BaoHanh baoHanh)
        {
            // Kiểm tra MaHoaDon hợp lệ
            if (!await IsValidMaHoaDonAsync(baoHanh.MaHoaDon))
            {
                throw new ArgumentException("Mã hóa đơn không tồn tại.");
            }

            // Kiểm tra MaSanPham hợp lệ
            if (!await IsValidMaSanPhamAsync(baoHanh.MaSanPham))
            {
                throw new ArgumentException("Mã sản phẩm không tồn tại.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE BaoHanh 
                                SET MaHoaDon = @MaHoaDon, MaSanPham = @MaSanPham, NgayBatDau = @NgayBatDau, 
                                    NgayKetThuc = @NgayKetThuc, TinhTrang = @TinhTrang, GhiChu = @GhiChu
                                WHERE MaBaoHanh = @MaBaoHanh";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBaoHanh", baoHanh.MaBaoHanh);
                    cmd.Parameters.AddWithValue("@MaHoaDon", baoHanh.MaHoaDon);
                    cmd.Parameters.AddWithValue("@MaSanPham", baoHanh.MaSanPham);
                    cmd.Parameters.AddWithValue("@NgayBatDau", baoHanh.NgayBatDau);
                    cmd.Parameters.AddWithValue("@NgayKetThuc", baoHanh.NgayKetThuc);
                    cmd.Parameters.AddWithValue("@TinhTrang", baoHanh.TinhTrang);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)baoHanh.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Xóa bảo hành
        public async Task<bool> DeleteBaoHanhAsync(string maBaoHanh)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM BaoHanh WHERE MaBaoHanh = @MaBaoHanh";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBaoHanh", maBaoHanh);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

    // Lớp model cho BaoHanh
    public class BaoHanh
    {
        public string MaBaoHanh { get; set; }
        public string MaHoaDon { get; set; }
        public string MaSanPham { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TinhTrang { get; set; }
        public string GhiChu { get; set; }
    }
}