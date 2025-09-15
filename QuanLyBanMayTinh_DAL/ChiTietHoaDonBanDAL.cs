using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL
{
    public class ChiTietHoaDonBanDAL
    {
        private readonly string connectionString = "Server=msystem;Database=Quan_ly_ban_may_tinh;Integrated Security=True;TrustServerCertificate = True";

        public async Task<List<ChiTietHoaDonBan>> GetAllChiTietHoaDonBanAsync()
        {
            List<ChiTietHoaDonBan> chiTietHoaDonBans = new List<ChiTietHoaDonBan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaChiTietHoaDon, MaHoaDon, MaSanPham, SoLuong, DonGia, GiamGia, GhiChu 
                                FROM ChiTietHoaDonBan";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                chiTietHoaDonBans.Add(new ChiTietHoaDonBan
                                {
                                    MaChiTietHoaDon = reader["MaChiTietHoaDon"].ToString(),
                                    MaHoaDon = reader["MaHoaDon"].ToString(),
                                    MaSanPham = reader["MaSanPham"].ToString(),
                                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                    DonGia = Convert.ToDecimal(reader["DonGia"]),
                                    GiamGia = Convert.ToDecimal(reader["GiamGia"]),
                                    GhiChu = reader["GhiChu"]?.ToString()
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi lấy danh sách chi tiết hóa đơn bán: " + ex.Message);
                    }
                }
            }

            return chiTietHoaDonBans;
        }

        public async Task<ChiTietHoaDonBan> GetChiTietHoaDonBanByIdAsync(string maChiTietHoaDon)
        {
            ChiTietHoaDonBan chiTietHoaDonBan = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaChiTietHoaDon, MaHoaDon, MaSanPham, SoLuong, DonGia, GiamGia, GhiChu 
                                FROM ChiTietHoaDonBan WHERE MaChiTietHoaDon = @MaChiTietHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietHoaDon", maChiTietHoaDon);
                    try
                    {
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                chiTietHoaDonBan = new ChiTietHoaDonBan
                                {
                                    MaChiTietHoaDon = reader["MaChiTietHoaDon"].ToString(),
                                    MaHoaDon = reader["MaHoaDon"].ToString(),
                                    MaSanPham = reader["MaSanPham"].ToString(),
                                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                    DonGia = Convert.ToDecimal(reader["DonGia"]),
                                    GiamGia = Convert.ToDecimal(reader["GiamGia"]),
                                    GhiChu = reader["GhiChu"]?.ToString()
                                };
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi lấy chi tiết hóa đơn bán: " + ex.Message);
                    }
                }
            }

            return chiTietHoaDonBan;
        }

        private async Task<bool> IsValidMaHoaDonAsync(string maHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM HoaDonBan WHERE MaHoaDon = @MaHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    try
                    {
                        await conn.OpenAsync();
                        int count = (int)await cmd.ExecuteScalarAsync();
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi kiểm tra mã hóa đơn: " + ex.Message);
                    }
                }
            }
        }

        private async Task<bool> IsValidMaSanPhamAsync(string maSanPham)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM SanPham WHERE MaSanPham = @MaSanPham";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSanPham", maSanPham);
                    try
                    {
                        await conn.OpenAsync();
                        int count = (int)await cmd.ExecuteScalarAsync();
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi kiểm tra mã sản phẩm: " + ex.Message);
                    }
                }
            }
        }

        public async Task<bool> AddChiTietHoaDonBanAsync(ChiTietHoaDonBan chiTietHoaDonBan)
        {
            if (!await IsValidMaHoaDonAsync(chiTietHoaDonBan.MaHoaDon))
                throw new ArgumentException("Mã hóa đơn không tồn tại.");

            if (!await IsValidMaSanPhamAsync(chiTietHoaDonBan.MaSanPham))
                throw new ArgumentException("Mã sản phẩm không tồn tại.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ChiTietHoaDonBan (MaChiTietHoaDon, MaHoaDon, MaSanPham, SoLuong, DonGia, GiamGia, GhiChu)
                                VALUES (@MaChiTietHoaDon, @MaHoaDon, @MaSanPham, @SoLuong, @DonGia, @GiamGia, @GhiChu)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietHoaDon", chiTietHoaDonBan.MaChiTietHoaDon);
                    cmd.Parameters.AddWithValue("@MaHoaDon", chiTietHoaDonBan.MaHoaDon);
                    cmd.Parameters.AddWithValue("@MaSanPham", chiTietHoaDonBan.MaSanPham);
                    cmd.Parameters.AddWithValue("@SoLuong", chiTietHoaDonBan.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", chiTietHoaDonBan.DonGia);
                    cmd.Parameters.AddWithValue("@GiamGia", chiTietHoaDonBan.GiamGia);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)chiTietHoaDonBan.GhiChu ?? DBNull.Value);

                    try
                    {
                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi thêm chi tiết hóa đơn bán: " + ex.Message);
                    }
                }
            }
        }

        public async Task<bool> UpdateChiTietHoaDonBanAsync(ChiTietHoaDonBan chiTietHoaDonBan)
        {
            if (!await IsValidMaHoaDonAsync(chiTietHoaDonBan.MaHoaDon))
                throw new ArgumentException("Mã hóa đơn không tồn tại.");

            if (!await IsValidMaSanPhamAsync(chiTietHoaDonBan.MaSanPham))
                throw new ArgumentException("Mã sản phẩm không tồn tại.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE ChiTietHoaDonBan 
                                SET MaHoaDon = @MaHoaDon, MaSanPham = @MaSanPham, SoLuong = @SoLuong, 
                                    DonGia = @DonGia, GiamGia = @GiamGia, GhiChu = @GhiChu
                                WHERE MaChiTietHoaDon = @MaChiTietHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietHoaDon", chiTietHoaDonBan.MaChiTietHoaDon);
                    cmd.Parameters.AddWithValue("@MaHoaDon", chiTietHoaDonBan.MaHoaDon);
                    cmd.Parameters.AddWithValue("@MaSanPham", chiTietHoaDonBan.MaSanPham);
                    cmd.Parameters.AddWithValue("@SoLuong", chiTietHoaDonBan.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", chiTietHoaDonBan.DonGia);
                    cmd.Parameters.AddWithValue("@GiamGia", chiTietHoaDonBan.GiamGia);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)chiTietHoaDonBan.GhiChu ?? DBNull.Value);

                    try
                    {
                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi cập nhật chi tiết hóa đơn bán: " + ex.Message);
                    }
                }
            }
        }

        public async Task<bool> DeleteChiTietHoaDonBanAsync(string maChiTietHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM ChiTietHoaDonBan WHERE MaChiTietHoaDon = @MaChiTietHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietHoaDon", maChiTietHoaDon);
                    try
                    {
                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi xóa chi tiết hóa đơn bán: " + ex.Message);
                    }
                }
            }
        }

        public async Task<bool> HasWarrantyAsync(string maChiTietHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM BaoHanh WHERE MaChiTietHoaDon = @MaChiTietHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaChiTietHoaDon", maChiTietHoaDon);
                    try
                    {
                        await conn.OpenAsync();
                        int count = (int)await cmd.ExecuteScalarAsync();
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi kiểm tra bảo hành: " + ex.Message);
                    }
                }
            }
        }

        public async Task<bool> DeleteByMaHoaDonAsync(string maHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM ChiTietHoaDonBan WHERE MaHoaDon = @MaHoaDon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    try
                    {
                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi khi xóa chi tiết hóa đơn bán theo mã hóa đơn: " + ex.Message);
                    }
                }
            }
        }
    }

    public class ChiTietHoaDonBan
    {
        public string MaChiTietHoaDon { get; set; }
        public string MaHoaDon { get; set; }
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiamGia { get; set; }
        public string GhiChu { get; set; }
    }
}