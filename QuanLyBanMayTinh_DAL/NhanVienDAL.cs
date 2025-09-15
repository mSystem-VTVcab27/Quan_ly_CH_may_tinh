using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL
{
    public class NhanVienDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        // Lấy danh sách tất cả nhân viên
        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            List<NhanVien> nhanViens = new List<NhanVien>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaNhanVien, HoTen, GioiTinh, NgayVaoLam, SoDienThoai, Email, DiaChi, GhiChu 
                                FROM NhanVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            nhanViens.Add(new NhanVien
                            {
                                MaNhanVien = reader["MaNhanVien"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                GioiTinh = reader["GioiTinh"]?.ToString(),
                                NgayVaoLam = reader["NgayVaoLam"] != DBNull.Value ? Convert.ToDateTime(reader["NgayVaoLam"]) : null,
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                Email = reader["Email"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }

            return nhanViens;
        }

        // Lấy nhân viên theo mã
        public async Task<NhanVien> GetNhanVienByIdAsync(string maNhanVien)
        {
            NhanVien nhanVien = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaNhanVien, HoTen, GioiTinh, NgayVaoLam, SoDienThoai, Email, DiaChi, GhiChu 
                                FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            nhanVien = new NhanVien
                            {
                                MaNhanVien = reader["MaNhanVien"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                GioiTinh = reader["GioiTinh"]?.ToString(),
                                NgayVaoLam = reader["NgayVaoLam"] != DBNull.Value ? Convert.ToDateTime(reader["NgayVaoLam"]) : null,
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                Email = reader["Email"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }

            return nhanVien;
        }

        // Thêm nhân viên mới
        public async Task<bool> AddNhanVienAsync(NhanVien nhanVien)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO NhanVien (MaNhanVien, HoTen, GioiTinh, NgayVaoLam, SoDienThoai, Email, DiaChi, GhiChu)
                                VALUES (@MaNhanVien, @HoTen, @GioiTinh, @NgayVaoLam, @SoDienThoai, @Email, @DiaChi, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", nhanVien.MaNhanVien);
                    cmd.Parameters.AddWithValue("@HoTen", nhanVien.HoTen);
                    cmd.Parameters.AddWithValue("@GioiTinh", (object)nhanVien.GioiTinh ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgayVaoLam", (object)nhanVien.NgayVaoLam ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoDienThoai", nhanVien.SoDienThoai);
                    cmd.Parameters.AddWithValue("@Email", nhanVien.Email);
                    cmd.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)nhanVien.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Cập nhật nhân viên
        public async Task<bool> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE NhanVien 
                                SET HoTen = @HoTen, GioiTinh = @GioiTinh, NgayVaoLam = @NgayVaoLam, 
                                    SoDienThoai = @SoDienThoai, Email = @Email, DiaChi = @DiaChi, GhiChu = @GhiChu
                                WHERE MaNhanVien = @MaNhanVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", nhanVien.MaNhanVien);
                    cmd.Parameters.AddWithValue("@HoTen", nhanVien.HoTen);
                    cmd.Parameters.AddWithValue("@GioiTinh", (object)nhanVien.GioiTinh ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgayVaoLam", (object)nhanVien.NgayVaoLam ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoDienThoai", nhanVien.SoDienThoai);
                    cmd.Parameters.AddWithValue("@Email", nhanVien.Email);
                    cmd.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)nhanVien.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Xóa nhân viên
        public async Task<bool> DeleteNhanVienAsync(string maNhanVien)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

    // Lớp model cho NhanVien
    public class NhanVien
    {
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
    }
}