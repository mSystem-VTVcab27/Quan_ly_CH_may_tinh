using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL
{
    public class KhachHangDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        // Lấy danh sách tất cả khách hàng
        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            List<KhachHang> khachHangs = new List<KhachHang>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaKhachHang, HoTen, SoDienThoai, DiaChi, GhiChu 
                                FROM KhachHang";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            khachHangs.Add(new KhachHang
                            {
                                MaKhachHang = reader["MaKhachHang"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }

            return khachHangs;
        }

        // Lấy khách hàng theo mã
        public async Task<KhachHang> GetKhachHangByIdAsync(string maKhachHang)
        {
            KhachHang khachHang = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaKhachHang, HoTen, SoDienThoai, DiaChi, GhiChu 
                                FROM KhachHang WHERE MaKhachHang = @MaKhachHang";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            khachHang = new KhachHang
                            {
                                MaKhachHang = reader["MaKhachHang"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }

            return khachHang;
        }

        // Thêm khách hàng mới
        public async Task<bool> AddKhachHangAsync(KhachHang khachHang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO KhachHang (MaKhachHang, HoTen, SoDienThoai, DiaChi, GhiChu)
                                VALUES (@MaKhachHang, @HoTen, @SoDienThoai, @DiaChi, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", khachHang.MaKhachHang);
                    cmd.Parameters.AddWithValue("@HoTen", khachHang.HoTen);
                    cmd.Parameters.AddWithValue("@SoDienThoai", khachHang.SoDienThoai);
                    cmd.Parameters.AddWithValue("@DiaChi", khachHang.DiaChi);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)khachHang.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Cập nhật khách hàng
        public async Task<bool> UpdateKhachHangAsync(KhachHang khachHang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE KhachHang 
                                SET HoTen = @HoTen, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, GhiChu = @GhiChu
                                WHERE MaKhachHang = @MaKhachHang";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", khachHang.MaKhachHang);
                    cmd.Parameters.AddWithValue("@HoTen", khachHang.HoTen);
                    cmd.Parameters.AddWithValue("@SoDienThoai", khachHang.SoDienThoai);
                    cmd.Parameters.AddWithValue("@DiaChi", khachHang.DiaChi);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)khachHang.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Xóa khách hàng
        public async Task<bool> DeleteKhachHangAsync(string maKhachHang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

    // Lớp model cho KhachHang
    public class KhachHang
    {
        public string MaKhachHang { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
    }
}