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
    public class A05_Info : ApplicationHeaderInfo
    {
        public decimal A05_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string IMG_URLOrg { get; set; }
        public string IMG_URL { get; set; }
        public string TRANGTHAIDANGKY { get; set; }
        public string SODANGKY { get; set; }
        public DateTime NGAYDANGKY { get; set; }
        public string NUOCDANGKY { get; set; }
        public string TCQLDL_TEN { get; set; }
        public string TCQLDL_DIACHI { get; set; }
        public string TCQLDL_DIENTHOAI { get; set; }
        public string TCQLDL_FAX { get; set; }
        public string TCQLDL_EMAIL { get; set; }
        public string SANPHAM_TEN { get; set; }
        public string SANPHAM_TOMTAT { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }
    }

    public class A05_Info_Export : ApplicationHeaderInfo
    {

        public decimal A05_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string IMG_URL { get; set; }
        public string TRANGTHAIDANGKY { get; set; }
        public string SODANGKY { get; set; }
        public string NUOCDANGKY { get; set; }
        public DateTime NGAYDANGKY { get; set; }
        
        public string TCQLDL_TEN { get; set; }
        public string TCQLDL_DIACHI { get; set; }
        public string TCQLDL_DIENTHOAI { get; set; }
        public string TCQLDL_FAX { get; set; }
        public string TCQLDL_EMAIL { get; set; }
        public string SANPHAM_TEN { get; set; }
        public string SANPHAM_TOMTAT { get; set; }
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

        #endregion

          
        #region Phí

        public string Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }

        public string Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }


        public string Fee_Id_3 { get; set; }
        public decimal Fee_Id_3_Check { get; set; }
        public string Fee_Id_3_Val { get; set; }

        public string Fee_Id_4 { get; set; }
        public decimal Fee_Id_4_Check { get; set; }
        public string Fee_Id_4_Val { get; set; }
 

        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        #endregion

        #region các tài liệu trong đơn

        string _strDsFile = "";
        public string strDanhSachFileDinhKem
        { get { return _strDsFile; } set { _strDsFile = value; } }

        public string Doc_Id_1 { get; set; }
        public string Doc_Id_102 { get; set; }

        public decimal Doc_Id_1_Check { get; set; }

        public string Doc_Id_2 { get; set; }
        public string Doc_Id_202 { get; set; }

        public decimal Doc_Id_2_Check { get; set; }

        public string Doc_Id_3 { get; set; }
        public string Doc_Id_302 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }

        public string Doc_Id_4 { get; set; }
        public string Doc_Id_402 { get; set; }

        public decimal Doc_Id_4_Check { get; set; }

        public string Doc_Id_5 { get; set; }
        public decimal Doc_Id_5_Check { get; set; }

        public decimal Doc_Id_6_Check { get; set; }
        public string Doc_Id_6 { get; set; }
        public string Doc_Id_602 { get; set; }

        public decimal Doc_Id_7_Check { get; set; }
        public string Doc_Id_7 { get; set; }

        public decimal Doc_Id_8_Check { get; set; }
        public string Doc_Id_8 { get; set; }


        public string Doc_Id_9 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }

        public string Doc_Id_10 { get; set; }
        public string Doc_Id_1002 { get; set; }
        public decimal Doc_Id_10_Check { get; set; }

        public string Doc_Id_11 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }

        public string Doc_Id_12 { get; set; }
        public decimal Doc_Id_12_Check { get; set; }

        public string Doc_Id_13 { get; set; }
        public decimal Doc_Id_13_Check { get; set; }
        
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
        public static void CopyA05_Info(ref A05_Info_Export p_appDetail, A05_Info p_A05_Info)
        {
            p_appDetail.A05_Id = p_A05_Info.A05_Id;
            p_appDetail.Language_Code = p_A05_Info.Language_Code;
            p_appDetail.IMG_URL = p_A05_Info.IMG_URL;
            p_appDetail.TRANGTHAIDANGKY = p_A05_Info.TRANGTHAIDANGKY;
            p_appDetail.SODANGKY = p_A05_Info.SODANGKY;
            p_appDetail.NUOCDANGKY = p_A05_Info.NUOCDANGKY;
            p_appDetail.NGAYDANGKY = p_A05_Info.NGAYDANGKY;
            p_appDetail.TCQLDL_TEN = p_A05_Info.TCQLDL_TEN;
            p_appDetail.TCQLDL_DIACHI = p_A05_Info.TCQLDL_DIACHI;
            p_appDetail.TCQLDL_DIENTHOAI = p_A05_Info.TCQLDL_DIENTHOAI;
            p_appDetail.TCQLDL_FAX = p_A05_Info.TCQLDL_FAX;
            p_appDetail.TCQLDL_EMAIL = p_A05_Info.TCQLDL_EMAIL;
            p_appDetail.SANPHAM_TEN = p_A05_Info.SANPHAM_TEN;
            p_appDetail.SANPHAM_TOMTAT = p_A05_Info.SANPHAM_TOMTAT;


    }
        public static void CopyAppHeaderInfo(ref A05_Info_Export p_appDetail, ApplicationHeaderInfo pAppInfo)
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
       
        public static void CopyOther_MasterInfo(ref A05_Info_Export p_appDetail, Other_MasterInfo pAppInfo, int p_position)
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
