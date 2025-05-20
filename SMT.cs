using DevExpress.XtraEditors;
using System.Windows.Forms;
using SfisClassLibrary;
using Excel2025;
using System.Data;
using System;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
 
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Reflection;
using System.Data.SqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using static DevExpress.Data.Mask.Internal.MaskSettings<T>;

namespace ProductionChangeRecord
{
    public partial class SMT : DevExpress.XtraEditors.XtraUserControl
    {
        public SMT()
        {
            InitializeComponent();
            qtyLyThuyet_ = qtyTonDau_ + qtyInput_ - qtySX_;
            QtyConlaitextBox.Text = qtyLyThuyet_.ToString();
            qtyChenhLech_ = qtyThucTe_ - qtyLyThuyet_;
            QtyChenhLechtextBox.Text = qtyChenhLech_.ToString();
            total_chenhlechtextEdit.ReadOnly = total_conlaitextEdit.ReadOnly = total_inputtextEdit.ReadOnly = total_SXtextEdit.ReadOnly = total_tontextEdit.ReadOnly = total_lythuyettextEdit.ReadOnly = true;
    
        }

        string sql = "";
        DataTable dt = new DataTable();
        DataTable dtLine = new DataTable();

        int qtyTonDau_ = 0;
        int qtyInput_ = 0;
        int qtySX_ = 0;
        int qtyChenhLech_ = 0;
        int qtyLyThuyet_ = 0;
        int qtyThucTe_ = 0;
        public static string empNo = "";

