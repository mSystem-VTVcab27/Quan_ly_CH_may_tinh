using BLL;
using QuanLyBanMayTinh_GUI;
using QuanLyBanMayTinh_GUI.Admin;
using QuanLyBanMayTinh_GUI.Staff;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmDangNhap : Form
    {
        private readonly TaiKhoanBLL _taiKhoanBLL;

        public frmDangNhap()
        {
            InitializeComponent();
            _taiKhoanBLL = new TaiKhoanBLL();
        }

        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenTaiKhoan = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text.Trim();

            try
            {
                bool isAuthenticated = await _taiKhoanBLL.AuthenticateAsync(tenTaiKhoan, matKhau);
                if (isAuthenticated)
                {
                    // Lấy thông tin tài khoản để kiểm tra vai trò
                    var taiKhoan = await _taiKhoanBLL.GetTaiKhoanByIdAsync(tenTaiKhoan);
                    if (taiKhoan == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Chuyển hướng dựa trên vai trò
                    if (taiKhoan.VaiTro.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        frmTrangChu5 trangChu1 = new frmTrangChu5(tenTaiKhoan);
                        trangChu1.Show();
                    }
                    else if (taiKhoan.VaiTro.Equals("Staff", StringComparison.OrdinalIgnoreCase))

                    {
                        frmTrangChu6 trangChu2 = new frmTrangChu6(tenTaiKhoan);
                        trangChu2.Show();
                    }
                    else
                    {
                        MessageBox.Show("Vai trò không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            lblUsername = new Label();
            lblPassword = new Label();
            btnDangNhap = new CD233929.ButtonCircle();
            panel1 = new Panel();
            monthCalendar1 = new MonthCalendar();
            buttonCircle1 = new CD233929.ButtonCircle();
            buttonCircle2 = new CD233929.ButtonCircle();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Cascadia Code SemiBold", 16.2F, FontStyle.Bold);
            txtUsername.Location = new Point(220, 99);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(233, 33);
            txtUsername.TabIndex = 0;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Cascadia Code SemiBold", 16.2F, FontStyle.Bold);
            txtPassword.Location = new Point(220, 194);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(233, 33);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lblUsername
            // 
            lblUsername.Font = new Font("Google Sans", 14.25F);
            lblUsername.Location = new Point(220, 60);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(144, 25);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "Tên tài khoản:";
            // 
            // lblPassword
            // 
            lblPassword.Font = new Font("Google Sans", 14.25F);
            lblPassword.Location = new Point(220, 156);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(144, 25);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu:";
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = Color.LimeGreen;
            btnDangNhap.BorderColor = Color.Transparent;
            btnDangNhap.BorderRadius = 20;
            btnDangNhap.Cursor = Cursors.Hand;
            btnDangNhap.DisabledTextColor = Color.Empty;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Google Sans", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(220, 251);
            btnDangNhap.Margin = new Padding(0);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(152, 40);
            btnDangNhap.TabIndex = 5;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(monthCalendar1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(185, 313);
            panel1.TabIndex = 6;
            // 
            // monthCalendar1
            // 
            monthCalendar1.CalendarDimensions = new Size(1, 2);
            monthCalendar1.Location = new Point(6, 6);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 0;
            monthCalendar1.DateChanged += monthCalendar1_DateChanged;
            // 
            // buttonCircle1
            // 
            buttonCircle1.BackColor = Color.DarkRed;
            buttonCircle1.BorderColor = Color.Transparent;
            buttonCircle1.BorderRadius = 20;
            buttonCircle1.Cursor = Cursors.Hand;
            buttonCircle1.DisabledTextColor = Color.Empty;
            buttonCircle1.FlatAppearance.BorderSize = 0;
            buttonCircle1.FlatStyle = FlatStyle.Flat;
            buttonCircle1.Font = new Font("Google Sans", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCircle1.ForeColor = Color.White;
            buttonCircle1.Location = new Point(506, 92);
            buttonCircle1.Margin = new Padding(0);
            buttonCircle1.Name = "buttonCircle1";
            buttonCircle1.Size = new Size(152, 40);
            buttonCircle1.TabIndex = 8;
            buttonCircle1.Text = "Quên mật khẩu";
            buttonCircle1.UseVisualStyleBackColor = false;
            buttonCircle1.Click += buttonCircle1_Click;
            // 
            // buttonCircle2
            // 
            buttonCircle2.BackColor = Color.Red;
            buttonCircle2.BorderColor = Color.Transparent;
            buttonCircle2.BorderRadius = 20;
            buttonCircle2.Cursor = Cursors.Hand;
            buttonCircle2.DisabledTextColor = Color.Empty;
            buttonCircle2.FlatAppearance.BorderSize = 0;
            buttonCircle2.FlatStyle = FlatStyle.Flat;
            buttonCircle2.Font = new Font("Google Sans", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCircle2.ForeColor = Color.White;
            buttonCircle2.Location = new Point(506, 23);
            buttonCircle2.Margin = new Padding(0);
            buttonCircle2.Name = "buttonCircle2";
            buttonCircle2.Size = new Size(152, 39);
            buttonCircle2.TabIndex = 9;
            buttonCircle2.Text = " Thoát/Exit";
            buttonCircle2.UseVisualStyleBackColor = false;
            buttonCircle2.Click += buttonCircle2_Click;
            // 
            // frmDangNhap
            // 
            ClientSize = new Size(686, 340);
            Controls.Add(buttonCircle2);
            Controls.Add(buttonCircle1);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(panel1);
            Controls.Add(btnDangNhap);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Font = new Font("Google Sans", 14.25F);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "frmDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnDangNhap2;
        private Label lblUsername;
        private Label lblPassword;

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
        private CD233929.ButtonCircle btnDangNhap;
        private Panel panel1;

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn thoát?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
        private CD233929.ButtonCircle buttonCircle1;
        private CD233929.ButtonCircle buttonCircle2;

        private void buttonCircle2_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn thoát?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonCircle1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản trị viên để khôi phục mật khẩu.", "Quên mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }
        private MonthCalendar monthCalendar1;
    }
}