using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    public partial class FrmChiTietHoaDonBan : UserControl
    {
        private readonly ChiTietHoaDonBanBLL _chiTietHoaDonBanBLL;
        private readonly HoaDonBanBLL _hoaDonBanBLL;
        private readonly SanPhamBLL _sanPhamBLL;

        public FrmChiTietHoaDonBan()
        {
            InitializeComponent();
            _chiTietHoaDonBanBLL = new ChiTietHoaDonBanBLL();
            _hoaDonBanBLL = new HoaDonBanBLL();
            _sanPhamBLL = new SanPhamBLL();
            LoadChiTietHoaDonBanList();
            LoadComboBoxes();
            SetupDataGridViewColumns();
        }

        private async void LoadChiTietHoaDonBanList()
        {
            try
            {
                var chiTietHoaDonBans = await _chiTietHoaDonBanBLL.GetAllChiTietHoaDonBanAsync();
                dataGridViewChiTietHoaDonBan.DataSource = chiTietHoaDonBans;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi tiết hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadComboBoxes()
        {
            try
            {
                // Tải danh sách hóa đơn bán cho ComboBox
                var hoaDonBans = await _hoaDonBanBLL.GetAllHoaDonBanAsync();
                cbMaHoaDon.DataSource = hoaDonBans;
                cbMaHoaDon.DisplayMember = "MaHoaDon";
                cbMaHoaDon.ValueMember = "MaHoaDon";
                cbMaHoaDon.SelectedIndex = -1; // Không chọn mặc định

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
            dataGridViewChiTietHoaDonBan.AutoGenerateColumns = false;

            // Xóa các cột cũ nếu có để tránh trùng lặp
            dataGridViewChiTietHoaDonBan.Columns.Clear();

            // Thêm và cấu hình từng cột
            dataGridViewChiTietHoaDonBan.Columns.Add("MaChiTietHoaDon", "Mã chi tiết");
            dataGridViewChiTietHoaDonBan.Columns["MaChiTietHoaDon"].DataPropertyName = "MaChiTietHoaDon";
            dataGridViewChiTietHoaDonBan.Columns["MaChiTietHoaDon"].Width = 150;

            dataGridViewChiTietHoaDonBan.Columns.Add("MaHoaDon", "Mã hóa đơn");
            dataGridViewChiTietHoaDonBan.Columns["MaHoaDon"].DataPropertyName = "MaHoaDon";
            dataGridViewChiTietHoaDonBan.Columns["MaHoaDon"].Width = 150;

            dataGridViewChiTietHoaDonBan.Columns.Add("MaSanPham", "Mã sản phẩm");
            dataGridViewChiTietHoaDonBan.Columns["MaSanPham"].DataPropertyName = "MaSanPham";
            dataGridViewChiTietHoaDonBan.Columns["MaSanPham"].Width = 150;

            dataGridViewChiTietHoaDonBan.Columns.Add("SoLuong", "Số lượng");
            dataGridViewChiTietHoaDonBan.Columns["SoLuong"].DataPropertyName = "SoLuong";
            dataGridViewChiTietHoaDonBan.Columns["SoLuong"].Width = 100;

            dataGridViewChiTietHoaDonBan.Columns.Add("DonGia", "Đơn giá");
            dataGridViewChiTietHoaDonBan.Columns["DonGia"].DataPropertyName = "DonGia";
            dataGridViewChiTietHoaDonBan.Columns["DonGia"].Width = 150;

            dataGridViewChiTietHoaDonBan.Columns.Add("GiamGia", "Giảm giá");
            dataGridViewChiTietHoaDonBan.Columns["GiamGia"].DataPropertyName = "GiamGia";
            dataGridViewChiTietHoaDonBan.Columns["GiamGia"].Width = 100;

            dataGridViewChiTietHoaDonBan.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewChiTietHoaDonBan.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewChiTietHoaDonBan.Columns["GhiChu"].Width = 268;
        }

        private void dataGridViewChiTietHoaDonBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewChiTietHoaDonBan.Rows[e.RowIndex];
                txtMaChiTietHoaDon.Text = row.Cells["MaChiTietHoaDon"].Value?.ToString();
                cbMaHoaDon.SelectedValue = row.Cells["MaHoaDon"].Value?.ToString();
                cbMaSanPham.SelectedValue = row.Cells["MaSanPham"].Value?.ToString();
                if (int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong))
                    nudSoLuong.Value = soLuong;
                if (decimal.TryParse(row.Cells["DonGia"].Value?.ToString(), out decimal donGia))
                    nudDonGia.Value = donGia;
                if (decimal.TryParse(row.Cells["GiamGia"].Value?.ToString(), out decimal giamGia))
                    nudGiamGia.Value = giamGia;
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var chiTietHoaDonBans = await _chiTietHoaDonBanBLL.GetAllChiTietHoaDonBanAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    chiTietHoaDonBans = chiTietHoaDonBans.Where(ct =>
                        ct.MaChiTietHoaDon.ToLower().Contains(keyword) ||
                        ct.MaSanPham.ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewChiTietHoaDonBan.DataSource = chiTietHoaDonBans;

                if (chiTietHoaDonBans.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy chi tiết hóa đơn bán nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm chi tiết hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (nudDonGia.Value < 0)
                {
                    MessageBox.Show("Đơn giá không được âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var chiTietHoaDonBan = new ChiTietHoaDonBan
                {
                    MaChiTietHoaDon = txtMaChiTietHoaDon.Text.Trim(),
                    MaHoaDon = cbMaHoaDon.SelectedValue?.ToString(),
                    MaSanPham = cbMaSanPham.SelectedValue?.ToString(),
                    SoLuong = (int)nudSoLuong.Value,
                    DonGia = (decimal)nudDonGia.Value,
                    GiamGia = (decimal)nudGiamGia.Value,
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(chiTietHoaDonBan.MaHoaDon) || string.IsNullOrEmpty(chiTietHoaDonBan.MaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn bán và sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _chiTietHoaDonBanBLL.AddChiTietHoaDonBanAsync(chiTietHoaDonBan);
                if (result)
                {
                    MessageBox.Show("Thêm chi tiết hóa đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiTietHoaDonBanList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chi tiết hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (nudDonGia.Value < 0)
                {
                    MessageBox.Show("Đơn giá không được âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var chiTietHoaDonBan = new ChiTietHoaDonBan
                {
                    MaChiTietHoaDon = txtMaChiTietHoaDon.Text.Trim(),
                    MaHoaDon = cbMaHoaDon.SelectedValue?.ToString(),
                    MaSanPham = cbMaSanPham.SelectedValue?.ToString(),
                    SoLuong = (int)nudSoLuong.Value,
                    DonGia = (decimal)nudDonGia.Value,
                    GiamGia = (decimal)nudGiamGia.Value,
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(chiTietHoaDonBan.MaHoaDon) || string.IsNullOrEmpty(chiTietHoaDonBan.MaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn bán và sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _chiTietHoaDonBanBLL.UpdateChiTietHoaDonBanAsync(chiTietHoaDonBan);
                if (result)
                {
                    MessageBox.Show("Cập nhật chi tiết hóa đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiTietHoaDonBanList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chi tiết hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maChiTietHoaDon = txtMaChiTietHoaDon.Text.Trim();
                if (string.IsNullOrEmpty(maChiTietHoaDon))
                {
                    MessageBox.Show("Vui lòng chọn chi tiết hóa đơn bán để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa chi tiết hóa đơn bán {maChiTietHoaDon}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _chiTietHoaDonBanBLL.DeleteChiTietHoaDonBanAsync(maChiTietHoaDon);
                    if (success)
                    {
                        MessageBox.Show("Xóa chi tiết hóa đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadChiTietHoaDonBanList();
                        ClearInputs();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chi tiết hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadChiTietHoaDonBanList();
        }

        private void ClearInputs()
        {
            txtMaChiTietHoaDon.Clear();
            cbMaHoaDon.SelectedIndex = -1;
            cbMaSanPham.SelectedIndex = -1;
            nudSoLuong.Value = 1;
            nudDonGia.Value = 0;
            nudGiamGia.Value = 0;
            txtGhiChu.Clear();
        }

        private void dataGridViewChiTietHoaDonBan_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewChiTietHoaDonBan.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewChiTietHoaDonBan.SelectedRows[0];
                txtMaChiTietHoaDon.Text = selectedRow.Cells["MaChiTietHoaDon"].Value?.ToString();
                cbMaHoaDon.SelectedValue = selectedRow.Cells["MaHoaDon"].Value?.ToString();
                cbMaSanPham.SelectedValue = selectedRow.Cells["MaSanPham"].Value?.ToString();
                if (int.TryParse(selectedRow.Cells["SoLuong"].Value?.ToString(), out int soLuong))
                    nudSoLuong.Value = soLuong;
                if (decimal.TryParse(selectedRow.Cells["DonGia"].Value?.ToString(), out decimal donGia))
                    nudDonGia.Value = donGia;
                if (decimal.TryParse(selectedRow.Cells["GiamGia"].Value?.ToString(), out decimal giamGia))
                    nudGiamGia.Value = giamGia;
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewChiTietHoaDonBan = new DataGridView();
            txtMaChiTietHoaDon = new TextBox();
            cbMaHoaDon = new ComboBox();
            cbMaSanPham = new ComboBox();
            nudSoLuong = new NumericUpDown();
            nudDonGia = new NumericUpDown();
            nudGiamGia = new NumericUpDown();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaChiTietHoaDon = new Label();
            lblMaHoaDon = new Label();
            lblMaSanPham = new Label();
            lblSoLuong = new Label();
            lblDonGia = new Label();
            lblGiamGia = new Label();
            lblGhiChu = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChiTietHoaDonBan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDonGia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGiamGia).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewChiTietHoaDonBan
            // 
            dataGridViewChiTietHoaDonBan.ColumnHeadersHeight = 29;
            dataGridViewChiTietHoaDonBan.Location = new Point(3, 296);
            dataGridViewChiTietHoaDonBan.MultiSelect = false;
            dataGridViewChiTietHoaDonBan.Name = "dataGridViewChiTietHoaDonBan";
            dataGridViewChiTietHoaDonBan.RowHeadersWidth = 51;
            dataGridViewChiTietHoaDonBan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewChiTietHoaDonBan.Size = new Size(1282, 516);
            dataGridViewChiTietHoaDonBan.TabIndex = 0;
            dataGridViewChiTietHoaDonBan.CellClick += dataGridViewChiTietHoaDonBan_CellClick;
            dataGridViewChiTietHoaDonBan.SelectionChanged += dataGridViewChiTietHoaDonBan_SelectionChanged;
            // 
            // txtMaChiTietHoaDon
            // 
            txtMaChiTietHoaDon.Font = new Font("Google Sans", 10.2F);
            txtMaChiTietHoaDon.Location = new Point(195, 82);
            txtMaChiTietHoaDon.Name = "txtMaChiTietHoaDon";
            txtMaChiTietHoaDon.Size = new Size(344, 29);
            txtMaChiTietHoaDon.TabIndex = 1;
            // 
            // cbMaHoaDon
            // 
            cbMaHoaDon.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaHoaDon.Font = new Font("Google Sans", 10.2F);
            cbMaHoaDon.Location = new Point(195, 132);
            cbMaHoaDon.Name = "cbMaHoaDon";
            cbMaHoaDon.Size = new Size(344, 30);
            cbMaHoaDon.TabIndex = 2;
            // 
            // cbMaSanPham
            // 
            cbMaSanPham.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaSanPham.Font = new Font("Google Sans", 10.2F);
            cbMaSanPham.Location = new Point(195, 182);
            cbMaSanPham.Name = "cbMaSanPham";
            cbMaSanPham.Size = new Size(344, 30);
            cbMaSanPham.TabIndex = 3;
            // 
            // nudSoLuong
            // 
            nudSoLuong.Font = new Font("Google Sans", 10.2F);
            nudSoLuong.Location = new Point(892, 83);
            nudSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(344, 29);
            nudSoLuong.TabIndex = 4;
            nudSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudDonGia
            // 
            nudDonGia.DecimalPlaces = 2;
            nudDonGia.Font = new Font("Google Sans", 10.2F);
            nudDonGia.Location = new Point(892, 133);
            nudDonGia.Name = "nudDonGia";
            nudDonGia.Size = new Size(344, 29);
            nudDonGia.TabIndex = 5;
            // 
            // nudGiamGia
            // 
            nudGiamGia.DecimalPlaces = 2;
            nudGiamGia.Font = new Font("Google Sans", 10.2F);
            nudGiamGia.Location = new Point(892, 183);
            nudGiamGia.Name = "nudGiamGia";
            nudGiamGia.Size = new Size(344, 29);
            nudGiamGia.TabIndex = 6;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(892, 233);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(344, 29);
            txtGhiChu.TabIndex = 7;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(152, 7);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(387, 29);
            txtTimKiem.TabIndex = 8;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 10.2F);
            btnThem.Location = new Point(133, 233);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(97, 45);
            btnThem.TabIndex = 9;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Google Sans", 10.2F);
            btnSua.Location = new Point(236, 233);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(97, 45);
            btnSua.TabIndex = 10;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Google Sans", 10.2F);
            btnXoa.Location = new Point(339, 233);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(97, 45);
            btnXoa.TabIndex = 11;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 10.2F);
            btnLamMoi.Location = new Point(442, 233);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(97, 45);
            btnLamMoi.TabIndex = 12;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 10.2F);
            btnTimKiem.Location = new Point(3, 3);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(118, 37);
            btnTimKiem.TabIndex = 13;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaChiTietHoaDon
            // 
            lblMaChiTietHoaDon.Font = new Font("Google Sans", 10.2F);
            lblMaChiTietHoaDon.Location = new Point(22, 82);
            lblMaChiTietHoaDon.Name = "lblMaChiTietHoaDon";
            lblMaChiTietHoaDon.Size = new Size(140, 25);
            lblMaChiTietHoaDon.TabIndex = 14;
            lblMaChiTietHoaDon.Text = "Mã chi tiết:";
            // 
            // lblMaHoaDon
            // 
            lblMaHoaDon.Font = new Font("Google Sans", 10.2F);
            lblMaHoaDon.Location = new Point(22, 132);
            lblMaHoaDon.Name = "lblMaHoaDon";
            lblMaHoaDon.Size = new Size(140, 25);
            lblMaHoaDon.TabIndex = 15;
            lblMaHoaDon.Text = "Mã hóa đơn:";
            // 
            // lblMaSanPham
            // 
            lblMaSanPham.Font = new Font("Google Sans", 10.2F);
            lblMaSanPham.Location = new Point(22, 182);
            lblMaSanPham.Name = "lblMaSanPham";
            lblMaSanPham.Size = new Size(140, 25);
            lblMaSanPham.TabIndex = 16;
            lblMaSanPham.Text = "Mã sản phẩm:";
            // 
            // lblSoLuong
            // 
            lblSoLuong.Font = new Font("Google Sans", 10.2F);
            lblSoLuong.Location = new Point(719, 83);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(140, 25);
            lblSoLuong.TabIndex = 17;
            lblSoLuong.Text = "Số lượng:";
            // 
            // lblDonGia
            // 
            lblDonGia.Font = new Font("Google Sans", 10.2F);
            lblDonGia.Location = new Point(719, 133);
            lblDonGia.Name = "lblDonGia";
            lblDonGia.Size = new Size(140, 25);
            lblDonGia.TabIndex = 18;
            lblDonGia.Text = "Đơn giá:";
            // 
            // lblGiamGia
            // 
            lblGiamGia.Font = new Font("Google Sans", 10.2F);
            lblGiamGia.Location = new Point(719, 183);
            lblGiamGia.Name = "lblGiamGia";
            lblGiamGia.Size = new Size(140, 25);
            lblGiamGia.TabIndex = 19;
            lblGiamGia.Text = "Giảm giá:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(719, 233);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(140, 25);
            lblGhiChu.TabIndex = 20;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // FrmChiTietHoaDonBan
            // 
            Controls.Add(dataGridViewChiTietHoaDonBan);
            Controls.Add(txtMaChiTietHoaDon);
            Controls.Add(cbMaHoaDon);
            Controls.Add(cbMaSanPham);
            Controls.Add(nudSoLuong);
            Controls.Add(nudDonGia);
            Controls.Add(nudGiamGia);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaChiTietHoaDon);
            Controls.Add(lblMaHoaDon);
            Controls.Add(lblMaSanPham);
            Controls.Add(lblSoLuong);
            Controls.Add(lblDonGia);
            Controls.Add(lblGiamGia);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 10.2F);
            Name = "FrmChiTietHoaDonBan";
            Size = new Size(1288, 836);
            ((System.ComponentModel.ISupportInitialize)dataGridViewChiTietHoaDonBan).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDonGia).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGiamGia).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewChiTietHoaDonBan;
        private TextBox txtMaChiTietHoaDon;
        private ComboBox cbMaHoaDon;
        private ComboBox cbMaSanPham;
        private NumericUpDown nudSoLuong;
        private NumericUpDown nudDonGia;
        private NumericUpDown nudGiamGia;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaChiTietHoaDon;
        private Label lblMaHoaDon;
        private Label lblMaSanPham;
        private Label lblSoLuong;
        private Label lblDonGia;
        private Label lblGiamGia;
        private Label lblGhiChu;
    }
}