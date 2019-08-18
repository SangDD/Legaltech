using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
  public  class A02_Info : ApplicationHeaderInfo
    {
        public decimal A02_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string TenThietKe { get; set; }
        public DateTime NgayTaoThietKe { get; set; }

        public string KhaiThactm { get; set; }
        public string KhaiThactm_TaiNuoc { get; set; }
        public DateTime KhaiThactm_Ngay { get; set; }

        public decimal SoAnh { get; set; }

        /// <summary>
        /// NHO
        /// LOGIC
        /// OTHER
        /// </summary>
        public string ChucNang { get; set; }

        /// <summary>
        /// Note khi chức năng = OTHER
        /// </summary>
        public string ChucNang_Other { get; set; }

        /// <summary>
        /// LUONGCUC
        /// MOS
        /// BI-MOS
        /// QUANG-DIENTU
        /// OTHER
        /// </summary>
        public string CauTruc { get; set; }

        /// <summary>
        /// note khi cấu trúc = OTHER
        /// </summary>
        public string CauTruc_OTHER { get; set; }

        /// <summary>
        /// TTL
        /// DTL
        /// ECL
        /// ITL
        /// CMOS
        /// NMOS
        /// PMOS
        /// OTHER
        /// </summary>
        public string CongNghe { get; set; }
        /// <summary>
        /// Note khi công nghệ = Other
        /// </summary>
        public string CongNghe_OTHER { get; set; }

        /// <summary>
        /// Mô tả vắn tắt (các đặc điểm phân biệt với các mạch tích hợp bán dẫn khác trên thị trường):*
        /// </summary>
        public string MoTaTomTat { get; set; }
    }

    public class A02_Info_Export : ApplicationHeaderInfo
    {

        public decimal A02_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }

        public string TenThietKe { get; set; }
        public DateTime NgayTaoThietKe { get; set; }

        public string KhaiThactm { get; set; }
        public string KhaiThactm_TaiNuoc { get; set; }
        public DateTime KhaiThactm_Ngay { get; set; }

        /// <summary>
        /// NHO
        /// LOGIC
        /// OTHER
        /// </summary>
        public string ChucNang { get; set; }

        /// <summary>
        /// Note khi chức năng = OTHER
        /// </summary>
        public string ChucNang_Other { get; set; }

        /// <summary>
        /// LUONGCUC
        /// MOS
        /// BI-MOS
        /// QUANG-DIENTU
        /// OTHER
        /// </summary>
        public string CauTruc { get; set; }

        /// <summary>
        /// note khi cấu trúc = OTHER
        /// </summary>
        public string CauTruc_OTHER { get; set; }

        /// <summary>
        /// TTL
        /// DTL
        /// ECL
        /// ITL
        /// CMOS
        /// NMOS
        /// PMOS
        /// OTHER
        /// </summary>
        public string CongNghe { get; set; }
        /// <summary>
        /// Note khi công nghệ = Other
        /// </summary>
        public string CongNghe_OTHER { get; set; }

        /// <summary>
        /// Mô tả vắn tắt (các đặc điểm phân biệt với các mạch tích hợp bán dẫn khác trên thị trường):*
        /// </summary>
        public string MoTaTomTat { get; set; }
        /// <summary>
        /// Số ảnh upload để tính phí
        /// </summary>
        public decimal SoAnh { get; set; }
        

        #region Chủ đơn khác

        public string Master_Name_1 { set; get; }
        public string Master_Address_1 { set; get; }
        public string Master_Phone_1 { set; get; }
        public string Master_Fax_1 { set; get; }
        public string Master_Email_1 { set; get; }

        public string TacGiaDongThoi_1 { set; get; }
        public string PhoBan_1 { set; get; }

        public string Master_Name_2 { set; get; }
        public string Master_Address_2 { set; get; }
        public string Master_Phone_2 { set; get; }
        public string Master_Fax_2 { set; get; }
        public string Master_Email_2 { set; get; }

        public string TacGiaDongThoi_2 { set; get; }
        public string PhoBan_2 { set; get; }

        #endregion

        

        #region Tác giả

        public string Author_Name { set; get; }

        public string Author_Address { set; get; }

        public string Author_Phone { set; get; }

        public string Author_Fax { set; get; }
        public string Author_Email { set; get; }

        public decimal Author_Country { set; get; }
        public string Author_Country_Display { set; get; }

        string _Author_Others;
        public string Author_Others
        {
            get
            {
                return _Author_Others;
            }
            set
            {
                _Author_Others = value;
            }
        }

        #endregion

        #region Tác giả 1

        public string Author_Name_1 { set; get; }

        public string Author_Address_1 { set; get; }

        public string Author_Phone_1 { set; get; }

        public string Author_Fax_1 { set; get; }
        public string Author_Email_1 { set; get; }

        public decimal Author_Country_1 { set; get; }
        public string Author_Country_Display_1 { set; get; }

        #endregion

        #region Tác giả 2

        public string Author_Name_2 { set; get; }

        public string Author_Address_2 { set; get; }

        public string Author_Phone_2 { set; get; }

        public string Author_Fax_2 { set; get; }
        public string Author_Email_2 { set; get; }

        public decimal Author_Country_2 { set; get; }
        public string Author_Country_Display_2 { set; get; }

        #endregion

        #region Phí

        /// <summary>
        /// Lệ phí nộp đơng
        /// </summary>
        public string Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }

        /// <summary>
        /// PHí thẩm định đơn
        /// </summary>
        public string Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }


        /// <summary>
        /// Phí công bố đơn
        /// </summary>
        public string Fee_Id_3 { get; set; }
        public decimal Fee_Id_3_Check { get; set; }
        public string Fee_Id_3_Val { get; set; }

        /// <summary>
        /// Đơn có trên 1 hình (từ hình thứ 2 trở đi)  
        /// </summary>
        public string Fee_Id_31 { get; set; }
        public decimal Fee_Id_31_Check { get; set; }
        public string Fee_Id_31_Val { get; set; }
 
        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        #endregion

        #region các tài liệu trong đơn

        string _strDsFile = "";
        public string strDanhSachFileDinhKem
        { get { return _strDsFile; } set { _strDsFile = value; } }

        /// <summary>
        /// Tờ khai, gồm.......trang   x .......bản
        /// </summary>
        public string Doc_Id_1 { get; set; }
        public string Doc_Id_102 { get; set; }

        public decimal Doc_Id_1_Check { get; set; }

        /// <summary>
        /// Bộ ảnh chụp hoặc bản vẽ TKBT gồm.......trang x .......bộ
        /// </summary>
        public string Doc_Id_2 { get; set; }
        public string Doc_Id_202 { get; set; }
   
        public decimal Doc_Id_2_Check { get; set; }

        /// <summary>
        ///  Bản sao chứng từ nộp phí, lệ phí
        /// </summary>
        public string Doc_Id_3 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }

        /// <summary>
        /// Giấy uỷ quyền bằng tiếng
        /// </summary>
        public string Doc_Id_4 { get; set; }

        public decimal Doc_Id_4_Check { get; set; }
        /// <summary>
        /// bản gốc
        /// </summary>
        public string Doc_Id_5 { get; set; }
        public decimal Doc_Id_5_Check { get; set; }

        /// <summary>
        ///bản sao 
        /// </summary>
        public string Doc_Id_6 { get; set; }
        public decimal Doc_Id_6_Check { get; set; }


        /// <summary>
        /// Bản gốc sẽ nộp sau
        /// </summary>
        public string Doc_Id_7 { get; set; }

        public decimal Doc_Id_7_Check { get; set; }
        /// <summary>
        ///Bản gốc đã nộp theo đơn số
        /// </summary>
        public string Doc_Id_8 { get; set; }

        public decimal Doc_Id_8_Check { get; set; }


        /// <summary>
        /// Bản dịch tiếng việt gồm x trang
        /// </summary>
        public string Doc_Id_9 { get; set; }

        public decimal Doc_Id_9_Check { get; set; }
        /// <summary>
        /// Tài liệu xác nhận quyền đăng ký (nếu thụ hưởng từ người khác)
        /// </summary>
        public string Doc_Id_10 { get; set; }

        public decimal Doc_Id_10_Check { get; set; }

       
        /// <summary>
        /// có tài liệu khác
        /// </summary>
        public string Doc_Id_11 { get; set; }

        public decimal Doc_Id_11_Check { get; set; }

        #endregion

        #region add thêm 1 số cột tùy biến để sau này cần thì thêm vào đây
        public string Extent_fld01 { get; set; }

        public string Extent_fld02 { get; set; }

        public string Extent_fld03 { get; set; }

        public string Extent_fld04 { get; set; }

        public string Extent_fld05 { get; set; }

        public string Extent_fld06 { get; set; }

        public string Extent_fld07 { get; set; }

        public string Extent_fld08 { get; set; }

        public string Extent_fld09 { get; set; }

        public string Extent_fld10 { get; set; }


        #endregion
        public static void CopyA02_Info(ref A02_Info_Export p_appDetail, A02_Info p_A02_Info)
        {
            p_appDetail.A02_Id = p_A02_Info.A02_Id;
            p_appDetail.Language_Code = p_A02_Info.Language_Code;
            p_appDetail.TenThietKe = p_A02_Info.TenThietKe;
            p_appDetail.NgayTaoThietKe = p_A02_Info.NgayTaoThietKe;
            p_appDetail.KhaiThactm = p_A02_Info.KhaiThactm;
            p_appDetail.KhaiThactm_TaiNuoc = p_A02_Info.KhaiThactm_TaiNuoc;
            p_appDetail.KhaiThactm_Ngay = p_A02_Info.KhaiThactm_Ngay;
            p_appDetail.ChucNang = p_A02_Info.ChucNang;
            p_appDetail.ChucNang_Other = p_A02_Info.ChucNang_Other;
            p_appDetail.CauTruc = p_A02_Info.CauTruc;
            p_appDetail.CauTruc_OTHER = p_A02_Info.CauTruc_OTHER;
            p_appDetail.CongNghe = p_A02_Info.CongNghe;
            p_appDetail.CongNghe_OTHER = p_A02_Info.CongNghe_OTHER;
            p_appDetail.MoTaTomTat = p_A02_Info.MoTaTomTat;
            p_appDetail.SoAnh = p_A02_Info.SoAnh;
            

        }
        public static void CopyAppHeaderInfo(ref A02_Info_Export p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
            //p_appDetail.ID = pAppInfo.Id;
            p_appDetail.Appcode = pAppInfo.Appcode;
            p_appDetail.Master_Name = pAppInfo.Master_Name;
            p_appDetail.Master_Address = pAppInfo.Master_Address;
            p_appDetail.Master_Phone = pAppInfo.Master_Phone;
            p_appDetail.Master_Fax = pAppInfo.Master_Fax;
            p_appDetail.Master_Email = pAppInfo.Master_Email;

            p_appDetail.Master_Type = pAppInfo.Master_Type == null ? "" : pAppInfo.Master_Type;
            p_appDetail.Customer_Code = pAppInfo.Customer_Code == null ? "239" : pAppInfo.Customer_Code;

            p_appDetail.Rep_Master_Type = pAppInfo.Rep_Master_Type;
            p_appDetail.Rep_Master_Name = pAppInfo.Rep_Master_Name;
            p_appDetail.Rep_Master_Address = pAppInfo.Rep_Master_Address;
            p_appDetail.Rep_Master_Phone = pAppInfo.Rep_Master_Phone;
            p_appDetail.Rep_Master_Fax = pAppInfo.Rep_Master_Fax;
            p_appDetail.Rep_Master_Email = pAppInfo.Rep_Master_Email;
            p_appDetail.Relationship = pAppInfo.Relationship;
            p_appDetail.Send_Date = pAppInfo.Send_Date;
            p_appDetail.Status = pAppInfo.Status;
            p_appDetail.Status_Form = pAppInfo.Status_Form;
            p_appDetail.Status_Content = pAppInfo.Status_Content;
            p_appDetail.Remark = pAppInfo.Remark;
            p_appDetail.AppName = pAppInfo.AppName;
            p_appDetail.Address = pAppInfo.Address;
            p_appDetail.DateNo = pAppInfo.DateNo;
            p_appDetail.Months = pAppInfo.Months;
            p_appDetail.Years = pAppInfo.Years;
        }
        
        public static void CopyAuthorsInfo(ref A02_Info_Export p_appDetail, AuthorsInfo pAppInfo, int p_position)
        {
            if (p_position == 0)
            {
                p_appDetail.Author_Name = pAppInfo.Author_Name;
                p_appDetail.Author_Address = pAppInfo.Author_Address;
                p_appDetail.Author_Phone = pAppInfo.Author_Phone;
                p_appDetail.Author_Fax = pAppInfo.Author_Fax;
                p_appDetail.Author_Email = pAppInfo.Author_Email;
                p_appDetail.Author_Country_Display = pAppInfo.Author_Country_Display;

            }
            else if (p_position == 1)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Author_Name_1 = pAppInfo.Author_Name;
                    p_appDetail.Author_Address_1 = pAppInfo.Author_Address;
                    p_appDetail.Author_Phone_1 = pAppInfo.Author_Phone;
                    p_appDetail.Author_Fax_1 = pAppInfo.Author_Fax;
                    p_appDetail.Author_Email_1 = pAppInfo.Author_Email;
                    p_appDetail.Author_Country_Display_1 = pAppInfo.Author_Country_Display;
                }
                else
                {
                    p_appDetail.Author_Name_1 = "";
                    p_appDetail.Author_Address_1 = "";
                    p_appDetail.Author_Phone_1 = "";
                    p_appDetail.Author_Fax_1 = "";
                    p_appDetail.Author_Email_1 = "";
                    p_appDetail.Author_Country_Display_1 = "";
                }
            }
            else if (p_position == 2)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Author_Name_2 = pAppInfo.Author_Name;
                    p_appDetail.Author_Address_2 = pAppInfo.Author_Address;
                    p_appDetail.Author_Phone_2 = pAppInfo.Author_Phone;
                    p_appDetail.Author_Fax_2 = pAppInfo.Author_Fax;
                    p_appDetail.Author_Email_2 = pAppInfo.Author_Email;
                    p_appDetail.Author_Country_Display_2 = pAppInfo.Author_Country_Display;
                }
                else
                {
                    p_appDetail.Author_Name_2 = "";
                    p_appDetail.Author_Address_2 = "";
                    p_appDetail.Author_Phone_2 = "";
                    p_appDetail.Author_Fax_2 = "";
                    p_appDetail.Author_Email_2 = "";
                    p_appDetail.Author_Country_Display_2 = "";
                }

            }
        }

        public static void CopyOther_MasterInfo(ref A02_Info_Export p_appDetail, Other_MasterInfo pAppInfo, int p_position)
        {
            if (p_position == 0)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Master_Name_1 = pAppInfo.Master_Name;
                    p_appDetail.Master_Address_1 = pAppInfo.Master_Address;
                    p_appDetail.Master_Phone_1 = pAppInfo.Master_Phone;
                    p_appDetail.Master_Fax_1 = pAppInfo.Master_Fax;
                    p_appDetail.Master_Email_1 = pAppInfo.Master_Email;
                    p_appDetail.TacGiaDongThoi_1 = pAppInfo.TacGiaDongThoi;
                    p_appDetail.PhoBan_1 = pAppInfo.PhoBan;
                }
                else
                {
                    p_appDetail.Master_Name_1 = "";
                    p_appDetail.Master_Address_1 = "";
                    p_appDetail.Master_Phone_1 = "";
                    p_appDetail.Master_Fax_1 = "";
                    p_appDetail.Master_Email_1 = "";
                    p_appDetail.TacGiaDongThoi_1 = "";
                    p_appDetail.PhoBan_1 = "";
                }
            }
            else if (p_position == 1)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Master_Name_2 = pAppInfo.Master_Name;
                    p_appDetail.Master_Address_2 = pAppInfo.Master_Address;
                    p_appDetail.Master_Phone_2 = pAppInfo.Master_Phone;
                    p_appDetail.Master_Fax_2 = pAppInfo.Master_Fax;
                    p_appDetail.Master_Email_2 = pAppInfo.Master_Email;

                    p_appDetail.TacGiaDongThoi_2 = pAppInfo.TacGiaDongThoi;
                    p_appDetail.PhoBan_2 = pAppInfo.PhoBan;
                }
                else
                {
                    p_appDetail.Master_Name_2 = "";
                    p_appDetail.Master_Address_2 = "";
                    p_appDetail.Master_Phone_2 = "";
                    p_appDetail.Master_Fax_2 = "";
                    p_appDetail.Master_Email_2 = "";

                    p_appDetail.TacGiaDongThoi_2 = "";
                    p_appDetail.PhoBan_2 = "";
                }
            }
        }

    }
}
