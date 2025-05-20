using DevExpress.XtraEditors;
using Oracle.ManagedDataAccess.Client;
using SfisClassLibrary;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ProductionChangeRecord
{

    public partial class BieuMauForm : DevExpress.XtraEditors.XtraUserControl
    {
       
        public BieuMauForm()
        {
            InitializeComponent();
           
        }
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);


        string sql = "";
        DataTable dt = new DataTable();
        DataTable dtLine = new DataTable();
        string key_part_no = "";
        int target_qty = 0;
         
        public static string empNo = "";
        string fullName = "";


        private DataTable getLine()
        {
            string sql = " select distinct line_name from sfis1.c_line_desc_t " +
                         " order by line_name ";
            return SfisClass.oraQueryDt(sql);
        }

        private void DisplayComboBoxInfo(ComboBoxEdit comboBox1, string data)
        {
            //comboBox1.Properties.Items.Clear();
            if(comboBox1.Properties.Items.Contains(data))
            {
               
            }
            else
            {
                comboBox1.Properties.Items.Add(data);
            }    
           
            //comboBox1.SelectedIndex = 0;  
            comboBox1.Text = data;
        }
        public bool WriteIniData(string Section, string Key, string Value, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                int OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                int OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }
        }
        public static string IniReadValue(string Section, string Key, string filepath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, filepath);
            return temp.ToString();
        }
        private void BieuMauForm_Load(object sender, EventArgs e)
        {
            SfisClass.setDbConnStr();
            Qty_Con_Lai_textEdit.ReadOnly = true;
            shifttextEdit.ReadOnly = true;

            mo_qtytextEdit.ReadOnly = true;
             
             
            sql = $@" SELECT a.mo_no ,
                        a.cong_doan ,
                        a.mo_cong_doan_truoc ,
                        a.line_out ,
                        a.model_no ,
                        a.mo_qty ,
                        a.line_in ,
                        a.ngaythang ,
                        a.ca ,
                        a.qty_input ,
                        a.qty_back ,
                        a.bo_phan_tra_ve ,
                        a.nguoi_xuat_ra ,
                        a.nguoi_nhan ,
                        a.qty_con_lai ,
                        a.ghi_chu ,
                        a.trang_thai,
                        a.TIME_KY_DUYET,
                        a.emp_phe_duyet
                 FROM tbilly.ProductionChangeRecord a
                 ORDER BY NGAYTHANG DESC ";
            LoadTable(sql);
            loadSign();
            motextEdit.Focus();

        }

        void LoadData()
        {
            dtLine = getLine();
            line_inCombobox.Properties.Items.Clear();
            line_outCombobox.Properties.Items.Clear();
            for (int i = 0; i < dtLine.Rows.Count; i++)
            {
                line_inCombobox.Properties.Items.Add(dtLine.Rows[i]["line_name"].ToString());
                line_outCombobox.Properties.Items.Add(dtLine.Rows[i]["line_name"].ToString());
            }

            
            // lấy thông tin thời gian hiện tại , chỉ lấy giờ và phút và gán cho hourtextEdit
            DateTime currentTime = DateTime.Now;

            // lấy thông tin ngày hiện tại và gán cho dateEdit

            dateTime.Text = currentTime.ToString("dd-MM-yyyy HH:mm");
            // kiểm tra thời gian hiện tại, nếu currentTime từ 08:00 đến 20:00 thì gán cho shifttextEdit là ca ngày, ngược lại gán cho shifttextEdit là ca đêm
            if (currentTime.Hour >= 8 && currentTime.Hour < 20)
            {
                shifttextEdit.Text = "Ca ngày";
            }
            else
            {
                shifttextEdit.Text = "Ca đêm";
            }

            nguoi_nhap_textEdit.Text = GetEmployeeName(empNo);

            //nguoi_xuat_textEdit.Text = GetEmployeeName(empNo);

            if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\data.ini"))
            {
                DisplayComboBoxInfo(stationtextEdit, IniReadValue("SFIS", "Section", System.Windows.Forms.Application.StartupPath + "\\data.ini"));
                DisplayComboBoxInfo(line_inCombobox, IniReadValue("SFIS", "LineIN", System.Windows.Forms.Application.StartupPath + "\\data.ini"));
                DisplayComboBoxInfo(line_outCombobox, IniReadValue("SFIS", "LineOUT", System.Windows.Forms.Application.StartupPath + "\\data.ini"));
            }

        }
        private void motextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Chỉ tải dữ liệu khi nhấn Enter
            {
                if (motextEdit.Text == "")
                {
                    SfisClass.displayMsg(msglabel, "Vui lòng nhập công đơn", false);
                    motextEdit.Focus();
                    return;
                }
                else
                {
                    LoadData();
                    string sql2 = $@" SELECT a.mo_no ,
                        a.cong_doan ,
                        a.mo_cong_doan_truoc ,
                        a.line_out ,
                        a.model_no ,
                        a.mo_qty ,
                        a.line_in ,
                        a.ngaythang ,
                        a.ca ,
                        a.qty_input ,
                        a.qty_back ,
                        a.bo_phan_tra_ve ,
                        a.nguoi_xuat_ra ,
                        a.nguoi_nhan ,
                        a.qty_con_lai ,
                        a.ghi_chu ,
                        a.trang_thai,
                        a.TIME_KY_DUYET,
                        a.emp_phe_duyet
                 FROM tbilly.ProductionChangeRecord a
                 WHERE a.mo_no = '{motextEdit.Text}' 
                 ORDER BY NGAYTHANG DESC ";

                    LoadTable(sql2);
                    sql = $"select a.mo_number,a.model_name,a.target_qty from sfism4.r105 a where a.mo_number='{motextEdit.Text}'";
                    dt = SfisClass.oraQueryDt(sql);
                    if (dt.Rows.Count > 0)
                    {
                        motextEdit.Text = dt.Rows[0]["mo_number"].ToString();
                        target_qty = int.Parse(dt.Rows[0]["target_qty"].ToString());
                        mo_qtytextEdit.Text = dt.Rows[0]["target_qty"].ToString();

                        key_part_no = dt.Rows[0]["model_name"].ToString();


                        sql = $"select  part_no from GlobalData.dbo.vw_bomwuse_product where ancestor_no='{key_part_no}' and product_no='{key_part_no}'";

                        dt = SfisClass.sqlQueryDtRMS(sql);
                        if (dt.Rows.Count > 0)
                        {
                            //gán list dt.rows[0]["part_no"] vào ModelCombobox
                            ModelCombobox.Properties.Items.Clear();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ModelCombobox.Properties.Items.Add(dt.Rows[i]["part_no"].ToString());
                            }

                            ModelCombobox.SelectedItem = ModelCombobox.Properties.Items[0];

                        }
                        else
                        {

                            SfisClass.displayMsg(msglabel, "Không tìm thấy dữ liệu trên BOM", false);
                        }
                        qty_back_textEdit.Text = "0";
                        qty_inputtextEdit.Text = "0";
                    }
                    else
                    {

                        SfisClass.displayMsg(msglabel, "Công đơn chưa được mở", false);
                    }
                    stationtextEdit.Focus();

                }
                

            }

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



        public void loadSign()
        {
            string STATUS_ = "Waiting for signature";

            string sqlNew = $@" SELECT a.mo_no ,
                    a.cong_doan ,
                    a.mo_cong_doan_truoc ,
                    a.line_out ,
                    a.model_no ,
                    a.mo_qty ,
                    a.line_in ,
                    a.ngaythang ,
                    a.ca ,
                    a.qty_input ,
                    a.qty_back ,
                    a.bo_phan_tra_ve ,
                    a.nguoi_xuat_ra ,
                    a.nguoi_nhan ,
                    a.qty_con_lai ,
                    a.ghi_chu ,
                    a.trang_thai,
                    a.TIME_KY_DUYET,
                    a.emp_phe_duyet
            FROM tbilly.ProductionChangeRecord a 
            WHERE LOWER(a.emp_phe_duyet) LIKE '%' || LOWER(:EMP) || '%'  and a.trang_thai  LIKE '%' || :STATUS || '%'
            ORDER BY NGAYTHANG DESC ";
            OracleParameter EMP = new OracleParameter("EMP", OracleDbType.NVarchar2, empNo, ParameterDirection.Input);
            OracleParameter STATUS = new OracleParameter("STATUS", OracleDbType.NVarchar2, STATUS_, ParameterDirection.Input);

            DataTable dtNew = SfisClass.oraQueryDt(sqlNew, EMP, STATUS);
            signdataGridView.DataSource = null; // Đặt DataSource thành null trước khi gán lại
            if (dtNew.Rows.Count > 0)
            {
                signdataGridView.DataSource = dtNew;
                if (signdataGridView.DataSource == dtNew)
                {
                     
                    signdataGridView.Columns["mo_no"].HeaderText = "當製程工單 Công đơn hiện tại";
                    signdataGridView.Columns["cong_doan"].HeaderText = "製程段 Công đoạn";
                    signdataGridView.Columns["mo_cong_doan_truoc"].HeaderText = "前製程工單 Công đơn công đoạn trước";
                    signdataGridView.Columns["line_out"].HeaderText = "轉出線別 Line xuất";
                    signdataGridView.Columns["model_no"].HeaderText = "料號 Model bản mạch";
                    signdataGridView.Columns["mo_qty"].HeaderText = "工單數量 Số lượng công đơn";
                    signdataGridView.Columns["line_in"].HeaderText = "轉入線別 Line nhập";
                    signdataGridView.Columns["ngaythang"].HeaderText = "日 期 Ngày tháng"; // Đã sửa lỗi chính tả
                    signdataGridView.Columns["ca"].HeaderText = "班別 Ca";
                    signdataGridView.Columns["qty_input"].HeaderText = "轉入數量 Số lượng nhập";
                    signdataGridView.Columns["qty_back"].HeaderText = "退回數量 Số lượng trả về";
                    signdataGridView.Columns["bo_phan_tra_ve"].HeaderText = "退回單位 Bộ phận trả về";
                    signdataGridView.Columns["nguoi_xuat_ra"].HeaderText = "轉出人員 Người xuất ra";
                    signdataGridView.Columns["nguoi_nhan"].HeaderText = "接收人員 Người nhận";
                    signdataGridView.Columns["qty_con_lai"].HeaderText = "剩餘數量 Số lượng còn lại";
                    signdataGridView.Columns["ghi_chu"].HeaderText = "備註 Ghi chú";
                    signdataGridView.Columns["TRANG_THAI"].HeaderText = "訂單狀態 Trạng thái";
                    signdataGridView.Columns["TIME_KY_DUYET"].HeaderText = "審批時間 Thời gian ký duyệt";
                    signdataGridView.Columns["emp_phe_duyet"].HeaderText = "確認者 Người phê duyệt";

                    // Đảm bảo chế độ tự động điều chỉnh cột là AllCells
                    signdataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Các định dạng khác (giữ nguyên nếu cần)
                    signdataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    signdataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
                    signdataGridView.BorderStyle = BorderStyle.FixedSingle;
                    signdataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    signdataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    signdataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
                    signdataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    signdataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                    signdataGridView.EnableHeadersVisualStyles = false;
                    signdataGridView.RowHeadersVisible = false;
                    signdataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    signdataGridView.AllowUserToAddRows = false;
                    signdataGridView.AllowUserToDeleteRows = false;


                    if (signdataGridView.Columns.Contains("ngaythang"))
                    {
                        signdataGridView.Columns["ngaythang"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }


                }
            }
        }
        void LoadTable(string sqlQuery)
        {
             
           
            dt = SfisClass.oraQueryDt(sqlQuery);

            if (dt.Rows.Count > 0)
            {
                listdataGridView.DataSource=null;
                listdataGridView.DataSource = dt;
                if (listdataGridView.DataSource == dt)
                {
                    listdataGridView.Columns["mo_no"].HeaderText = "當製程工單 Công đơn hiện tại";
                    listdataGridView.Columns["cong_doan"].HeaderText = "製程段 Công đoạn";
                    listdataGridView.Columns["mo_cong_doan_truoc"].HeaderText = "前製程工單 Công đơn công đoạn trước";
                    listdataGridView.Columns["line_out"].HeaderText = "轉出線別 Line xuất";
                    listdataGridView.Columns["model_no"].HeaderText = "料號 Model bản mạch";
                    listdataGridView.Columns["mo_qty"].HeaderText = "工單數量 Số lượng công đơn";
                    listdataGridView.Columns["line_in"].HeaderText = "轉入線別 Line nhập";
                    listdataGridView.Columns["ngaythang"].HeaderText = "日 期 Ngày tháng"; // Đã sửa lỗi chính tả
                    listdataGridView.Columns["ca"].HeaderText = "班別 Ca"; 
                    listdataGridView.Columns["qty_input"].HeaderText = "轉入數量 Số lượng nhập";
                    listdataGridView.Columns["qty_back"].HeaderText = "退回數量 Số lượng trả về";
                    listdataGridView.Columns["bo_phan_tra_ve"].HeaderText = "退回單位 Bộ phận trả về";
                    listdataGridView.Columns["nguoi_xuat_ra"].HeaderText = "轉出人員 Người xuất ra";
                    listdataGridView.Columns["nguoi_nhan"].HeaderText = "接收人員 Người nhận";
                    listdataGridView.Columns["qty_con_lai"].HeaderText = "剩餘數量 Số lượng còn lại";
                    listdataGridView.Columns["ghi_chu"].HeaderText = "備註 Ghi chú";
                    listdataGridView.Columns["TRANG_THAI"].HeaderText = "訂單狀態 Trạng thái";
                    listdataGridView.Columns["TIME_KY_DUYET"].HeaderText = "審批時間 Thời gian ký duyệt";
                    listdataGridView.Columns["emp_phe_duyet"].HeaderText = "確認者 Người phê duyệt";

                    // Đảm bảo chế độ tự động điều chỉnh cột là AllCells
                    listdataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Các định dạng khác (giữ nguyên nếu cần)
                    listdataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    listdataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
                    listdataGridView.BorderStyle = BorderStyle.FixedSingle;
                    listdataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    listdataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    listdataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
                    listdataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    listdataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                    listdataGridView.EnableHeadersVisualStyles = false;
                    listdataGridView.RowHeadersVisible = false;
                    listdataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    listdataGridView.AllowUserToAddRows = false;
                    listdataGridView.AllowUserToDeleteRows = false;


                    if (listdataGridView.Columns.Contains("ngaythang"))
                    {
                        listdataGridView.Columns["ngaythang"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }


                }
            }
        }

        private void addsimpleButton_Click(object sender, EventArgs e)
        {
            // Lấy giá trị và loại bỏ khoảng trắng thừa
            string moNo = motextEdit.Text?.Trim();
            string dateTimeText = dateTime.Text?.Trim();
            string shift = shifttextEdit.Text?.Trim();
            string qtyInputText = qty_inputtextEdit.Text?.Trim();
            string qtyBackText = qty_back_textEdit.Text?.Trim();
            string boPhanTraVe = bo_phan_tra_ve_textEdit.Text?.Trim();
            string nguoiXuat = nguoi_xuat_textEdit.Text?.Trim();
            string nguoiNhap = nguoi_nhap_textEdit.Text?.Trim();
            string station = stationtextEdit.Text?.Trim();
            string modelNo = ModelCombobox.Text?.Trim();
            string moPrevious = mo_previoustextEdit.Text?.Trim();
            string lineIn = line_inCombobox.Text?.Trim();
            string lineOut = line_outCombobox.Text?.Trim();
            string note = notetextBox.Text?.Trim();
            string empPheDuyetValue = "";
            string moQtyText = mo_qtytextEdit.Text?.Trim();

            #region Write GroupName & LineName& Section & StationName  into  data.ini  setting file

            WriteIniData("SFIS", "LineIN", line_inCombobox.Text, Application.StartupPath + "\\data.ini");
            WriteIniData("SFIS", "LineOUT", line_outCombobox.Text, Application.StartupPath + "\\data.ini");
            WriteIniData("SFIS", "Section", stationtextEdit.Text, Application.StartupPath + "\\data.ini");
            
            #endregion

            // Kiểm tra các giá trị trong ô textEdit có đầy đủ hay không
            if (string.IsNullOrEmpty(moNo) || string.IsNullOrEmpty(shift) || string.IsNullOrEmpty(qtyInputText) || string.IsNullOrEmpty(qtyBackText) || string.IsNullOrEmpty(nguoiXuat) || string.IsNullOrEmpty(nguoiNhap) || string.IsNullOrEmpty(modelNo) || string.IsNullOrEmpty(moQtyText))
            {
                SfisClass.displayMsg(msglabel, "Vui lòng nhập đầy đủ thông tin", false);
                return;
            }
            else
            {
                if (int.TryParse(qtyInputText, out int qtyInput) && int.TryParse(moQtyText, out int target_qty))
                {
                    // kiểm tra số lượng nhập vào có lớn hơn số lượng công đơn hay không
                    if (qtyInput > target_qty)
                    {
                        SfisClass.displayMsg(msglabel, "Số lượng nhập vào không được lớn hơn số lượng công đơn", false);
                        return;
                    }
                    else
                    {
                        if (int.TryParse(qtyBackText, out int qtyBack))
                        {
                            string status = "";
                            int qty_con_lai = 0;
                            int totalQtyInput = 0;
                            int totalQtyBack = 0;

                            string sumQtySql = $"SELECT SUM(QTY_INPUT) FROM tbilly.ProductionChangeRecord WHERE MO_NO = '{moNo}' and MODEL_NO='{modelNo}'";
                            dt = SfisClass.oraQueryDt(sumQtySql);
                            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                            {
                                totalQtyInput = Convert.ToInt32(dt.Rows[0][0]);
                            }

                            sumQtySql = $"SELECT SUM(QTY_BACK) FROM tbilly.ProductionChangeRecord WHERE MO_NO = '{moNo}' and MODEL_NO='{modelNo}'";
                            dt = SfisClass.oraQueryDt(sumQtySql);
                            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                            {
                                totalQtyBack = Convert.ToInt32(dt.Rows[0][0]);
                            }

                            // Công thức tính số lượng còn lại đã được điều chỉnh
                            qty_con_lai = target_qty - (totalQtyInput + qtyInput) + (totalQtyBack + qtyBack);

                            if (qty_con_lai < 0)
                            {
                                SfisClass.displayMsg(msglabel, $"Số lượng còn lại sau giao dịch là {qty_con_lai}, vui lòng cân đối lại số lượng nhập và trả về ", false);
                                return;
                            }

                            if (qty_con_lai == 0)
                            {
                                SignXtraForm signXtraForm = new SignXtraForm();
                                signXtraForm.ShowDialog();
                                empPheDuyetValue = signXtraForm.empNo;
                                // hiển thị form mới để nhập thông tin người phê duyệt

                                status = "Waiting for signature";
                                if (empPheDuyetValue == "")
                                {
                                    SfisClass.displayMsg(msglabel, "請輸入審核人 Vui lòng nhập người phê duyệt", false);
                                    return;
                                }
                               
                            }
                            else
                            {
                                status = ""; // Hoặc trạng thái phù hợp khác khi chưa cần ký duyệt

                            }

                            sql = $@"INSERT INTO tbilly.ProductionChangeRecord (
                                                                        MO_NO,
                                                                        NGAYTHANG,
                                                                        CA,
                                                                        QTY_INPUT,
                                                                        QTY_BACK,
                                                                        BO_PHAN_TRA_VE,
                                                                        QTY_CON_LAI,
                                                                        NGUOI_XUAT_RA,
                                                                        NGUOI_NHAN,
                                                                        GHI_CHU,
                                                                        CONG_DOAN,
                                                                        MO_CONG_DOAN_TRUOC,
                                                                        LINE_IN,
                                                                        LINE_OUT,
                                                                        MODEL_NO,
                                                                        MO_QTY,
                                                                        TRANG_THAI,
                                                                        EMP_PHE_DUYET
                                                                    ) VALUES (
                                                                        :MO_NO,
                                                                        :NGAYTHANG,
                                                                        :CA,
                                                                        :QTY_INPUT,
                                                                        :QTY_BACK,
                                                                        :BO_PHAN_TRA_VE,
                                                                        :QTY_CON_LAI,
                                                                        :NGUOI_XUAT_RA,
                                                                        :NGUOI_NHAN,
                                                                        :GHI_CHU,
                                                                        :CONG_DOAN,
                                                                        :MO_CONG_DOAN_TRUOC,
                                                                        :LINE_IN,
                                                                        :LINE_OUT,
                                                                        :MODEL_NO,
                                                                        :MO_QTY,
                                                                        :TRANG_THAI,
                                                                        :EMP_PHE_DUYET
                                                                    )";
                            OracleParameter mo_no = new OracleParameter("MO_NO", OracleDbType.Varchar2, moNo, ParameterDirection.Input);
                            OracleParameter ngaythang = new OracleParameter("NGAYTHANG", OracleDbType.Date, DateTime.ParseExact(dateTimeText, "dd-MM-yyyy HH:mm", null), ParameterDirection.Input);
                            OracleParameter ca_param = new OracleParameter("CA", OracleDbType.NVarchar2, shift, ParameterDirection.Input);
                            OracleParameter qty_input_param = new OracleParameter("QTY_INPUT", OracleDbType.Int32, qtyInput, ParameterDirection.Input);
                            OracleParameter qty_back_param = new OracleParameter("QTY_BACK", OracleDbType.Int32, qtyBack, ParameterDirection.Input);
                            OracleParameter bo_phan_tra_ve_param = new OracleParameter("BO_PHAN_TRA_VE", OracleDbType.NVarchar2, boPhanTraVe, ParameterDirection.Input);
                            OracleParameter qty_con_lai_param = new OracleParameter("QTY_CON_LAI", OracleDbType.Int32, qty_con_lai, ParameterDirection.Input);
                            OracleParameter nguoi_xuat_param = new OracleParameter("NGUOI_XUAT_RA", OracleDbType.NVarchar2, nguoiXuat, ParameterDirection.Input);
                            OracleParameter nguoi_nhap_param = new OracleParameter("NGUOI_NHAN", OracleDbType.NVarchar2, nguoiNhap, ParameterDirection.Input);
                            OracleParameter ghi_chu_param = new OracleParameter("GHI_CHU", OracleDbType.NVarchar2, note, ParameterDirection.Input);
                            OracleParameter cong_doan_param = new OracleParameter("CONG_DOAN", OracleDbType.Varchar2, station, ParameterDirection.Input);
                            OracleParameter mo_cong_doan_truoc_param = new OracleParameter("MO_CONG_DOAN_TRUOC", OracleDbType.Varchar2, moPrevious, ParameterDirection.Input);
                            OracleParameter line_in_param = new OracleParameter("LINE_IN", OracleDbType.Varchar2, lineIn, ParameterDirection.Input);
                            OracleParameter line_out_param = new OracleParameter("LINE_OUT", OracleDbType.Varchar2, lineOut, ParameterDirection.Input);
                            OracleParameter model_no_param = new OracleParameter("MODEL_NO", OracleDbType.Varchar2, modelNo, ParameterDirection.Input);
                            OracleParameter mo_qty_param = new OracleParameter("MO_QTY", OracleDbType.Int32, target_qty, ParameterDirection.Input);
                            OracleParameter trang_thai_param = new OracleParameter("TRANG_THAI", OracleDbType.Varchar2, status, ParameterDirection.Input);
                            OracleParameter emp_phe_duyet_param = new OracleParameter("EMP_PHE_DUYET", OracleDbType.NVarchar2, empPheDuyetValue, ParameterDirection.Input);

                            // Thực hiện câu lệnh SQL
                            if (SfisClass.oraExcute(sql, mo_no, ngaythang, ca_param, qty_input_param, qty_back_param, bo_phan_tra_ve_param, qty_con_lai_param, nguoi_xuat_param, nguoi_nhap_param, ghi_chu_param, cong_doan_param, mo_cong_doan_truoc_param, line_in_param, line_out_param, model_no_param, mo_qty_param, trang_thai_param, emp_phe_duyet_param) > 0)
                            {
                                SfisClass.displayMsg(msglabel, "Thêm thành công", true);
                                ClearForm();
                            }
                            else
                            {
                                SfisClass.displayMsg(msglabel, "Không thành công", false);
                            }
                             BieuMauForm_Load(sender, e);
                        }
                        else
                        {
                            SfisClass.displayMsg(msglabel, "Vui lòng nhập số hợp lệ cho số lượng trả", false);
                        }
                    }
                }
                else
                {
                    SfisClass.displayMsg(msglabel, "Vui lòng nhập số hợp lệ cho số lượng nhập và số lượng công đơn.", false);
                }
            }
        }

        private void ClearForm()
        {
            motextEdit.Text = "";
            dateTime.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            shifttextEdit.Text = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "Ca ngày" : "Ca đêm";
            qty_inputtextEdit.Text = "0";
            qty_back_textEdit.Text = "0";
            Qty_Con_Lai_textEdit.Text = "";
            nguoi_xuat_textEdit.Text = "";
            nguoi_nhap_textEdit.Text = "";
            stationtextEdit.Text = "";
            ModelCombobox.SelectedIndex = -1;
            mo_previoustextEdit.Text = "";
            notetextBox.Text = "";
            bo_phan_tra_ve_textEdit.Text = "";
            mo_qtytextEdit.Text = "";
            shifttextEdit.Text = "";
            dateTime.Text = "";

          
            motextEdit.Focus();
        }
        private void bo_phan_tra_ve_textEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                nguoi_xuat_textEdit.Focus();
                nguoi_xuat_textEdit.SelectAll();
            }
        }

        private void ModelCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModelCombobox.SelectedItem != null)
            {
                // Tính toán và hiển thị QTY_CON_LAI từ tbilly.ProductionChangeRecord
                msglabel.Text = "";
                addsimpleButton.Enabled = true;
                string moNo = motextEdit.Text.Trim();
                string modelNo = ModelCombobox.Text.Trim(); // Lấy Model No từ Combobox

                string sqlQtyConLai = $@"SELECT
                                                                MO_NO,
                                                                MODEL_NO,
                                                                MO_QTY,
                                                                SUM(QTY_INPUT) AS Tong_QTY_INPUT,
                                                                SUM(QTY_BACK) AS Tong_QTY_BACK,
                                                                MO_QTY - SUM(QTY_INPUT) + SUM(QTY_BACK) AS QTY_CON_LAI_last
                                                            FROM
                                                                tbilly.productionchangerecord
                                                            WHERE
                                                                MO_NO = '{moNo}'
                                                                AND MODEL_NO = '{modelNo}'
                                                            GROUP BY
                                                                MO_NO,
                                                                MODEL_NO,
                                                                MO_QTY";

                DataTable dtQtyConLai = SfisClass.oraQueryDt(sqlQtyConLai);
                if (dtQtyConLai.Rows.Count > 0 && dtQtyConLai.Rows[0]["QTY_CON_LAI_last"] != DBNull.Value)
                {
                    Qty_Con_Lai_textEdit.Text = dtQtyConLai.Rows[0]["QTY_CON_LAI_last"].ToString();
                    if (int.Parse(Qty_Con_Lai_textEdit.Text) == 0)
                    {
                        addsimpleButton.Enabled = false;
                        SfisClass.displayMsg(msglabel, $"Công đơn {moNo} và model {modelNo} số lượng bản đã được nhập đủ", false);
                        return;
                    }

                }
                else
                {
                    // Nếu không có bản ghi hoặc không tính được, hiển thị số lượng mục tiêu ban đầu
                    Qty_Con_Lai_textEdit.Text = target_qty.ToString();
                }
            }
        }

        private void Qty_Con_Lai_textEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void qty_inputtextEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void nguoi_xuat_textEdit_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void nguoi_nhap_textEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void nguoi_phe_duyet_textEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void nguoi_xuat_textEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Lấy và loại bỏ khoảng trắng thừa từ tên người xuất ra
            string enteredName = nguoi_xuat_textEdit.Text?.Trim();

            // Kiểm tra nếu tên người xuất ra chưa được nhập
            if (string.IsNullOrEmpty(enteredName))
            {

                SfisClass.displayMsg(msglabel, "Vui lòng nhập tên người xuất ra.", false);
                nguoi_xuat_textEdit.Focus();
                return; // Thêm return để ngăn các bước tiếp theo
            }
            else
            {
                // Gọi hàm để lấy tên đầy đủ của nhân viên
                string fullName = GetEmployeeName(enteredName);

                // Kiểm tra nếu không tìm thấy nhân viên
                if (string.IsNullOrEmpty(fullName))
                {

                    SfisClass.displayMsg(msglabel, "Không tìm thấy nhân viên này.", false);
                    nguoi_xuat_textEdit.Text = ""; // Xóa nội dung đã nhập
                    nguoi_xuat_textEdit.Focus();
                    return; // Thêm return để ngăn việc di chuyển focus
                }
                else
                {
                    // Cập nhật TextEdit với tên đầy đủ của nhân viên
                    nguoi_xuat_textEdit.Text = fullName;
                    nguoi_nhap_textEdit.Focus();
                    nguoi_nhap_textEdit.SelectAll();
                }
            }
        }

        private void nguoi_phe_duyet_textEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {


        }

        private void nguoi_nhap_textEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Lấy và loại bỏ khoảng trắng thừa từ tên người nhận
            string enteredName = nguoi_nhap_textEdit.Text?.Trim();

            // Kiểm tra nếu tên người nhận đã được nhập
            if (string.IsNullOrEmpty(enteredName))
            {

                SfisClass.displayMsg(msglabel, "Vui lòng nhập tên người nhận.", false);
                nguoi_nhap_textEdit.Focus();
                return; // Thêm return để ngăn các bước tiếp theo thực hiện khi chưa nhập tên
            }

            // Gọi hàm để lấy tên đầy đủ của nhân viên
            string fullName = GetEmployeeName(enteredName);

            // Kiểm tra nếu không tìm thấy nhân viên
            if (string.IsNullOrEmpty(fullName))
            {

                SfisClass.displayMsg(msglabel, "Không tìm thấy nhân viên này.", false);
                nguoi_nhap_textEdit.Text = ""; // Xóa nội dung đã nhập
                nguoi_nhap_textEdit.Focus();
                return; // Thêm return để ngăn việc di chuyển focus tiếp theo
            }
            else
            {
                // Cập nhật TextEdit với tên đầy đủ của nhân viên
                nguoi_nhap_textEdit.Text = fullName;
                notetextBox.Focus();
                notetextBox.SelectAll();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (listdataGridView.SelectedRows.Count > 0 && dt.Rows.Count > 0)
            {
                // xuất dữ liệu ra file excel
                Excel2025.ExcelExporter.ExportDataGridViewToExcel(listdataGridView);

            }
        }

        private void motextEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (motextEdit.Text == "")
            {
                SfisClass.displayMsg(msglabel, "Vui lòng nhập công đơn", false);
                motextEdit.Focus();
                return;
            }
            else
            {
                LoadData();
                string sql2 = $@" SELECT a.mo_no ,
                        a.cong_doan ,
                        a.mo_cong_doan_truoc ,
                        a.line_out ,
                        a.model_no ,
                        a.mo_qty ,
                        a.line_in ,
                        a.ngaythang ,
                        a.ca ,
                        a.qty_input ,
                        a.qty_back ,
                        a.bo_phan_tra_ve ,
                        a.nguoi_xuat_ra ,
                        a.nguoi_nhan ,
                        a.qty_con_lai ,
                        a.ghi_chu ,
                        a.trang_thai,
                        a.TIME_KY_DUYET,
                        a.emp_phe_duyet
                 FROM tbilly.ProductionChangeRecord a
                 WHERE a.mo_no = '{motextEdit.Text}' 
                 ORDER BY NGAYTHANG DESC ";

                LoadTable(sql2);
                sql = $"select a.mo_number,a.model_name,a.target_qty from sfism4.r105 a where a.mo_number='{motextEdit.Text}'";
                dt = SfisClass.oraQueryDt(sql);
                if (dt.Rows.Count > 0)
                {
                    motextEdit.Text = dt.Rows[0]["mo_number"].ToString();
                    target_qty = int.Parse(dt.Rows[0]["target_qty"].ToString());
                    mo_qtytextEdit.Text = dt.Rows[0]["target_qty"].ToString();

                    key_part_no = dt.Rows[0]["model_name"].ToString();


                    sql = $"select  part_no from GlobalData.dbo.vw_bomwuse_product where ancestor_no='{key_part_no}' and product_no='{key_part_no}'";

                    DataTable dtModel = SfisClass.sqlQueryDtRMS(sql);
                    if (dtModel.Rows.Count > 0)
                    {
                        //gán list dt.rows[0]["part_no"] vào ModelCombobox
                        ModelCombobox.Properties.Items.Clear();
                        for (int i = 0; i < dtModel.Rows.Count; i++)
                        {
                            ModelCombobox.Properties.Items.Add(dtModel.Rows[i]["part_no"].ToString());
                        }

                        string moNo = motextEdit.Text.Trim();
                        string modelNo = ModelCombobox.Text.Trim(); // Lấy Model No từ Combobox

                        string sqlQtyConLai = $@"SELECT
                                                                MO_NO,
                                                                MODEL_NO,
                                                                MO_QTY,
                                                                SUM(QTY_INPUT) AS Tong_QTY_INPUT,
                                                                SUM(QTY_BACK) AS Tong_QTY_BACK,
                                                                MO_QTY - SUM(QTY_INPUT) + SUM(QTY_BACK) AS QTY_CON_LAI_last
                                                            FROM
                                                                tbilly.productionchangerecord
                                                            WHERE
                                                                MO_NO = '{moNo}'
                                                                AND MODEL_NO = '{modelNo}'
                                                            GROUP BY
                                                                MO_NO,
                                                                MODEL_NO,
                                                                MO_QTY";

                        DataTable dtQtyConLai = SfisClass.oraQueryDt(sqlQtyConLai);
                        if (dtQtyConLai.Rows.Count > 0 && dtQtyConLai.Rows[0]["QTY_CON_LAI_last"] != DBNull.Value)
                        {
                            Qty_Con_Lai_textEdit.Text = dtQtyConLai.Rows[0]["QTY_CON_LAI_last"].ToString();
                            if (int.Parse(Qty_Con_Lai_textEdit.Text) == 0)
                            {
                                addsimpleButton.Enabled = false;
                                SfisClass.displayMsg(msglabel, $"Công đơn {moNo} và model {modelNo} số lượng bản đã được nhập đủ", false);
                                return;
                            }

                        }
                        else
                        {
                            // Nếu không có bản ghi hoặc không tính được, hiển thị số lượng mục tiêu ban đầu
                            Qty_Con_Lai_textEdit.Text = target_qty.ToString();
                        }

                    }
                    else
                    {

                        SfisClass.displayMsg(msglabel, "Không tìm thấy dữ liệu trên BOM", false);
                    }
                    qty_back_textEdit.Text = "0";
                    qty_inputtextEdit.Text = "0";
                }
                else
                {

                    SfisClass.displayMsg(msglabel, "Công đơn chưa được mở", false);
                }
                stationtextEdit.Focus();

            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ClearForm();
            Qty_Con_Lai_textEdit.ReadOnly = true;
            shifttextEdit.ReadOnly = true;

            mo_qtytextEdit.ReadOnly = true;


            sql = $@" SELECT a.mo_no ,
                        a.cong_doan ,
                        a.mo_cong_doan_truoc ,
                        a.line_out ,
                        a.model_no ,
                        a.mo_qty ,
                        a.line_in ,
                        a.ngaythang ,
                        a.ca ,
                        a.qty_input ,
                        a.qty_back ,
                        a.bo_phan_tra_ve ,
                        a.nguoi_xuat_ra ,
                        a.nguoi_nhan ,
                        a.qty_con_lai ,
                        a.ghi_chu ,
                        a.trang_thai,
                        a.TIME_KY_DUYET,
                        a.emp_phe_duyet
                 FROM tbilly.ProductionChangeRecord a
                 ORDER BY NGAYTHANG DESC ";
            LoadTable(sql);
            loadSign();
        }

        private void listdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải là click vào một hàng hợp lệ không (không phải header row)  
            if (e.RowIndex >= 0)
            {
                // hiển thị thông tin lên textEdit  
                DataGridViewRow row = listdataGridView.Rows[e.RowIndex];
                motextEdit.Text = row.Cells["mo_no"].Value.ToString();
                //stationtextEdit.Text = row.Cells["cong_doan"].Value.ToString();
                //line_outCombobox.Text = row.Cells["line_out"].Value.ToString();
                //line_inCombobox.Text = row.Cells["line_in"].Value.ToString();
                ModelCombobox.Text = row.Cells["model_no"].Value.ToString();
                mo_qtytextEdit.Text = row.Cells["mo_qty"].Value.ToString();
                qty_inputtextEdit.Text = row.Cells["qty_input"].Value.ToString();
                qty_back_textEdit.Text = row.Cells["qty_back"].Value.ToString();
                bo_phan_tra_ve_textEdit.Text = row.Cells["bo_phan_tra_ve"].Value.ToString();
                //nguoi_xuat_textEdit.Text = row.Cells["nguoi_xuat_ra"].Value.ToString();
                //nguoi_nhap_textEdit.Text = row.Cells["nguoi_nhan"].Value.ToString();
                 
                // Instead of calling the Validating event directly, call the method that contains the logic  
                motextEdit_Validating(this, new System.ComponentModel.CancelEventArgs());
            }
        }

        private void signdataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng nào được click không (tránh trường hợp click vào header)
            if (e.RowIndex >= 0)
            {
                // Lấy hàng được click
                DataGridViewRow row = signdataGridView.Rows[e.RowIndex];

                // Lấy giá trị của cột "mo_no"
                string moNo = row.Cells["mo_no"].Value?.ToString();

                // Lấy giá trị của cột "model_no"
                string modelNo = row.Cells["model_no"].Value?.ToString();

              
                TotalViewXtraForm.mo_no = moNo;
                TotalViewXtraForm.model_name = modelNo;
                TotalViewXtraForm.empNo = empNo;
                TotalViewXtraForm.type = "OTHER";
                TotalViewXtraForm totalView = new TotalViewXtraForm();
                totalView.ShowDialog();
                 
                loadSign();
                
            }
        }
    }
}