        private void UpdateCalculatedQuantities()
        {
            if (int.TryParse(qtyTon_dautextBox.Text, out int tonDau) &&
                int.TryParse(qty_inputtextBox.Text, out int input) &&
                int.TryParse(qtySXtextBox.Text, out int sx) &&
                int.TryParse(QtyThucTetextBox.Text, out int thucTe))
            {
                qtyTonDau_ = tonDau;
                qtyInput_ = input;
                qtySX_ = sx;
                qtyThucTe_ = thucTe;

                qtyLyThuyet_ = qtyTonDau_ + qtyInput_ - qtySX_;
                QtyConlaitextBox.Text = qtyLyThuyet_.ToString();
                qtyChenhLech_ = qtyThucTe_ - qtyLyThuyet_;
                QtyChenhLechtextBox.Text = qtyChenhLech_.ToString();
            }
            else
            {
                // Xử lý trường hợp không thể chuyển đổi thành số (ví dụ: hiển thị thông báo lỗi)
                QtyConlaitextBox.Text = "";
                QtyChenhLechtextBox.Text = "";
            }
        }
        private DataTable getLine()
        {
            string sql = " select distinct line_name from sfis1.c_line_desc_t  where line_name like 'S%' order by line_name ";
            return SfisClass.oraQueryDt(sql);
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
        void LoadData()
        {
            dtLine = getLine();
            linecomboBox.Items.Clear();

            for (int i = 0; i < dtLine.Rows.Count; i++)
            {
                linecomboBox.Items.Add(dtLine.Rows[i]["line_name"].ToString());

            }

            linecomboBox.SelectedItem = linecomboBox.Items[0];
            optextBox.Text = GetEmployeeName(empNo);


        }

        public void loadSign()
        {

            signdataGridView.DataSource = null;
            string sqlNew = $@"  select a.ngay_thang,a.model_no,a.mo_no,a.mo_qty,a.material,a.qty_ton_dau,a.qty_input,a.qty_san_xuat,a.qty_con_lai,a.qty_thuc_te,a.qty_chenh_lech, a.line_sx,
                                        a.op,a.trang_thai,a.nguoi_nhan,a.time_ky_duyet,a.ghi_chu   from tbilly.chip_smt_record a
                                        WHERE LOWER(a.nguoi_nhan) LIKE '%' || LOWER(:EMP) || '%'  and a.trang_thai  ='Waiting for signature'
                                        ORDER BY ngay_thang DESC ";
            OracleParameter EMP = new OracleParameter("EMP", OracleDbType.NVarchar2, empNo, ParameterDirection.Input);

            DataTable dtSign = SfisClass.oraQueryDt(sqlNew, EMP);

            if (dtSign.Rows.Count > 0)
            {
                signdataGridView.DataSource = dtSign;
                if (signdataGridView.DataSource == dtSign)
                {

                    signdataGridView.Columns["MO_NO"].HeaderText = "當製程工單 Công đơn hiện tại";//1
                    signdataGridView.Columns["QTY_INPUT"].HeaderText = "物料上線數量 Số lượng lĩnh vào chuyền";//3
                    signdataGridView.Columns["QTY_SAN_XUAT"].HeaderText = "生產后余量 Số lượng đã sản xuất";//4
                    signdataGridView.Columns["QTY_TON_DAU"].HeaderText = "實際盤點數量 Số lượng tồn đầu";//15
                    signdataGridView.Columns["QTY_CHENH_LECH"].HeaderText = "差異 Số lượng chênh lệch";//16
                    signdataGridView.Columns["QTY_CON_LAI"].HeaderText = "剩餘數量 Số lượng còn lại";//5
                    signdataGridView.Columns["MODEL_NO"].HeaderText = "料號 Model";//17
                    signdataGridView.Columns["NGAY_THANG"].HeaderText = "日 期 Ngày tháng"; // 2
                    signdataGridView.Columns["NGUOI_NHAN"].HeaderText = "確認者 Người xác nhận";//13
                    signdataGridView.Columns["QTY_THUC_TE"].HeaderText = "剩餘量(理論上 Số lượng thực tế";//7
                    signdataGridView.Columns["GHI_CHU"].HeaderText = "備註 Ghi chú";//8
                    signdataGridView.Columns["TRANG_THAI"].HeaderText = "訂單狀態 Trạng thái";//12
                    signdataGridView.Columns["TIME_KY_DUYET"].HeaderText = "審批時間 Thời gian ký duyệt";//14
                    signdataGridView.Columns["MATERIAL"].HeaderText = "料號 Mã liệu";//10
                    signdataGridView.Columns["MO_QTY"].HeaderText = "工單數量 Số lượng công đơn";//11
                    signdataGridView.Columns["OP"].HeaderText = "轉出人員 Người xuất ra";//6
                    signdataGridView.Columns["LINE_SX"].HeaderText = "退回單位 Bộ phận trả về";//9

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


                    if (signdataGridView.Columns.Contains("NGAY_THANG"))
                    {
                        signdataGridView.Columns["NGAY_THANG"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                }
            }
        }
        public void loadAll(string sqlStr)
        {

            DataTable dtAll = SfisClass.oraQueryDt(sqlStr);
           
            if (dtAll.Rows.Count > 0)
            {
                recorddataGridView.DataSource = null;
                recorddataGridView.DataSource = dtAll;
                if (recorddataGridView.DataSource == dtAll)
                {

                    recorddataGridView.Columns["MO_NO"].HeaderText = "當製程工單 Công đơn hiện tại";//1
                    recorddataGridView.Columns["QTY_INPUT"].HeaderText = "物料上線數量 Số lượng lĩnh vào chuyền";//3
                    recorddataGridView.Columns["QTY_SAN_XUAT"].HeaderText = "生產后余量 Số lượng đã sản xuất";//4
                    recorddataGridView.Columns["QTY_TON_DAU"].HeaderText = "實際盤點數量 Số lượng tồn đầu";//15
                    recorddataGridView.Columns["QTY_CHENH_LECH"].HeaderText = "差異 Số lượng chênh lệch";//16
                    recorddataGridView.Columns["QTY_CON_LAI"].HeaderText = "剩餘數量 Số lượng còn lại";//5
                    recorddataGridView.Columns["MODEL_NO"].HeaderText = "料號 Model";//17
                    recorddataGridView.Columns["NGAY_THANG"].HeaderText = "日 期 Ngày tháng"; // 2
                    recorddataGridView.Columns["NGUOI_NHAN"].HeaderText = "確認者 Người xác nhận";//13
                    recorddataGridView.Columns["QTY_THUC_TE"].HeaderText = "剩餘量(理論上 Số lượng thực tế";//7
                    recorddataGridView.Columns["GHI_CHU"].HeaderText = "備註 Ghi chú";//8
                    recorddataGridView.Columns["TRANG_THAI"].HeaderText = "訂單狀態 Trạng thái";//12
                    recorddataGridView.Columns["TIME_KY_DUYET"].HeaderText = "審批時間 Thời gian ký duyệt";//14
                    recorddataGridView.Columns["MATERIAL"].HeaderText = "料號 Mã liệu";//10
                    recorddataGridView.Columns["MO_QTY"].HeaderText = "工單數量 Số lượng công đơn";//11
                    recorddataGridView.Columns["OP"].HeaderText = "轉出人員 Người xuất ra";//6

                    recorddataGridView.Columns["LINE_SX"].HeaderText = "退回單位 Bộ phận trả về";//9

                    // Đảm bảo chế độ tự động điều chỉnh cột là AllCells
                    recorddataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Các định dạng khác (giữ nguyên nếu cần)
                    recorddataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    recorddataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
                    recorddataGridView.BorderStyle = BorderStyle.FixedSingle;
                    recorddataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    recorddataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    recorddataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
                    recorddataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    recorddataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                    recorddataGridView.EnableHeadersVisualStyles = false;
                    recorddataGridView.RowHeadersVisible = false;
                    recorddataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    recorddataGridView.AllowUserToAddRows = false;
                    recorddataGridView.AllowUserToDeleteRows = false;


                    if (recorddataGridView.Columns.Contains("NGAY_THANG"))
                    {
                        recorddataGridView.Columns["NGAY_THANG"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                }
            }
        }
        void LoadMoBase()
        {
            string sqlMo = $"select a.model_name,a.target_qty from sfism4.r105 a where a.mo_number='{motextBox.Text}'";
            DataTable dtMo = SfisClass.oraQueryDt(sqlMo);
            if (dtMo.Rows.Count > 0)
            {

                moQtytextBox.Text = dtMo.Rows[0]["target_qty"].ToString();

                modeltextBox.Text = dtMo.Rows[0]["model_name"].ToString();

            }
            else
            {

                SfisClass.displayMsg(msglabel, "Công đơn chưa được mở", false);
            }

        }

        private void motextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                total_inputtextEdit.Text = total_SXtextEdit.Text = total_tontextEdit.Text = total_conlaitextEdit.Text = total_lythuyettextEdit.Text = total_chenhlechtextEdit.Text = "0";
                if (string.IsNullOrEmpty(motextBox.Text))
                {
                    SfisClass.displayMsg(msglabel, "Vui lòng nhập thông tin W/O vào ô này.", false);
                    return;
                }
                else
                {
                    string mo = motextBox.Text.Trim();
                    LoadData();
                    string sql1 = $@"  select a.ngay_thang,a.model_no,a.mo_no,a.mo_qty,a.material,a.qty_ton_dau,a.qty_input,a.qty_san_xuat,a.qty_con_lai,a.qty_thuc_te,a.qty_chenh_lech, a.line_sx,
                                        a.op,a.trang_thai,a.nguoi_nhan,a.time_ky_duyet,a.ghi_chu   from tbilly.chip_smt_record a where a.mo_no='{mo}'  
                                        ORDER BY ngay_thang DESC ";
                    loadAll(sql1);
                    string sql = $@" select  a.part_no 
                                                from accservice.dbo.vw_sfis_pcprocessd_all a 
                                                where a.process_no='{mo}' and a.qty_req>0";
                    dt = SfisClass.sqlQueryDtEDI(sql);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            malieucomboBox.Items.Add(dt.Rows[i]["part_no"].ToString());
                        }
                        malieucomboBox.SelectedItem = malieucomboBox.Items[0];
                        LoadMoBase();
                    }
                    else
                    {
                        SfisClass.displayMsg(msglabel, "Không tìm thấy thông tin mã liệu cho W/O này trong hệ thống.", false);
                        return;
                    }

                }
            }
        }

        private void Root_Shown(object sender, EventArgs e)
        {


        }

        private void SMT_Load(object sender, EventArgs e)
        {
            string sqlAll = $@"  select a.ngay_thang,a.model_no,a.mo_no,a.mo_qty,a.material,a.qty_ton_dau,a.qty_input,a.qty_san_xuat,a.qty_con_lai,a.qty_thuc_te,a.qty_chenh_lech, a.line_sx,
                                        a.op,a.trang_thai,a.nguoi_nhan,time_ky_duyet,a.ghi_chu   from tbilly.chip_smt_record a
                                        ORDER BY ngay_thang DESC ";
            loadAll(sqlAll);

            loadSign();
            qtySXtextBox.Text = "0";
            qty_inputtextBox.Text = "0";
            qtyTon_dautextBox.Text = "0";
            QtyThucTetextBox.Text = "0";
            QtyConlaitextBox.Text = "0";
            QtyChenhLechtextBox.Text = "0";

            dtLine = getLine();
            lineSearchcomboBox.Items.Clear();

            for (int i = 0; i < dtLine.Rows.Count; i++)
            {
                lineSearchcomboBox.Items.Add(dtLine.Rows[i]["line_name"].ToString());

            }

            lineSearchcomboBox.SelectedItem = lineSearchcomboBox.Items[0];

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // Lấy giá trị và loại bỏ khoảng trắng thừa
            string moNo = motextBox.Text?.Trim();
            string dateTimeText = dateTimePicker1.Text?.Trim();
            string model = modeltextBox.Text?.Trim();
            string moQty = moQtytextBox.Text?.Trim();
            string material = malieucomboBox.Text?.Trim();
            string qty_tondau = qtyTon_dautextBox.Text?.Trim();
            string qtyInput = qty_inputtextBox.Text?.Trim();
            string qtySX = qtySXtextBox.Text?.Trim();
            string qtyConLai = QtyConlaitextBox.Text?.Trim();
            string qty_thucte = QtyThucTetextBox.Text?.Trim();
            string qty_chenhlech = QtyChenhLechtextBox.Text?.Trim();
            string op = optextBox.Text?.Trim();
            string note = notetextBox.Text?.Trim();
            string trang_thai = "Waiting for signature";
            string nguoiNhan = OpXacNhantextBox.Text?.Trim();
            string lineSX = linecomboBox.Text?.Trim();


            // Kiểm tra các giá trị trong ô textEdit có đầy đủ hay không
            if (string.IsNullOrEmpty(moNo) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(moQty) || string.IsNullOrEmpty(material)
                || string.IsNullOrEmpty(qty_tondau) || string.IsNullOrEmpty(qtyInput) || string.IsNullOrEmpty(qtySX) || string.IsNullOrEmpty(qtyConLai)
                || string.IsNullOrEmpty(qty_chenhlech) || string.IsNullOrEmpty(op) || string.IsNullOrEmpty(lineSX))
            {
                SfisClass.displayMsg(msglabel, "Vui lòng nhập đầy đủ thông tin", false);
                return;
            }
            else
            {
                if (int.TryParse(qty_tondau, out int int_qty_tondau) && int.TryParse(qtyInput, out int int_qtyInput) && int.TryParse(qty_thucte, out int int_qty_thucte) && int.TryParse(qtySX, out int int_qtySX))
                {

                    sql = $@"INSERT INTO tbilly.chip_smt_record (
                                                                        MO_NO,
                                                                        NGAY_THANG,
                                                                        
                                                                        QTY_INPUT,
                                                                        QTY_SAN_XUAT,
                                                                        QTY_CON_LAI,
                                                                        OP,
                                                                        QTY_THUC_TE,
                                                                        GHI_CHU,
                                                                        LINE_SX,
                                                                        MATERIAL,
                                                                        MO_QTY,
                                                                        TRANG_THAI,
                                                                        NGUOI_NHAN,
                                                                      
                                                                        QTY_TON_DAU,
                                                                        QTY_CHENH_LECH,
                                                                        MODEL_NO
                                                                    ) VALUES (
                                                                        :MO_NO,
                                                                        :NGAY_THANG,
                                                                      
                                                                        :QTY_INPUT,
                                                                        :QTY_SAN_XUAT,
                                                                        :QTY_CON_LAI,
                                                                        :OP,
                                                                        :QTY_THUC_TE,
                                                                        :GHI_CHU,
                                                                        :LINE_SX,
                                                                        :MATERIAL,
                                                                        :MO_QTY,
                                                                        :TRANG_THAI,
                                                                        :NGUOI_NHAN,
                                                                       
                                                                        :QTY_TON_DAU,
                                                                        :QTY_CHENH_LECH,
                                                                        :MODEL_NO
                                                                    )";
                    OracleParameter mo_no_param = new OracleParameter("MO_NO", OracleDbType.Varchar2, moNo, ParameterDirection.Input);
                    OracleParameter ngay_thang_param = new OracleParameter("NGAY_THANG", OracleDbType.Date, DateTime.ParseExact(dateTimeText, "dd-MM-yyyy HH:mm", null), ParameterDirection.Input);
                    OracleParameter qtyInput_param = new OracleParameter("QTY_INPUT", OracleDbType.Int32, qtyInput, ParameterDirection.Input);
                    OracleParameter qty_sx_param = new OracleParameter("QTY_SAN_XUAT", OracleDbType.Int32, qtySX, ParameterDirection.Input);
                    OracleParameter qty_ConLai_param = new OracleParameter("QTY_CON_LAI", OracleDbType.Int32, qtyConLai, ParameterDirection.Input);
                    OracleParameter op_param = new OracleParameter("OP", OracleDbType.NVarchar2, op, ParameterDirection.Input);
                    OracleParameter qty_thuc_te_param = new OracleParameter("QTY_THUC_TE", OracleDbType.Int32, qty_thucte, ParameterDirection.Input);
                    OracleParameter ghi_chu_param = new OracleParameter("GHI_CHU", OracleDbType.NVarchar2, note, ParameterDirection.Input);
                    OracleParameter nguoi_nhan_param = new OracleParameter("NGUOI_NHAN", OracleDbType.NVarchar2, nguoiNhan, ParameterDirection.Input);
                    OracleParameter line_sx_param = new OracleParameter("LINE_SX", OracleDbType.NVarchar2, lineSX, ParameterDirection.Input);
                    OracleParameter material_param = new OracleParameter("MATERIAL", OracleDbType.Varchar2, material, ParameterDirection.Input);
                    OracleParameter mo_qty_param = new OracleParameter("MO_QTY", OracleDbType.Int32, moQty, ParameterDirection.Input);
                    OracleParameter trang_thai_param = new OracleParameter("TRANG_THAI", OracleDbType.Varchar2, trang_thai, ParameterDirection.Input);
                    OracleParameter qty_dau_ton_param = new OracleParameter("QTY_TON_DAU", OracleDbType.Int32, qty_tondau, ParameterDirection.Input);
                    OracleParameter qty_chenh_lech_param = new OracleParameter("QTY_CHENH_LECH", OracleDbType.Int32, qty_chenhlech, ParameterDirection.Input);
                    OracleParameter model_no_param = new OracleParameter("MODEL_NO", OracleDbType.Varchar2, model, ParameterDirection.Input);



                    // Thực hiện câu lệnh SQL
                    if (SfisClass.oraExcute(sql, mo_no_param, ngay_thang_param, qtyInput_param, qty_sx_param, qty_ConLai_param, op_param, qty_thuc_te_param, ghi_chu_param,
                        nguoi_nhan_param, line_sx_param, material_param, mo_qty_param, trang_thai_param, qty_dau_ton_param, qty_chenh_lech_param, model_no_param) > 0)
                    {
                        SfisClass.displayMsg(msglabel, "Thêm thành công", true);
                        ClearForm();
                    }
                    else
                    {
                        SfisClass.displayMsg(msglabel, "Không thành công", false);
                    }

                    SMT_Load(sender, e);


                }
                else
                {
                    SfisClass.displayMsg(msglabel, "Số lượng phải là số.", false);
                }
            }
        }
        void ClearForm()
        {
            motextBox.Text = "";
            modeltextBox.Text = "";
            moQtytextBox.Text = "";
            malieucomboBox.Text = "";
            qtyTon_dautextBox.Text = "0";
            qty_inputtextBox.Text = "0";
            qtySXtextBox.Text = "0";
            QtyConlaitextBox.Text = "0";
            QtyThucTetextBox.Text = "0";
            QtyChenhLechtextBox.Text = "0";
            notetextBox.Text = "";
        }

