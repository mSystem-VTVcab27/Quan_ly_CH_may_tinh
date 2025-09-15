using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL
{
    public class DanhMucSanPhamDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        // Lấy danh sách tất cả danh mục sản phẩm
        public async Task<List<DanhMucSanPham>> GetAllDanhMucSanPhamAsync()
        {
            List<DanhMucSanPham> danhMucSanPhams = new List<DanhMucSanPham>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaDanhMuc, TenDanhMuc, MoTa, GhiChu 
                                FROM DanhMucSanPham";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            danhMucSanPhams.Add(new DanhMucSanPham
                            {
                                MaDanhMuc = reader["MaDanhMuc"].ToString(),
                                TenDanhMuc = reader["TenDanhMuc"].ToString(),
                                MoTa = reader["MoTa"]?.ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }

            return danhMucSanPhams;
        }

        // Lấy danh mục sản phẩm theo mã
        public async Task<DanhMucSanPham> GetDanhMucSanPhamByIdAsync(string maDanhMuc)
        {
            DanhMucSanPham danhMucSanPham = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaDanhMuc, TenDanhMuc, MoTa, GhiChu 
                                FROM DanhMucSanPham WHERE MaDanhMuc = @MaDanhMuc";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDanhMuc", maDanhMuc);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            danhMucSanPham = new DanhMucSanPham
                            {
                                MaDanhMuc = reader["MaDanhMuc"].ToString(),
                                TenDanhMuc = reader["TenDanhMuc"].ToString(),
                                MoTa = reader["MoTa"]?.ToString(),
                                GhiChu = reader["GhiChu"]?.ToString()
                            };
                        }
                    }
                }
            }

            return danhMucSanPham;
        }

        // Thêm danh mục sản phẩm mới
        public async Task<bool> AddDanhMucSanPhamAsync(DanhMucSanPham danhMucSanPham)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO DanhMucSanPham (MaDanhMuc, TenDanhMuc, MoTa, GhiChu)
                                VALUES (@MaDanhMuc, @TenDanhMuc, @MoTa, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDanhMuc", danhMucSanPham.MaDanhMuc);
                    cmd.Parameters.AddWithValue("@TenDanhMuc", danhMucSanPham.TenDanhMuc);
                    cmd.Parameters.AddWithValue("@MoTa", (object)danhMucSanPham.MoTa ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)danhMucSanPham.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Cập nhật danh mục sản phẩm
        public async Task<bool> UpdateDanhMucSanPhamAsync(DanhMucSanPham danhMucSanPham)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE DanhMucSanPham 
                                SET TenDanhMuc = @TenDanhMuc, MoTa = @MoTa, GhiChu = @GhiChu
                                WHERE MaDanhMuc = @MaDanhMuc";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDanhMuc", danhMucSanPham.MaDanhMuc);
                    cmd.Parameters.AddWithValue("@TenDanhMuc", danhMucSanPham.TenDanhMuc);
                    cmd.Parameters.AddWithValue("@MoTa", (object)danhMucSanPham.MoTa ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)danhMucSanPham.GhiChu ?? DBNull.Value);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Xóa danh mục sản phẩm
        public async Task<bool> DeleteDanhMucSanPhamAsync(string maDanhMuc)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM DanhMucSanPham WHERE MaDanhMuc = @MaDanhMuc";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDanhMuc", maDanhMuc);
                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

    // Lớp model cho DanhMucSanPham
    public class DanhMucSanPham
    {
        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public string GhiChu { get; set; }
    }
}