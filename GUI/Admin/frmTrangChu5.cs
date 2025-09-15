using GUI;
using QuanLyBanMayTinh_GUI.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanMayTinh_GUI.Admin
{
    public partial class frmTrangChu5 : Form
    {
        public frmTrangChu5(string tenTaiKhoan)
        {
            InitializeComponent();
            welcome1.BringToFront();
            pictureBox1.BringToFront();
            label2.BringToFront();
            label3.BringToFront();
            label4.BringToFront();
            label5.BringToFront();
            label6.BringToFront();
            label7.BringToFront();
            label8.BringToFront();    

        }


        private void button1_Click(object sender, EventArgs e)
        {
            frmNhaCungCap1.BringToFront();
            frmDanhMucSanPham1.BringToFront();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn thoát chương trình?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmSanPham1.BringToFront();
        }

        private void welcome1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmChiTietHoaDonBan1.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmKhachHang1.BringToFront();
            frmHoaDonBan1.BringToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmPhieuNhap1.BringToFront();
            frmChiTietPhieuNhap1.BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmBaoHanh1.BringToFront();
        }

        private void buttonCircle1_Click(object sender, EventArgs e)
        {

        }
    }
}