        private void excelButton_Click(object sender, EventArgs e)
        {
            if (recorddataGridView.Rows.Count == 0)
            {
                SfisClass.displayMsg(msglabel, "Không có dữ liệu để xuất ra Excel", false);
                return;
            }
            ExcelExporter.ExportDataGridViewToExcel(recorddataGridView);
        }

        private void qtyTon_dautextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(qtyTon_dautextBox.Text))
            {
                if (!int.TryParse(qtyTon_dautextBox.Text, out qtyTonDau_))
                {
                    SfisClass.displayMsg(msglabel, "Số lượng tồn đầu phải là số.", false);
                    qtyTon_dautextBox.Focus();
                    qtyTon_dautextBox.SelectAll();
                    e.Cancel = true;
                }
                UpdateCalculatedQuantities();
            }
        }

        private void qty_inputtextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(qty_inputtextBox.Text))
            {
                if (!int.TryParse(qty_inputtextBox.Text, out qtyInput_))
                {
                    SfisClass.displayMsg(msglabel, "Số lượng lĩnh vào phải là số.", false);
                    qty_inputtextBox.Focus();
                    qty_inputtextBox.SelectAll();
                    e.Cancel = true;
                }
                UpdateCalculatedQuantities();
            }
        }

        private void qtySXtextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(qtySXtextBox.Text))
            {
                if (!int.TryParse(qtySXtextBox.Text, out qtySX_))
                {
                    SfisClass.displayMsg(msglabel, "Số lượng sản xuất phải là số.", false);
                    qtySXtextBox.Focus();
                    qtySXtextBox.SelectAll();
                    e.Cancel = true;
                }
                UpdateCalculatedQuantities();
            }
        }

        private void QtyThucTetextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(QtyThucTetextBox.Text))
            {
                if (!int.TryParse(QtyThucTetextBox.Text, out qtyThucTe_))
                {
                    SfisClass.displayMsg(msglabel, "Số lượng thực tế phải là số.", false);
                    QtyThucTetextBox.Focus();
                    QtyThucTetextBox.SelectAll();
                    e.Cancel = true;
                }
                UpdateCalculatedQuantities();
            }
        }


        private void recorddataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = recorddataGridView.Rows[e.RowIndex];
                motextBox.Text = row.Cells["MO_NO"].Value.ToString();
                modeltextBox.Text = row.Cells["MODEL_NO"].Value.ToString();
                moQtytextBox.Text = row.Cells["MO_QTY"].Value.ToString();
                malieucomboBox.Text = row.Cells["MATERIAL"].Value.ToString();

                optextBox.Text = row.Cells["OP"].Value.ToString();
                notetextBox.Text = row.Cells["GHI_CHU"].Value.ToString();
                // motextBox tự động enter khi có giá trị
                modeltextBox.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        motextBox_KeyDown(s, args);
                    }
                };

            }
        }

        private void OpXacNhantextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string enteredName = OpXacNhantextBox.Text?.Trim();

            // Kiểm tra nếu tên người nhận đã được nhập
            if (string.IsNullOrEmpty(enteredName))
            {

                SfisClass.displayMsg(msglabel, "Vui lòng nhập tên mã nhân viên người xác nhận.", false);
                OpXacNhantextBox.Focus();
                return; // Thêm return để ngăn các bước tiếp theo thực hiện khi chưa nhập tên
            }

            // Gọi hàm để lấy tên đầy đủ của nhân viên
            string fullName = GetEmployeeName(enteredName);

            // Kiểm tra nếu không tìm thấy nhân viên
            if (string.IsNullOrEmpty(fullName))
            {

                SfisClass.displayMsg(msglabel, "Không tìm thấy nhân viên này.", false);
                OpXacNhantextBox.Text = ""; // Xóa nội dung đã nhập
                OpXacNhantextBox.Focus();
                return; // Thêm return để ngăn việc di chuyển focus tiếp theo
            }
            else
            {
                // Cập nhật TextEdit với tên đầy đủ của nhân viên
                OpXacNhantextBox.Text = fullName;
                notetextBox.Focus();
                notetextBox.SelectAll();
            }
        }

        private void OpXacNhantextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string enteredName = OpXacNhantextBox.Text?.Trim();

                // Kiểm tra nếu tên người nhận đã được nhập
                if (string.IsNullOrEmpty(enteredName))
                {

                    SfisClass.displayMsg(msglabel, "Vui lòng nhập tên mã nhân viên người xác nhận.", false);
                    OpXacNhantextBox.Focus();
                    return; // Thêm return để ngăn các bước tiếp theo thực hiện khi chưa nhập tên
                }

                // Gọi hàm để lấy tên đầy đủ của nhân viên
                string fullName = GetEmployeeName(enteredName);

                // Kiểm tra nếu không tìm thấy nhân viên
                if (string.IsNullOrEmpty(fullName))
                {

                    SfisClass.displayMsg(msglabel, "Không tìm thấy nhân viên này.", false);
                    OpXacNhantextBox.Text = ""; // Xóa nội dung đã nhập
                    OpXacNhantextBox.Focus();
                    return; // Thêm return để ngăn việc di chuyển focus tiếp theo
                }
                else
                {
                    // Cập nhật TextEdit với tên đầy đủ của nhân viên
                    OpXacNhantextBox.Text = fullName;
                    notetextBox.Focus();
                    notetextBox.SelectAll();
                }
            }
        }

        private void signdataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng nào được click không (tránh trường hợp click vào header)

        }

        private void signdataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng được click
                DataGridViewRow row = signdataGridView.Rows[e.RowIndex];

                // Lấy giá trị của cột "mo_no"
                string moNo = row.Cells["mo_no"].Value?.ToString();

                // Lấy giá trị của cột "model_no"
                string modelNo = row.Cells["model_no"].Value?.ToString();
                string malieu = row.Cells["MATERIAL"].Value?.ToString();

                // Bây giờ bạn có thể sử dụng giá trị của moNo và modelNo
                // Ví dụ: hiển thị chúng trong một TextBox hoặc thực hiện hành động khác
                TotalViewXtraForm.mo_no = moNo;
                TotalViewXtraForm.model_name = modelNo;
                TotalViewXtraForm.empNo = empNo;
                TotalViewXtraForm.malieu = malieu;
                TotalViewXtraForm.type = "SMT";
                TotalViewXtraForm totalView = new TotalViewXtraForm();
                totalView.ShowDialog();

                loadSign();
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ClearForm();
            total_inputtextEdit.Text = total_SXtextEdit.Text = total_tontextEdit.Text = total_conlaitextEdit.Text = total_lythuyettextEdit.Text = total_chenhlechtextEdit.Text = "0";
            string sqlAll = $@"  select a.ngay_thang,a.model_no,a.mo_no,a.mo_qty,a.material,a.qty_ton_dau,a.qty_input,a.qty_san_xuat,a.qty_con_lai,a.qty_thuc_te,a.qty_chenh_lech, a.line_sx,
                                        a.op,a.trang_thai,a.nguoi_nhan,time_ky_duyet,a.ghi_chu   from tbilly.chip_smt_record a
                                        ORDER BY ngay_thang DESC ";
            loadAll(sqlAll);
            loadSign();

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            recorddataGridView.DataSource = null;
            string sql2 = $@"   select a.ngay_thang,a.model_no,a.mo_no,a.mo_qty,a.material,a.qty_ton_dau,a.qty_input,a.qty_san_xuat,a.qty_con_lai,a.qty_thuc_te,a.qty_chenh_lech, a.line_sx,
                                    a.op,a.trang_thai,a.nguoi_nhan,time_ky_duyet,a.ghi_chu   from tbilly.chip_smt_record a where a.model_no='{datatextEdit.Text}' and a.material='{lieutextEdit.Text}' and 
                                    a.line_sx='{lineSearchcomboBox.Text}' and  
                                    a.ngay_thang between to_date('{fromdateTimePicker.Text.Trim()}','DD-MM-YYYY HH24:MI') and to_date('{todateTimePicker.Text.Trim()}','DD-MM-YYYY HH24:MI')
                                    ORDER BY ngay_thang DESC  ";

             total_inputtextEdit.Text = total_SXtextEdit.Text = total_tontextEdit.Text = total_conlaitextEdit.Text = total_lythuyettextEdit.Text = total_chenhlechtextEdit.Text = "0";
            loadAll(sql2);
            if(recorddataGridView.Rows.Count >0)
            {
                // tính tổng số lượng các cột
                int totalQtyInput = 0;
                int totalQtySX = 0;
                int totalQtyTonDau = 0;
                int totalQtyConLai = 0;
                int totalQtyThucTe = 0;
                int totalQtyChenhLech = 0;
               
                for (int i = 0; i < recorddataGridView.Rows.Count; i++)
                {
                    totalQtyInput += Convert.ToInt32(recorddataGridView.Rows[i].Cells["QTY_INPUT"].Value);
                    totalQtySX += Convert.ToInt32(recorddataGridView.Rows[i].Cells["QTY_SAN_XUAT"].Value);
                    totalQtyTonDau += Convert.ToInt32(recorddataGridView.Rows[i].Cells["QTY_TON_DAU"].Value);
                    totalQtyConLai += Convert.ToInt32(recorddataGridView.Rows[i].Cells["QTY_CON_LAI"].Value);
                    totalQtyThucTe += Convert.ToInt32(recorddataGridView.Rows[i].Cells["QTY_THUC_TE"].Value);
                    totalQtyChenhLech += Convert.ToInt32(recorddataGridView.Rows[i].Cells["QTY_CHENH_LECH"].Value);
                }
                total_inputtextEdit.Text = totalQtyInput.ToString();
                total_SXtextEdit.Text = totalQtySX.ToString();
                total_tontextEdit.Text = totalQtyTonDau.ToString();
                total_conlaitextEdit.Text = totalQtyConLai.ToString();
                total_lythuyettextEdit.Text = totalQtyThucTe.ToString();
                total_chenhlechtextEdit.Text = totalQtyChenhLech.ToString();

            }
            else
            {
                SfisClass.displayMsg(msglabel, "Tìm thấy thông tin", true);
            }

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}
