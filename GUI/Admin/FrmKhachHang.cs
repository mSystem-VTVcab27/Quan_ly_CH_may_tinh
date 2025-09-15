using BLL;
using DAL;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmKhachHang : UserControl
    {
        private readonly KhachHangBLL _khachHangBLL;

        public FrmKhachHang()
        {
            InitializeComponent();
            _khachHangBLL = new KhachHangBLL();
            LoadKhachHangList();
            SetupDataGridViewColumns();
        }

        private async void LoadKhachHangList()
        {
            try
            {
                var khachHangs = await _khachHangBLL.GetAllKhachHangAsync();
                dataGridViewKhachHang.DataSource = khachHangs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            // Tắt tự động tạo cột
            dataGridViewKhachHang.AutoGenerateColumns = false;

            // Xóa các cột cũ nếu có
            dataGridViewKhachHang.Columns.Clear();

            // Thêm và cấu hình từng cột
            dataGridViewKhachHang.Columns.Add("MaKhachHang", "Mã khách hàng");
            dataGridViewKhachHang.Columns["MaKhachHang"].DataPropertyName = "MaKhachHang"; // Gán DataPropertyName để liên kết với dữ liệu
            dataGridViewKhachHang.Columns["MaKhachHang"].Width = 100;

            dataGridViewKhachHang.Columns.Add("HoTen", "Họ tên");
            dataGridViewKhachHang.Columns["HoTen"].DataPropertyName = "HoTen";
            dataGridViewKhachHang.Columns["HoTen"].Width = 150;

            dataGridViewKhachHang.Columns.Add("SoDienThoai", "Số điện thoại");
            dataGridViewKhachHang.Columns["SoDienThoai"].DataPropertyName = "SoDienThoai";
            dataGridViewKhachHang.Columns["SoDienThoai"].Width = 100;

            dataGridViewKhachHang.Columns.Add("DiaChi", "Địa chỉ");
            dataGridViewKhachHang.Columns["DiaChi"].DataPropertyName = "DiaChi";
            dataGridViewKhachHang.Columns["DiaChi"].Width = 200;

            dataGridViewKhachHang.Columns.Add("GhiChu", "Ghi chú");
            dataGridViewKhachHang.Columns["GhiChu"].DataPropertyName = "GhiChu";
            dataGridViewKhachHang.Columns["GhiChu"].Width = 200;
        }

        private void dataGridViewKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện nhấp vào ô (GetCellClick)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewKhachHang.Rows[e.RowIndex];
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var khachHangs = await _khachHangBLL.GetAllKhachHangAsync();

                if (!string.IsNullOrEmpty(keyword))
                {
                    khachHangs = khachHangs.Where(k =>
                        k.MaKhachHang.ToLower().Contains(keyword) ||
                        k.HoTen.ToLower().Contains(keyword)
                    ).ToList();
                }

                dataGridViewKhachHang.DataSource = khachHangs;

                if (khachHangs.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var khachHang = new KhachHang
                {
                    MaKhachHang = txtMaKhachHang.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _khachHangBLL.AddKhachHangAsync(khachHang);
                if (result)
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhachHangList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var khachHang = new KhachHang
                {
                    MaKhachHang = txtMaKhachHang.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    GhiChu = txtGhiChu.Text.Trim()
                };

                bool result = await _khachHangBLL.UpdateKhachHangAsync(khachHang);
                if (result)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhachHangList();
                    ClearInputs();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maKhachHang = txtMaKhachHang.Text.Trim();
                if (string.IsNullOrEmpty(maKhachHang))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {maKhachHang}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool success = await _khachHangBLL.DeleteKhachHangAsync(maKhachHang);
                    if (success)
                    {
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKhachHangList();
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
                MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            LoadKhachHangList();
        }

        private void ClearInputs()
        {
            txtMaKhachHang.Clear();
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtGhiChu.Clear();
        }

        private void dataGridViewKhachHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewKhachHang.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewKhachHang.SelectedRows[0];
                txtMaKhachHang.Text = selectedRow.Cells["MaKhachHang"].Value?.ToString();
                txtHoTen.Text = selectedRow.Cells["HoTen"].Value?.ToString();
                txtSoDienThoai.Text = selectedRow.Cells["SoDienThoai"].Value?.ToString();
                txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value?.ToString();
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void InitializeComponent()
        {
            dataGridViewKhachHang = new DataGridView();
            txtMaKhachHang = new TextBox();
            txtHoTen = new TextBox();
            txtSoDienThoai = new TextBox();
            txtDiaChi = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnTimKiem = new Button();
            lblMaKhachHang = new Label();
            lblHoTen = new Label();
            lblSoDienThoai = new Label();
            lblDiaChi = new Label();
            lblGhiChu = new Label();
            btnXuatExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewKhachHang).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewKhachHang
            // 
            dataGridViewKhachHang.ColumnHeadersHeight = 29;
            dataGridViewKhachHang.Location = new Point(10, 318);
            dataGridViewKhachHang.MultiSelect = false;
            dataGridViewKhachHang.Name = "dataGridViewKhachHang";
            dataGridViewKhachHang.RowHeadersWidth = 51;
            dataGridViewKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewKhachHang.Size = new Size(600, 473);
            dataGridViewKhachHang.TabIndex = 0;
            dataGridViewKhachHang.CellClick += dataGridViewKhachHang_CellClick;
            dataGridViewKhachHang.SelectionChanged += dataGridViewKhachHang_SelectionChanged;
            // 
            // txtMaKhachHang
            // 
            txtMaKhachHang.Font = new Font("Google Sans", 10.2F);
            txtMaKhachHang.Location = new Point(196, 61);
            txtMaKhachHang.Name = "txtMaKhachHang";
            txtMaKhachHang.Size = new Size(261, 29);
            txtMaKhachHang.TabIndex = 1;
            // 
            // txtHoTen
            // 
            txtHoTen.Font = new Font("Google Sans", 10.2F);
            txtHoTen.Location = new Point(196, 111);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(261, 29);
            txtHoTen.TabIndex = 2;
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Font = new Font("Google Sans", 10.2F);
            txtSoDienThoai.Location = new Point(196, 161);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(261, 29);
            txtSoDienThoai.TabIndex = 3;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Font = new Font("Google Sans", 10.2F);
            txtDiaChi.Location = new Point(196, 211);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(261, 29);
            txtDiaChi.TabIndex = 4;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Google Sans", 10.2F);
            txtGhiChu.Location = new Point(196, 261);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(261, 29);
            txtGhiChu.TabIndex = 5;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Google Sans", 10.2F);
            txtTimKiem.Location = new Point(162, 8);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(295, 29);
            txtTimKiem.TabIndex = 6;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Google Sans", 9F);
            btnThem.Location = new Point(493, 45);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(117, 43);
            btnThem.TabIndex = 7;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Google Sans", 9F);
            btnSua.Location = new Point(493, 97);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(117, 43);
            btnSua.TabIndex = 8;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Google Sans", 9F);
            btnXoa.Location = new Point(493, 145);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(117, 43);
            btnXoa.TabIndex = 9;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Google Sans", 9F);
            btnLamMoi.Location = new Point(493, 197);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(117, 43);
            btnLamMoi.TabIndex = 10;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Google Sans", 9F);
            btnTimKiem.Location = new Point(20, 3);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(122, 38);
            btnTimKiem.TabIndex = 11;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // lblMaKhachHang
            // 
            lblMaKhachHang.Font = new Font("Google Sans", 10.2F);
            lblMaKhachHang.Location = new Point(20, 63);
            lblMaKhachHang.Name = "lblMaKhachHang";
            lblMaKhachHang.Size = new Size(136, 25);
            lblMaKhachHang.TabIndex = 12;
            lblMaKhachHang.Text = "Mã khách hàng:";
            // 
            // lblHoTen
            // 
            lblHoTen.Font = new Font("Google Sans", 10.2F);
            lblHoTen.Location = new Point(20, 113);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(100, 25);
            lblHoTen.TabIndex = 13;
            lblHoTen.Text = "Họ tên:";
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.Font = new Font("Google Sans", 10.2F);
            lblSoDienThoai.Location = new Point(20, 163);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(122, 25);
            lblSoDienThoai.TabIndex = 14;
            lblSoDienThoai.Text = "Số điện thoại:";
            // 
            // lblDiaChi
            // 
            lblDiaChi.Font = new Font("Google Sans", 10.2F);
            lblDiaChi.Location = new Point(20, 213);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(100, 25);
            lblDiaChi.TabIndex = 15;
            lblDiaChi.Text = "Địa chỉ:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.Font = new Font("Google Sans", 10.2F);
            lblGhiChu.Location = new Point(20, 263);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(100, 25);
            lblGhiChu.TabIndex = 16;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.Location = new Point(493, 245);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(117, 43);
            btnXuatExcel.TabIndex = 40;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // FrmKhachHang
            // 
            Controls.Add(btnXuatExcel);
            Controls.Add(dataGridViewKhachHang);
            Controls.Add(txtMaKhachHang);
            Controls.Add(txtHoTen);
            Controls.Add(txtSoDienThoai);
            Controls.Add(txtDiaChi);
            Controls.Add(txtGhiChu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnTimKiem);
            Controls.Add(lblMaKhachHang);
            Controls.Add(lblHoTen);
            Controls.Add(lblSoDienThoai);
            Controls.Add(lblDiaChi);
            Controls.Add(lblGhiChu);
            Font = new Font("Google Sans", 9F);
            Name = "FrmKhachHang";
            Size = new Size(618, 794);
            ((System.ComponentModel.ISupportInitialize)dataGridViewKhachHang).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewKhachHang;
        private TextBox txtMaKhachHang;
        private TextBox txtHoTen;
        private TextBox txtSoDienThoai;
        private TextBox txtDiaChi;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnTimKiem;
        private Label lblMaKhachHang;
        private Label lblHoTen;
        private Label lblSoDienThoai;
        private Label lblDiaChi;
        private Label lblGhiChu;

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }
        private Button btnXuatExcel;

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewKhachHang.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!");
                    return;
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("DSKhachHang");
                    for (int i = 0; i < dataGridViewKhachHang.Columns.Count; i++)
                        worksheet.Cells[1, i + 1].Value = dataGridViewKhachHang.Columns[i].HeaderText;

                    for (int i = 0; i < dataGridViewKhachHang.Rows.Count; i++)
                        for (int j = 0; j < dataGridViewKhachHang.Columns.Count; j++)
                            worksheet.Cells[i + 2, j + 1].Value = dataGridViewKhachHang.Rows[i].Cells[j].Value?.ToString();

                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveFileDialog.FileName = "DSKhachHang.xlsx";
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