using System;
using ObjectInfos.ModuleTrademark;
using Common.Extensions;
using System.Web;

namespace ObjectInfos
{
    public class AppClassInfo
    {
        public string Code { get; set; }
        public string Name_Vi { get; set; }
        public string Name_En { get; set; }

        public string KeySearch { get; set; }

        public string GroupCode { get; set; }

        public string DisplayValue { get; set; }
    }

    public class ApplicationHeaderInfo
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Appcode { get; set; }
        public string Master_Name { get; set; }
        public string Master_Address { get; set; }
        public string Master_Phone { get; set; }
        public string Master_Fax { get; set; }
        public string Master_Email { get; set; }

        public string Rep_Master_Type { get; set; }
        public string Rep_Master_Name { get; set; }
        public string Rep_Master_Address { get; set; }
        public string Rep_Master_Phone { get; set; }
        public string Rep_Master_Fax { get; set; }
        public string Rep_Master_Email { get; set; }
        public string Relationship { get; set; }
        public DateTime Send_Date { get; set; }
        public decimal Status { get; set; }
        public decimal Status_Form { get; set; }
        public decimal Status_Content { get; set; }
        public DateTime Filing_Date { get; set; }

        public string Str_Send_Date
        {
            get
            {
                return Send_Date.ToString("dd-MM-yyyy");
            }
        }

        public string Str_Filing_Date
        {
            get
            {
                return Filing_Date.ToString("dd-MM-yyyy");
            }
        }

        public DateTime Accept_Date { get; set; }
        public string Str_Accept_Date
        {
            get
            {
                return Accept_Date.ToString("dd-MM-yyyy");
            }
        }

        public DateTime Public_Date { get; set; }
        public string Str_Public_Date
        {
            get
            {
                return Public_Date.ToString("dd-MM-yyyy");
            }
        }

        public DateTime Accept_Content_Date { get; set; }
        public string Str_Accept_Content_Date
        {
            get
            {
                return Accept_Content_Date.ToString("dd-MM-yyyy");
            }
        }

        public DateTime Grant_Date { get; set; }
        public string Str_Grant_Date
        {
            get
            {
                return Grant_Date.ToString("dd-MM-yyyy");
            }
        }

        public DateTime Grant_Public_Date { get; set; }
        public string Str_Grant_Public_Date
        {
            get
            {
                return Grant_Public_Date.ToString("dd-MM-yyyy");
            }
        }

        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Languague_Code { get; set; }
        public string Remark { get; set; }
        public string Status_Name { get; set; }
        public string Status_Formm_Name { get; set; }
        public string Status_Content_Name { get; set; }
        public string AppName { get; set; }

        /// <summary>
        /// Khai tai dau day
        /// </summary>
        public string Address { get; set; }
        public string DateNo { get; set; }
        public string Months { get; set; }
        public string Years { get; set; }

        public string Client_Reference { get; set; }
        public string Case_Name { get; set; }
        public string Case_Code { get; set; }

        public DateTime Status_Date { get; set; }
        public int Date_Wait { get; set; }

        public string Customer_Name { get; set; }
        public decimal Customer_Country { get; set; }
        public string Customer_Country_Name { get; set; }

        /// <summary>
        /// Phân biệt là xem hay sửa xóa
        /// </summary>
        public string ActionView { get; set; }

        public HttpPostedFileBase File_Copy_Filing { get; set; }
        public string Url_copy_filing { get; set; }

        public HttpPostedFileBase File_Translate_Filing { get; set; }
        public string URL_TRANSLATE_FILING { get; set; }

        public string Note { get; set; }
        public string App_No { get; set; }
        public string App_Degree { get; set; }

        public string DDSHCN { get; set; }
        public string MADDSHCN { get; set; }
        // Người xử lý gần nhất
        public string User_Processing { get; set; }

        public string Lawer_Name { get; set; }
        public string Lawer_User_Name { get; set; }
        public string User_Admin_Grant { get; set; }
        public decimal Admin_Id { get; set; }

        public string Currency_Type { get; set; }

        public string Comment_Filling { get; set; }

        public decimal Id_Vi { get; set; }
        public string Url_Billing { get; set; }

