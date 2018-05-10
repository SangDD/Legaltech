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
            DaGuiLenCuc = 6
        }
    }
}
