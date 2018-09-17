using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Common
    {
        public static int RecordOnpage = 10;
        public static decimal Country_VietNam_Id = 234;
        public static string Currency_Type_VND = "VND";
        public static string Currency_Type_USD = "USD";

        public static decimal Tax = 5;
    }

    /// <summary>
    /// Danh sach cac mau don thuoc phan trademark
    /// </summary>
    public class TradeMarkAppCode
    {
        public static string AppCodeSuaDoiDangKy = "TM01SDD";
        public static string AppCodeYeuCauGiahan = "TM08SDQT";
        public static string AppCodeDangKyQuocTeNH = "TM06DKQT";
        public static string AppCodeDangKyChuyenDoi = "TM07DKCD";
        public static string AppCodeTraCuuNhanHieu = "TM03YCTCNH";
        public static string AppCodeDangKynhanHieu = "TM04NH";
        public static string AppCode_TM_3B_PLB_01_SDD = "TM_PLB01SDD";
        public static string AppCode_TM_3C_PLB_02_CGD = "TM_PLB02CGD";
        public static string AppCode_TM_3D_PLC_05_KN = "TM_PLC05KN";

        public static string AppCode_TM_4C2_PLD_01_HDCN = "TM_PLD01HDCN";
    }


    public class ErrorCode
    {
        public static readonly int Error = -33;
        public static readonly int Success = 0;
    }
}
