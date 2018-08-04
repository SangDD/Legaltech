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
    }

    public class Report_Enums
    {
        public const string gc_FilterAppHeaderId = "APP_HEADER_ID";
        public const string gc_RPT_NAME = "RPTNAME";
        public const string gc_TITLE_NAME = "TITLENAME";
    }
}
