using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class C07_Info : ApplicationHeaderInfo
    {
        public decimal C07_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string LOGOURL { get; set; }
        public string IMG_URLOrg { get; set; }
        
        public string SODK_QUOCTE { get; set; }
        public DateTime NGAY_DK_QUOCTE { get; set; }
        public DateTime NGAY_UT_DKQT { get; set; }
        public string CHUNH_TEN { get; set; }
        public string CHUNH_DIACHI { get; set; }
        public string YC_DK_NH_CHUYENDOI { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }
    }

    public class C07_Info_Export : ApplicationHeaderInfo
    {

        public decimal C07_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string LOGOURL { get; set; }
        public string SODK_QUOCTE { get; set; }
        public DateTime NGAY_DK_QUOCTE { get; set; }
        public DateTime NGAY_UT_DKQT { get; set; }
        public string CHUNH_TEN { get; set; }
        public string CHUNH_DIACHI { get; set; }
        public string YC_DK_NH_CHUYENDOI { get; set; }
        #region Chủ đơn khác

        public string Master_Name_1 { set; get; }
        public string Master_Address_1 { set; get; }
        public string Master_Phone_1 { set; get; }
        public string Master_Fax_1 { set; get; }
        public string Master_Email_1 { set; get; }


        public string Master_Name_2 { set; get; }
        public string Master_Address_2 { set; get; }
        public string Master_Phone_2 { set; get; }
        public string Master_Fax_2 { set; get; }
        public string Master_Email_2 { set; get; }

        #endregion


        #region Phí
        //Lệ phí nộp đơn 
        public string Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }
        //Phí phân loại quốc tế về nhãn hiệu
        public string Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }

        //Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi )
        public string Fee_Id_21 { get; set; }
        public decimal Fee_Id_21_Check { get; set; }
        public string Fee_Id_21_Val { get; set; }

        // Phí công bố đơn (trường hợp đăng ký quốc tế nhãn hiệu chưa được chấp nhận bảo hộ tại Việt Nam)
        public string Fee_Id_3 { get; set; }
        public decimal Fee_Id_3_Check { get; set; }
        public string Fee_Id_3_Val { get; set; }

        //Phí tra cứu phục vụ việc thẩm định      
        public string Fee_Id_4 { get; set; }
        public decimal Fee_Id_4_Check { get; set; }
        public string Fee_Id_4_Val { get; set; }

        //Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi )
        public string Fee_Id_41 { get; set; }
        public decimal Fee_Id_41_Check { get; set; }
        public string Fee_Id_41_Val { get; set; }

        //Phí thẩm định đơn
        public string Fee_Id_5 { get; set; }
        public decimal Fee_Id_5_Check { get; set; }
        public string Fee_Id_5_Val { get; set; }
        //Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi )
        public string Fee_Id_51 { get; set; }
        public decimal Fee_Id_51_Check { get; set; }
        public string Fee_Id_51_Val { get; set; }

        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        public string strListClass { get; set; }
        
        #endregion

        #region các tài liệu trong đơn

        string _strDsFile = "";
        public string strDanhSachFileDinhKem
        { get { return _strDsFile; } set { _strDsFile = value; } }
        //Tờ khai, gồm.......trang x .......bản
        public string Doc_Id_0 { get; set; }
        public string Doc_Id_002 { get; set; }

        public decimal Doc_Id_0_Check { get; set; }

        //Mẫu nhãn hiệu, gồm .... mẫu
        public string Doc_Id_1 { get; set; }
        public string Doc_Id_102 { get; set; }

        public decimal Doc_Id_1_Check { get; set; }
        //Bản sao chứng từ nộp phí, lệ phí (trường hợp nộp phí, lệ phí qua dịch vụ bưu chính hoặc nộp trực tiếp vào tài khoản của Cục Sở hữu trí tuệ)

        public string Doc_Id_2 { get; set; }
        public string Doc_Id_202 { get; set; }
        // Bản sao ĐKQTNH đã bị mất hiệu lực tại nước xuất xứ 

        public decimal Doc_Id_2_Check { get; set; }
        //Giấy uỷ quyền bằng tiếng.......

        public string Doc_Id_3 { get; set; }
        public string Doc_Id_302 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }
        public string Doc_Id_4 { get; set; }
        public string Doc_Id_402 { get; set; }
        // bản dịch tiếng Việt, gồm..........trang  

        public decimal Doc_Id_4_Check { get; set; }
        public string Doc_Id_5 { get; set; }
        public string Doc_Id_502 { get; set; }

        public decimal Doc_Id_5_Check { get; set; }
        public decimal Doc_Id_6_Check { get; set; }
        //bản gốc                 

        public string Doc_Id_6 { get; set; }
        public string Doc_Id_602 { get; set; }
        public decimal Doc_Id_7_Check { get; set; }
        public string Doc_Id_702 { get; set; }
        //bản sao

        public string Doc_Id_7 { get; set; }
        public decimal Doc_Id_8_Check { get; set; }
        public string Doc_Id_802 { get; set; }
        //bản gốc sẽ nộp sau9

        public string Doc_Id_8 { get; set; }
        //bản gốc đã nộp theo đơn số

        public string Doc_Id_9 { get; set; }
        public string Doc_Id_902 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }
        //Các tài liệu khác, cụ thể là

        public string Doc_Id_10 { get; set; }
        public string Doc_Id_1002 { get; set; }
        public decimal Doc_Id_10_Check { get; set; }
        public string Doc_Id_11 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }

        
        #endregion

        public static void CopyC07_Info(ref C07_Info_Export p_appDetail, C07_Info p_C07_Info)
        {
            p_appDetail.C07_Id = p_C07_Info.C07_Id;
            p_appDetail.Language_Code = p_C07_Info.Language_Code;
            p_appDetail.LOGOURL = p_C07_Info.LOGOURL;
            p_appDetail.SODK_QUOCTE = p_C07_Info.SODK_QUOCTE;
            p_appDetail.NGAY_DK_QUOCTE = p_C07_Info.NGAY_DK_QUOCTE;
            p_appDetail.NGAY_UT_DKQT = p_C07_Info.NGAY_UT_DKQT;
            p_appDetail.CHUNH_TEN = p_C07_Info.CHUNH_TEN;
            p_appDetail.CHUNH_DIACHI = p_C07_Info.CHUNH_DIACHI;
            p_appDetail.YC_DK_NH_CHUYENDOI = p_C07_Info.YC_DK_NH_CHUYENDOI;

    }
        public static void CopyAppHeaderInfo(ref C07_Info_Export p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
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
       
        public static void CopyOther_MasterInfo(ref C07_Info_Export p_appDetail, Other_MasterInfo pAppInfo, int p_position)
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
                }
                else
                {
                    p_appDetail.Master_Name_1 = "";
                    p_appDetail.Master_Address_1 = "";
                    p_appDetail.Master_Phone_1 = "";
                    p_appDetail.Master_Fax_1 = "";
                    p_appDetail.Master_Email_1 = "";
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

                }
                else
                {
                    p_appDetail.Master_Name_2 = "";
                    p_appDetail.Master_Address_2 = "";
                    p_appDetail.Master_Phone_2 = "";
                    p_appDetail.Master_Fax_2 = "";
                    p_appDetail.Master_Email_2 = "";

                }
            }
        }

    }
}
