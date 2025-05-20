using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionChangeRecord
{
    public class DataClass
    {
        public string MO_NO { get; set; }
        public DateTime? NGAYTHANG { get; set; }
        public DateTime? THOI_GIAN { get; set; }
        public string CA { get; set; }
        public int? QTY_INPUT { get; set; }
        public int? QTY_BACK { get; set; }
        public string BO_PHAN_TRA_VE { get; set; }
        public int? QTY_CON_LAI { get; set; }
        public string NGUOI_XUAT_RA { get; set; }
        public string NGUOI_NHAN { get; set; }
        public string GHI_CHU { get; set; }
        public DateTime? TIME_UPDATE { get; set; }
        public string CONG_DOAN { get; set; }
        public string MO_CONG_DOAN_TRUOC { get; set; }
        public string LINE_IN { get; set; }
        public string LINE_OUT { get; set; }
        public string MODEL_NO { get; set; }
        public string EMP { get; set; }
        public string EMP_PHE_DUYET { get; set; }
        public string FULL_NAME { get; set; }
        public int? MO_QTY { get; set; }

        public DataClass()
        {
            // Constructor mặc định
        }

        public DataClass(string mO_NO, DateTime? nGAYTHANG, DateTime? tHOI_GIAN, string cA, int? qTY_INPUT, int? qTY_BACK, string bO_PHAN_TRA_VE, int? qTY_CON_LAI, string nGUOI_XUAT_RA, string nGUOI_NHAN, string gHI_CHU, DateTime? tIME_UPDATE, string cONG_DOAN, string mO_CONG_DOAN_TRUOC, string lINE_IN, string lINE_OUT, string mODEL_NO, int? mO_QTY, string emp_, string full_name, string eMP_PHE_DUYET)
        {
            MO_NO = mO_NO;
            NGAYTHANG = nGAYTHANG;
            THOI_GIAN = tHOI_GIAN;
            CA = cA;
            QTY_INPUT = qTY_INPUT;
            QTY_BACK = qTY_BACK;
            BO_PHAN_TRA_VE = bO_PHAN_TRA_VE;
            QTY_CON_LAI = qTY_CON_LAI;
            NGUOI_XUAT_RA = nGUOI_XUAT_RA;
            NGUOI_NHAN = nGUOI_NHAN;
            GHI_CHU = gHI_CHU;
            TIME_UPDATE = tIME_UPDATE;
            CONG_DOAN = cONG_DOAN;
            MO_CONG_DOAN_TRUOC = mO_CONG_DOAN_TRUOC;
            LINE_IN = lINE_IN;
            LINE_OUT = lINE_OUT;
            MODEL_NO = mODEL_NO;
            MO_QTY = mO_QTY;
            EMP = emp_;
            FULL_NAME = full_name;
            EMP_PHE_DUYET = eMP_PHE_DUYET;
        }
    }
}
