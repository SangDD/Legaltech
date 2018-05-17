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
            Luu_tam = 0,
            DaGui_ChoPhanLoai = 1,
            DaPhanChoLuatSu = 2,
            LuatSuDaConfirm = 3,
            ChoKHConfirm = 4,
            KhacHangDaConfirm = 5,
            KhacHangDaTuChoi = 51,
            DaGuiLenCuc = 6
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
    }
}
