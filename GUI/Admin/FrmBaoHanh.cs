using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    public partial class FrmBaoHanh : UserControl
    {
        private readonly BaoHanhBLL _baoHanhBLL;
        private readonly HoaDonBanBLL _hoaDonBanBLL;
        private readonly SanPhamBLL _sanPhamBLL;

        public FrmBaoHanh()
        {
            InitializeComponent();
            _baoHanhBLL = new BaoHanhBLL();
            _hoaDonBanBLL = new HoaDonBanBLL();
            _sanPhamBLL = new SanPhamBLL();
            LoadBaoHanhList();
            LoadComboBoxes();
            SetupDataGridViewColumns();
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now.AddYears(1);

        }

        private async void LoadBaoHanhList()
        {
            try
            {
                var baoHanhs = await _baoHanhBLL.GetAllBaoHanhAsync();
                dataGridViewBaoHanh.DataSource = baoHanhs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách bảo hành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadComboBoxes()
        {
            try
            {
                var hoaDonBans = await _hoaDonBanBLL.GetAllHoaDonBanAsync();
                cbMaHoaDon.DataSource = hoaDonBans;
                cbMaHoaDon.DisplayMember = "MaHoaDon";
                cbMaHoaDon.ValueMember = "MaHoaDon";
                cbMaHoaDon.SelectedIndex = -1;

                cbFilterMaHoaDon.DataSource = hoaDonBans.ToList();
                cbFilterMaHoaDon.DisplayMember = "MaHoaDon";
                cbFilterMaHoaDon.ValueMember = "MaHoaDon";
                cbFilterMaHoaDon.SelectedIndex = -1;

                var sanPhams = await _sanPhamBLL.GetAllSanPhamAsync();
                cbMaSanPham.DataSource = sanPhams;
                cbMaSanPham.DisplayMember = "TenSanPham";
                cbMaSanPham.ValueMember = "MaSanPham";
                cbMaSanPham.SelectedIndex = -1;

                cbFilterMaSanPham.DataSource = sanPhams.ToList();
                cbFilterMaSanPham.DisplayMember = "TenSanPham";
                cbFilterMaSanPham.ValueMember = "MaSanPham";
                cbFilterMaSanPham.SelectedIndex = -1;

                cbTinhTrang.Items.AddRange(new[] { "Còn bảo hành", "Hết bảo hành", "Đang xử lý" });
                cbTinhTrang.SelectedIndex = -1;

                cbFilterTinhTrang.Items.AddRange(new[] { "Còn bảo hành", "Hết bảo hành", "Đang xử lý" });
                cbFilterTinhTrang.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ComboBox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột để ta có thể tự định nghĩa
            dataGridViewBaoHanh.AutoGenerateColumns = false;

            // Xóa các cột cũ nếu có để tránh trùng lặp
            dataGridViewBaoHanh.Columns.Clear();

            // Thêm và cấu hình từng cột
            dataGridViewBaoHanh.Columns.Add("MaBaoHanh", "Mã bảo hành");
            dataGridViewBaoHanh.Columns["MaBaoHanh"].DataPropertyName = "MaBaoHanh";
            dataGridViewBaoHanh.Columns["MaBaoHanh"].Width = 100;

            dataGridViewBaoHanh.Columns.Add("MaHoaDon", "Mã hóa đơn");
            dataGridViewBaoHanh.Columns["MaHoaDon"].DataPropertyName = "MaHoaDon";
            dataGridViewBaoHanh.Columns["MaHoaDon"].Width = 150;

            dataGridViewBaoHanh.Columns.Add("MaSanPham", "Mã sản phẩm");
            dataGridViewBaoHanh.Columns["MaSanPham"].DataPropertyName = "MaSanPham";
            dataGridViewBaoHanh.Columns["MaSanPham"].Width = 150;

            dataGridViewBaoHanh.Columns.Add("NgayBatDau", "Ngày bắt đầu");
            dataGridViewBaoHanh.Columns["NgayBatDau"].DataPropertyName = "NgayBatDau";
            dataGridViewBaoHanh.Columns["NgayBatDau"].Width = 150;

            dataGridViewBaoHanh.Columns.Add("NgayKetThuc", "Ngày kết thúc");
            dataGridViewBaoHanh.Columns["NgayKetThuc"].DataPropertyName = "NgayKetThuc";
            dataGridViewBaoHanh.Columns["NgayKetThuc"].Width = 150;

            dataGridViewBaoHanh.Columns.Add("TinhTrang", "Tình trạng");
            dataGridViewBaoHanh.Columns["TinhTrang"].DataPropertyName = "TinhTrang";
            dataGridViewBaoHanh.Columns["TinhTrang"].Width = 120;

            dataGridViewBaoHanh.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewBaoHanh.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewBaoHanh.Columns["GhiChu"].Width = 249;
        }

        private void dataGridViewBaoHanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewBaoHanh.Rows[e.RowIndex];
                txtMaBaoHanh.Text = row.Cells["MaBaoHanh"].Value?.ToString();
                cbMaHoaDon.SelectedValue = row.Cells["MaHoaDon"].Value?.ToString();
                cbMaSanPham.SelectedValue = row.Cells["MaSanPham"].Value?.ToString();
                dtpNgayBatDau.Value = row.Cells["NgayBatDau"].Value != null ? Convert.ToDateTime(row.Cells["NgayBatDau"].Value) : DateTime.Now;
                dtpNgayKetThuc.Value = row.Cells["NgayKetThuc"].Value != null ? Convert.ToDateTime(row.Cells["NgayKetThuc"].Value) : DateTime.Now;
                cbTinhTrang.SelectedItem = row.Cells["TinhTrang"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                string filterMaHoaDon = cbFilterMaHoaDon.SelectedValue?.ToString();
                string filterMaSanPham = cbFilterMaSanPham.SelectedValue?.ToString();
                string filterTinhTrang = cbFilterTinhTrang.SelectedItem?.ToString();
                DateTime? filterStartDate = dtpFilterStartDate.Value > DateTime.MinValue ? dtpFilterStartDate.Value : (DateTime?)null;
                DateTime? filterEndDate = dtpFilterEndDate.Value > DateTime.MinValue ? dtpFilterEndDate.Value : (DateTime?)null;

                var baoHanhs = await _baoHanhBLL.GetAllBaoHanhAsync();

                baoHanhs = baoHanhs.Where(bh =>
                    (string.IsNullOrEmpty(keyword) ||
                     bh.MaBaoHanh.ToLower().Contains(keyword) ||
                     bh.MaSanPham.ToLower().Contains(keyword) ||
                     bh.TinhTrang.ToLower().Contains(keyword)) &&
                    (string.IsNullOrEmpty(filterMaHoaDon) || bh.MaHoaDon == filterMaHoaDon) &&
                    (string.IsNullOrEmpty(filterMaSanPham) || bh.MaSanPham == filterMaSanPham) &&
                    (string.IsNullOrEmpty(filterTinhTrang) || bh.TinhTrang == filterTinhTrang) &&
                    (!filterStartDate.HasValue || bh.NgayBatDau >= filterStartDate.Value) &&
                    (!filterEndDate.HasValue || bh.NgayKetThuc <= filterEndDate.Value)
                ).ToList();

                dataGridViewBaoHanh.DataSource = baoHanhs;

                if (baoHanhs.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu bảo hành nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phiếu bảo hành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpNgayBatDau.Value >= dtpNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải sớm hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var baoHanh = new BaoHanh
                {
                    MaBaoHanh = txtMaBaoHanh.Text.Trim(),
                    MaHoaDon = cbMaHoaDon.SelectedValue?.ToString(),
                    MaSanPham = cbMaSanPham.SelectedValue?.ToString(),
                    NgayBatDau = dtpNgayBatDau.Value,
                    NgayKetThuc = dtpNgayKetThuc.Value,
                    TinhTrang = cbTinhTrang.SelectedItem?.ToString(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(baoHanh.MaHoaDon) || string.IsNullOrEmpty(baoHanh.MaSanPham) || string.IsNullOrEmpty(baoHanh.TinhTrang))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn, sản phẩm và tình trạng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _baoHanhBLL.AddBaoHanhAsync(baoHanh);
                if (result)
                {
                    MessageBox.Show("Thêm phiếu bảo hành thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBaoHanhList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiếu bảo hành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpNgayBatDau.Value >= dtpNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải sớm hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var baoHanh = new BaoHanh
                {
                    MaBaoHanh = txtMaBaoHanh.Text.Trim(),
                    MaHoaDon = cbMaHoaDon.SelectedValue?.ToString(),
                    MaSanPham = cbMaSanPham.SelectedValue?.ToString(),
                    NgayBatDau = dtpNgayBatDau.Value,
                    NgayKetThuc = dtpNgayKetThuc.Value,
                    TinhTrang = cbTinhTrang.SelectedItem?.ToString(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(baoHanh.MaHoaDon) || string.IsNullOrEmpty(baoHanh.MaSanPham) || string.IsNullOrEmpty(baoHanh.TinhTrang))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn, sản phẩm và tình trạng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _baoHanhBLL.UpdateBaoHanhAsync(baoHanh);
                if (result)
                {
                    MessageBox.Show("Cập nhật phiếu bảo hành thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBaoHanhList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật phiếu bảo hành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maBaoHanh = txtMaBaoHanh.Text.Trim();
                if (string.IsNullOrEmpty(maBaoHanh))
                {
                    MessageBox.Show("Vui lòng chọn phiếu bảo hành để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu bảo hành {maBaoHanh}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _baoHanhBLL.DeleteBaoHanhAsync(maBaoHanh);
                    if (success)
                    {
                        MessageBox.Show("Xóa phiếu bảo hành thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBaoHanhList();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa phiếu bảo hành: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            ClearFilterInputs();
            txtTimKiem.Clear();
            LoadBaoHanhList();
            dataGridViewBaoHanh.Refresh();
        }

        private void ClearInputs()
        {
            txtMaBaoHanh.Clear();
            cbMaHoaDon.SelectedIndex = -1;
            cbMaSanPham.SelectedIndex = -1;
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now.AddYears(1);
            cbTinhTrang.SelectedIndex = -1;
            txtGhiChu.Clear();
        }

        private void ClearFilterInputs()
        {
            cbFilterMaHoaDon.SelectedIndex = -1;
            cbFilterMaSanPham.SelectedIndex = -1;
            cbFilterTinhTrang.SelectedIndex = -1;
            dtpFilterStartDate.Value = DateTime.Now;
            dtpFilterEndDate.Value = DateTime.Now;
        }

        private void dataGridViewBaoHanh_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBaoHanh.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewBaoHanh.SelectedRows[0];
                txtMaBaoHanh.Text = selectedRow.Cells["MaBaoHanh"].Value?.ToString();
                cbMaHoaDon.SelectedValue = selectedRow.Cells["MaHoaDon"].Value?.ToString();
                cbMaSanPham.SelectedValue = selectedRow.Cells["MaSanPham"].Value?.ToString();
                dtpNgayBatDau.Value = selectedRow.Cells["NgayBatDau"].Value != null ? Convert.ToDateTime(selectedRow.Cells["NgayBatDau"].Value) : DateTime.Now;
                dtpNgayKetThuc.Value = selectedRow.Cells["NgayKetThuc"].Value != null ? Convert.ToDateTime(selectedRow.Cells["NgayKetThuc"].Value) : DateTime.Now;
                cbTinhTrang.SelectedItem = selectedRow.Cells["TinhTrang"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewBaoHanh = new DataGridView();
            txtMaBaoHanh = new TextBox();
            cbMaHoaDon = new ComboBox();
            cbMaSanPham = new ComboBox();
            dtpNgayBatDau = new DateTimePicker();
            dtpNgayKetThuc = new DateTimePicker();
            cbTinhTrang = new ComboBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaBaoHanh = new Label();
            lblMaHoaDon = new Label();
            lblMaSanPham = new Label();
            lblNgayBatDau = new Label();
            lblNgayKetThuc = new Label();
            lblTinhTrang = new Label();
            lblGhiChu = new Label();
            cbFilterMaHoaDon = new ComboBox();
            cbFilterMaSanPham = new ComboBox();
            cbFilterTinhTrang = new ComboBox();
            dtpFilterStartDate = new DateTimePicker();
            dtpFilterEndDate = new DateTimePicker();
            lblFilterMaHoaDon = new Label();
            lblFilterMaSanPham = new Label();
            lblFilterTinhTrang = new Label();
            lblFilterStartDate = new Label();
            lblFilterEndDate = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBaoHanh).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewBaoHanh
            // 
            dataGridViewBaoHanh.ColumnHeadersHeight = 29;
            dataGridViewBaoHanh.Location = new Point(3, 369);
            dataGridViewBaoHanh.MultiSelect = false;
            dataGridViewBaoHanh.Name = "dataGridViewBaoHanh";
            dataGridViewBaoHanh.RowHeadersWidth = 51;
            dataGridViewBaoHanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBaoHanh.Size = new Size(1282, 436);
            dataGridViewBaoHanh.TabIndex = 0;
            dataGridViewBaoHanh.CellClick += dataGridViewBaoHanh_CellClick;
            dataGridViewBaoHanh.SelectionChanged += dataGridViewBaoHanh_SelectionChanged;
            // 
            // txtMaBaoHanh
            // 
            txtMaBaoHanh.Font = new Font("Google Sans", 10.2F);
            txtMaBaoHanh.Location = new Point(208, 17);
            txtMaBaoHanh.Name = "txtMaBaoHanh";
            txtMaBaoHanh.Size = new Size(425, 29);
            txtMaBaoHanh.TabIndex = 1;
            // 
            // cbMaHoaDon
            // 
            cbMaHoaDon.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaHoaDon.Font = new Font("Google Sans", 10.2F);
            cbMaHoaDon.Location = new Point(208, 67);
            cbMaHoaDon.Name = "cbMaHoaDon";
            cbMaHoaDon.Size = new Size(425, 30);
            cbMaHoaDon.TabIndex = 2;
            // 
            // cbMaSanPham
            // 
            cbMaSanPham.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaSanPham.Font = new Font("Google Sans", 10.2F);
            cbMaSanPham.Location = new Point(208, 117);
            cbMaSanPham.Name = "cbMaSanPham";
            cbMaSanPham.Size = new Size(425, 30);
            cbMaSanPham.TabIndex = 3;
            // 
            // dtpNgayBatDau
            // 
            dtpNgayBatDau.Font = new Font("Google Sans", 10.2F);
            dtpNgayBatDau.Format = DateTimePickerFormat.Short;
            dtpNgayBatDau.Location = new Point(921, 15);
            dtpNgayBatDau.Name = "dtpNgayBatDau";
            dtpNgayBatDau.Size = new Size(335, 29);
            dtpNgayBatDau.TabIndex = 4;
            // 
            // dtpNgayKetThuc
            // 
            dtpNgayKetThuc.Font = new Font("Google Sans", 10.2F);
            dtpNgayKetThuc.Format = DateTimePickerFormat.Short;
            dtpNgayKetThuc.Location = new Point(921, 65);
            dtpNgayKetThuc.Name = "dtpNgayKetThuc";
            dtpNgayKetThuc.Size = new Size(335, 29);
            dtpNgayKetThuc.TabIndex = 5;
            // 
            // cbTinhTrang
            // 
            cbTinhTrang.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTinhTrang.Font = new Font("Google Sans", 10.2F);
            cbTinhTrang.Location = new Point(921, 115);
            cbTinhTrang.Name = "cbTinhTrang";
            cbTinhTrang.Size = new Size(335, 30);
            cbTinhTrang.TabIndex = 6;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(921, 165);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(335, 29);
            txtGhiChu.TabIndex = 7;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(958, 251);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(193, 26);
            txtTimKiem.TabIndex = 8;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 10.2F);
            btnThem.Location = new Point(51, 170);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(141, 41);
            btnThem.TabIndex = 9;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Google Sans", 10.2F);
            btnSua.Location = new Point(198, 170);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(141, 41);
            btnSua.TabIndex = 10;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Google Sans", 10.2F);
            btnXoa.Location = new Point(345, 170);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(141, 41);
            btnXoa.TabIndex = 11;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 10.2F);
            btnLamMoi.Location = new Point(492, 170);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(141, 41);
            btnLamMoi.TabIndex = 12;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTimKiem.Location = new Point(958, 297);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(193, 38);
            btnTimKiem.TabIndex = 13;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaBaoHanh
            // 
            lblMaBaoHanh.Font = new Font("Google Sans", 10.2F);
            lblMaBaoHanh.Location = new Point(29, 22);
            lblMaBaoHanh.Name = "lblMaBaoHanh";
            lblMaBaoHanh.Size = new Size(134, 25);
            lblMaBaoHanh.TabIndex = 14;
            lblMaBaoHanh.Text = "Mã bảo hành:";
            // 
            // lblMaHoaDon
            // 
            lblMaHoaDon.Font = new Font("Google Sans", 10.2F);
            lblMaHoaDon.Location = new Point(29, 72);
            lblMaHoaDon.Name = "lblMaHoaDon";
            lblMaHoaDon.Size = new Size(134, 25);
            lblMaHoaDon.TabIndex = 15;
            lblMaHoaDon.Text = "Mã hóa đơn:";
            // 
            // lblMaSanPham
            // 
            lblMaSanPham.Font = new Font("Google Sans", 10.2F);
            lblMaSanPham.Location = new Point(29, 122);
            lblMaSanPham.Name = "lblMaSanPham";
            lblMaSanPham.Size = new Size(134, 25);
            lblMaSanPham.TabIndex = 16;
            lblMaSanPham.Text = "Mã sản phẩm:";
            // 
            // lblNgayBatDau
            // 
            lblNgayBatDau.Font = new Font("Google Sans", 10.2F);
            lblNgayBatDau.Location = new Point(742, 20);
            lblNgayBatDau.Name = "lblNgayBatDau";
            lblNgayBatDau.Size = new Size(134, 25);
            lblNgayBatDau.TabIndex = 17;
            lblNgayBatDau.Text = "Ngày bắt đầu:";
            // 
            // lblNgayKetThuc
            // 
            lblNgayKetThuc.Font = new Font("Google Sans", 10.2F);
            lblNgayKetThuc.Location = new Point(742, 70);
            lblNgayKetThuc.Name = "lblNgayKetThuc";
            lblNgayKetThuc.Size = new Size(134, 25);
            lblNgayKetThuc.TabIndex = 18;
            lblNgayKetThuc.Text = "Ngày kết thúc:";
            // 
            // lblTinhTrang
            // 
            lblTinhTrang.Font = new Font("Google Sans", 10.2F);
            lblTinhTrang.Location = new Point(742, 120);
            lblTinhTrang.Name = "lblTinhTrang";
            lblTinhTrang.Size = new Size(134, 25);
            lblTinhTrang.TabIndex = 19;
            lblTinhTrang.Text = "Tình trạng:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(742, 170);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(134, 25);
            lblGhiChu.TabIndex = 20;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // cbFilterMaHoaDon
            // 
            cbFilterMaHoaDon.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterMaHoaDon.Font = new Font("Google Sans", 10.2F);
            cbFilterMaHoaDon.Location = new Point(324, 253);
            cbFilterMaHoaDon.Name = "cbFilterMaHoaDon";
            cbFilterMaHoaDon.Size = new Size(179, 30);
            cbFilterMaHoaDon.TabIndex = 22;
            // 
            // cbFilterMaSanPham
            // 
            cbFilterMaSanPham.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterMaSanPham.Font = new Font("Google Sans", 10.2F);
            cbFilterMaSanPham.Location = new Point(324, 303);
            cbFilterMaSanPham.Name = "cbFilterMaSanPham";
            cbFilterMaSanPham.Size = new Size(179, 30);
            cbFilterMaSanPham.TabIndex = 23;
            // 
            // cbFilterTinhTrang
            // 
            cbFilterTinhTrang.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterTinhTrang.Font = new Font("Google Sans", 10.2F);
            cbFilterTinhTrang.Location = new Point(721, 302);
            cbFilterTinhTrang.Name = "cbFilterTinhTrang";
            cbFilterTinhTrang.Size = new Size(179, 30);
            cbFilterTinhTrang.TabIndex = 24;
            // 
            // dtpFilterStartDate
            // 
            dtpFilterStartDate.Font = new Font("Google Sans", 10.2F);
            dtpFilterStartDate.Format = DateTimePickerFormat.Short;
            dtpFilterStartDate.Location = new Point(721, 251);
            dtpFilterStartDate.Name = "dtpFilterStartDate";
            dtpFilterStartDate.Size = new Size(179, 29);
            dtpFilterStartDate.TabIndex = 25;
            // 
            // dtpFilterEndDate
            // 
            dtpFilterEndDate.Font = new Font("Google Sans", 10.2F);
            dtpFilterEndDate.Format = DateTimePickerFormat.Short;
            dtpFilterEndDate.Location = new Point(921, 15);
            dtpFilterEndDate.Name = "dtpFilterEndDate";
            dtpFilterEndDate.Size = new Size(335, 29);
            dtpFilterEndDate.TabIndex = 26;
            // 
            // lblFilterMaHoaDon
            // 
            lblFilterMaHoaDon.Font = new Font("Google Sans", 10.2F);
            lblFilterMaHoaDon.Location = new Point(145, 258);
            lblFilterMaHoaDon.Name = "lblFilterMaHoaDon";
            lblFilterMaHoaDon.Size = new Size(163, 25);
            lblFilterMaHoaDon.TabIndex = 27;
            lblFilterMaHoaDon.Text = "Lọc mã hóa đơn:";
            // 
            // lblFilterMaSanPham
            // 
            lblFilterMaSanPham.Font = new Font("Google Sans", 10.2F);
            lblFilterMaSanPham.Location = new Point(145, 308);
            lblFilterMaSanPham.Name = "lblFilterMaSanPham";
            lblFilterMaSanPham.Size = new Size(163, 25);
            lblFilterMaSanPham.TabIndex = 28;
            lblFilterMaSanPham.Text = "Lọc mã sản phẩm:";
            // 
            // lblFilterTinhTrang
            // 
            lblFilterTinhTrang.Font = new Font("Google Sans", 10.2F);
            lblFilterTinhTrang.Location = new Point(542, 305);
            lblFilterTinhTrang.Name = "lblFilterTinhTrang";
            lblFilterTinhTrang.Size = new Size(144, 25);
            lblFilterTinhTrang.TabIndex = 29;
            lblFilterTinhTrang.Text = "Lọc tình trạng:";
            // 
            // lblFilterStartDate
            // 
            lblFilterStartDate.Font = new Font("Google Sans", 10.2F);
            lblFilterStartDate.Location = new Point(542, 256);
            lblFilterStartDate.Name = "lblFilterStartDate";
            lblFilterStartDate.Size = new Size(155, 25);
            lblFilterStartDate.TabIndex = 30;
            lblFilterStartDate.Text = "Lọc ngày bắt đầu:";
            // 
            // lblFilterEndDate
            // 
            lblFilterEndDate.Font = new Font("Google Sans", 10.2F);
            lblFilterEndDate.Location = new Point(742, 20);
            lblFilterEndDate.Name = "lblFilterEndDate";
            lblFilterEndDate.Size = new Size(134, 25);
            lblFilterEndDate.TabIndex = 31;
            lblFilterEndDate.Text = "Lọc ngày kết thúc:";
            // 
            // FrmBaoHanh
            // 
            Controls.Add(dataGridViewBaoHanh);
            Controls.Add(txtMaBaoHanh);
            Controls.Add(cbMaHoaDon);
            Controls.Add(cbMaSanPham);
            Controls.Add(dtpNgayBatDau);
            Controls.Add(dtpNgayKetThuc);
            Controls.Add(cbTinhTrang);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaBaoHanh);
            Controls.Add(lblMaHoaDon);
            Controls.Add(lblMaSanPham);
            Controls.Add(lblNgayBatDau);
            Controls.Add(lblNgayKetThuc);
            Controls.Add(lblTinhTrang);
            Controls.Add(lblGhiChu);
            Controls.Add(cbFilterMaHoaDon);
            Controls.Add(cbFilterMaSanPham);
            Controls.Add(cbFilterTinhTrang);
            Controls.Add(dtpFilterStartDate);
            Controls.Add(dtpFilterEndDate);
            Controls.Add(lblFilterMaHoaDon);
            Controls.Add(lblFilterMaSanPham);
            Controls.Add(lblFilterTinhTrang);
            Controls.Add(lblFilterStartDate);
            Controls.Add(lblFilterEndDate);
            Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FrmBaoHanh";
            Size = new Size(1288, 836);
            ((System.ComponentModel.ISupportInitialize)dataGridViewBaoHanh).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewBaoHanh;
        private TextBox txtMaBaoHanh;
        private ComboBox cbMaHoaDon;
        private ComboBox cbMaSanPham;
        private DateTimePicker dtpNgayBatDau;
        private DateTimePicker dtpNgayKetThuc;
        private ComboBox cbTinhTrang;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaBaoHanh;
        private Label lblMaHoaDon;
        private Label lblMaSanPham;
        private Label lblNgayBatDau;
        private Label lblNgayKetThuc;
        private Label lblTinhTrang;
        private Label lblGhiChu;
        private ComboBox cbFilterMaHoaDon;
        private ComboBox cbFilterMaSanPham;
        private ComboBox cbFilterTinhTrang;
        private DateTimePicker dtpFilterStartDate;
        private DateTimePicker dtpFilterEndDate;
        private Label lblFilterMaHoaDon;
        private Label lblFilterMaSanPham;
        private Label lblFilterTinhTrang;
        private Label lblFilterStartDate;
        private Label lblFilterEndDate;
    }
}