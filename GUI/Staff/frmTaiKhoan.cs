using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmTaiKhoan : UserControl
    {
        private readonly TaiKhoanBLL _taiKhoanBLL;

        public frmTaiKhoan()
        {
            InitializeComponent();
            _taiKhoanBLL = new TaiKhoanBLL();
            LoadTaiKhoanList();
            LoadVaiTroComboBox();
        }

        private async void LoadTaiKhoanList()
        {
            try
            {
                var taiKhoans = await _taiKhoanBLL.GetAllTaiKhoanAsync();
                dataGridViewTaiKhoan.DataSource = taiKhoans;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVaiTroComboBox()
        {
            cbVaiTro.Items.AddRange(new[] { "admin", "staff" });
            cbVaiTro.SelectedIndex = 0; // 
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var taiKhoan = new TaiKhoan
                {
                    TenTaiKhoan = txtTenTaiKhoan.Text.Trim(),
                    MatKhau = txtMatKhau.Text.Trim(),
                    VaiTro = cbVaiTro.SelectedItem.ToString(),
                    MaNhanVien = txtMaNhanVien.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _taiKhoanBLL.AddTaiKhoanAsync(taiKhoan);
                if (result)
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTaiKhoanList();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var taiKhoan = new TaiKhoan
                {
                    TenTaiKhoan = txtTenTaiKhoan.Text.Trim(),
                    MatKhau = txtMatKhau.Text.Trim(),
                    VaiTro = cbVaiTro.SelectedItem.ToString(),
                    MaNhanVien = txtMaNhanVien.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _taiKhoanBLL.UpdateTaiKhoanAsync(taiKhoan);
                if (result)
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTaiKhoanList();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string tenTaiKhoan = txtTenTaiKhoan.Text.Trim();
                if (string.IsNullOrEmpty(tenTaiKhoan))
                {
                    MessageBox.Show("Vui lòng chọn tài khoản để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản {tenTaiKhoan}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _taiKhoanBLL.DeleteTaiKhoanAsync(tenTaiKhoan);
                    if (success)
                    {
                        MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTaiKhoanList();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoadTaiKhoanList();
        }

        private void ClearInputs()
        {
            txtTenTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtMaNhanVien.Clear();
            txtGhiChu.Clear();
            cbVaiTro.SelectedIndex = 0;
        }

        private void dataGridViewTaiKhoan_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewTaiKhoan.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewTaiKhoan.SelectedRows[0];
                txtTenTaiKhoan.Text = selectedRow.Cells["TenTaiKhoan"].Value?.ToString();
                txtMatKhau.Text = string.Empty; // Không hiển thị mật khẩu băm
                cbVaiTro.SelectedItem = selectedRow.Cells["VaiTro"].Value?.ToString();
                txtMaNhanVien.Text = selectedRow.Cells["MaNhanVien"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewTaiKhoan = new DataGridView();
            txtTenTaiKhoan = new TextBox();
            txtMatKhau = new TextBox();
            cbVaiTro = new ComboBox();
            txtMaNhanVien = new TextBox();
            txtGhiChu = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            lblTenTaiKhoan = new Label();
            lblMatKhau = new Label();
            lblVaiTro = new Label();
            lblMaNhanVien = new Label();
            lblGhiChu = new Label();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            panel1 = new Panel();
            ((ISupportInitialize)dataGridViewTaiKhoan).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewTaiKhoan
            // 
            dataGridViewTaiKhoan.ColumnHeadersHeight = 29;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("HarmonyOS Sans", 11.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridViewTaiKhoan.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewTaiKhoan.Location = new Point(3, 383);
            dataGridViewTaiKhoan.MultiSelect = false;
            dataGridViewTaiKhoan.Name = "dataGridViewTaiKhoan";
            dataGridViewTaiKhoan.RowHeadersWidth = 51;
            dataGridViewTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTaiKhoan.Size = new Size(1011, 272);
            dataGridViewTaiKhoan.TabIndex = 0;
            dataGridViewTaiKhoan.SelectionChanged += dataGridViewTaiKhoan_SelectionChanged;
            // 
            // txtTenTaiKhoan
            // 
            txtTenTaiKhoan.Cursor = Cursors.IBeam;
            txtTenTaiKhoan.Font = new Font("Times New Roman", 12F);
            txtTenTaiKhoan.Location = new Point(135, 46);
            txtTenTaiKhoan.Name = "txtTenTaiKhoan";
            txtTenTaiKhoan.Size = new Size(273, 30);
            txtTenTaiKhoan.TabIndex = 1;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Cursor = Cursors.IBeam;
            txtMatKhau.Font = new Font("Times New Roman", 12F);
            txtMatKhau.Location = new Point(699, 110);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(273, 30);
            txtMatKhau.TabIndex = 2;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // cbVaiTro
            // 
            cbVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVaiTro.Font = new Font("Times New Roman", 12F);
            cbVaiTro.Location = new Point(699, 48);
            cbVaiTro.Name = "cbVaiTro";
            cbVaiTro.Size = new Size(273, 30);
            cbVaiTro.TabIndex = 3;
            // 
            // txtMaNhanVien
            // 
            txtMaNhanVien.Cursor = Cursors.IBeam;
            txtMaNhanVien.Font = new Font("Times New Roman", 12F);
            txtMaNhanVien.Location = new Point(135, 111);
            txtMaNhanVien.Name = "txtMaNhanVien";
            txtMaNhanVien.Size = new Size(273, 30);
            txtMaNhanVien.TabIndex = 4;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Cursor = Cursors.IBeam;
            txtGhiChu.Font = new Font("Times New Roman", 12F);
            txtGhiChu.Location = new Point(135, 177);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(273, 30);
            txtGhiChu.TabIndex = 5;
            // 
            // btnThem
            // 
            btnThem.Cursor = Cursors.Hand;
            btnThem.Font = new Font("Google Sans", 10.8F);
            btnThem.Location = new Point(227, 259);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(141, 39);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Cursor = Cursors.Hand;
            btnSua.Font = new Font("Google Sans", 10.8F);
            btnSua.Location = new Point(437, 259);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(141, 39);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.Font = new Font("Google Sans", 10.8F);
            btnXoa.Location = new Point(646, 259);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(141, 39);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Cursor = Cursors.Hand;
            btnLamMoi.Font = new Font("Google Sans", 10.8F);
            btnLamMoi.Location = new Point(227, 14);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(141, 39);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // lblTenTaiKhoan
            // 
            lblTenTaiKhoan.Font = new Font("Google Sans", 10.8F);
            lblTenTaiKhoan.Location = new Point(11, 48);
            lblTenTaiKhoan.Name = "lblTenTaiKhoan";
            lblTenTaiKhoan.Size = new Size(154, 25);
            lblTenTaiKhoan.TabIndex = 10;
            lblTenTaiKhoan.Text = "Tên tài khoản:";
            // 
            // lblMatKhau
            // 
            lblMatKhau.Font = new Font("Google Sans", 10.8F);
            lblMatKhau.Location = new Point(575, 111);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(94, 25);
            lblMatKhau.TabIndex = 11;
            lblMatKhau.Text = "Mật khẩu:";
            // 
            // lblVaiTro
            // 
            lblVaiTro.Font = new Font("Google Sans", 10.8F);
            lblVaiTro.Location = new Point(575, 48);
            lblVaiTro.Name = "lblVaiTro";
            lblVaiTro.Size = new Size(79, 25);
            lblVaiTro.TabIndex = 12;
            lblVaiTro.Text = "Vai trò:";
            // 
            // lblMaNhanVien
            // 
            lblMaNhanVien.Font = new Font("Google Sans", 10.8F);
            lblMaNhanVien.Location = new Point(11, 113);
            lblMaNhanVien.Name = "lblMaNhanVien";
            lblMaNhanVien.Size = new Size(121, 25);
            lblMaNhanVien.TabIndex = 13;
            lblMaNhanVien.Text = "Mã nhân viên:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.8F);
            lblGhiChu.Location = new Point(11, 178);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(83, 25);
            lblGhiChu.TabIndex = 14;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Cursor = Cursors.IBeam;
            txtTimKiem.Font = new Font("Cascadia Code", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTimKiem.Location = new Point(400, 19);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(216, 27);
            txtTimKiem.TabIndex = 16;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Cursor = Cursors.Hand;
            btnTimKiem.Font = new Font("Google Sans", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTimKiem.Location = new Point(646, 14);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(141, 39);
            btnTimKiem.TabIndex = 17;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.GradientInactiveCaption;
            panel1.Controls.Add(txtTimKiem);
            panel1.Controls.Add(btnTimKiem);
            panel1.Controls.Add(btnLamMoi);
            panel1.Location = new Point(0, 314);
            panel1.Name = "panel1";
            panel1.Size = new Size(1017, 344);
            panel1.TabIndex = 18;
            // 
            // frmTaiKhoan
            // 
            Controls.Add(dataGridViewTaiKhoan);
            Controls.Add(txtTenTaiKhoan);
            Controls.Add(txtMatKhau);
            Controls.Add(cbVaiTro);
            Controls.Add(txtMaNhanVien);
            Controls.Add(txtGhiChu);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(lblTenTaiKhoan);
            Controls.Add(lblMatKhau);
            Controls.Add(lblVaiTro);
            Controls.Add(lblMaNhanVien);
            Controls.Add(lblGhiChu);
            Controls.Add(panel1);
            Font = new Font("Times New Roman", 12F);
            Name = "frmTaiKhoan";
            Size = new Size(1020, 658);
            Load += frmTaiKhoan_Load;
            ((ISupportInitialize)dataGridViewTaiKhoan).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewTaiKhoan;
        private TextBox txtTenTaiKhoan;
        private TextBox txtMatKhau;
        private ComboBox cbVaiTro;
        private TextBox txtMaNhanVien;
        private TextBox txtGhiChu;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Label lblTenTaiKhoan;
        private Label lblMatKhau;
        private Label lblVaiTro;
        private Label lblMaNhanVien;
        private Label lblGhiChu;

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {

        }
        private TextBox txtTimKiem;
        private Button btnTimKiem;

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var taiKhoans = await _taiKhoanBLL.GetAllTaiKhoanAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    taiKhoans = taiKhoans.Where(t =>
                        t.TenTaiKhoan.ToLower().Contains(keyword) ||
                        (t.MaNhanVien != null && t.MaNhanVien.ToLower().Contains(keyword))
                    ).ToList();
                }

                dataGridViewTaiKhoan.DataSource = taiKhoans;

                if (taiKhoans.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy tài khoản nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            btnTimKiem_Click(sender, e);
        }
        private Panel panel1;
    }
}