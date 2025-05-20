using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ProductionChangeRecord
{
    public partial class Main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public Main()
        {
            InitializeComponent();
        }
        string sql = "";
        DataTable dt = new DataTable();
        public static string userName;
        public static string FullName;
        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
           
        }
        private void ShowUserControl<T>() where T : UserControl, new()
        {
            // Kiểm tra xem UserControl của kiểu T đã được hiển thị chưa
            foreach (Control control in this.Controls)
            {
                if (control is T)
                {
                    control.BringToFront(); // Nếu đã hiển thị, đưa nó lên trước
                    return;
                }
            }

            // Nếu chưa hiển thị, tạo một instance mới và thêm vào form
            T newUserControl = new T();
            newUserControl.Dock = DockStyle.Fill; // Để UserControl lấp đầy vùng nội dung
            this.Controls.Add(newUserControl);
            newUserControl.BringToFront(); // Đảm bảo nó hiển thị trên cùng
        }

        private void Main_Load(object sender, EventArgs e)
        {
            userLabel.Text = FullName;
            string filePath = Assembly.GetExecutingAssembly().Location;
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

            this.Text = "AVC_Production_Record Version: " + fileVersionInfo.FileVersion;
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
            
        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true; // Hủy bỏ việc đóng form
            }
            else
            {
                e.Cancel = false; // Cho phép đóng form
                // thoát toàn bộ chương trình
                Application.Exit();
                Application.ExitThread();
              
            }
           
        }

        private void accordionControlElement4_Click(object sender, EventArgs e)
        {
            ShowUserControl<BieuMauForm>();
        }

        private void accordionControlElement3_Click_1(object sender, EventArgs e)
        {
            ShowUserControl<SMT>();
        }
    }
}
