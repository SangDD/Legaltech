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
            DuLieuCu = -2,
            Luu_tam = 1,
            DaGui_ChoPhanLoai_Admin = 2,
            DaGui_ChoPhanLoai = 3,
            DaPhanChoLuatSu = 4,
            LuatSuDaConfirm = 5,
            AdminReject = 51,
            ChoKHConfirm = 6,
            KhacHangDaConfirm = 7,
            KhacHangDaTuChoi = 8,
            DaPhanChoNhanVien = 9,
            DaNopDon = 10,
            DaGuiLenCuc = 11, // luật sư đã gửi lên cục
            AdminGuiKetQuaNopDon = 12,
            AdminTuChoiKetQuaNopDon = 13,

            Customer_Review = 14,

            ChapNhan_ThongBaoHinhThuc = 15,
            TuChoi_ThongBaoHinhThuc = 16,


            CongBoDon = 17,
            ChapNhan_ThongBaoNoiDung = 18,
            TuChoi_ThongBaoNoiDung = 19,
            ThongBaoCapBang = 20,
            CongBoBang = 21
        }

        public enum Notice_Accept_Status
        {
            Cuc_TraLoi = 0,                             // khi có kết quả thì mặc định trạng thái này luôn, vì có TH admin reject hoặc phân cho luật sư khác
            LuatSu_GuiChoAdminDuyet = 1,                // Ls update kết quả thông báo chờ admin duyệt
            Admin_DuyetGuiChoKhachHang = 2,             // admin duyệt va gửi cho khách hàng
            Admin_TuchoiDuyet = 3,                      // admin từ chối duyệt và gửi cho luật sư
            KhachHang_Review_TraLoi = 4                 // khách hàng đã review thì gửi lại cho luật sư
        }

        public enum Notice_Reject_Status
        {
            Cuc_TraLoi = 0,                             // khi có kết quả thì mặc định trạng thái này luôn, vì có TH admin reject hoặc phân cho luật sư khác
            LuatSu_GuiChoAdminDuyet = 1,                // Ls update kết quả thông báo chờ admin duyệt
            Admin_DuyetGuiChoKhachHang = 2,             // admin duyệt va gửi cho khách hàng
            Admin_TuchoiDuyet = 3,                      // admin từ chối duyệt và gửi cho luật sư
            KhachHang_Review_TraLoi = 4,                // khách hàng đã review thì gửi lại cho luật sư
            LuatSu_DichTraLoiCuc = 5,                   // luật sư dịch xong thì gửi cho admin
            Admin_Duyet_Dich = 6,                       // admin duyệt thì gửi cho luật sư -> Luật sư nộp lên cục
            Admin_TuChoi_Dich = 7,                      // admin duyệt thì gửi cho luật sư 
            LuatSu_Update_Deadline = 8,                 // Luật sư nộp lên cục và update deadline mới 
            LuatSu_Update_KetQua = 9                    // Luật sư nộp lên cục và update deadline mới 
            // nếu thành công thì nhảy lên trạng thái  ChapNhan_ThongBaoHinhThuc/ChapNhan_ThongBaoNoiDung(App_Status) 
            // nếu lỗi thì nhảy lên trạng thái LuatSu_GuiChoAdminDuyet                                       
        }

        public enum Notice_Type
        {
            HinhThuc = 1,
            CongBo_Don = 2,
            NoiDung = 3,
            ThongBao_Cap_Bang = 4,
            CongBo_Bang = 5
        }

        public enum Notice_Result
        {
            ChapNhan = 1,
            TuChoi = 2
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

        public enum Document_Type_Enum
        {
            BanGoc = 1,
            BanSao = 2,
            Khac = 3
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
            Approved = 2,
            Reject = 3
        }

        public enum Billing_Pay_Status
        {
            Payment = 1,
            Paid = 2
        }

        public enum Billing_Type
        {
            App = 1,
            Search = 2,
            Question = 3
        }

        public enum Billing_Detail_Type
        {
            App = 1,
            TimeSheet = 2,
            Service = 3,
            Others = 4
            //Foeign = 5
        }

        public enum Billing_Insert_Type
        {
            App = 0,
            Search = 1,
            Advise_Filling = 2,
            Accept_Form = 3,
            Public_Form = 4,                // công bố đơn
            Accept_Content = 5,
            Grant_Accept = 6,               // thông báo cấp bằng
            Grant_Public = 7                // công bố bằng
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
            Billing = 5,
            Wiki = 6,
            Register = 7
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
        public static string AppCode_A01 = "A01";
        public static string AppCode_A03_IndustryDesign = "A03";

    }

    public class CommonWiki
    {
        public const int Stt_luutam = 1;
        public const int Stt_guibai = 2;
        public const int Stt_daduyet = 3;
        public const int Stt_tuchoi = 4;
    }

    public enum Remind_Type_Enum
    {
        REMIND_TYPE_APP = 1,
        REMIND_TYPE_DOC = 2,
        REMIND_TYPE_BILL = 3
    }

    public enum Remind_Status_Enum
    {
        New = 0,
        Active = 1,
        Process = 2
    }

    public class Search_Object_Enum
    {
        public const int Trademark = 1;
        public const int Patent = 2;
        public const int Legal_Inquiries = 3;
        public const int IndusDesign = 4;
    }

    public class Search_Status_Enum
    {
        public const int Trademark = 1;
        public const int Patent = 2;
        public const int Legal_Inquiries = 3;
    }

    public class B_Todo
    {
        public const string TypeRequest = "ORDER";
        public const string TypeProcess = "TODO";
    }

    public class CommonSearch
    {
        public const int Stt_PhanChoLuatSu = 1;                 // chờ admin phân loại cho luật sư
        public const int Stt_ChoLuatSuPhanHoi = 2;              // chờ luật sư phản hồi
        public const int Stt_ChoAdminDuyet = 3;                 // chờ admin duyệt
        public const int Stt_DaPhanHoi = 4;                     // duyệt xong thì phản hồi cho khách hàng
        public const int Admin_Reject = 5;                      // admin reject

    }
}
