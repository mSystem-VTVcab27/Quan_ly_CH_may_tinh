using BLL;
using DAL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmHoaDonBan : UserControl
    {
        private readonly HoaDonBanBLL _hoaDonBanBLL;

        public FrmHoaDonBan()
        {
            InitializeComponent();
            _hoaDonBanBLL = new HoaDonBanBLL();
            LoadHoaDonBanList();
            SetupDataGridViewColumns();
            dtpNgayBan.Value = DateTime.Now; // Mặc định ngày hiện tại
            dataGridViewHoaDonBan.AutoGenerateColumns = false;
        }

        private async void LoadHoaDonBanList()
        {
            try
            {
                var hoaDonBans = await _hoaDonBanBLL.GetAllHoaDonBanAsync();
                dataGridViewHoaDonBan.DataSource = hoaDonBans;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            dataGridViewHoaDonBan.AutoGenerateColumns = false;
            dataGridViewHoaDonBan.Columns.Clear();
            dataGridViewHoaDonBan.Columns.Add("MaHoaDon", "Mã hóa đơn");
            dataGridViewHoaDonBan.Columns["MaHoaDon"].DataPropertyName = "MaHoaDon";
            dataGridViewHoaDonBan.Columns["MaHoaDon"].Width = 90;

            dataGridViewHoaDonBan.Columns.Add("NgayBan", "Ngày bán");
            dataGridViewHoaDonBan.Columns["NgayBan"].DataPropertyName = "NgayBan";
            dataGridViewHoaDonBan.Columns["NgayBan"].Width = 100;

            dataGridViewHoaDonBan.Columns.Add("MaKhachHang", "Mã khách hàng");
            dataGridViewHoaDonBan.Columns["MaKhachHang"].DataPropertyName = "MaKhachHang";
            dataGridViewHoaDonBan.Columns["MaKhachHang"].Width = 100;

            dataGridViewHoaDonBan.Columns.Add("MaNhanVien", "Mã nhân viên");
            dataGridViewHoaDonBan.Columns["MaNhanVien"].DataPropertyName = "MaNhanVien";
            dataGridViewHoaDonBan.Columns["MaNhanVien"].Width = 100;

            dataGridViewHoaDonBan.Columns.Add("TongTien", "Tổng tiền");
            dataGridViewHoaDonBan.Columns["TongTien"].DataPropertyName = "TongTien";
            dataGridViewHoaDonBan.Columns["TongTien"].Width = 100;

            dataGridViewHoaDonBan.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewHoaDonBan.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewHoaDonBan.Columns["GhiChu"].Width = 100;
        }

        private void dataGridViewHoaDonBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewHoaDonBan.Rows[e.RowIndex];
                txtMaHoaDon.Text = row.Cells["MaHoaDon"].Value?.ToString();
                dtpNgayBan.Value = row.Cells["NgayBan"].Value != null ? Convert.ToDateTime(row.Cells["NgayBan"].Value) : DateTime.Now;
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value?.ToString();
                txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value?.ToString();
                txtTongTien.Text = row.Cells["TongTien"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var hoaDonBans = await _hoaDonBanBLL.GetAllHoaDonBanAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    hoaDonBans = hoaDonBans.Where(h =>
                        h.MaHoaDon.ToLower().Contains(keyword) ||
                        h.NgayBan.ToString("dd/MM/yyyy").ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewHoaDonBan.DataSource = hoaDonBans;

                if (hoaDonBans.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn bán nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtTongTien.Text.Trim(), out decimal tongTien) || tongTien < 0)
                {
                    MessageBox.Show("Tổng tiền phải là số dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var hoaDonBan = new HoaDonBan
                {
                    MaHoaDon = txtMaHoaDon.Text.Trim(),
                    NgayBan = dtpNgayBan.Value,
                    MaKhachHang = string.IsNullOrEmpty(txtMaKhachHang.Text.Trim()) ? null : txtMaKhachHang.Text.Trim(),
                    MaNhanVien = txtMaNhanVien.Text.Trim(),
                    TongTien = tongTien,
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _hoaDonBanBLL.AddHoaDonBanAsync(hoaDonBan);
                if (result)
                {
                    MessageBox.Show("Thêm hóa đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHoaDonBanList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtTongTien.Text.Trim(), out decimal tongTien) || tongTien < 0)
                {
                    MessageBox.Show("Tổng tiền phải là số dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var hoaDonBan = new HoaDonBan
                {
                    MaHoaDon = txtMaHoaDon.Text.Trim(),
                    NgayBan = dtpNgayBan.Value,
                    MaKhachHang = string.IsNullOrEmpty(txtMaKhachHang.Text.Trim()) ? null : txtMaKhachHang.Text.Trim(),
                    MaNhanVien = txtMaNhanVien.Text.Trim(),
                    TongTien = tongTien,
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _hoaDonBanBLL.UpdateHoaDonBanAsync(hoaDonBan);
                if (result)
                {
                    MessageBox.Show("Cập nhật hóa đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHoaDonBanList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maHoaDon = txtMaHoaDon.Text.Trim();
                if (string.IsNullOrEmpty(maHoaDon))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn bán để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn bán {maHoaDon}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _hoaDonBanBLL.DeleteHoaDonBanAsync(maHoaDon);
                    if (success)
                    {
                        MessageBox.Show("Xóa hóa đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadHoaDonBanList();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn bán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadHoaDonBanList();
        }

        private void ClearInputs()
        {
            txtMaHoaDon.Clear();
            dtpNgayBan.Value = DateTime.Now;
            txtMaKhachHang.Clear();
            txtMaNhanVien.Clear();
            txtTongTien.Clear();
            txtGhiChu.Clear();
        }

        private void dataGridViewHoaDonBan_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewHoaDonBan.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewHoaDonBan.SelectedRows[0];
                txtMaHoaDon.Text = selectedRow.Cells["MaHoaDon"].Value?.ToString();
                dtpNgayBan.Value = selectedRow.Cells["NgayBan"].Value != null ? Convert.ToDateTime(selectedRow.Cells["NgayBan"].Value) : DateTime.Now;
                txtMaKhachHang.Text = selectedRow.Cells["MaKhachHang"].Value?.ToString();
                txtMaNhanVien.Text = selectedRow.Cells["MaNhanVien"].Value?.ToString();
                txtTongTien.Text = selectedRow.Cells["TongTien"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewHoaDonBan = new DataGridView();
            txtMaHoaDon = new TextBox();
            dtpNgayBan = new DateTimePicker();
            txtMaKhachHang = new TextBox();
            txtMaNhanVien = new TextBox();
            txtTongTien = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaHoaDon = new Label();
            lblNgayBan = new Label();
            lblMaKhachHang = new Label();
            lblMaNhanVien = new Label();
            lblTongTien = new Label();
            lblGhiChu = new Label();
            btnXuatExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewHoaDonBan).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewHoaDonBan
            // 
            dataGridViewHoaDonBan.ColumnHeadersHeight = 29;
            dataGridViewHoaDonBan.Location = new Point(3, 430);
            dataGridViewHoaDonBan.MultiSelect = false;
            dataGridViewHoaDonBan.Name = "dataGridViewHoaDonBan";
            dataGridViewHoaDonBan.RowHeadersWidth = 51;
            dataGridViewHoaDonBan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHoaDonBan.Size = new Size(607, 361);
            dataGridViewHoaDonBan.TabIndex = 0;
            dataGridViewHoaDonBan.CellClick += dataGridViewHoaDonBan_CellClick;
            dataGridViewHoaDonBan.SelectionChanged += dataGridViewHoaDonBan_SelectionChanged;
            // 
            // txtMaHoaDon
            // 
            txtMaHoaDon.Font = new Font("Google Sans", 10.2F);
            txtMaHoaDon.Location = new Point(248, 60);
            txtMaHoaDon.Name = "txtMaHoaDon";
            txtMaHoaDon.Size = new Size(362, 29);
            txtMaHoaDon.TabIndex = 1;
            // 
            // dtpNgayBan
            // 
            dtpNgayBan.Font = new Font("Google Sans", 10.2F);
            dtpNgayBan.Format = DateTimePickerFormat.Short;
            dtpNgayBan.Location = new Point(248, 110);
            dtpNgayBan.Name = "dtpNgayBan";
            dtpNgayBan.Size = new Size(362, 29);
            dtpNgayBan.TabIndex = 2;
            // 
            // txtMaKhachHang
            // 
            txtMaKhachHang.Font = new Font("Google Sans", 10.2F);
            txtMaKhachHang.Location = new Point(248, 160);
            txtMaKhachHang.Name = "txtMaKhachHang";
            txtMaKhachHang.Size = new Size(362, 29);
            txtMaKhachHang.TabIndex = 3;
            // 
            // txtMaNhanVien
            // 
            txtMaNhanVien.Font = new Font("Google Sans", 10.2F);
            txtMaNhanVien.Location = new Point(248, 210);
            txtMaNhanVien.Name = "txtMaNhanVien";
            txtMaNhanVien.Size = new Size(362, 29);
            txtMaNhanVien.TabIndex = 4;
            // 
            // txtTongTien
            // 
            txtTongTien.Font = new Font("Google Sans", 10.2F);
            txtTongTien.Location = new Point(248, 260);
            txtTongTien.Name = "txtTongTien";
            txtTongTien.Size = new Size(362, 29);
            txtTongTien.TabIndex = 5;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(248, 310);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(362, 29);
            txtGhiChu.TabIndex = 6;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Google Sans", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTimKiem.Location = new Point(248, 3);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(362, 29);
            txtTimKiem.TabIndex = 7;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 10.2F);
            btnThem.Location = new Point(4, 361);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(108, 43);
            btnThem.TabIndex = 8;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Google Sans", 10.2F);
            btnSua.Location = new Point(118, 361);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(108, 43);
            btnSua.TabIndex = 9;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Google Sans", 10.2F);
            btnXoa.Location = new Point(232, 361);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(108, 43);
            btnXoa.TabIndex = 10;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 10.2F);
            btnLamMoi.Location = new Point(502, 361);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(108, 43);
            btnLamMoi.TabIndex = 11;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTimKiem.Location = new Point(18, 3);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(131, 38);
            btnTimKiem.TabIndex = 12;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaHoaDon
            // 
            lblMaHoaDon.Font = new Font("Google Sans", 10.2F);
            lblMaHoaDon.Location = new Point(18, 64);
            lblMaHoaDon.Name = "lblMaHoaDon";
            lblMaHoaDon.Size = new Size(136, 25);
            lblMaHoaDon.TabIndex = 13;
            lblMaHoaDon.Text = "Mã hóa đơn:";
            // 
            // lblNgayBan
            // 
            lblNgayBan.Font = new Font("Google Sans", 10.2F);
            lblNgayBan.Location = new Point(18, 114);
            lblNgayBan.Name = "lblNgayBan";
            lblNgayBan.Size = new Size(136, 25);
            lblNgayBan.TabIndex = 14;
            lblNgayBan.Text = "Ngày bán:";
            // 
            // lblMaKhachHang
            // 
            lblMaKhachHang.Font = new Font("Google Sans", 10.2F);
            lblMaKhachHang.Location = new Point(18, 164);
            lblMaKhachHang.Name = "lblMaKhachHang";
            lblMaKhachHang.Size = new Size(136, 25);
            lblMaKhachHang.TabIndex = 15;
            lblMaKhachHang.Text = "Mã khách hàng:";
            // 
            // lblMaNhanVien
            // 
            lblMaNhanVien.Font = new Font("Google Sans", 10.2F);
            lblMaNhanVien.Location = new Point(18, 214);
            lblMaNhanVien.Name = "lblMaNhanVien";
            lblMaNhanVien.Size = new Size(136, 25);
            lblMaNhanVien.TabIndex = 16;
            lblMaNhanVien.Text = "Mã nhân viên:";
            // 
            // lblTongTien
            // 
            lblTongTien.Font = new Font("Google Sans", 10.2F);
            lblTongTien.Location = new Point(18, 264);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(136, 25);
            lblTongTien.TabIndex = 17;
            lblTongTien.Text = "Tổng tiền:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(18, 314);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(136, 25);
            lblGhiChu.TabIndex = 18;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.Font = new Font("Google Sans", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnXuatExcel.Location = new Point(360, 361);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(119, 43);
            btnXuatExcel.TabIndex = 39;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // FrmHoaDonBan
            // 
            Controls.Add(btnXuatExcel);
            Controls.Add(dataGridViewHoaDonBan);
            Controls.Add(txtMaHoaDon);
            Controls.Add(dtpNgayBan);
            Controls.Add(txtMaKhachHang);
            Controls.Add(txtMaNhanVien);
            Controls.Add(txtTongTien);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaHoaDon);
            Controls.Add(lblNgayBan);
            Controls.Add(lblMaKhachHang);
            Controls.Add(lblMaNhanVien);
            Controls.Add(lblTongTien);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FrmHoaDonBan";
            Size = new Size(618, 794);
            Load += FrmHoaDonBan_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewHoaDonBan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewHoaDonBan;
        private TextBox txtMaHoaDon;
        private DateTimePicker dtpNgayBan;
        private TextBox txtMaKhachHang;
        private TextBox txtMaNhanVien;
        private TextBox txtTongTien;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaHoaDon;
        private Label lblNgayBan;
        private Label lblMaKhachHang;
        private Label lblMaNhanVien;
        private Label lblTongTien;
        private Label lblGhiChu;

        private void FrmHoaDonBan_Load(object sender, EventArgs e)
        {

        }
        private Button btnXuatExcel;

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewHoaDonBan.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!");
                    return;
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("DSHoaDon");
                    for (int i = 0; i < dataGridViewHoaDonBan.Columns.Count; i++)
                        worksheet.Cells[1, i + 1].Value = dataGridViewHoaDonBan.Columns[i].HeaderText;

                    for (int i = 0; i <     dataGridViewHoaDonBan.Rows.Count; i++)
                        for (int j = 0; j < dataGridViewHoaDonBan.Columns.Count; j++)
                            worksheet.Cells[i + 2, j + 1].Value = dataGridViewHoaDonBan.Rows[i].Cells[j].Value?.ToString();

                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveFileDialog.FileName = "DSHoaDon.xlsx";
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