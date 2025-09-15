using BLL;
using GUI;
using System;
using System.Windows.Forms;

namespace QuanLyBanMayTinh_GUI.Staff
{
    public partial class frmTrangChu6 : Form
    {
        private readonly string _tenTaiKhoan;
        private readonly TaiKhoanBLL _taiKhoanBLL;

        public frmTrangChu6(string tenTaiKhoan)
        {
            InitializeComponent();
            _tenTaiKhoan = tenTaiKhoan;
            _taiKhoanBLL = new TaiKhoanBLL();
            LoadUserInfo();
            welcome1.BringToFront();
        }

        private async void LoadUserInfo()
        {
            try
            {
                var taiKhoan = await _taiKhoanBLL.GetTaiKhoanByIdAsync(_tenTaiKhoan);
                lblLoiChao.Text = $"{taiKhoan.TenTaiKhoan} ({taiKhoan.VaiTro})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            btnLogout = new Button();
            label1 = new Label();
            lblLoiChao = new Label();
            buttonCircle1 = new CD233929.ButtonCircle();
            buttonCircle2 = new CD233929.ButtonCircle();
            welcome1 = new QuanLyBanMayTinh_GUI.Welcome.Welcome();
            frmTaiKhoan2 = new GUI.frmTaiKhoan();
            frmNhanVien2 = new GUI.frmNhanVien();
            SuspendLayout();
            // 
            // btnLogout
            // 
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Font = new Font("Google Sans", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogout.Location = new Point(1083, 13);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(105, 36);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Đăng xuất";
            btnLogout.Click += btnLogout_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.Hand;
            label1.Font = new Font("MS Reference Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(1207, 15);
            label1.Name = "label1";
            label1.Size = new Size(29, 28);
            label1.TabIndex = 2;
            label1.Text = "X";
            label1.Click += label1_Click;
            // 
            // lblLoiChao
            // 
            lblLoiChao.AutoSize = true;
            lblLoiChao.Font = new Font("Google Sans", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLoiChao.Location = new Point(12, 13);
            lblLoiChao.Name = "lblLoiChao";
            lblLoiChao.Size = new Size(58, 23);
            lblLoiChao.TabIndex = 3;
            lblLoiChao.Text = "label2";
            // 
            // buttonCircle1
            // 
            buttonCircle1.BackColor = Color.FromArgb(50, 48, 48);
            buttonCircle1.BorderColor = Color.Transparent;
            buttonCircle1.BorderRadius = 30;
            buttonCircle1.DisabledTextColor = Color.Empty;
            buttonCircle1.FlatAppearance.BorderSize = 0;
            buttonCircle1.FlatStyle = FlatStyle.Flat;
            buttonCircle1.Font = new Font("Google Sans", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCircle1.ForeColor = Color.White;
            buttonCircle1.Location = new Point(375, 21);
            buttonCircle1.Margin = new Padding(0);
            buttonCircle1.Name = "buttonCircle1";
            buttonCircle1.Size = new Size(221, 56);
            buttonCircle1.TabIndex = 6;
            buttonCircle1.Text = "Quản lý nhân viên";
            buttonCircle1.UseVisualStyleBackColor = false;
            buttonCircle1.Click += buttonCircle1_Click;
            // 
            // buttonCircle2
            // 
            buttonCircle2.BackColor = Color.FromArgb(50, 48, 48);
            buttonCircle2.BorderColor = Color.Transparent;
            buttonCircle2.BorderRadius = 30;
            buttonCircle2.DisabledTextColor = Color.Empty;
            buttonCircle2.FlatAppearance.BorderSize = 0;
            buttonCircle2.FlatStyle = FlatStyle.Flat;
            buttonCircle2.Font = new Font("Google Sans", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCircle2.ForeColor = Color.White;
            buttonCircle2.Location = new Point(664, 21);
            buttonCircle2.Margin = new Padding(0);
            buttonCircle2.Name = "buttonCircle2";
            buttonCircle2.Size = new Size(221, 56);
            buttonCircle2.TabIndex = 7;
            buttonCircle2.Text = "Quản lý tài khoản";
            buttonCircle2.UseVisualStyleBackColor = false;
            buttonCircle2.Click += buttonCircle2_Click;
            // 
            // welcome1
            // 
            welcome1.Location = new Point(12, 99);
            welcome1.Name = "welcome1";
            welcome1.Size = new Size(1127, 649);
            welcome1.TabIndex = 8;
            // 
            // frmTaiKhoan2
            // 
            frmTaiKhoan2.Font = new Font("Times New Roman", 12F);
            frmTaiKhoan2.Location = new Point(117, 77);
            frmTaiKhoan2.Name = "frmTaiKhoan2";
            frmTaiKhoan2.Size = new Size(1022, 671);
            frmTaiKhoan2.TabIndex = 9;
            // 
            // frmNhanVien2
            // 
            frmNhanVien2.Font = new Font("Google Sans", 10.8F);
            frmNhanVien2.Location = new Point(117, 80);
            frmNhanVien2.Name = "frmNhanVien2";
            frmNhanVien2.Size = new Size(1022, 668);
            frmNhanVien2.TabIndex = 10;
            // 
            // frmTrangChu6
            // 
            BackColor = SystemColors.Window;
            ClientSize = new Size(1261, 760);
            Controls.Add(buttonCircle2);
            Controls.Add(buttonCircle1);
            Controls.Add(lblLoiChao);
            Controls.Add(label1);
            Controls.Add(btnLogout);
            Controls.Add(frmNhanVien2);
            Controls.Add(welcome1);
            Controls.Add(frmTaiKhoan2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmTrangChu6";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Trang chủ";
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            frmDangNhap loginForm = new frmDangNhap();
            loginForm.Show();
            Close();
        }
        private Button btnLogout;
        private Label label1;

        private void label1_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn thoát chương trình?", "Confirm",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private Label lblLoiChao;
        private frmTaiKhoan frmTaiKhoan1;
        private frmNhanVien frmNhanVien1;
        private CD233929.ButtonCircle buttonCircle1;
        private CD233929.ButtonCircle buttonCircle2;

        private void buttonCircle1_Click(object sender, EventArgs e)
        {
            frmNhanVien2.BringToFront();
        }

        private void buttonCircle2_Click(object sender, EventArgs e)
        {
            frmTaiKhoan2.BringToFront();
        }
        private Welcome.Welcome welcome1;

        private void frmNhanVien1_Load(object sender, EventArgs e)
        {

        }
        private GUI.frmTaiKhoan frmTaiKhoan2;
        private GUI.frmNhanVien frmNhanVien2;
    }
}
