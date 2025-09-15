using BLL;
using DAL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmSanPham : UserControl
    {
        private readonly SanPhamBLL _sanPhamBLL;
        private readonly DanhMucSanPhamBLL _danhMucSanPhamBLL;

        public FrmSanPham()
        {
            InitializeComponent();
            _sanPhamBLL = new SanPhamBLL();
            _danhMucSanPhamBLL = new DanhMucSanPhamBLL();
            LoadSanPhamList();
            LoadDanhMucComboBox();
            SetupDataGridViewColumns();
        }

        private async void LoadSanPhamList()
        {
            try
            {
                var sanPhams = await _sanPhamBLL.GetAllSanPhamAsync();
                dataGridViewSanPham.DataSource = sanPhams;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadDanhMucComboBox()
        {
            try
            {
                var danhMucs = await _danhMucSanPhamBLL.GetAllDanhMucSanPhamAsync();
                cbMaDanhMuc.DataSource = danhMucs;
                cbMaDanhMuc.DisplayMember = "TenDanhMuc";
                cbMaDanhMuc.ValueMember = "MaDanhMuc";
                cbMaDanhMuc.SelectedIndex = -1; // Không chọn mặc định

                // Tải danh mục cho bộ lọc
                cbFilterMaDanhMuc.DataSource = danhMucs.ToList(); // Sao chép danh sách để tránh ảnh hưởng
                cbFilterMaDanhMuc.DisplayMember = "TenDanhMuc";
                cbFilterMaDanhMuc.ValueMember = "MaDanhMuc";
                cbFilterMaDanhMuc.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột để ta có thể tự định nghĩa
            dataGridViewSanPham.AutoGenerateColumns = false;

            // Xóa các cột cũ để tránh trùng lặp
            dataGridViewSanPham.Columns.Clear();

            // Thêm và cấu hình từng cột
            dataGridViewSanPham.Columns.Add("MaSanPham", "Mã sản phẩm");
            dataGridViewSanPham.Columns["MaSanPham"].DataPropertyName = "MaSanPham";
            dataGridViewSanPham.Columns["MaSanPham"].Width = 100;

            dataGridViewSanPham.Columns.Add("TenSanPham", "Tên sản phẩm");
            dataGridViewSanPham.Columns["TenSanPham"].DataPropertyName = "TenSanPham";
            dataGridViewSanPham.Columns["TenSanPham"].Width = 200;

            dataGridViewSanPham.Columns.Add("MaDanhMuc", "Mã danh mục");
            dataGridViewSanPham.Columns["MaDanhMuc"].DataPropertyName = "MaDanhMuc";
            dataGridViewSanPham.Columns["MaDanhMuc"].Width = 100;

            dataGridViewSanPham.Columns.Add("GiaNhap", "Giá nhập");
            dataGridViewSanPham.Columns["GiaNhap"].DataPropertyName = "GiaNhap";
            dataGridViewSanPham.Columns["GiaNhap"].Width = 100;

            dataGridViewSanPham.Columns.Add("GiaBan", "Giá bán");
            dataGridViewSanPham.Columns["GiaBan"].DataPropertyName = "GiaBan";
            dataGridViewSanPham.Columns["GiaBan"].Width = 100;

            dataGridViewSanPham.Columns.Add("BaoHanh", "Bảo hành");
            dataGridViewSanPham.Columns["BaoHanh"].DataPropertyName = "BaoHanh";
            dataGridViewSanPham.Columns["BaoHanh"].Width = 100;

            dataGridViewSanPham.Columns.Add("MoTa", "Mô tả");
            dataGridViewSanPham.Columns["MoTa"].DataPropertyName = "MoTa";
            dataGridViewSanPham.Columns["MoTa"].Width = 150;

            dataGridViewSanPham.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewSanPham.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewSanPham.Columns["GhiChu"].Width = 219;
        }

        private void dataGridViewSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewSanPham.Rows[e.RowIndex];
                txtMaSanPham.Text = row.Cells["MaSanPham"].Value?.ToString();
                txtTenSanPham.Text = row.Cells["TenSanPham"].Value?.ToString();
                cbMaDanhMuc.SelectedValue = row.Cells["MaDanhMuc"].Value?.ToString();
                if (decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out decimal giaNhap))
                    nudGiaNhap.Value = giaNhap;
                if (decimal.TryParse(row.Cells["GiaBan"].Value?.ToString(), out decimal giaBan))
                    nudGiaBan.Value = giaBan;
                if (int.TryParse(row.Cells["BaoHanh"].Value?.ToString(), out int baoHanh))
                    nudBaoHanh.Value = baoHanh;
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                string filterMaDanhMuc = cbFilterMaDanhMuc.SelectedValue?.ToString();
                decimal? minGiaNhap = nudFilterMinGiaNhap.Value > 0 ? (decimal?)nudFilterMinGiaNhap.Value : null;
                decimal? maxGiaNhap = nudFilterMaxGiaNhap.Value > 0 ? (decimal?)nudFilterMaxGiaNhap.Value : null;
                decimal? minGiaBan = nudFilterMinGiaBan.Value > 0 ? (decimal?)nudFilterMinGiaBan.Value : null;
                decimal? maxGiaBan = nudFilterMaxGiaBan.Value > 0 ? (decimal?)nudFilterMaxGiaBan.Value : null;
                int? minBaoHanh = nudFilterMinBaoHanh.Value > 0 ? (int?)nudFilterMinBaoHanh.Value : null;
                int? maxBaoHanh = nudFilterMaxBaoHanh.Value > 0 ? (int?)nudFilterMaxBaoHanh.Value : null;

                var sanPhams = await _sanPhamBLL.GetAllSanPhamAsync();

                sanPhams = sanPhams.Where(s =>
                    (string.IsNullOrEmpty(keyword) ||
                     s.MaSanPham.ToLower().Contains(keyword) ||
                     s.TenSanPham.ToLower().Contains(keyword)) &&
                    (string.IsNullOrEmpty(filterMaDanhMuc) ||
                     s.MaDanhMuc == filterMaDanhMuc) &&
                    (!minGiaNhap.HasValue || s.GiaNhap >= minGiaNhap.Value) &&
                    (!maxGiaNhap.HasValue || s.GiaNhap <= maxGiaNhap.Value) &&
                    (!minGiaBan.HasValue || s.GiaBan >= minGiaBan.Value) &&
                    (!maxGiaBan.HasValue || s.GiaBan <= maxGiaBan.Value) &&
                    (!minBaoHanh.HasValue || s.BaoHanh >= minBaoHanh.Value) &&
                    (!maxBaoHanh.HasValue || s.BaoHanh <= maxBaoHanh.Value)
                ).ToList();

                dataGridViewSanPham.DataSource = sanPhams;

                if (sanPhams.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (nudGiaNhap.Value <= 0 || nudGiaBan.Value <= 0)
                {
                    MessageBox.Show("Giá nhập và giá bán phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nudBaoHanh.Value < 0)
                {
                    MessageBox.Show("Thời gian bảo hành không được âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sanPham = new SanPham
                {
                    MaSanPham = txtMaSanPham.Text.Trim(),
                    TenSanPham = txtTenSanPham.Text.Trim(),
                    MaDanhMuc = cbMaDanhMuc.SelectedValue?.ToString(),
                    GiaNhap = (decimal)nudGiaNhap.Value,
                    GiaBan = (decimal)nudGiaBan.Value,
                    BaoHanh = (int)nudBaoHanh.Value,
                    MoTa = txtMoTa.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(sanPham.MaDanhMuc))
                {
                    MessageBox.Show("Vui lòng chọn danh mục sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _sanPhamBLL.AddSanPhamAsync(sanPham);
                if (result)
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSanPhamList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (nudGiaNhap.Value <= 0 || nudGiaBan.Value <= 0)
                {
                    MessageBox.Show("Giá nhập và giá bán phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nudBaoHanh.Value < 0)
                {
                    MessageBox.Show("Thời gian bảo hành không được âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sanPham = new SanPham
                {
                    MaSanPham = txtMaSanPham.Text.Trim(),
                    TenSanPham = txtTenSanPham.Text.Trim(),
                    MaDanhMuc = cbMaDanhMuc.SelectedValue?.ToString(),
                    GiaNhap = (decimal)nudGiaNhap.Value,
                    GiaBan = (decimal)nudGiaBan.Value,
                    BaoHanh = (int)nudBaoHanh.Value,
                    MoTa = txtMoTa.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(sanPham.MaDanhMuc))
                {
                    MessageBox.Show("Vui lòng chọn danh mục sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _sanPhamBLL.UpdateSanPhamAsync(sanPham);
                if (result)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSanPhamList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maSanPham = txtMaSanPham.Text.Trim();
                if (string.IsNullOrEmpty(maSanPham))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm {maSanPham}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _sanPhamBLL.DeleteSanPhamAsync(maSanPham);
                    if (success)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSanPhamList();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            ClearFilterInputs();
            txtTimKiem.Clear();
            LoadSanPhamList();
        }

        private void ClearInputs()
        {
            txtMaSanPham.Clear();
            txtTenSanPham.Clear();
            cbMaDanhMuc.SelectedIndex = -1;
            nudGiaNhap.Value = 0;
            nudGiaBan.Value = 0;
            nudBaoHanh.Value = 0;
            txtMoTa.Clear();
            txtGhiChu.Clear();
        }

        private void ClearFilterInputs()
        {
            cbFilterMaDanhMuc.SelectedIndex = -1;
            nudFilterMinGiaNhap.Value = 0;
            nudFilterMaxGiaNhap.Value = 0;
            nudFilterMinGiaBan.Value = 0;
            nudFilterMaxGiaBan.Value = 0;
            nudFilterMinBaoHanh.Value = 0;
            nudFilterMaxBaoHanh.Value = 0;
        }

        private void dataGridViewSanPham_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewSanPham.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewSanPham.SelectedRows[0];
                txtMaSanPham.Text = selectedRow.Cells["MaSanPham"].Value?.ToString();
                txtTenSanPham.Text = selectedRow.Cells["TenSanPham"].Value?.ToString();
                cbMaDanhMuc.SelectedValue = selectedRow.Cells["MaDanhMuc"].Value?.ToString();
                if (decimal.TryParse(selectedRow.Cells["GiaNhap"].Value?.ToString(), out decimal giaNhap))
                    nudGiaNhap.Value = giaNhap;
                if (decimal.TryParse(selectedRow.Cells["GiaBan"].Value?.ToString(), out decimal giaBan))
                    nudGiaBan.Value = giaBan;
                if (int.TryParse(selectedRow.Cells["BaoHanh"].Value?.ToString(), out int baoHanh))
                    nudBaoHanh.Value = baoHanh;
                txtMoTa.Text = selectedRow.Cells["MoTa"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewSanPham = new DataGridView();
            txtMaSanPham = new TextBox();
            txtTenSanPham = new TextBox();
            cbMaDanhMuc = new ComboBox();
            nudGiaNhap = new NumericUpDown();
            nudGiaBan = new NumericUpDown();
            nudBaoHanh = new NumericUpDown();
            txtMoTa = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaSanPham = new Label();
            lblTenSanPham = new Label();
            lblMaDanhMuc = new Label();
            lblGiaNhap = new Label();
            lblGiaBan = new Label();
            lblBaoHanh = new Label();
            lblMoTa = new Label();
            lblGhiChu = new Label();
            cbFilterMaDanhMuc = new ComboBox();
            nudFilterMinGiaNhap = new NumericUpDown();
            nudFilterMaxGiaNhap = new NumericUpDown();
            nudFilterMinGiaBan = new NumericUpDown();
            nudFilterMaxGiaBan = new NumericUpDown();
            nudFilterMinBaoHanh = new NumericUpDown();
            nudFilterMaxBaoHanh = new NumericUpDown();
            lblFilterMaDanhMuc = new Label();
            lblFilterMinGiaNhap = new Label();
            lblFilterMaxGiaNhap = new Label();
            lblFilterMinGiaBan = new Label();
            lblFilterMaxGiaBan = new Label();
            lblFilterMinBaoHanh = new Label();
            lblFilterMaxBaoHanh = new Label();
            btnXuatExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSanPham).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGiaNhap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGiaBan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBaoHanh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMinGiaNhap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMaxGiaNhap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMinGiaBan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMaxGiaBan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMinBaoHanh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMaxBaoHanh).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewSanPham
            // 
            dataGridViewSanPham.ColumnHeadersHeight = 29;
            dataGridViewSanPham.Location = new Point(3, 391);
            dataGridViewSanPham.MultiSelect = false;
            dataGridViewSanPham.Name = "dataGridViewSanPham";
            dataGridViewSanPham.RowHeadersWidth = 51;
            dataGridViewSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSanPham.Size = new Size(1282, 423);
            dataGridViewSanPham.TabIndex = 0;
            dataGridViewSanPham.CellClick += dataGridViewSanPham_CellClick;
            dataGridViewSanPham.SelectionChanged += dataGridViewSanPham_SelectionChanged;
            // 
            // txtMaSanPham
            // 
            txtMaSanPham.Font = new Font("Google Sans", 10.2F);
            txtMaSanPham.Location = new Point(187, 10);
            txtMaSanPham.Name = "txtMaSanPham";
            txtMaSanPham.Size = new Size(438, 29);
            txtMaSanPham.TabIndex = 1;
            // 
            // txtTenSanPham
            // 
            txtTenSanPham.Font = new Font("Google Sans", 10.2F);
            txtTenSanPham.Location = new Point(187, 60);
            txtTenSanPham.Name = "txtTenSanPham";
            txtTenSanPham.Size = new Size(438, 29);
            txtTenSanPham.TabIndex = 2;
            // 
            // cbMaDanhMuc
            // 
            cbMaDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaDanhMuc.Font = new Font("Google Sans", 10.2F);
            cbMaDanhMuc.Location = new Point(187, 110);
            cbMaDanhMuc.Name = "cbMaDanhMuc";
            cbMaDanhMuc.Size = new Size(438, 30);
            cbMaDanhMuc.TabIndex = 3;
            // 
            // nudGiaNhap
            // 
            nudGiaNhap.DecimalPlaces = 2;
            nudGiaNhap.Location = new Point(838, 12);
            nudGiaNhap.Name = "nudGiaNhap";
            nudGiaNhap.Size = new Size(438, 29);
            nudGiaNhap.TabIndex = 4;
            // 
            // nudGiaBan
            // 
            nudGiaBan.DecimalPlaces = 2;
            nudGiaBan.Location = new Point(838, 62);
            nudGiaBan.Name = "nudGiaBan";
            nudGiaBan.Size = new Size(438, 29);
            nudGiaBan.TabIndex = 5;
            // 
            // nudBaoHanh
            // 
            nudBaoHanh.Location = new Point(838, 112);
            nudBaoHanh.Name = "nudBaoHanh";
            nudBaoHanh.Size = new Size(438, 29);
            nudBaoHanh.TabIndex = 6;
            // 
            // txtMoTa
            // 
            txtMoTa.Font = new Font("Google Sans", 10.2F);
            txtMoTa.Location = new Point(838, 162);
            txtMoTa.Name = "txtMoTa";
            txtMoTa.Size = new Size(438, 29);
            txtMoTa.TabIndex = 7;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(838, 212);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(438, 29);
            txtGhiChu.TabIndex = 8;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(838, 284);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(253, 29);
            txtTimKiem.TabIndex = 9;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(173, 204);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(102, 37);
            btnThem.TabIndex = 10;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(286, 204);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(102, 37);
            btnSua.TabIndex = 11;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(405, 204);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(102, 37);
            btnXoa.TabIndex = 12;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(523, 204);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(102, 37);
            btnLamMoi.TabIndex = 13;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(838, 330);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(253, 40);
            btnTimKiem.TabIndex = 14;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaSanPham
            // 
            lblMaSanPham.Font = new Font("Google Sans", 10.2F);
            lblMaSanPham.Location = new Point(7, 12);
            lblMaSanPham.Name = "lblMaSanPham";
            lblMaSanPham.Size = new Size(132, 25);
            lblMaSanPham.TabIndex = 15;
            lblMaSanPham.Text = "Mã sản phẩm:";
            // 
            // lblTenSanPham
            // 
            lblTenSanPham.Font = new Font("Google Sans", 10.2F);
            lblTenSanPham.Location = new Point(7, 62);
            lblTenSanPham.Name = "lblTenSanPham";
            lblTenSanPham.Size = new Size(132, 25);
            lblTenSanPham.TabIndex = 16;
            lblTenSanPham.Text = "Tên sản phẩm:";
            // 
            // lblMaDanhMuc
            // 
            lblMaDanhMuc.Font = new Font("Google Sans", 10.2F);
            lblMaDanhMuc.Location = new Point(7, 112);
            lblMaDanhMuc.Name = "lblMaDanhMuc";
            lblMaDanhMuc.Size = new Size(132, 25);
            lblMaDanhMuc.TabIndex = 17;
            lblMaDanhMuc.Text = "Danh mục:";
            // 
            // lblGiaNhap
            // 
            lblGiaNhap.Font = new Font("Google Sans", 10.2F);
            lblGiaNhap.Location = new Point(658, 14);
            lblGiaNhap.Name = "lblGiaNhap";
            lblGiaNhap.Size = new Size(132, 25);
            lblGiaNhap.TabIndex = 18;
            lblGiaNhap.Text = "Giá nhập:";
            // 
            // lblGiaBan
            // 
            lblGiaBan.Font = new Font("Google Sans", 10.2F);
            lblGiaBan.Location = new Point(658, 64);
            lblGiaBan.Name = "lblGiaBan";
            lblGiaBan.Size = new Size(132, 25);
            lblGiaBan.TabIndex = 19;
            lblGiaBan.Text = "Giá bán:";
            // 
            // lblBaoHanh
            // 
            lblBaoHanh.Font = new Font("Google Sans", 10.2F);
            lblBaoHanh.Location = new Point(658, 114);
            lblBaoHanh.Name = "lblBaoHanh";
            lblBaoHanh.Size = new Size(158, 25);
            lblBaoHanh.TabIndex = 20;
            lblBaoHanh.Text = "Bảo hành (tháng):";
            // 
            // lblMoTa
            // 
            lblMoTa.Font = new Font("Google Sans", 10.2F);
            lblMoTa.Location = new Point(658, 164);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(132, 25);
            lblMoTa.TabIndex = 21;
            lblMoTa.Text = "Mô tả:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(658, 214);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(132, 25);
            lblGhiChu.TabIndex = 22;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // cbFilterMaDanhMuc
            // 
            cbFilterMaDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterMaDanhMuc.Font = new Font("Google Sans", 10.2F);
            cbFilterMaDanhMuc.Location = new Point(368, 284);
            cbFilterMaDanhMuc.Name = "cbFilterMaDanhMuc";
            cbFilterMaDanhMuc.Size = new Size(438, 30);
            cbFilterMaDanhMuc.TabIndex = 24;
            // 
            // nudFilterMinGiaNhap
            // 
            nudFilterMinGiaNhap.DecimalPlaces = 2;
            nudFilterMinGiaNhap.Location = new Point(368, 335);
            nudFilterMinGiaNhap.Name = "nudFilterMinGiaNhap";
            nudFilterMinGiaNhap.Size = new Size(142, 29);
            nudFilterMinGiaNhap.TabIndex = 25;
            // 
            // nudFilterMaxGiaNhap
            // 
            nudFilterMaxGiaNhap.DecimalPlaces = 2;
            nudFilterMaxGiaNhap.Location = new Point(664, 335);
            nudFilterMaxGiaNhap.Name = "nudFilterMaxGiaNhap";
            nudFilterMaxGiaNhap.Size = new Size(142, 29);
            nudFilterMaxGiaNhap.TabIndex = 26;
            // 
            // nudFilterMinGiaBan
            // 
            nudFilterMinGiaBan.DecimalPlaces = 2;
            nudFilterMinGiaBan.Location = new Point(838, 12);
            nudFilterMinGiaBan.Name = "nudFilterMinGiaBan";
            nudFilterMinGiaBan.Size = new Size(200, 29);
            nudFilterMinGiaBan.TabIndex = 27;
            // 
            // nudFilterMaxGiaBan
            // 
            nudFilterMaxGiaBan.DecimalPlaces = 2;
            nudFilterMaxGiaBan.Location = new Point(1076, 12);
            nudFilterMaxGiaBan.Name = "nudFilterMaxGiaBan";
            nudFilterMaxGiaBan.Size = new Size(200, 29);
            nudFilterMaxGiaBan.TabIndex = 28;
            // 
            // nudFilterMinBaoHanh
            // 
            nudFilterMinBaoHanh.Location = new Point(838, 62);
            nudFilterMinBaoHanh.Name = "nudFilterMinBaoHanh";
            nudFilterMinBaoHanh.Size = new Size(200, 29);
            nudFilterMinBaoHanh.TabIndex = 29;
            // 
            // nudFilterMaxBaoHanh
            // 
            nudFilterMaxBaoHanh.Location = new Point(1076, 62);
            nudFilterMaxBaoHanh.Name = "nudFilterMaxBaoHanh";
            nudFilterMaxBaoHanh.Size = new Size(200, 29);
            nudFilterMaxBaoHanh.TabIndex = 30;
            // 
            // lblFilterMaDanhMuc
            // 
            lblFilterMaDanhMuc.Font = new Font("Google Sans", 10.2F);
            lblFilterMaDanhMuc.Location = new Point(188, 286);
            lblFilterMaDanhMuc.Name = "lblFilterMaDanhMuc";
            lblFilterMaDanhMuc.Size = new Size(132, 25);
            lblFilterMaDanhMuc.TabIndex = 31;
            lblFilterMaDanhMuc.Text = "Lọc danh mục:";
            // 
            // lblFilterMinGiaNhap
            // 
            lblFilterMinGiaNhap.Font = new Font("Google Sans", 10.2F);
            lblFilterMinGiaNhap.Location = new Point(188, 337);
            lblFilterMinGiaNhap.Name = "lblFilterMinGiaNhap";
            lblFilterMinGiaNhap.Size = new Size(132, 25);
            lblFilterMinGiaNhap.TabIndex = 32;
            lblFilterMinGiaNhap.Text = "Giá nhập từ:";
            // 
            // lblFilterMaxGiaNhap
            // 
            lblFilterMaxGiaNhap.Font = new Font("Google Sans", 10.2F);
            lblFilterMaxGiaNhap.Location = new Point(562, 339);
            lblFilterMaxGiaNhap.Name = "lblFilterMaxGiaNhap";
            lblFilterMaxGiaNhap.Size = new Size(48, 25);
            lblFilterMaxGiaNhap.TabIndex = 33;
            lblFilterMaxGiaNhap.Text = "Đến:";
            // 
            // lblFilterMinGiaBan
            // 
            lblFilterMinGiaBan.Font = new Font("Google Sans", 10.2F);
            lblFilterMinGiaBan.Location = new Point(658, 14);
            lblFilterMinGiaBan.Name = "lblFilterMinGiaBan";
            lblFilterMinGiaBan.Size = new Size(132, 25);
            lblFilterMinGiaBan.TabIndex = 34;
            lblFilterMinGiaBan.Text = "Giá bán từ:";
            // 
            // lblFilterMaxGiaBan
            // 
            lblFilterMaxGiaBan.Font = new Font("Google Sans", 10.2F);
            lblFilterMaxGiaBan.Location = new Point(896, 14);
            lblFilterMaxGiaBan.Name = "lblFilterMaxGiaBan";
            lblFilterMaxGiaBan.Size = new Size(132, 25);
            lblFilterMaxGiaBan.TabIndex = 35;
            lblFilterMaxGiaBan.Text = "Đến:";
            // 
            // lblFilterMinBaoHanh
            // 
            lblFilterMinBaoHanh.Font = new Font("Google Sans", 10.2F);
            lblFilterMinBaoHanh.Location = new Point(658, 64);
            lblFilterMinBaoHanh.Name = "lblFilterMinBaoHanh";
            lblFilterMinBaoHanh.Size = new Size(158, 25);
            lblFilterMinBaoHanh.TabIndex = 36;
            lblFilterMinBaoHanh.Text = "Bảo hành từ:";
            // 
            // lblFilterMaxBaoHanh
            // 
            lblFilterMaxBaoHanh.Font = new Font("Google Sans", 10.2F);
            lblFilterMaxBaoHanh.Location = new Point(896, 64);
            lblFilterMaxBaoHanh.Name = "lblFilterMaxBaoHanh";
            lblFilterMaxBaoHanh.Size = new Size(132, 25);
            lblFilterMaxBaoHanh.TabIndex = 37;
            lblFilterMaxBaoHanh.Text = "Đến:";
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.Location = new Point(20, 330);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(119, 37);
            btnXuatExcel.TabIndex = 38;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // FrmSanPham
            // 
            Controls.Add(btnXuatExcel);
            Controls.Add(dataGridViewSanPham);
            Controls.Add(txtMaSanPham);
            Controls.Add(txtTenSanPham);
            Controls.Add(cbMaDanhMuc);
            Controls.Add(nudGiaNhap);
            Controls.Add(nudGiaBan);
            Controls.Add(nudBaoHanh);
            Controls.Add(txtMoTa);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaSanPham);
            Controls.Add(lblTenSanPham);
            Controls.Add(lblMaDanhMuc);
            Controls.Add(lblGiaNhap);
            Controls.Add(lblGiaBan);
            Controls.Add(lblBaoHanh);
            Controls.Add(lblMoTa);
            Controls.Add(lblGhiChu);
            Controls.Add(cbFilterMaDanhMuc);
            Controls.Add(nudFilterMaxGiaNhap);
            Controls.Add(nudFilterMinGiaBan);
            Controls.Add(nudFilterMaxGiaBan);
            Controls.Add(nudFilterMinBaoHanh);
            Controls.Add(nudFilterMaxBaoHanh);
            Controls.Add(lblFilterMaDanhMuc);
            Controls.Add(lblFilterMinGiaNhap);
            Controls.Add(lblFilterMaxGiaNhap);
            Controls.Add(lblFilterMinGiaBan);
            Controls.Add(lblFilterMaxGiaBan);
            Controls.Add(lblFilterMinBaoHanh);
            Controls.Add(lblFilterMaxBaoHanh);
            Controls.Add(nudFilterMinGiaNhap);
            Font = new Font("Google Sans", 10.2F);
            Name = "FrmSanPham";
            Size = new Size(1288, 836);
            ((System.ComponentModel.ISupportInitialize)dataGridViewSanPham).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGiaNhap).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGiaBan).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBaoHanh).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMinGiaNhap).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMaxGiaNhap).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMinGiaBan).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMaxGiaBan).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMinBaoHanh).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterMaxBaoHanh).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewSanPham;
        private TextBox txtMaSanPham;
        private TextBox txtTenSanPham;
        private ComboBox cbMaDanhMuc;
        private NumericUpDown nudGiaNhap;
        private NumericUpDown nudGiaBan;
        private NumericUpDown nudBaoHanh;
        private TextBox txtMoTa;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaSanPham;
        private Label lblTenSanPham;
        private Label lblMaDanhMuc;
        private Label lblGiaNhap;
        private Label lblGiaBan;
        private Label lblBaoHanh;
        private Label lblMoTa;
        private Label lblGhiChu;
        private ComboBox cbFilterMaDanhMuc;
        private NumericUpDown nudFilterMinGiaNhap;
        private NumericUpDown nudFilterMaxGiaNhap;
        private NumericUpDown nudFilterMinGiaBan;
        private NumericUpDown nudFilterMaxGiaBan;
        private NumericUpDown nudFilterMinBaoHanh;
        private NumericUpDown nudFilterMaxBaoHanh;
        private Label lblFilterMaDanhMuc;
        private Label lblFilterMinGiaNhap;
        private Label lblFilterMaxGiaNhap;
        private Label lblFilterMinGiaBan;
        private Label lblFilterMaxGiaBan;
        private Label lblFilterMinBaoHanh;
        private Label lblFilterMaxBaoHanh;
        private Button btnXuatExcel;

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewSanPham.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!");
                    return;
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("DSSanPham");
                    for (int i = 0; i < dataGridViewSanPham.Columns.Count; i++)
                        worksheet.Cells[1, i + 1].Value = dataGridViewSanPham.Columns[i].HeaderText;

                    for (int i = 0; i < dataGridViewSanPham.Rows.Count; i++)
                        for (int j = 0; j < dataGridViewSanPham.Columns.Count; j++)
                            worksheet.Cells[i + 2, j + 1].Value = dataGridViewSanPham.Rows[i].Cells[j].Value?.ToString();

                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveFileDialog.FileName = "DSSanPham.xlsx";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            var file = new FileInfo(saveFileDialog.FileName);
                            package.SaveAs(file);
                            MessageBox.Show("Xuất Excel thành công: " + file.FullName);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hoặc mở Excel: " + ex.Message);
            }
        }
    }
}