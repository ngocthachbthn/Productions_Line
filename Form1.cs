using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SfisClassLibrary;
namespace ProductionChangeRecord
{
    public partial class AVC_Production_Record : Form
    {
        public AVC_Production_Record()
        {
            InitializeComponent();
        }
        string sql = "";
        DataTable dt = null;
        string programName = "AVC_Production_Record";

        private void loginButton_Click(object sender, EventArgs e)
        {

            if (passwordtextEdit.Text.Length > 0 && usertextEdit.Text.Length > 0)
            {
                sql = $" select a.EMP_NO,a.EMP_CNAME  from RMS.dbo.EMP_CURRENT a  WHERE a.EMP_NO='{usertextEdit.Text.ToUpper().Trim()}' and PASS_WORD='{passwordtextEdit.Text.Trim()}'";

                dt = SfisClass.sqlQueryDtRMS(sql);
                if (dt.Rows.Count > 0)
                {

                    SfisClass.displayMsg(msglabel, "Đăng nhập thành công ---> Tiếp tục tới giao diện dành cho người dùng", true);
                    string userName = dt.Rows[0]["EMP_NO"].ToString();
                    string FullName = dt.Rows[0]["EMP_CNAME"].ToString();

                    Main.FullName = FullName;
                    Main.userName = userName;
                    //BieuMauForm bieuMauForm = new BieuMauForm();
                    BieuMauForm.empNo = userName;
                    SMT.empNo = userName;

                    //bieuMauForm.Show(); // Hiển thị form con

                    Main mainForm = new Main();
                    mainForm.Show(); // Hiển thị form chính (MDI parent)

                    this.Hide();

                }
                else
                {
                    SfisClass.displayMsg(msglabel, "Đăng nhập không thành công ---> Kiểm tra lại tài khoản hoặc mật khẩu", false);
                    usertextEdit.Focus();
                    usertextEdit.SelectAll();
                }

            }
            else
            {
                SfisClass.displayMsg(msglabel, "Vui lòng nhập đầy đủ thông tin", false);
                usertextEdit.Focus();
                usertextEdit.SelectAll();

            }

        }

        private void usertextEdit_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (usertextEdit.Text.Length > 0)
                {
                    passwordtextEdit.Focus();
                    passwordtextEdit.SelectAll();
                }
                else
                {
                    SfisClass.displayMsg(msglabel, "Vui lòng nhập tài khoản", false); //avc_mfg\SFIS Programs\AVC_Production_Record
                    usertextEdit.Focus();
                    usertextEdit.SelectAll();
                }

            }
        }

        private void passwordtextEdit_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (passwordtextEdit.Text.Length > 0)
                {
                    loginButton_Click(sender, e);
                }
                else
                {
                    SfisClass.displayMsg(msglabel, "Vui lòng nhập mật khẩu", false);
                    passwordtextEdit.Focus();
                    passwordtextEdit.SelectAll();
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            usertextEdit.Text = "";
            passwordtextEdit.Text = "";
            usertextEdit.Focus();
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            SfisClass.setDbConnStr();

            AutoUpdater.CheckForUpdates(programName, @"avc_mfg\SFIS Programs\AVC_Production_Record\");
            string filePath = Assembly.GetExecutingAssembly().Location;
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

            this.Text = "AVC_Production_Record Version: " + fileVersionInfo.FileVersion;
        }
    }
}