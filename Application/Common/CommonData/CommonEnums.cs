namespace Common.CommonData
{
    public class CommonEnums
    {
        public enum OrderOptions
        {
            None,
            String,
            Number,
            Date
        }

        public enum FunctionType
        {
            NoRequiredLogin,
            Menu,
            Inner,
            ImmediateSelf
        }

        public enum App_Status
        {
            Luu_tam = 1,
            DaGui_ChoPhanLoai = 2,
            DaPhanChoLuatSu = 3,
            LuatSuDaConfirm = 4,
            ChoKHConfirm = 5,
            KhacHangDaConfirm = 6,
            KhacHangDaTuChoi = 7,
            DaPhanChoNhanVien = 8,
            DaNopDon = 9,
            DaGuiLenCuc = 10
        }

        public enum UserType
        {
            SupperAdmin = 0,
            Admin = 1,
            Lawer = 2,
            Customer = 3,
            Employee = 4
        }

        public enum UserStatus
        {
            New = 0,
            Active = 1,
            Locked = 3
        }

        public enum TimeSheet_Status
        {
            New = 0,
            Approve = 1,
            Reject = 2
        }

        public enum Action_Type
        {
            Accept = 1,
            Reject = 2
        }

        public enum Docking_Type_Enum
        {
            In_Book = 1,
            Out_Book = 2
        }

        public enum Docking_Status
        {
            Wait_Original = 1,
            Wait_Translate = 2,
            Completed = 3
        }

        public enum Billing_Status
        {
            New_Wait_Approve = 1,
            Approved = 2
        }

        public enum Billing_Pay_Status
        {
            Payment = 1,
            Paid = 2
        }

        public enum Billing_Type
        {
            App = 1,
            Search = 2
        }

        public enum Billing_Detail_Type
        {
            App = 1,
            TimeSheet = 2,
            Service = 3,
            Others = 4
            //Foeign = 5
        }

        public enum Operator_Type
        {
            Insert = 1,
            Update = 2,
            Approve = 3,
            View = 4
        }

        public enum Todo_Type
        {
            App = 1,
            Question = 2,
            Search = 3,
            TimeSheet = 4,
            Billing = 5
        }
    }

    public class Report_Enums
    {
        public const string gc_FilterAppHeaderId = "APP_HEADER_ID";
        public const string gc_RPT_NAME = "RPTNAME";
        public const string gc_TITLE_NAME = "TITLENAME";
    }

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

    public class CommonWiki
    {
        public const int Stt_luutam = 1;
        public const int Stt_guibai = 2;
        public const int Stt_daduyet = 3;
        public const int Stt_tuchoi = 4;
    }

    public class B_Todo
    {
        public const string TypeRequest = "ORDER";
        public const string TypeProcess = "TODO";
    }
}
