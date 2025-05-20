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
 using Excel2025;
using Oracle.ManagedDataAccess.Client;

namespace ProductionChangeRecord
{
    public partial class TotalViewXtraForm : DevExpress.XtraEditors.XtraForm
    {
        public TotalViewXtraForm()
        {
            InitializeComponent();
        }
        public static string empNo = "";
        public static string mo_no = "";
        public static string model_name = "";
        public static string malieu = "";
        string fullName = "";
        public static string type;
        DataTable dtNew, dataTable = new DataTable();

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
        void loadData(string _Emp, string _model, string _mo,string _malieu)
        {
            resultdataGridView.DataSource = null; // Đặt DataSource về null trước khi gán lại
            if (type == "SMT")
            {
                  
                string sqlNew = $@"  select a.ngay_thang,a.model_no,a.mo_no,a.mo_qty,a.material,a.qty_ton_dau,a.qty_input,a.qty_san_xuat,a.qty_con_lai,a.qty_thuc_te,a.qty_chenh_lech, a.line_sx,
                                        a.op,a.trang_thai,a.nguoi_nhan,a.time_ky_duyet,a.ghi_chu   from tbilly.chip_smt_record a
                                         WHERE  a.mo_no=:mo_no and a.model_no=:model_name   and a.nguoi_nhan like '%'||:emp_no||'%' and a.trang_thai='Waiting for signature' and a.material=:malieu
                                            ORDER BY NGAY_THANG DESC ";
                OracleParameter emp_no_param = new OracleParameter("emp_no", OracleDbType.NVarchar2, _Emp, ParameterDirection.Input);
                OracleParameter malieu_param = new OracleParameter("malieu", OracleDbType.Varchar2, _malieu, ParameterDirection.Input);
                OracleParameter mo_no_param = new OracleParameter("mo_no", OracleDbType.Varchar2, _mo, ParameterDirection.Input);
                OracleParameter model_name_param = new OracleParameter("model_name", OracleDbType.Varchar2, _model, ParameterDirection.Input);


                dataTable = SfisClass.oraQueryDt(sqlNew, mo_no_param, model_name_param,emp_no_param,malieu_param);
                 
                if (dataTable.Rows.Count > 0)
                {
                    resultdataGridView.DataSource = dataTable;
                    if (resultdataGridView.DataSource == dataTable)
                    {

                        resultdataGridView.Columns["MO_NO"].HeaderText = "當製程工單 Công đơn hiện tại";//1
                        resultdataGridView.Columns["QTY_INPUT"].HeaderText = "物料上線數量 Số lượng lĩnh vào chuyền";//3
                        resultdataGridView.Columns["QTY_SAN_XUAT"].HeaderText = "生產后余量 Số lượng đã sản xuất";//4
                        resultdataGridView.Columns["QTY_TON_DAU"].HeaderText = "實際盤點數量 Số lượng tồn đầu";//15
                        resultdataGridView.Columns["QTY_CHENH_LECH"].HeaderText = "差異 Số lượng chênh lệch";//16
                        resultdataGridView.Columns["QTY_CON_LAI"].HeaderText = "剩餘數量 Số lượng thực tế còn lại";//5
                        resultdataGridView.Columns["MODEL_NO"].HeaderText = "料號 Model";//17
                        resultdataGridView.Columns["NGAY_THANG"].HeaderText = "日 期 Ngày tháng"; // 2
                        resultdataGridView.Columns["NGUOI_NHAN"].HeaderText = "確認者 Người xác nhận";//13
                        resultdataGridView.Columns["QTY_THUC_TE"].HeaderText = "剩餘量(理論上 Số lượng thực tế";//7
                        resultdataGridView.Columns["GHI_CHU"].HeaderText = "備註 Ghi chú";//8
                        resultdataGridView.Columns["TRANG_THAI"].HeaderText = "訂單狀態 Trạng thái";//12
                        resultdataGridView.Columns["TIME_KY_DUYET"].HeaderText = "審批時間 Thời gian ký duyệt";//14
                        resultdataGridView.Columns["MATERIAL"].HeaderText = "料號 Mã liệu";//10
                        resultdataGridView.Columns["MO_QTY"].HeaderText = "工單數量 Số lượng công đơn";//11
                        resultdataGridView.Columns["OP"].HeaderText = "轉出人員 Người xuất ra";//6
                        resultdataGridView.Columns["LINE_SX"].HeaderText = "退回單位 Bộ phận trả về";//9

                        // Đảm bảo chế độ tự động điều chỉnh cột là AllCells
                        resultdataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        // Các định dạng khác (giữ nguyên nếu cần)
                        resultdataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                        resultdataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
                        resultdataGridView.BorderStyle = BorderStyle.FixedSingle;
                        resultdataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        resultdataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        resultdataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
                        resultdataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        resultdataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                        resultdataGridView.EnableHeadersVisualStyles = false;
                        resultdataGridView.RowHeadersVisible = false;
                        resultdataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        resultdataGridView.AllowUserToAddRows = false;
                        resultdataGridView.AllowUserToDeleteRows = false;


                        if (resultdataGridView.Columns.Contains("NGAY_THANG"))
                        {
                            resultdataGridView.Columns["NGAY_THANG"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        }
                    }
                }
            }
            else
            {
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
            WHERE  a.mo_no=:mo_no and a.model_no=:model_name and a.trang_thai='Waiting for signature'  
            ORDER BY NGAYTHANG DESC ";

                
                OracleParameter mo_no_param = new OracleParameter("mo_no", OracleDbType.Varchar2, _mo, ParameterDirection.Input);
                OracleParameter model_name_param = new OracleParameter("model_name", OracleDbType.Varchar2, _model, ParameterDirection.Input);


                dtNew = SfisClass.oraQueryDt(sqlNew, mo_no_param, model_name_param);

                if (dtNew.Rows.Count > 0)
                {
                    resultdataGridView.DataSource = dtNew;
                    if (resultdataGridView.DataSource == dtNew)
                    {

                        resultdataGridView.Columns["mo_no"].HeaderText = "當製程工單 Công đơn hiện tại";
                        resultdataGridView.Columns["cong_doan"].HeaderText = "製程段 Công đoạn";
                        resultdataGridView.Columns["mo_cong_doan_truoc"].HeaderText = "前製程工單 Công đơn công đoạn trước";
                        resultdataGridView.Columns["line_out"].HeaderText = "轉出線別 Line xuất";
                        resultdataGridView.Columns["model_no"].HeaderText = "料號 Model bản mạch";
                        resultdataGridView.Columns["mo_qty"].HeaderText = "工單數量 Số lượng công đơn";
                        resultdataGridView.Columns["line_in"].HeaderText = "轉入線別 Line nhập";
                        resultdataGridView.Columns["ngaythang"].HeaderText = "日 期 Ngày tháng"; // Đã sửa lỗi chính tả
                        resultdataGridView.Columns["ca"].HeaderText = "班別 Ca";
                        resultdataGridView.Columns["qty_input"].HeaderText = "轉入數量 Số lượng nhập";
                        resultdataGridView.Columns["qty_back"].HeaderText = "退回數量 Số lượng trả về";
                        resultdataGridView.Columns["bo_phan_tra_ve"].HeaderText = "退回單位 Bộ phận trả về";
                        resultdataGridView.Columns["nguoi_xuat_ra"].HeaderText = "轉出人員 Người xuất ra";
                        resultdataGridView.Columns["nguoi_nhan"].HeaderText = "接收人員 Người nhận";
                        resultdataGridView.Columns["qty_con_lai"].HeaderText = "剩餘數量 Số lượng còn lại";
                        resultdataGridView.Columns["ghi_chu"].HeaderText = "備註 Ghi chú";
                        resultdataGridView.Columns["TRANG_THAI"].HeaderText = "訂單狀態 Trạng thái";
                        resultdataGridView.Columns["TIME_KY_DUYET"].HeaderText = "審批時間 Thời gian ký duyệt";
                        resultdataGridView.Columns["emp_phe_duyet"].HeaderText = "確認者 Người phê duyệt";


                        // Đảm bảo chế độ tự động điều chỉnh cột là AllCells
                        resultdataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        // Các định dạng khác (giữ nguyên nếu cần)
                        resultdataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                        resultdataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
                        resultdataGridView.BorderStyle = BorderStyle.FixedSingle;
                        resultdataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        resultdataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        resultdataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
                        resultdataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        resultdataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9, FontStyle.Bold);
                        resultdataGridView.EnableHeadersVisualStyles = false;
                        resultdataGridView.RowHeadersVisible = false;
                        resultdataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        resultdataGridView.AllowUserToAddRows = false;
                        resultdataGridView.AllowUserToDeleteRows = false;


                        if (resultdataGridView.Columns.Contains("ngaythang"))
                        {
                            resultdataGridView.Columns["ngaythang"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        }


                    }
                }
            }
        }
        private void TotalViewXtraForm_Load(object sender, EventArgs e)
        {
            msglabel.Text = "";
            loadData(empNo, model_name, mo_no,malieu);
           
        }

        private void signButton_Click(object sender, EventArgs e)
        {
             if(type == "SMT")
            {
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string mo = dataTable.Rows[i]["mo_no"].ToString();
                        string model = dataTable.Rows[i]["model_no"].ToString();
                        string malieu = dataTable.Rows[i]["material"].ToString();

                        fullName = GetEmployeeName(empNo);

                        string sql = $@"UPDATE  tbilly.chip_smt_record  SET trang_thai = 'Signed' ,time_ky_duyet = SYSDATE WHERE model_no = :model_name and mo_no=:mo_no and nguoi_nhan like '%'||:emp_no||'%'  and trang_thai='Waiting for signature' ";

                        OracleParameter model_name_param = new OracleParameter("model_name", OracleDbType.Varchar2, model, ParameterDirection.Input);
                        OracleParameter emp_no_param = new OracleParameter("emp_no", OracleDbType.NVarchar2, fullName, ParameterDirection.Input);
                        OracleParameter malieu_param = new OracleParameter("malieu", OracleDbType.Varchar2, malieu, ParameterDirection.Input);
                        OracleParameter mo_param = new OracleParameter("mo_no", OracleDbType.Varchar2, mo, ParameterDirection.Input);
                        int result = SfisClass.oraExcute(sql, emp_no_param, model_name_param, mo_param, malieu_param);
                        if (result > 0)
                        {
                            SfisClass.displayMsg(msglabel, "簽名成功 Đã ký thành công", true);

                        }
                         
                    }


                }
            }
            else
            {
                if (dtNew.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNew.Rows.Count; i++)
                    {
                        string mo = dtNew.Rows[i]["mo_no"].ToString();
                        string model = dtNew.Rows[i]["model_no"].ToString();
                        fullName = GetEmployeeName(empNo);

                        string sql = $@"UPDATE tbilly.ProductionChangeRecord SET trang_thai = 'Signed', EMP_PHE_DUYET =:EM_PHE_DUYET ,time_ky_duyet = SYSDATE WHERE model_no = :model_name and mo_no=:mo_no ";

                        OracleParameter model_name_param = new OracleParameter("model_name", OracleDbType.Varchar2, model, ParameterDirection.Input);
                        OracleParameter emp_phe_duyet_param = new OracleParameter("EM_PHE_DUYET", OracleDbType.NVarchar2, fullName, ParameterDirection.Input);
                        OracleParameter mo_param = new OracleParameter("mo_no", OracleDbType.Varchar2, mo, ParameterDirection.Input);
                        int result = SfisClass.oraExcute(sql, emp_phe_duyet_param, model_name_param, mo_param);
                        if (result > 0)
                        {
                            SfisClass.displayMsg(msglabel, "簽名成功 Đã ký thành công", true);

                        }
                        BieuMauForm bieuMauForm = new BieuMauForm();
                        bieuMauForm.loadSign();

                    }


                }
            }
               
             loadData(empNo, model_name, mo_no,null);

        }

        private void excelsimpleButton_Click(object sender, EventArgs e)
        {
            msglabel.Text = "";
            if (dtNew.Rows.Count > 0 || dataTable.Rows.Count>0)
            {
              Excel2025.ExcelExporter.ExportDataGridViewToExcel(resultdataGridView);
                 
                SfisClass.displayMsg(msglabel, "數據匯出成功 Xuất dữ liệu thành công", true);
            }
            else
            {
                 
                SfisClass.displayMsg(msglabel, "沒有可匯出的數據  Không có dữ liệu để xuất", false);
            }
        }
    }
}