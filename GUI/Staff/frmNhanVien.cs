using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DAL;

namespace GUI
{
    public partial class frmNhanVien : UserControl
    {
        private readonly NhanVienDAL _nhanVienDAL;

        public frmNhanVien()
        {
            InitializeComponent();
            _nhanVienDAL = new NhanVienDAL();
            LoadNhanVienList();
            LoadGioiTinhComboBox();
        }

        private async void LoadNhanVienList()
        {
            try
            {
                var nhanViens = await _nhanVienDAL.GetAllNhanVienAsync();
                dataGridViewNhanVien.DataSource = nhanViens;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGioiTinhComboBox()
        {
            cbGioiTinh.Items.AddRange(new[] { "Nam", "Nữ", "Khác" });
            cbGioiTinh.SelectedIndex = 0;
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var nhanViens = await _nhanVienDAL.GetAllNhanVienAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    nhanViens = nhanViens.Where(n =>
                        n.MaNhanVien.ToLower().Contains(keyword) ||
                        n.HoTen.ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewNhanVien.DataSource = nhanViens;

                if (nhanViens.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var nhanVien = new NhanVien
                {
                    MaNhanVien = txtMaNhanVien.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = cbGioiTinh.SelectedItem?.ToString(),
                    NgayVaoLam = dtpNgayVaoLam.Value == dtpNgayVaoLam.MinDate ? null : dtpNgayVaoLam.Value,
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _nhanVienDAL.AddNhanVienAsync(nhanVien);
                if (result)
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhanVienList();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var nhanVien = new NhanVien
                {
                    MaNhanVien = txtMaNhanVien.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = cbGioiTinh.SelectedItem?.ToString(),
                    NgayVaoLam = dtpNgayVaoLam.Value == dtpNgayVaoLam.MinDate ? null : dtpNgayVaoLam.Value,
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _nhanVienDAL.UpdateNhanVienAsync(nhanVien);
                if (result)
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhanVienList();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maNhanVien = txtMaNhanVien.Text.Trim();
                if (string.IsNullOrEmpty(maNhanVien))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {maNhanVien}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _nhanVienDAL.DeleteNhanVienAsync(maNhanVien);
                    if (success)
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadNhanVienList();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadNhanVienList();
        }

        private void ClearInputs()
        {
            txtMaNhanVien.Clear();
            txtHoTen.Clear();
            cbGioiTinh.SelectedIndex = 0;
            dtpNgayVaoLam.Value = dtpNgayVaoLam.MinDate;
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtGhiChu.Clear();
        }

        private void dataGridViewNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewNhanVien.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewNhanVien.SelectedRows[0];
                txtMaNhanVien.Text = selectedRow.Cells["MaNhanVien"].Value?.ToString();
                txtHoTen.Text = selectedRow.Cells["HoTen"].Value?.ToString();
                cbGioiTinh.SelectedItem = selectedRow.Cells["GioiTinh"].Value?.ToString();
                dtpNgayVaoLam.Value = selectedRow.Cells["NgayVaoLam"].Value != null ? Convert.ToDateTime(selectedRow.Cells["NgayVaoLam"].Value) : dtpNgayVaoLam.MinDate;
                txtSoDienThoai.Text = selectedRow.Cells["SoDienThoai"].Value?.ToString();
                txtEmail.Text = selectedRow.Cells["Email"].Value?.ToString();
                txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }


        private void InitializeComponent()
        {
            dataGridViewNhanVien = new DataGridView();
            txtMaNhanVien = new TextBox();
            txtHoTen = new TextBox();
            cbGioiTinh = new ComboBox();
            dtpNgayVaoLam = new DateTimePicker();
            txtSoDienThoai = new TextBox();
            txtEmail = new TextBox();
            txtDiaChi = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaNhanVien = new Label();
            lblHoTen = new Label();
            lblGioiTinh = new Label();
            lblNgayVaoLam = new Label();
            lblSoDienThoai = new Label();
            lblEmail = new Label();
            lblDiaChi = new Label();
            lblGhiChu = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewNhanVien).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewNhanVien
            // 
            dataGridViewNhanVien.ColumnHeadersHeight = 60;
            dataGridViewNhanVien.Location = new Point(3, 308);
            dataGridViewNhanVien.MultiSelect = false;
            dataGridViewNhanVien.Name = "dataGridViewNhanVien";
            dataGridViewNhanVien.RowHeadersWidth = 51;
            dataGridViewNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewNhanVien.Size = new Size(1011, 347);
            dataGridViewNhanVien.TabIndex = 0;
            dataGridViewNhanVien.SelectionChanged += dataGridViewNhanVien_SelectionChanged;
            // 
            // txtMaNhanVien
            // 
            txtMaNhanVien.Font = new Font("Times New Roman", 12F);
            txtMaNhanVien.Location = new Point(140, 24);
            txtMaNhanVien.Name = "txtMaNhanVien";
            txtMaNhanVien.Size = new Size(212, 30);
            txtMaNhanVien.TabIndex = 1;
            // 
            // txtHoTen
            // 
            txtHoTen.Font = new Font("Times New Roman", 12F);
            txtHoTen.Location = new Point(140, 74);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(212, 30);
            txtHoTen.TabIndex = 2;
            // 
            // cbGioiTinh
            // 
            cbGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cbGioiTinh.Font = new Font("Times New Roman", 12F);
            cbGioiTinh.Location = new Point(784, 24);
            cbGioiTinh.Name = "cbGioiTinh";
            cbGioiTinh.Size = new Size(212, 30);
            cbGioiTinh.TabIndex = 3;
            // 
            // dtpNgayVaoLam
            // 
            dtpNgayVaoLam.Font = new Font("Times New Roman", 12F);
            dtpNgayVaoLam.Format = DateTimePickerFormat.Short;
            dtpNgayVaoLam.Location = new Point(784, 75);
            dtpNgayVaoLam.Name = "dtpNgayVaoLam";
            dtpNgayVaoLam.Size = new Size(212, 30);
            dtpNgayVaoLam.TabIndex = 4;
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Font = new Font("Times New Roman", 12F);
            txtSoDienThoai.Location = new Point(140, 131);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(212, 30);
            txtSoDienThoai.TabIndex = 5;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Times New Roman", 12F);
            txtEmail.Location = new Point(140, 181);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(212, 30);
            txtEmail.TabIndex = 6;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Font = new Font("Times New Roman", 12F);
            txtDiaChi.Location = new Point(784, 132);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(212, 30);
            txtDiaChi.TabIndex = 7;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Times New Roman", 12F);
            txtGhiChu.Location = new Point(784, 182);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(212, 30);
            txtGhiChu.TabIndex = 8;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Times New Roman", 12F);
            txtTimKiem.Location = new Point(670, 22);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(232, 30);
            txtTimKiem.TabIndex = 9;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(20, 18);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(99, 38);
            btnThem.TabIndex = 10;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(125, 18);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(99, 38);
            btnSua.TabIndex = 11;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(230, 18);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(99, 38);
            btnXoa.TabIndex = 12;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(335, 18);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(114, 38);
            btnLamMoi.TabIndex = 13;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(908, 19);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(106, 36);
            btnTimKiem.TabIndex = 14;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaNhanVien
            // 
            lblMaNhanVien.Font = new Font("Google Sans", 9F);
            lblMaNhanVien.Location = new Point(26, 30);
            lblMaNhanVien.Name = "lblMaNhanVien";
            lblMaNhanVien.Size = new Size(198, 25);
            lblMaNhanVien.TabIndex = 15;
            lblMaNhanVien.Text = "Mã nhân viên:";
            // 
            // lblHoTen
            // 
            lblHoTen.Font = new Font("Google Sans", 9F);
            lblHoTen.Location = new Point(26, 80);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(198, 25);
            lblHoTen.TabIndex = 16;
            lblHoTen.Text = "Họ tên:";
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.Font = new Font("Google Sans", 9F);
            lblGioiTinh.Location = new Point(652, 29);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new Size(198, 25);
            lblGioiTinh.TabIndex = 17;
            lblGioiTinh.Text = "Giới tính:";
            // 
            // lblNgayVaoLam
            // 
            lblNgayVaoLam.Font = new Font("Google Sans", 9F);
            lblNgayVaoLam.Location = new Point(652, 79);
            lblNgayVaoLam.Name = "lblNgayVaoLam";
            lblNgayVaoLam.Size = new Size(198, 25);
            lblNgayVaoLam.TabIndex = 18;
            lblNgayVaoLam.Text = "Ngày vào làm:";
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.Font = new Font("Google Sans", 9F);
            lblSoDienThoai.Location = new Point(26, 137);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(198, 25);
            lblSoDienThoai.TabIndex = 19;
            lblSoDienThoai.Text = "Số điện thoại:";
            // 
            // lblEmail
            // 
            lblEmail.Font = new Font("Google Sans", 9F);
            lblEmail.Location = new Point(26, 187);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(198, 25);
            lblEmail.TabIndex = 20;
            lblEmail.Text = "Email:";
            // 
            // lblDiaChi
            // 
            lblDiaChi.Font = new Font("Google Sans", 9F);
            lblDiaChi.Location = new Point(652, 136);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(198, 25);
            lblDiaChi.TabIndex = 21;
            lblDiaChi.Text = "Địa chỉ:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 9F);
            lblGhiChu.Location = new Point(652, 186);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(198, 25);
            lblGhiChu.TabIndex = 22;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.GradientInactiveCaption;
            panel1.Controls.Add(btnTimKiem);
            panel1.Controls.Add(txtTimKiem);
            panel1.Controls.Add(btnLamMoi);
            panel1.Controls.Add(btnThem);
            panel1.Controls.Add(btnXoa);
            panel1.Controls.Add(btnSua);
            panel1.Location = new Point(0, 233);
            panel1.Name = "panel1";
            panel1.Size = new Size(1017, 425);
            panel1.TabIndex = 23;
            // 
            // frmNhanVien
            // 
            Controls.Add(dataGridViewNhanVien);
            Controls.Add(txtMaNhanVien);
            Controls.Add(txtHoTen);
            Controls.Add(cbGioiTinh);
            Controls.Add(dtpNgayVaoLam);
            Controls.Add(txtSoDienThoai);
            Controls.Add(txtEmail);
            Controls.Add(txtDiaChi);
            Controls.Add(txtGhiChu);
            Controls.Add(lblMaNhanVien);
            Controls.Add(lblHoTen);
            Controls.Add(lblGioiTinh);
            Controls.Add(lblNgayVaoLam);
            Controls.Add(lblSoDienThoai);
            Controls.Add(lblEmail);
            Controls.Add(lblDiaChi);
            Controls.Add(lblGhiChu);
            Controls.Add(panel1);
            Font = new Font("Google Sans", 10.8F);
            Name = "frmNhanVien";
            Size = new Size(1020, 658);
            Load += frmNhanVien_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewNhanVien).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewNhanVien;
        private TextBox txtMaNhanVien;
        private TextBox txtHoTen;
        private ComboBox cbGioiTinh;
        private DateTimePicker dtpNgayVaoLam;
        private TextBox txtSoDienThoai;
        private TextBox txtEmail;
        private TextBox txtDiaChi;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaNhanVien;
        private Label lblHoTen;
        private Label lblGioiTinh;
        private Label lblNgayVaoLam;
        private Label lblSoDienThoai;
        private Label lblEmail;
        private Label lblDiaChi;
        private Label lblGhiChu;

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            btnTimKiem_Click(sender, e);
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {

        }
        private Panel panel1;
    }
}