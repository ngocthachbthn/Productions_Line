using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SfisClassLibrary;
namespace ProductionChangeRecord
{
    public partial class SignXtraForm : DevExpress.XtraEditors.XtraForm
    {
        public SignXtraForm()
        {
            InitializeComponent();
        }
        public string empNo = "";
        string fullName= "";
        private void SignXtraForm_Load(object sender, EventArgs e)
        {

        }
        string GetEmployeeName(string empNo)
        {
            string sql1 = $"SELECT EMP_NO, EMP_CNAME FROM RMS.dbo.EMP_CURRENT WHERE EMP_NO = '{empNo}'";
            DataTable dt1 = SfisClass.sqlQueryDtRMS(sql1);
            if (dt1.Rows.Count > 0)
            {
                return dt1.Rows[0]["EMP_CNAME"].ToString() + " - " + dt1.Rows[0]["EMP_NO"].ToString();
            }
            else
            {

                return "";

            }
        }
        private void emptextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Lấy và loại bỏ khoảng trắng thừa từ tên người phê duyệt
                string enteredName = emptextBox.Text?.Trim();

                // Kiểm tra nếu tên người phê duyệt đã được nhập (sau khi trim)
                if (!string.IsNullOrEmpty(enteredName))
                {
                    // Gọi hàm để lấy tên đầy đủ của nhân viên
                     fullName = GetEmployeeName(enteredName);

                    // Kiểm tra nếu không tìm thấy nhân viên
                    if (string.IsNullOrEmpty(fullName))
                    {

                        SfisClass.displayMsg(msglabel, "Không tìm thấy nhân viên này.", false);
                        emptextBox.Text = ""; // Xóa nội dung đã nhập
                        emptextBox.Focus();
                        return; // Thêm return để ngăn các bước xử lý khác (nếu có)
                    }
                    else
                    {
                        // Cập nhật TextEdit với tên đầy đủ của nhân viên (nếu cần)
                        if (emptextBox.Text != fullName)
                        {
                            emptextBox.Text = fullName;
                            empNo = fullName;
                            this.Close();
                        }

                    }
                }
            }
        }

        private void emptextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SignXtraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult dg = MessageBox.Show("您想退出嗎 Bạn có muốn thoát không?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dg == DialogResult.No)
            //{
            //    e.Cancel = true; // Hủy bỏ việc đóng form
                
            //}
            //else
            //{
            //    empNo ="";
            //}
           
             
        }
    }
}