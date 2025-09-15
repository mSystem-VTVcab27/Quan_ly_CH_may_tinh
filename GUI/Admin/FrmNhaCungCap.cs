using System;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    public partial class FrmNhaCungCap : UserControl
    {
        private readonly NhaCungCapBLL _nhaCungCapBLL;

        public FrmNhaCungCap()
        {
            InitializeComponent();
            _nhaCungCapBLL = new NhaCungCapBLL();
            LoadNhaCungCapList();
            SetupDataGridViewColumns();
        }

        private async void LoadNhaCungCapList()
        {
            try
            {
                var nhaCungCaps = await _nhaCungCapBLL.GetAllNhaCungCapAsync();
                dataGridViewNhaCungCap.DataSource = nhaCungCaps;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột để tự định nghĩa
            dataGridViewNhaCungCap.AutoGenerateColumns = false;

            // Xóa các cột hiện tại để tránh trùng lặp khi gọi lại hàm này
            dataGridViewNhaCungCap.Columns.Clear();

            // Định nghĩa các cột
            dataGridViewNhaCungCap.Columns.Add("MaNhaCungCap", "Mã nhà cung cấp");
            dataGridViewNhaCungCap.Columns["MaNhaCungCap"].DataPropertyName = "MaNhaCungCap";
            dataGridViewNhaCungCap.Columns["MaNhaCungCap"].Width = 130;

            dataGridViewNhaCungCap.Columns.Add("TenNhaCungCap", "Tên nhà cung cấp");
            dataGridViewNhaCungCap.Columns["TenNhaCungCap"].DataPropertyName = "TenNhaCungCap";
            dataGridViewNhaCungCap.Columns["TenNhaCungCap"].Width = 150;

            dataGridViewNhaCungCap.Columns.Add("SoDienThoai", "Số điện thoại");
            dataGridViewNhaCungCap.Columns["SoDienThoai"].DataPropertyName = "SoDienThoai";
            dataGridViewNhaCungCap.Columns["SoDienThoai"].Width = 100;

            dataGridViewNhaCungCap.Columns.Add("DiaChi", "Địa chỉ");
            dataGridViewNhaCungCap.Columns["DiaChi"].DataPropertyName = "DiaChi";
            dataGridViewNhaCungCap.Columns["DiaChi"].Width = 200;

            dataGridViewNhaCungCap.Columns.Add("Email", "Email");
            dataGridViewNhaCungCap.Columns["Email"].DataPropertyName = "Email";
            dataGridViewNhaCungCap.Columns["Email"].Width = 150;

            dataGridViewNhaCungCap.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewNhaCungCap.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewNhaCungCap.Columns["GhiChu"].Width = 200;
        }

        private void dataGridViewNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewNhaCungCap.Rows[e.RowIndex];
                txtMaNhaCungCap.Text = row.Cells["MaNhaCungCap"].Value?.ToString();
                txtTenNhaCungCap.Text = row.Cells["TenNhaCungCap"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var nhaCungCaps = await _nhaCungCapBLL.GetAllNhaCungCapAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    nhaCungCaps = nhaCungCaps.Where(n =>
                        n.MaNhaCungCap.ToLower().Contains(keyword) ||
                        n.TenNhaCungCap.ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewNhaCungCap.DataSource = nhaCungCaps;

                if (nhaCungCaps.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhà cung cấp nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var nhaCungCap = new DAL.NhaCungCap // Sử dụng DAL.NhaCungCap
                {
                    MaNhaCungCap = txtMaNhaCungCap.Text.Trim(),
                    TenNhaCungCap = txtTenNhaCungCap.Text.Trim(),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _nhaCungCapBLL.AddNhaCungCapAsync(nhaCungCap);
                if (result)
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhaCungCapList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {

                var nhaCungCap = new DAL.NhaCungCap // Sử dụng DAL.NhaCungCap
                {
                    MaNhaCungCap = txtMaNhaCungCap.Text.Trim(),
                    TenNhaCungCap = txtTenNhaCungCap.Text.Trim(),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };


                bool result = await _nhaCungCapBLL.UpdateNhaCungCapAsync(nhaCungCap);
                if (result)
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhaCungCapList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maNhaCungCap = txtMaNhaCungCap.Text.Trim();
                if (string.IsNullOrEmpty(maNhaCungCap))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp {maNhaCungCap}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _nhaCungCapBLL.DeleteNhaCungCapAsync(maNhaCungCap);
                    if (success)
                    {
                        MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadNhaCungCapList();
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
                MessageBox.Show($"Lỗi khi xóa nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadNhaCungCapList();
        }

        private void ClearInputs()
        {
            txtMaNhaCungCap.Clear();
            txtTenNhaCungCap.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtGhiChu.Clear();
        }

        private void dataGridViewNhaCungCap_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewNhaCungCap.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewNhaCungCap.SelectedRows[0];
                txtMaNhaCungCap.Text = selectedRow.Cells["MaNhaCungCap"].Value?.ToString();
                txtTenNhaCungCap.Text = selectedRow.Cells["TenNhaCungCap"].Value?.ToString();
                txtSoDienThoai.Text = selectedRow.Cells["SoDienThoai"].Value?.ToString();
                txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value?.ToString();
                txtEmail.Text = selectedRow.Cells["Email"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewNhaCungCap = new DataGridView();
            txtMaNhaCungCap = new TextBox();
            txtTenNhaCungCap = new TextBox();
            txtSoDienThoai = new TextBox();
            txtDiaChi = new TextBox();
            txtEmail = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaNhaCungCap = new Label();
            lblTenNhaCungCap = new Label();
            lblSoDienThoai = new Label();
            lblDiaChi = new Label();
            lblEmail = new Label();
            lblGhiChu = new Label();
            button3 = new Button();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewNhaCungCap).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewNhaCungCap
            // 
            dataGridViewNhaCungCap.ColumnHeadersHeight = 29;
            dataGridViewNhaCungCap.Location = new Point(422, 17);
            dataGridViewNhaCungCap.MultiSelect = false;
            dataGridViewNhaCungCap.Name = "dataGridViewNhaCungCap";
            dataGridViewNhaCungCap.RowHeadersWidth = 51;
            dataGridViewNhaCungCap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewNhaCungCap.Size = new Size(859, 367);
            dataGridViewNhaCungCap.TabIndex = 0;
            dataGridViewNhaCungCap.CellClick += dataGridViewNhaCungCap_CellClick;
            dataGridViewNhaCungCap.SelectionChanged += dataGridViewNhaCungCap_SelectionChanged;
            // 
            // txtMaNhaCungCap
            // 
            txtMaNhaCungCap.Font = new Font("Google Sans", 10.2F);
            txtMaNhaCungCap.Location = new Point(196, 52);
            txtMaNhaCungCap.Name = "txtMaNhaCungCap";
            txtMaNhaCungCap.Size = new Size(194, 29);
            txtMaNhaCungCap.TabIndex = 1;
            // 
            // txtTenNhaCungCap
            // 
            txtTenNhaCungCap.Font = new Font("Google Sans", 10.2F);
            txtTenNhaCungCap.Location = new Point(196, 104);
            txtTenNhaCungCap.Name = "txtTenNhaCungCap";
            txtTenNhaCungCap.Size = new Size(194, 29);
            txtTenNhaCungCap.TabIndex = 2;
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Font = new Font("Google Sans", 10.2F);
            txtSoDienThoai.Location = new Point(196, 152);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(194, 29);
            txtSoDienThoai.TabIndex = 3;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Font = new Font("Google Sans", 10.2F);
            txtDiaChi.Location = new Point(196, 204);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(194, 29);
            txtDiaChi.TabIndex = 4;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Google Sans", 10.2F);
            txtEmail.Location = new Point(196, 252);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(194, 29);
            txtEmail.TabIndex = 5;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(196, 303);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(194, 29);
            txtGhiChu.TabIndex = 6;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Google Sans", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTimKiem.Location = new Point(130, 13);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(260, 29);
            txtTimKiem.TabIndex = 7;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 9F);
            btnThem.Location = new Point(24, 354);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(79, 39);
            btnThem.TabIndex = 8;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 9F);
            btnLamMoi.Location = new Point(281, 354);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(109, 39);
            btnLamMoi.TabIndex = 11;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTimKiem.Location = new Point(20, 8);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(104, 36);
            btnTimKiem.TabIndex = 12;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaNhaCungCap
            // 
            lblMaNhaCungCap.Font = new Font("Google Sans", 10.2F);
            lblMaNhaCungCap.Location = new Point(20, 54);
            lblMaNhaCungCap.Name = "lblMaNhaCungCap";
            lblMaNhaCungCap.Size = new Size(153, 25);
            lblMaNhaCungCap.TabIndex = 13;
            lblMaNhaCungCap.Text = "Mã nhà cung cấp:";
            // 
            // lblTenNhaCungCap
            // 
            lblTenNhaCungCap.Font = new Font("Google Sans", 10.2F);
            lblTenNhaCungCap.Location = new Point(20, 104);
            lblTenNhaCungCap.Name = "lblTenNhaCungCap";
            lblTenNhaCungCap.Size = new Size(153, 25);
            lblTenNhaCungCap.TabIndex = 14;
            lblTenNhaCungCap.Text = "Tên nhà cung cấp:";
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.Font = new Font("Google Sans", 10.2F);
            lblSoDienThoai.Location = new Point(20, 154);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(133, 25);
            lblSoDienThoai.TabIndex = 15;
            lblSoDienThoai.Text = "Số điện thoại:";
            // 
            // lblDiaChi
            // 
            lblDiaChi.Font = new Font("Google Sans", 10.2F);
            lblDiaChi.Location = new Point(20, 204);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(133, 25);
            lblDiaChi.TabIndex = 16;
            lblDiaChi.Text = "Địa chỉ:";
            // 
            // lblEmail
            // 
            lblEmail.Font = new Font("Google Sans", 10.2F);
            lblEmail.Location = new Point(20, 254);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(133, 25);
            lblEmail.TabIndex = 17;
            lblEmail.Text = "Email:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(20, 303);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(84, 25);
            lblGhiChu.TabIndex = 18;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // button3
            // 
            button3.Font = new Font("Google Sans", 9F);
            button3.Location = new Point(196, 354);
            button3.Name = "button3";
            button3.Size = new Size(79, 39);
            button3.TabIndex = 10;
            button3.Text = "Xóa";
            button3.Click += btnXoa_Click;
            // 
            // button4
            // 
            button4.Font = new Font("Google Sans", 9F);
            button4.Location = new Point(110, 354);
            button4.Name = "button4";
            button4.Size = new Size(79, 39);
            button4.TabIndex = 9;
            button4.Text = "Sửa";
            button4.Click += btnSua_Click;
            // 
            // FrmNhaCungCap
            // 
            Controls.Add(dataGridViewNhaCungCap);
            Controls.Add(txtMaNhaCungCap);
            Controls.Add(txtTenNhaCungCap);
            Controls.Add(txtSoDienThoai);
            Controls.Add(txtDiaChi);
            Controls.Add(txtEmail);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaNhaCungCap);
            Controls.Add(lblTenNhaCungCap);
            Controls.Add(lblSoDienThoai);
            Controls.Add(lblDiaChi);
            Controls.Add(lblEmail);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FrmNhaCungCap";
            Size = new Size(1283, 407);
            ((System.ComponentModel.ISupportInitialize)dataGridViewNhaCungCap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewNhaCungCap;
        private TextBox txtMaNhaCungCap;
        private TextBox txtTenNhaCungCap;
        private TextBox txtSoDienThoai;
        private TextBox txtDiaChi;
        private TextBox txtEmail;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaNhaCungCap;
        private Label lblTenNhaCungCap;
        private Label lblSoDienThoai;
        private Label lblDiaChi;
        private Label lblEmail;
        private Label lblGhiChu;

        private void FrmNhaCungCap_Load(object sender, EventArgs e)
        {

        }
        private Button button3;
        private Button button4;
    }
}
