using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    public partial class FrmDanhMucSanPham : UserControl
    {
        private readonly DanhMucSanPhamBLL _danhMucSanPhamBLL;

        public FrmDanhMucSanPham()
        {
            InitializeComponent();
            _danhMucSanPhamBLL = new DanhMucSanPhamBLL();
            LoadDanhMucSanPhamList();
            SetupDataGridViewColumns();
        }

        private async void LoadDanhMucSanPhamList()
        {
            try
            {
                var danhMucSanPhams = await _danhMucSanPhamBLL.GetAllDanhMucSanPhamAsync();
                dataGridViewDanhMuc.DataSource = danhMucSanPhams;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột
            dataGridViewDanhMuc.AutoGenerateColumns = false;

            // Xóa các cột cũ nếu có
            dataGridViewDanhMuc.Columns.Clear();

            // Thêm và cấu hình từng cột
            dataGridViewDanhMuc.Columns.Add("MaDanhMuc", "Mã danh mục");
            dataGridViewDanhMuc.Columns["MaDanhMuc"].DataPropertyName = "MaDanhMuc";
            dataGridViewDanhMuc.Columns["MaDanhMuc"].Width = 130;

            dataGridViewDanhMuc.Columns.Add("TenDanhMuc", "Tên danh mục");
            dataGridViewDanhMuc.Columns["TenDanhMuc"].DataPropertyName = "TenDanhMuc";
            dataGridViewDanhMuc.Columns["TenDanhMuc"].Width = 150;

            dataGridViewDanhMuc.Columns.Add("MoTa", "Mô tả");
            dataGridViewDanhMuc.Columns["MoTa"].DataPropertyName = "MoTa";
            dataGridViewDanhMuc.Columns["MoTa"].Width = 200;

            dataGridViewDanhMuc.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewDanhMuc.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewDanhMuc.Columns["GhiChu"].Width = 213;
        }

        private void dataGridViewDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewDanhMuc.Rows[e.RowIndex];
                txtMaDanhMuc.Text = row.Cells["MaDanhMuc"].Value?.ToString();
                txtTenDanhMuc.Text = row.Cells["TenDanhMuc"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var danhMucSanPhams = await _danhMucSanPhamBLL.GetAllDanhMucSanPhamAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    danhMucSanPhams = danhMucSanPhams.Where(d =>
                        d.MaDanhMuc.ToLower().Contains(keyword) ||
                        d.TenDanhMuc.ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewDanhMuc.DataSource = danhMucSanPhams;

                if (danhMucSanPhams.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy danh mục nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var danhMucSanPham = new DanhMucSanPham
                {
                    MaDanhMuc = txtMaDanhMuc.Text.Trim(),
                    TenDanhMuc = txtTenDanhMuc.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _danhMucSanPhamBLL.AddDanhMucSanPhamAsync(danhMucSanPham);
                if (result)
                {
                    MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhMucSanPhamList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var danhMucSanPham = new DanhMucSanPham
                {
                    MaDanhMuc = txtMaDanhMuc.Text.Trim(),
                    TenDanhMuc = txtTenDanhMuc.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _danhMucSanPhamBLL.UpdateDanhMucSanPhamAsync(danhMucSanPham);
                if (result)
                {
                    MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhMucSanPhamList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maDanhMuc = txtMaDanhMuc.Text.Trim();
                if (string.IsNullOrEmpty(maDanhMuc))
                {
                    MessageBox.Show("Vui lòng chọn danh mục để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục {maDanhMuc}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _danhMucSanPhamBLL.DeleteDanhMucSanPhamAsync(maDanhMuc);
                    if (success)
                    {
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhMucSanPhamList();
                        ClearInputs();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadDanhMucSanPhamList();
        }

        private void ClearInputs()
        {
            txtMaDanhMuc.Clear();
            txtTenDanhMuc.Clear();
            txtMoTa.Clear();
            txtGhiChu.Clear();
        }

        private void dataGridViewDanhMuc_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewDanhMuc.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewDanhMuc.SelectedRows[0];
                txtMaDanhMuc.Text = selectedRow.Cells["MaDanhMuc"].Value?.ToString();
                txtTenDanhMuc.Text = selectedRow.Cells["TenDanhMuc"].Value?.ToString();
                txtMoTa.Text = selectedRow.Cells["MoTa"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewDanhMuc = new DataGridView();
            txtMaDanhMuc = new TextBox();
            txtTenDanhMuc = new TextBox();
            txtMoTa = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaDanhMuc = new Label();
            lblTenDanhMuc = new Label();
            lblMoTa = new Label();
            lblGhiChu = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDanhMuc).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewDanhMuc
            // 
            dataGridViewDanhMuc.ColumnHeadersHeight = 29;
            dataGridViewDanhMuc.Location = new Point(426, 9);
            dataGridViewDanhMuc.MultiSelect = false;
            dataGridViewDanhMuc.Name = "dataGridViewDanhMuc";
            dataGridViewDanhMuc.RowHeadersWidth = 51;
            dataGridViewDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDanhMuc.Size = new Size(854, 300);
            dataGridViewDanhMuc.TabIndex = 0;
            dataGridViewDanhMuc.CellClick += dataGridViewDanhMuc_CellClick;
            dataGridViewDanhMuc.SelectionChanged += dataGridViewDanhMuc_SelectionChanged;
            // 
            // txtMaDanhMuc
            // 
            txtMaDanhMuc.Font = new Font("Google Sans", 10.2F);
            txtMaDanhMuc.Location = new Point(177, 22);
            txtMaDanhMuc.Name = "txtMaDanhMuc";
            txtMaDanhMuc.Size = new Size(200, 29);
            txtMaDanhMuc.TabIndex = 1;
            // 
            // txtTenDanhMuc
            // 
            txtTenDanhMuc.Font = new Font("Google Sans", 10.2F);
            txtTenDanhMuc.Location = new Point(177, 130);
            txtTenDanhMuc.Name = "txtTenDanhMuc";
            txtTenDanhMuc.Size = new Size(200, 29);
            txtTenDanhMuc.TabIndex = 2;
            // 
            // txtMoTa
            // 
            txtMoTa.Font = new Font("Google Sans", 10.2F);
            txtMoTa.Location = new Point(177, 71);
            txtMoTa.Name = "txtMoTa";
            txtMoTa.Size = new Size(200, 29);
            txtMoTa.TabIndex = 3;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(177, 179);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(200, 29);
            txtGhiChu.TabIndex = 4;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(3, 286);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(282, 26);
            txtTimKiem.TabIndex = 5;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 10.2F);
            btnThem.Location = new Point(12, 223);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(88, 43);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Google Sans", 10.2F);
            btnSua.Location = new Point(106, 223);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(87, 43);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Google Sans", 10.2F);
            btnXoa.Location = new Point(199, 223);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(86, 43);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 10.2F);
            btnLamMoi.Location = new Point(291, 223);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(108, 43);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTimKiem.Location = new Point(291, 281);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(108, 39);
            btnTimKiem.TabIndex = 10;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaDanhMuc
            // 
            lblMaDanhMuc.Font = new Font("Google Sans", 10.2F);
            lblMaDanhMuc.Location = new Point(8, 25);
            lblMaDanhMuc.Name = "lblMaDanhMuc";
            lblMaDanhMuc.Size = new Size(140, 25);
            lblMaDanhMuc.TabIndex = 11;
            lblMaDanhMuc.Text = "Mã danh mục:";
            // 
            // lblTenDanhMuc
            // 
            lblTenDanhMuc.Font = new Font("Google Sans", 10.2F);
            lblTenDanhMuc.Location = new Point(8, 133);
            lblTenDanhMuc.Name = "lblTenDanhMuc";
            lblTenDanhMuc.Size = new Size(140, 25);
            lblTenDanhMuc.TabIndex = 12;
            lblTenDanhMuc.Text = "Tên danh mục:";
            // 
            // lblMoTa
            // 
            lblMoTa.Font = new Font("Google Sans", 10.2F);
            lblMoTa.Location = new Point(8, 74);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(140, 25);
            lblMoTa.TabIndex = 13;
            lblMoTa.Text = "Mô tả:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(8, 183);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(140, 25);
            lblGhiChu.TabIndex = 14;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // FrmDanhMucSanPham
            // 
            Controls.Add(dataGridViewDanhMuc);
            Controls.Add(txtMaDanhMuc);
            Controls.Add(txtTenDanhMuc);
            Controls.Add(txtMoTa);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaDanhMuc);
            Controls.Add(lblTenDanhMuc);
            Controls.Add(lblMoTa);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FrmDanhMucSanPham";
            Size = new Size(1283, 323);
            ((System.ComponentModel.ISupportInitialize)dataGridViewDanhMuc).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewDanhMuc;
        private TextBox txtMaDanhMuc;
        private TextBox txtTenDanhMuc;
        private TextBox txtMoTa;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaDanhMuc;
        private Label lblTenDanhMuc;
        private Label lblMoTa;
        private Label lblGhiChu;

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            btnTimKiem_Click(sender, e);
        }
    }
}
