using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL

{

    public class NhaCungCapDAL
    {

        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";
        // Lấy danh sách tất cả nhà cung cấp
        public async Task<List<NhaCungCap>> GetAllNhaCungCapAsync()
        {
            List<NhaCungCap> nhaCungCaps = new List<NhaCungCap>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaNhaCungCap, TenNhaCungCap, SoDienThoai, DiaChi, Email, GhiChu 
                                FROM NhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            nhaCungCaps.Add(new NhaCungCap
                            {
                                MaNhaCungCap = reader["MaNhaCungCap"].ToString(),
                                TenNhaCungCap = reader["TenNhaCungCap"].ToString(),
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                Email = reader["Email"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }
            return nhaCungCaps;
        }
        // Lấy nhà cung cấp theo mã
        public async Task<NhaCungCap> GetNhaCungCapByIdAsync(string maNhaCungCap)
        {
            NhaCungCap nhaCungCap = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaNhaCungCap, TenNhaCungCap, SoDienThoai, DiaChi, Email, GhiChu 
                                FROM NhaCungCap WHERE MaNhaCungCap = @MaNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhaCungCap", maNhaCungCap);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            nhaCungCap = new NhaCungCap
                            {
                                MaNhaCungCap = reader["MaNhaCungCap"].ToString(),
                                TenNhaCungCap = reader["TenNhaCungCap"].ToString(),
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                Email = reader["Email"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }
            return nhaCungCap;
        }
        // Thêm nhà cung cấp mới
        public async Task<bool> AddNhaCungCapAsync(NhaCungCap nhaCungCap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO NhaCungCap (MaNhaCungCap, TenNhaCungCap, SoDienThoai, DiaChi, Email, GhiChu)

                                VALUES (@MaNhaCungCap, @TenNhaCungCap, @SoDienThoai, @DiaChi, @Email, @GhiChu)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhaCungCap", nhaCungCap.MaNhaCungCap);
                    cmd.Parameters.AddWithValue("@TenNhaCungCap", nhaCungCap.TenNhaCungCap);
                    cmd.Parameters.AddWithValue("@SoDienThoai", nhaCungCap.SoDienThoai);
                    cmd.Parameters.AddWithValue("@DiaChi", nhaCungCap.DiaChi);
                    cmd.Parameters.AddWithValue("@Email", nhaCungCap.Email);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)nhaCungCap.GhiChu ?? DBNull.Value);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        // Cập nhật nhà cung cấp
        public async Task<bool> UpdateNhaCungCapAsync(NhaCungCap nhaCungCap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE NhaCungCap 
                                SET TenNhaCungCap = @TenNhaCungCap, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, 
                                    Email = @Email, GhiChu = @GhiChu
                                WHERE MaNhaCungCap = @MaNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhaCungCap", nhaCungCap.MaNhaCungCap);
                    cmd.Parameters.AddWithValue("@TenNhaCungCap", nhaCungCap.TenNhaCungCap);
                    cmd.Parameters.AddWithValue("@SoDienThoai", nhaCungCap.SoDienThoai);
                    cmd.Parameters.AddWithValue("@DiaChi", nhaCungCap.DiaChi);
                    cmd.Parameters.AddWithValue("@Email", nhaCungCap.Email);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)nhaCungCap.GhiChu ?? DBNull.Value);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        // Xóa nhà cung cấp
        public async Task<bool> DeleteNhaCungCapAsync(string maNhaCungCap)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM NhaCungCap WHERE MaNhaCungCap = @MaNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhaCungCap", maNhaCungCap);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
    public class NhaCungCap
    {
        public string MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string GhiChu { get; set; }
    }
}