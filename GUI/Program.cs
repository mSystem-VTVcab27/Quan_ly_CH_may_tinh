using GUI;

namespace QuanLyMayTinh_GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Chạy form đăng nhập đầu tiên
            Application.Run(new frmDangNhap());
        }
    }
}