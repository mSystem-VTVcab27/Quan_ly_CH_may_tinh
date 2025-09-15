using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    public partial class FrmPhieuNhap : UserControl
    {
        private readonly PhieuNhapBLL _phieuNhapBLL;
        private readonly NhaCungCapBLL _nhaCungCapBLL;
        private readonly NhanVienBLL _nhanVienBLL;

        public FrmPhieuNhap()
        {
            InitializeComponent();
            _phieuNhapBLL = new PhieuNhapBLL();
            _nhaCungCapBLL = new NhaCungCapBLL();
            _nhanVienBLL = new NhanVienBLL();
            LoadPhieuNhapList();
            LoadComboBoxes();
            SetupDataGridViewColumns();
            dtpNgayNhap.Value = DateTime.Now; // Mặc định ngày hiện tại
            dataGridViewPhieuNhap.AutoGenerateColumns = false;
        }

        private async void LoadPhieuNhapList()
        {
            try
            {
                var phieuNhaps = await _phieuNhapBLL.GetAllPhieuNhapAsync();
                dataGridViewPhieuNhap.DataSource = phieuNhaps;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadComboBoxes()
        {
            try
            {
                // Tải danh sách nhà cung cấp cho ComboBox
                var nhaCungCaps = await _nhaCungCapBLL.GetAllNhaCungCapAsync();
                cbMaNhaCungCap.DataSource = nhaCungCaps;
                cbMaNhaCungCap.DisplayMember = "TenNhaCungCap";
                cbMaNhaCungCap.ValueMember = "MaNhaCungCap";
                cbMaNhaCungCap.SelectedIndex = -1; // Không chọn mặc định

                // Tải danh sách nhân viên cho ComboBox
                var nhanViens = await _nhanVienBLL.GetAllNhanVienAsync();
                cbMaNhanVien.DataSource = nhanViens;
                cbMaNhanVien.DisplayMember = "HoTen";
                cbMaNhanVien.ValueMember = "MaNhanVien";
                cbMaNhanVien.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ComboBox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột để ta có thể tự định nghĩa
            dataGridViewPhieuNhap.AutoGenerateColumns = false;

            // Xóa các cột cũ nếu có để tránh trùng lặp
            dataGridViewPhieuNhap.Columns.Clear();

            // Thêm và cấu hình từng cột
            dataGridViewPhieuNhap.Columns.Add("MaPhieuNhap", "Mã phiếu nhập");
            dataGridViewPhieuNhap.Columns["MaPhieuNhap"].DataPropertyName = "MaPhieuNhap";
            dataGridViewPhieuNhap.Columns["MaPhieuNhap"].Width = 105;

            dataGridViewPhieuNhap.Columns.Add("NgayNhap", "Ngày nhập");
            dataGridViewPhieuNhap.Columns["NgayNhap"].DataPropertyName = "NgayNhap";
            dataGridViewPhieuNhap.Columns["NgayNhap"].Width = 100;

            dataGridViewPhieuNhap.Columns.Add("MaNhaCungCap", "Mã nhà cung cấp");
            dataGridViewPhieuNhap.Columns["MaNhaCungCap"].DataPropertyName = "MaNhaCungCap";
            dataGridViewPhieuNhap.Columns["MaNhaCungCap"].Width = 120;

            dataGridViewPhieuNhap.Columns.Add("MaNhanVien", "Mã nhân viên");
            dataGridViewPhieuNhap.Columns["MaNhanVien"].DataPropertyName = "MaNhanVien";
            dataGridViewPhieuNhap.Columns["MaNhanVien"].Width = 100;

            dataGridViewPhieuNhap.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewPhieuNhap.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewPhieuNhap.Columns["GhiChu"].Width = 200;
        }

        private void dataGridViewPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewPhieuNhap.Rows[e.RowIndex];
                txtMaPhieuNhap.Text = row.Cells["MaPhieuNhap"].Value?.ToString();
                dtpNgayNhap.Value = row.Cells["NgayNhap"].Value != null ? Convert.ToDateTime(row.Cells["NgayNhap"].Value) : DateTime.Now;
                cbMaNhaCungCap.SelectedValue = row.Cells["MaNhaCungCap"].Value?.ToString();
                cbMaNhanVien.SelectedValue = row.Cells["MaNhanVien"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var phieuNhaps = await _phieuNhapBLL.GetAllPhieuNhapAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    phieuNhaps = phieuNhaps.Where(p =>
                        p.MaPhieuNhap.ToLower().Contains(keyword) ||
                        p.NgayNhap.ToString("dd/MM/yyyy").ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewPhieuNhap.DataSource = phieuNhaps;

                if (phieuNhaps.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu nhập nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpNgayNhap.Value > DateTime.Now)
                {
                    MessageBox.Show("Ngày nhập không được là ngày trong tương lai.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var phieuNhap = new PhieuNhap
                {
                    MaPhieuNhap = txtMaPhieuNhap.Text.Trim(),
                    NgayNhap = dtpNgayNhap.Value,
                    MaNhaCungCap = cbMaNhaCungCap.SelectedValue?.ToString(),
                    MaNhanVien = cbMaNhanVien.SelectedValue?.ToString(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(phieuNhap.MaNhaCungCap) || string.IsNullOrEmpty(phieuNhap.MaNhanVien))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp và nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _phieuNhapBLL.AddPhieuNhapAsync(phieuNhap);
                if (result)
                {
                    MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPhieuNhapList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpNgayNhap.Value > DateTime.Now)
                {
                    MessageBox.Show("Ngày nhập không được là ngày trong tương lai.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var phieuNhap = new PhieuNhap
                {
                    MaPhieuNhap = txtMaPhieuNhap.Text.Trim(),
                    NgayNhap = dtpNgayNhap.Value,
                    MaNhaCungCap = cbMaNhaCungCap.SelectedValue?.ToString(),
                    MaNhanVien = cbMaNhanVien.SelectedValue?.ToString(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                if (string.IsNullOrEmpty(phieuNhap.MaNhaCungCap) || string.IsNullOrEmpty(phieuNhap.MaNhanVien))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp và nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = await _phieuNhapBLL.UpdatePhieuNhapAsync(phieuNhap);
                if (result)
                {
                    MessageBox.Show("Cập nhật phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPhieuNhapList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieuNhap = txtMaPhieuNhap.Text.Trim();
                if (string.IsNullOrEmpty(maPhieuNhap))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu nhập {maPhieuNhap}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _phieuNhapBLL.DeletePhieuNhapAsync(maPhieuNhap);
                    if (success)
                    {
                        MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPhieuNhapList();
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
                MessageBox.Show($"Lỗi khi xóa phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadPhieuNhapList();
        }

        private void ClearInputs()
        {
            txtMaPhieuNhap.Clear();
            dtpNgayNhap.Value = DateTime.Now;
            cbMaNhaCungCap.SelectedIndex = -1;
            cbMaNhanVien.SelectedIndex = -1;
            txtGhiChu.Clear();
        }

        private void dataGridViewPhieuNhap_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPhieuNhap.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewPhieuNhap.SelectedRows[0];
                txtMaPhieuNhap.Text = selectedRow.Cells["MaPhieuNhap"].Value?.ToString();
                dtpNgayNhap.Value = selectedRow.Cells["NgayNhap"].Value != null ? Convert.ToDateTime(selectedRow.Cells["NgayNhap"].Value) : DateTime.Now;
                cbMaNhaCungCap.SelectedValue = selectedRow.Cells["MaNhaCungCap"].Value?.ToString();
                cbMaNhanVien.SelectedValue = selectedRow.Cells["MaNhanVien"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewPhieuNhap = new DataGridView();
            txtMaPhieuNhap = new TextBox();
            dtpNgayNhap = new DateTimePicker();
            cbMaNhaCungCap = new ComboBox();
            cbMaNhanVien = new ComboBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaPhieuNhap = new Label();
            lblNgayNhap = new Label();
            lblMaNhaCungCap = new Label();
            lblMaNhanVien = new Label();
            lblGhiChu = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPhieuNhap).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewPhieuNhap
            // 
            dataGridViewPhieuNhap.ColumnHeadersHeight = 29;
            dataGridViewPhieuNhap.Location = new Point(3, 328);
            dataGridViewPhieuNhap.MultiSelect = false;
            dataGridViewPhieuNhap.Name = "dataGridViewPhieuNhap";
            dataGridViewPhieuNhap.RowHeadersWidth = 51;
            dataGridViewPhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPhieuNhap.Size = new Size(612, 481);
            dataGridViewPhieuNhap.TabIndex = 0;
            dataGridViewPhieuNhap.CellClick += dataGridViewPhieuNhap_CellClick;
            dataGridViewPhieuNhap.SelectionChanged += dataGridViewPhieuNhap_SelectionChanged;
            // 
            // txtMaPhieuNhap
            // 
            txtMaPhieuNhap.Font = new Font("Google Sans", 10.2F);
            txtMaPhieuNhap.Location = new Point(176, 72);
            txtMaPhieuNhap.Name = "txtMaPhieuNhap";
            txtMaPhieuNhap.Size = new Size(289, 29);
            txtMaPhieuNhap.TabIndex = 1;
            // 
            // dtpNgayNhap
            // 
            dtpNgayNhap.Font = new Font("Google Sans", 10.2F);
            dtpNgayNhap.Format = DateTimePickerFormat.Short;
            dtpNgayNhap.Location = new Point(176, 122);
            dtpNgayNhap.Name = "dtpNgayNhap";
            dtpNgayNhap.Size = new Size(289, 29);
            dtpNgayNhap.TabIndex = 2;
            // 
            // cbMaNhaCungCap
            // 
            cbMaNhaCungCap.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaNhaCungCap.Font = new Font("Google Sans", 10.2F);
            cbMaNhaCungCap.Location = new Point(176, 172);
            cbMaNhaCungCap.Name = "cbMaNhaCungCap";
            cbMaNhaCungCap.Size = new Size(289, 30);
            cbMaNhaCungCap.TabIndex = 3;
            // 
            // cbMaNhanVien
            // 
            cbMaNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaNhanVien.Font = new Font("Google Sans", 10.2F);
            cbMaNhanVien.Location = new Point(176, 222);
            cbMaNhanVien.Name = "cbMaNhanVien";
            cbMaNhanVien.Size = new Size(289, 30);
            cbMaNhanVien.TabIndex = 4;
            cbMaNhanVien.SelectedIndexChanged += cbMaNhanVien_SelectedIndexChanged;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(176, 272);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(289, 29);
            txtGhiChu.TabIndex = 5;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Google Sans", 10.2F);
            txtTimKiem.Location = new Point(141, 10);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(324, 29);
            txtTimKiem.TabIndex = 6;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(486, 112);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(114, 41);
            btnThem.TabIndex = 7;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(486, 160);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(114, 41);
            btnSua.TabIndex = 8;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(486, 211);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(114, 41);
            btnXoa.TabIndex = 9;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(486, 261);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(114, 41);
            btnLamMoi.TabIndex = 10;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTimKiem.Location = new Point(3, 3);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(108, 36);
            btnTimKiem.TabIndex = 11;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaPhieuNhap
            // 
            lblMaPhieuNhap.Font = new Font("Google Sans", 10.2F);
            lblMaPhieuNhap.Location = new Point(3, 72);
            lblMaPhieuNhap.Name = "lblMaPhieuNhap";
            lblMaPhieuNhap.Size = new Size(130, 25);
            lblMaPhieuNhap.TabIndex = 12;
            lblMaPhieuNhap.Text = "Mã phiếu nhập:";
            // 
            // lblNgayNhap
            // 
            lblNgayNhap.Font = new Font("Google Sans", 10.2F);
            lblNgayNhap.Location = new Point(3, 122);
            lblNgayNhap.Name = "lblNgayNhap";
            lblNgayNhap.Size = new Size(130, 25);
            lblNgayNhap.TabIndex = 13;
            lblNgayNhap.Text = "Ngày nhập:";
            // 
            // lblMaNhaCungCap
            // 
            lblMaNhaCungCap.Font = new Font("Google Sans", 10.2F);
            lblMaNhaCungCap.Location = new Point(3, 172);
            lblMaNhaCungCap.Name = "lblMaNhaCungCap";
            lblMaNhaCungCap.Size = new Size(130, 25);
            lblMaNhaCungCap.TabIndex = 14;
            lblMaNhaCungCap.Text = "Nhà cung cấp:";
            // 
            // lblMaNhanVien
            // 
            lblMaNhanVien.Font = new Font("Google Sans", 10.2F);
            lblMaNhanVien.Location = new Point(3, 222);
            lblMaNhanVien.Name = "lblMaNhanVien";
            lblMaNhanVien.Size = new Size(130, 25);
            lblMaNhanVien.TabIndex = 15;
            lblMaNhanVien.Text = "Nhân viên:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(3, 272);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(130, 25);
            lblGhiChu.TabIndex = 16;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // FrmPhieuNhap
            // 
            Controls.Add(dataGridViewPhieuNhap);
            Controls.Add(txtMaPhieuNhap);
            Controls.Add(dtpNgayNhap);
            Controls.Add(cbMaNhaCungCap);
            Controls.Add(cbMaNhanVien);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaPhieuNhap);
            Controls.Add(lblNgayNhap);
            Controls.Add(lblMaNhaCungCap);
            Controls.Add(lblMaNhanVien);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 10.2F);
            Name = "FrmPhieuNhap";
            Size = new Size(618, 812);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPhieuNhap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewPhieuNhap;
        private TextBox txtMaPhieuNhap;
        private DateTimePicker dtpNgayNhap;
        private ComboBox cbMaNhaCungCap;
        private ComboBox cbMaNhanVien;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaPhieuNhap;
        private Label lblNgayNhap;
        private Label lblMaNhaCungCap;
        private Label lblMaNhanVien;
        private Label lblGhiChu;

        private void cbMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}