        public DateTime Expected_Accept_Date { get; set; }
        public DateTime Expected_Public_Date { get; set; }
        public DateTime Expected_Accept_Content_Date { get; set; }
        public DateTime Expected_Grant_Date { get; set; }
        public DateTime Expected_Grant_Public_Date { get; set; }
    }


    public class AppInfoExport : AppDetail04NHInfo
    {
        //Tai lieu dinh kem
        public decimal Document_Id { get; set; }
        public decimal Lstord { get; set; }

        //dành cho Classinfo
        public string Textinput { get; set; }
        public string Code { get; set; }
        public string strTongSonhom { get; set; }
        public string strTongSoSP { get; set; }
        public string strListClass { get; set; }
        //tài liệu khác 
        public string strDanhSachFileDinhKem { get; set; }
        //Phí
        public decimal TM04NH_200 { get; set; }
        public decimal TM04NH_201 { get; set; }
        public decimal TM04NH_2011 { get; set; }
        public decimal TM04NH_203 { get; set; }
        public decimal TM04NH_204 { get; set; }
        public decimal TM04NH_205 { get; set; }
        public decimal TM04NH_2051 { get; set; }
        public decimal TM04NH_207 { get; set; }
        public decimal TM04NH_2071 { get; set; }
        public decimal TM04NH_200_Val { get; set; }
        public decimal TM04NH_201_Val { get; set; }
        public decimal TM04NH_2011_Val { get; set; }
        public decimal TM04NH_203_Val { get; set; }
        public decimal TM04NH_204_Val { get; set; }
        public decimal TM04NH_205_Val { get; set; }
        public decimal TM04NH_2051_Val { get; set; }
        public decimal TM04NH_207_Val { get; set; }
        public decimal TM04NH_2071_Val { get; set; }
        public decimal TM04NH_TOTAL { get; set; }

        //Tài liệu đính kèm 
        //POA
        public decimal TM_04NH_D_04_ISU { get; set; }
        public string TM_04NH_D_04_CHAR01 { get; set; }

        //bản gốc
        public decimal TM_04NH_D_05_ISU { get; set; }
        //bản sao
        public decimal TM_04NH_D_06_ISU { get; set; }

        //bản gốc nop sau
        public decimal TM_04NH_D_07_ISU { get; set; }
        //bản sao
        public decimal TM_04NH_D_08_ISU { get; set; }
        public string TM_04NH_D_08_CHAR01 { get; set; }

        //Ban dich tieng viet 
        public decimal TM_04NH_D_09_ISU { get; set; }
        public string TM_04NH_D_09_CHAR01 { get; set; }


        //Tài liệu xác nhận được
        public decimal TM_04NH_D_10_ISU { get; set; }
        public string TM_04NH_D_10_CHAR01 { get; set; }

        //Tài liệu xác nhận quyền đăng ký nhãn hiệu
        public decimal TM_04NH_D_11_ISU { get; set; }
        //Tài liệu xác nhận thụ hưởng quyền đăng ký từ người khác
        public decimal TM_04NH_D_12_ISU { get; set; }

        //Quy chế sử dụng NH tập thể/chứng nhận, gồm
        public decimal TM_04NH_D_13_ISU { get; set; }
        public string TM_04NH_D_13_CHAR01 { get; set; }
        public string TM_04NH_D_13_CHAR02 { get; set; }


        public decimal TM_04NH_D_14_ISU { get; set; }
        public string TM_04NH_D_14_CHAR01 { get; set; }

        public decimal TM_04NH_D_15_ISU { get; set; }
        public string TM_04NH_D_15_CHAR01 { get; set; }
        public decimal TM_04NH_D_16_ISU { get; set; }
        public string TM_04NH_D_16_CHAR01 { get; set; }
        public decimal TM_04NH_D_17_ISU { get; set; }


        //Bản đồ khu vực địa lý
        public decimal TM_04NH_D_18_ISU { get; set; }

        //Văn bản của UBND tỉnh
        public decimal TM_04NH_D_19_ISU { get; set; }

        //Có tài liệu bổ trợ 
        public decimal TM_04NH_D_20_ISU { get; set; }

        //Tài liệu khác
        public decimal TM_04NH_D_22_ISU { get; set; }

        #region add thêm 1 số cột tùy biến để sau này cần thì thêm vào đây
        public string Extent_fld01 { get; set; } //su dung cho TH ma DNSC 

        public string Extent_fld02 { get; set; } //Su dung cho Nguoi shcn

        public string Extent_fld03 { get; set; }

        public string Extent_fld04 { get; set; }

        public string Extent_fld05 { get; set; }

        public string Extent_fld06 { get; set; }

        public string Extent_fld07 { get; set; }

        public string Extent_fld08 { get; set; }

        public string Extent_fld09 { get; set; }

        public string Extent_fld10 { get; set; }
        #endregion

    }

    public class CreateInstance
    {
        public static AppInfoExport CopyAppHeaderInfo(AppInfoExport pAppExportInfo, ApplicationHeaderInfo pAppInfo)
        {
            pAppExportInfo.STT = pAppInfo.STT;
            pAppExportInfo.Id = pAppInfo.Id;
            pAppExportInfo.Appcode = pAppInfo.Appcode;
            pAppExportInfo.Master_Name = pAppInfo.Master_Name;
            pAppExportInfo.Master_Address = pAppInfo.Master_Address;
            pAppExportInfo.Master_Phone = pAppInfo.Master_Phone;
            pAppExportInfo.Master_Fax = pAppInfo.Master_Fax;
            pAppExportInfo.Master_Email = pAppInfo.Master_Email;
            pAppExportInfo.Rep_Master_Type = pAppInfo.Rep_Master_Type;
            pAppExportInfo.Rep_Master_Name = pAppInfo.Rep_Master_Name;
            pAppExportInfo.Rep_Master_Address = pAppInfo.Rep_Master_Address;
            pAppExportInfo.Rep_Master_Phone = pAppInfo.Rep_Master_Phone;
            pAppExportInfo.Rep_Master_Fax = pAppInfo.Rep_Master_Fax;
            pAppExportInfo.Rep_Master_Email = pAppInfo.Rep_Master_Email;
            pAppExportInfo.Relationship = pAppInfo.Relationship;
            pAppExportInfo.Send_Date = pAppInfo.Send_Date;
            pAppExportInfo.Status = pAppInfo.Status;
            pAppExportInfo.Status_Form = pAppInfo.Status_Form;
            pAppExportInfo.Status_Content = pAppInfo.Status_Content;
            pAppExportInfo.Remark = pAppInfo.Remark;
            pAppExportInfo.AppName = pAppInfo.AppName;
            pAppExportInfo.Address = pAppInfo.Address;
            pAppExportInfo.DateNo = pAppInfo.DateNo;
            pAppExportInfo.Months = pAppInfo.Months;
            pAppExportInfo.Years = pAppInfo.Years;
            pAppExportInfo.DDSHCN = pAppInfo.DDSHCN;
            pAppExportInfo.MADDSHCN = pAppInfo.MADDSHCN;
            return pAppExportInfo;
        }

        public static AppInfoExport CopyAppDetailInfo(AppInfoExport pAppExportInfo, AppDetail04NHInfo pAppInfo)
        {
            pAppExportInfo.Appno = pAppInfo.Appno;
            pAppExportInfo.Duadate = pAppInfo.Duadate;
            pAppExportInfo.Logourl = pAppInfo.Logourl;


            pAppExportInfo.Dactichhanghoa = pAppInfo.Dactichhanghoa;
            pAppExportInfo.Color = pAppInfo.Color;
            pAppExportInfo.Description = pAppInfo.Description;
            pAppExportInfo.Huongquyenuutien = pAppInfo.Huongquyenuutien;


            pAppExportInfo.Used_Special = pAppInfo.Used_Special;
            pAppExportInfo.Sodon_Ut = pAppInfo.Sodon_Ut;
            pAppExportInfo.Ngaynopdon_Ut = pAppInfo.Ngaynopdon_Ut;
            pAppExportInfo.Nuocnopdon_Ut = pAppInfo.Nuocnopdon_Ut;

            pAppExportInfo.Nguongocdialy = pAppInfo.Nguongocdialy;
            pAppExportInfo.Chatluong = pAppInfo.Chatluong;
            pAppExportInfo.Dactinhkhac = pAppInfo.Dactinhkhac;
            pAppExportInfo.LoaiNhanHieu = pAppInfo.LoaiNhanHieu;


            pAppExportInfo.Cdk_Name_1 = pAppInfo.Cdk_Name_1;
            pAppExportInfo.Cdk_Address_1 = pAppInfo.Cdk_Address_1;
            pAppExportInfo.Cdk_Phone_1 = pAppInfo.Cdk_Phone_1;
            pAppExportInfo.Cdk_Fax_1 = pAppInfo.Cdk_Fax_1;
            pAppExportInfo.Cdk_Email_1 = pAppInfo.Cdk_Email_1;


            pAppExportInfo.Cdk_Name_2 = pAppInfo.Cdk_Name_2;
            pAppExportInfo.Cdk_Address_2 = pAppInfo.Cdk_Address_2;
            pAppExportInfo.Cdk_Phone_2 = pAppInfo.Cdk_Phone_2;
            pAppExportInfo.Cdk_Fax_2 = pAppInfo.Cdk_Fax_2;
            pAppExportInfo.Cdk_Email_2 = pAppInfo.Cdk_Email_2;

            pAppExportInfo.Cdk_Name_3 = pAppInfo.Cdk_Name_3;
            pAppExportInfo.Cdk_Address_3 = pAppInfo.Cdk_Address_3;
            pAppExportInfo.Cdk_Phone_3 = pAppInfo.Cdk_Phone_3;
            pAppExportInfo.Cdk_Fax_3 = pAppInfo.Cdk_Fax_3;
            pAppExportInfo.Cdk_Email_3 = pAppInfo.Cdk_Email_3;

            pAppExportInfo.Cdk_Name_4 = pAppInfo.Cdk_Name_4;
            pAppExportInfo.Cdk_Address_4 = pAppInfo.Cdk_Address_4;
            pAppExportInfo.Cdk_Phone_4 = pAppInfo.Cdk_Phone_4;
            pAppExportInfo.Cdk_Fax_4 = pAppInfo.Cdk_Fax_4;
            pAppExportInfo.Cdk_Email_4 = pAppInfo.Cdk_Email_4;


            pAppExportInfo.YCCapPho1 = pAppInfo.YCCapPho1;
            pAppExportInfo.YCCapPho2 = pAppInfo.YCCapPho2;
            pAppExportInfo.YCCapPho3 = pAppInfo.YCCapPho3;
            pAppExportInfo.YCCapPho4 = pAppInfo.YCCapPho4;
            pAppExportInfo.CodeLogo = pAppInfo.CodeLogo;

            return pAppExportInfo;

        }

        public static AppInfoExport CopySysDocumentInfo(AppInfoExport pAppExportInfo, SysAppDocumentInfo pAppInfo)
        {
            return pAppExportInfo;
        }
    }

    public class AppTM06DKQTInfoExport : App_Detail_TM06DKQT_Info
    {
        //Tai lieu dinh kem
        public decimal Document_Id { get; set; }
        public decimal Lstord { get; set; }

        //dành cho Classinfo
        public string Textinput { get; set; }
        public string Code { get; set; }
        public string strTongSonhom { get; set; }
        public string strTongSoSP { get; set; }
        public string strListClass { get; set; }
        //tài liệu khác 

        public string strDanhSachFileDinhKem { get; set; }

        public string strNgayNopDon { get; set; }

        //Phí

    }

    public class CreateInstanceTM06DKQT
    {
        public static AppTM06DKQTInfoExport CopyAppHeaderInfo(AppTM06DKQTInfoExport pAppExportInfo, ApplicationHeaderInfo pAppInfo)
        {
            pAppExportInfo.STT = pAppInfo.STT;
            pAppExportInfo.Id = pAppInfo.Id;
            pAppExportInfo.Appcode = pAppInfo.Appcode;
            pAppExportInfo.Master_Name = pAppInfo.Master_Name;
            pAppExportInfo.Master_Address = pAppInfo.Master_Address;
            pAppExportInfo.Master_Phone = pAppInfo.Master_Phone;
            pAppExportInfo.Master_Fax = pAppInfo.Master_Fax;
            pAppExportInfo.Master_Email = pAppInfo.Master_Email;
            pAppExportInfo.Rep_Master_Type = pAppInfo.Rep_Master_Type;
            pAppExportInfo.Rep_Master_Name = pAppInfo.Rep_Master_Name;
            pAppExportInfo.Rep_Master_Address = pAppInfo.Rep_Master_Address;
            pAppExportInfo.Rep_Master_Phone = pAppInfo.Rep_Master_Phone;
            pAppExportInfo.Rep_Master_Fax = pAppInfo.Rep_Master_Fax;
            pAppExportInfo.Rep_Master_Email = pAppInfo.Rep_Master_Email;
            pAppExportInfo.Relationship = pAppInfo.Relationship;
            pAppExportInfo.Send_Date = pAppInfo.Send_Date;
            pAppExportInfo.Status = pAppInfo.Status;
            pAppExportInfo.Status_Form = pAppInfo.Status_Form;
            pAppExportInfo.Status_Content = pAppInfo.Status_Content;
            pAppExportInfo.Remark = pAppInfo.Remark;
            pAppExportInfo.AppName = pAppInfo.AppName;
            pAppExportInfo.Address = pAppInfo.Address;
            pAppExportInfo.DateNo = pAppInfo.DateNo;
            pAppExportInfo.Months = pAppInfo.Months;
            pAppExportInfo.Years = pAppInfo.Years;
            return pAppExportInfo;
        }

        public static AppTM06DKQTInfoExport CopySysDocumentInfo(AppTM06DKQTInfoExport pAppExportInfo, SysAppDocumentInfo pAppInfo)
        {
            return pAppExportInfo;
        }
    }

}
