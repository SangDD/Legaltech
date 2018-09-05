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
            KhacHangDaTuChoi = 61,
            DaGuiLenCuc = 7
        }

        public enum UserType
        {
            Admin = 1,
            Lawer = 2,
            Customer = 3
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
    }

    public class Report_Enums
    {
        public const string gc_FilterAppHeaderId = "APP_HEADER_ID";
        public const string gc_RPT_NAME = "RPTNAME";
        public const string gc_TITLE_NAME = "TITLENAME";
    }

    public class CommonWiki 
    {
        public const int Stt_luutam = 1;
        public const int Stt_guibai = 2;
        public const int Stt_daduyet = 3;
        public const int Stt_tuchoi = 4;
    }
}
