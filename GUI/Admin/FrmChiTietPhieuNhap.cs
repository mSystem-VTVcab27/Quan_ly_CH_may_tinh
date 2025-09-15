using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    public partial class FrmChiTietPhieuNhap : UserControl
    {
        private readonly ChiTietPhieuNhapBLL _chiTietPhieuNhapBLL;
        private readonly PhieuNhapBLL _phieuNhapBLL;
        private readonly SanPhamBLL _sanPhamBLL;

        public FrmChiTietPhieuNhap()
        {
            InitializeComponent();
            _chiTietPhieuNhapBLL = new ChiTietPhieuNhapBLL();
            _phieuNhapBLL = new PhieuNhapBLL();
            _sanPhamBLL = new SanPhamBLL();
            LoadChiTietPhieuNhapList();
            LoadComboBoxes();
            SetupDataGridViewColumns();
        }

        private async void LoadChiTietPhieuNhapList()
        {
            try
            {
                var chiTietPhieuNhaps = await _chiTietPhieuNhapBLL.GetAllChiTietPhieuNhapAsync();
                dataGridViewChiTietPhieuNhap.DataSource = chiTietPhieuNhaps;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadComboBoxes()
        {
            try
            {
                // Tải danh sách phiếu nhập cho ComboBox
                var phieuNhaps = await _phieuNhapBLL.GetAllPhieuNhapAsync();
                cbMaPhieuNhap.DataSource = phieuNhaps;
                cbMaPhieuNhap.DisplayMember = "MaPhieuNhap";
                cbMaPhieuNhap.ValueMember = "MaPhieuNhap";
                cbMaPhieuNhap.SelectedIndex = -1; // Không chọn mặc định

                // Tải danh sách sản phẩm cho ComboBox
                var sanPhams = await _sanPhamBLL.GetAllSanPhamAsync();
                cbMaSanPham.DataSource = sanPhams;
                cbMaSanPham.DisplayMember = "TenSanPham";
                cbMaSanPham.ValueMember = "MaSanPham";
                cbMaSanPham.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ComboBox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột để ta có thể tự định nghĩa
            dataGridViewChiTietPhieuNhap.AutoGenerateColumns = false;

            // Xóa các cột cũ nếu có để tránh trùng lặp
            dataGridViewChiTietPhieuNhap.Columns.Clear();

            // Thêm và cấu hình cột "Mã Chi Tiết"
            dataGridViewChiTietPhieuNhap.Columns.Add("MaChiTietPhieuNhap", "Mã chi tiết");
            dataGridViewChiTietPhieuNhap.Columns["MaChiTietPhieuNhap"].DataPropertyName = "MaChiTietPhieuNhap";
            dataGridViewChiTietPhieuNhap.Columns["MaChiTietPhieuNhap"].Width = 100;

            // Thêm và cấu hình cột "Mã Phiếu Nhập"
            dataGridViewChiTietPhieuNhap.Columns.Add("MaPhieuNhap", "Mã phiếu nhập");
            dataGridViewChiTietPhieuNhap.Columns["MaPhieuNhap"].DataPropertyName = "MaPhieuNhap";
            dataGridViewChiTietPhieuNhap.Columns["MaPhieuNhap"].Width = 100;

            // Thêm và cấu hình cột "Mã Sản Phẩm"
            dataGridViewChiTietPhieuNhap.Columns.Add("MaSanPham", "Mã sản phẩm");
            dataGridViewChiTietPhieuNhap.Columns["MaSanPham"].DataPropertyName = "MaSanPham";
            dataGridViewChiTietPhieuNhap.Columns["MaSanPham"].Width = 100;

            // Thêm và cấu hình cột "Số Lượng"
            dataGridViewChiTietPhieuNhap.Columns.Add("SoLuong", "Số lượng");
            dataGridViewChiTietPhieuNhap.Columns["SoLuong"].DataPropertyName = "SoLuong";
            dataGridViewChiTietPhieuNhap.Columns["SoLuong"].Width = 80;

            // Thêm và cấu hình cột "Giá Nhập"
            dataGridViewChiTietPhieuNhap.Columns.Add("GiaNhap", "Giá nhập");
            dataGridViewChiTietPhieuNhap.Columns["GiaNhap"].DataPropertyName = "GiaNhap";
            dataGridViewChiTietPhieuNhap.Columns["GiaNhap"].Width = 100;

            // Thêm và cấu hình cột "Ghi Chú"
            dataGridViewChiTietPhieuNhap.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewChiTietPhieuNhap.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewChiTietPhieuNhap.Columns["GhiChu"].Width = 200;
        }

        private void dataGridViewChiTietPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewChiTietPhieuNhap.Rows[e.RowIndex];
                txtMaChiTietPhieuNhap.Text = row.Cells["MaChiTietPhieuNhap"].Value?.ToString();
                cbMaPhieuNhap.SelectedValue = row.Cells["MaPhieuNhap"].Value?.ToString();
                cbMaSanPham.SelectedValue = row.Cells["MaSanPham"].Value?.ToString();
                if (int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong))
                    nudSoLuong.Value = soLuong;
                if (decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out decimal giaNhap))
                    nudGiaNhap.Value = giaNhap;
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var chiTietPhieuNhaps = await _chiTietPhieuNhapBLL.GetAllChiTietPhieuNhapAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    chiTietPhieuNhaps = chiTietPhieuNhaps.Where(ct =>
                        ct.MaChiTietPhieuNhap.ToLower().Contains(keyword) ||
                        ct.MaSanPham.ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewChiTietPhieuNhap.DataSource = chiTietPhieuNhaps;

                if (chiTietPhieuNhaps.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy chi tiết phiếu nhập nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (nudSoLuong.Value <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nudGiaNhap.Value <= 0)
                {
                    MessageBox.Show("Giá nhập phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var chiTietPhieuNhap = new ChiTietPhieuNhap
                {
                    MaChiTietPhieuNhap = txtMaChiTietPhieuNhap.Text.Trim(),
                    MaPhieuNhap = cbMaPhieuNhap.SelectedValue?.ToString(),
                    MaSanPham = cbMaSanPham.SelectedValue?.ToString(),
                    SoLuong = (int)nudSoLuong.Value,
                    GiaNhap = (decimal)nudGiaNhap.Value,
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(chiTietPhieuNhap.MaPhieuNhap) || string.IsNullOrEmpty(chiTietPhieuNhap.MaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập và sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _chiTietPhieuNhapBLL.AddChiTietPhieuNhapAsync(chiTietPhieuNhap);
                if (result)
                {
                    MessageBox.Show("Thêm chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiTietPhieuNhapList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (nudSoLuong.Value <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nudGiaNhap.Value <= 0)
                {
                    MessageBox.Show("Giá nhập phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var chiTietPhieuNhap = new ChiTietPhieuNhap
                {
                    MaChiTietPhieuNhap = txtMaChiTietPhieuNhap.Text.Trim(),
                    MaPhieuNhap = cbMaPhieuNhap.SelectedValue?.ToString(),
                    MaSanPham = cbMaSanPham.SelectedValue?.ToString(),
                    SoLuong = (int)nudSoLuong.Value,
                    GiaNhap = (decimal)nudGiaNhap.Value,
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(chiTietPhieuNhap.MaPhieuNhap) || string.IsNullOrEmpty(chiTietPhieuNhap.MaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập và sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _chiTietPhieuNhapBLL.UpdateChiTietPhieuNhapAsync(chiTietPhieuNhap);
                if (result)
                {
                    MessageBox.Show("Cập nhật chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiTietPhieuNhapList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maChiTietPhieuNhap = txtMaChiTietPhieuNhap.Text.Trim();
                if (string.IsNullOrEmpty(maChiTietPhieuNhap))
                {
                    MessageBox.Show("Vui lòng chọn chi tiết phiếu nhập để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa chi tiết phiếu nhập {maChiTietPhieuNhap}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _chiTietPhieuNhapBLL.DeleteChiTietPhieuNhapAsync(maChiTietPhieuNhap);
                    if (success)
                    {
                        MessageBox.Show("Xóa chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadChiTietPhieuNhapList();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadChiTietPhieuNhapList();
        }

        private void ClearInputs()
        {
            txtMaChiTietPhieuNhap.Clear();
            cbMaPhieuNhap.SelectedIndex = -1;
            cbMaSanPham.SelectedIndex = -1;
            nudSoLuong.Value = 1;
            nudGiaNhap.Value = 0;
            txtGhiChu.Clear();
        }

        private void dataGridViewChiTietPhieuNhap_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewChiTietPhieuNhap.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewChiTietPhieuNhap.SelectedRows[0];
                txtMaChiTietPhieuNhap.Text = selectedRow.Cells["MaChiTietPhieuNhap"].Value?.ToString();
                cbMaPhieuNhap.SelectedValue = selectedRow.Cells["MaPhieuNhap"].Value?.ToString();
                cbMaSanPham.SelectedValue = selectedRow.Cells["MaSanPham"].Value?.ToString();
                if (int.TryParse(selectedRow.Cells["SoLuong"].Value?.ToString(), out int soLuong))
                    nudSoLuong.Value = soLuong;
                if (decimal.TryParse(selectedRow.Cells["GiaNhap"].Value?.ToString(), out decimal giaNhap))
                    nudGiaNhap.Value = giaNhap;
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewChiTietPhieuNhap = new DataGridView();
            txtMaChiTietPhieuNhap = new TextBox();
            cbMaPhieuNhap = new ComboBox();
            cbMaSanPham = new ComboBox();
            nudSoLuong = new NumericUpDown();
            nudGiaNhap = new NumericUpDown();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaChiTietPhieuNhap = new Label();
            lblMaPhieuNhap = new Label();
            lblMaSanPham = new Label();
            lblSoLuong = new Label();
            lblGiaNhap = new Label();
            lblGhiChu = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChiTietPhieuNhap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGiaNhap).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewChiTietPhieuNhap
            // 
            dataGridViewChiTietPhieuNhap.ColumnHeadersHeight = 29;
            dataGridViewChiTietPhieuNhap.Location = new Point(3, 371);
            dataGridViewChiTietPhieuNhap.MultiSelect = false;
            dataGridViewChiTietPhieuNhap.Name = "dataGridViewChiTietPhieuNhap";
            dataGridViewChiTietPhieuNhap.RowHeadersWidth = 51;
            dataGridViewChiTietPhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewChiTietPhieuNhap.Size = new Size(612, 437);
            dataGridViewChiTietPhieuNhap.TabIndex = 0;
            dataGridViewChiTietPhieuNhap.CellClick += dataGridViewChiTietPhieuNhap_CellClick;
            dataGridViewChiTietPhieuNhap.SelectionChanged += dataGridViewChiTietPhieuNhap_SelectionChanged;
            // 
            // txtMaChiTietPhieuNhap
            // 
            txtMaChiTietPhieuNhap.Font = new Font("Google Sans", 10.2F);
            txtMaChiTietPhieuNhap.Location = new Point(185, 64);
            txtMaChiTietPhieuNhap.Name = "txtMaChiTietPhieuNhap";
            txtMaChiTietPhieuNhap.Size = new Size(261, 29);
            txtMaChiTietPhieuNhap.TabIndex = 1;
            // 
            // cbMaPhieuNhap
            // 
            cbMaPhieuNhap.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaPhieuNhap.Font = new Font("Google Sans", 10.2F);
            cbMaPhieuNhap.Location = new Point(185, 114);
            cbMaPhieuNhap.Name = "cbMaPhieuNhap";
            cbMaPhieuNhap.Size = new Size(261, 30);
            cbMaPhieuNhap.TabIndex = 2;
            // 
            // cbMaSanPham
            // 
            cbMaSanPham.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaSanPham.Font = new Font("Google Sans", 10.2F);
            cbMaSanPham.Location = new Point(185, 164);
            cbMaSanPham.Name = "cbMaSanPham";
            cbMaSanPham.Size = new Size(261, 30);
            cbMaSanPham.TabIndex = 3;
            // 
            // nudSoLuong
            // 
            nudSoLuong.Font = new Font("Google Sans", 10.2F);
            nudSoLuong.Location = new Point(185, 214);
            nudSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(261, 29);
            nudSoLuong.TabIndex = 4;
            nudSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudGiaNhap
            // 
            nudGiaNhap.DecimalPlaces = 2;
            nudGiaNhap.Font = new Font("Google Sans", 10.2F);
            nudGiaNhap.Location = new Point(185, 264);
            nudGiaNhap.Name = "nudGiaNhap";
            nudGiaNhap.Size = new Size(261, 29);
            nudGiaNhap.TabIndex = 5;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(185, 314);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(261, 29);
            txtGhiChu.TabIndex = 6;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Google Sans", 10.2F);
            txtTimKiem.Location = new Point(131, 9);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(315, 29);
            txtTimKiem.TabIndex = 7;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 10.2F);
            btnThem.Location = new Point(477, 144);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(117, 44);
            btnThem.TabIndex = 8;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Google Sans", 10.2F);
            btnSua.Location = new Point(477, 194);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(117, 44);
            btnSua.TabIndex = 9;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Google Sans", 10.2F);
            btnXoa.Location = new Point(477, 249);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(117, 44);
            btnXoa.TabIndex = 10;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 10.2F);
            btnLamMoi.Location = new Point(477, 299);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(117, 44);
            btnLamMoi.TabIndex = 11;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 9F);
            btnTimKiem.Location = new Point(3, 3);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(102, 39);
            btnTimKiem.TabIndex = 12;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaChiTietPhieuNhap
            // 
            lblMaChiTietPhieuNhap.Font = new Font("Google Sans", 10.2F);
            lblMaChiTietPhieuNhap.Location = new Point(20, 69);
            lblMaChiTietPhieuNhap.Name = "lblMaChiTietPhieuNhap";
            lblMaChiTietPhieuNhap.Size = new Size(120, 25);
            lblMaChiTietPhieuNhap.TabIndex = 13;
            lblMaChiTietPhieuNhap.Text = "Mã chi tiết:";
            // 
            // lblMaPhieuNhap
            // 
            lblMaPhieuNhap.Font = new Font("Google Sans", 10.2F);
            lblMaPhieuNhap.Location = new Point(20, 119);
            lblMaPhieuNhap.Name = "lblMaPhieuNhap";
            lblMaPhieuNhap.Size = new Size(129, 25);
            lblMaPhieuNhap.TabIndex = 14;
            lblMaPhieuNhap.Text = "Mã phiếu nhập:";
            // 
            // lblMaSanPham
            // 
            lblMaSanPham.Font = new Font("Google Sans", 10.2F);
            lblMaSanPham.Location = new Point(20, 169);
            lblMaSanPham.Name = "lblMaSanPham";
            lblMaSanPham.Size = new Size(120, 25);
            lblMaSanPham.TabIndex = 15;
            lblMaSanPham.Text = "Mã sản phẩm:";
            // 
            // lblSoLuong
            // 
            lblSoLuong.Font = new Font("Google Sans", 10.2F);
            lblSoLuong.Location = new Point(20, 219);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(100, 25);
            lblSoLuong.TabIndex = 16;
            lblSoLuong.Text = "Số lượng:";
            // 
            // lblGiaNhap
            // 
            lblGiaNhap.Font = new Font("Google Sans", 10.2F);
            lblGiaNhap.Location = new Point(20, 269);
            lblGiaNhap.Name = "lblGiaNhap";
            lblGiaNhap.Size = new Size(100, 25);
            lblGiaNhap.TabIndex = 17;
            lblGiaNhap.Text = "Giá nhập:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(20, 319);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(100, 25);
            lblGhiChu.TabIndex = 18;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // FrmChiTietPhieuNhap
            // 
            Controls.Add(dataGridViewChiTietPhieuNhap);
            Controls.Add(txtMaChiTietPhieuNhap);
            Controls.Add(cbMaPhieuNhap);
            Controls.Add(cbMaSanPham);
            Controls.Add(nudSoLuong);
            Controls.Add(nudGiaNhap);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaChiTietPhieuNhap);
            Controls.Add(lblMaPhieuNhap);
            Controls.Add(lblMaSanPham);
            Controls.Add(lblSoLuong);
            Controls.Add(lblGiaNhap);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FrmChiTietPhieuNhap";
            Size = new Size(618, 839);
            ((System.ComponentModel.ISupportInitialize)dataGridViewChiTietPhieuNhap).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGiaNhap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewChiTietPhieuNhap;
        private TextBox txtMaChiTietPhieuNhap;
        private ComboBox cbMaPhieuNhap;
        private ComboBox cbMaSanPham;
        private NumericUpDown nudSoLuong;
        private NumericUpDown nudGiaNhap;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaChiTietPhieuNhap;
        private Label lblMaPhieuNhap;
        private Label lblMaSanPham;
        private Label lblSoLuong;
        private Label lblGiaNhap;
        private Label lblGhiChu;
    }
}