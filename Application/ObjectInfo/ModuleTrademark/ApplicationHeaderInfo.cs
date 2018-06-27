using System;
using ObjectInfos.ModuleTrademark;
using Common.Extensions;

namespace ObjectInfos
{
    public class AppClassInfo
    {
        public string Code { get; set; }
        public string Name_Vi { get; set; }
        public string Name_En { get; set; }

        public string KeySearch { get; set; }

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
        public DateTime Accept_Date { get; set; }
        public DateTime Public_Date { get; set; }
        public DateTime Accept_Content_Date { get; set; }
        public DateTime Grant_Date { get; set; }
        public DateTime Grant_Public_Date { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Languague_Code { get; set; }
        public string Remark { get; set; }
        public string Status_Nname { get; set; }
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

    }

    public class CreateInstance
    {
        public static AppInfoExport CopyAppHeaderInfo( AppInfoExport pAppExportInfo, ApplicationHeaderInfo pAppInfo)
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

        public static AppInfoExport CopyAppDetailInfo( AppInfoExport pAppExportInfo, AppDetail04NHInfo pAppInfo)
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

        public static AppTM06DKQTInfoExport CopyAppDetailInfo(AppTM06DKQTInfoExport pAppExportInfo, App_Detail_TM06DKQT_Info pAppInfo)
        {
            pAppExportInfo.THANHVIEN_ND_TC = pAppInfo.THANHVIEN_ND_TC;
            pAppExportInfo.DON_GIAY_DKNHCS = pAppInfo.DON_GIAY_DKNHCS;
            pAppExportInfo.REF_APPNO = pAppInfo.REF_APPNO;
            pAppExportInfo.REF_APPNO = pAppInfo.REF_APPNO;
            pAppExportInfo.REF_APPNO_TEXT = pAppInfo.REF_APPNO;
            pAppExportInfo.COUNTRY_ID01_TEXT = pAppInfo.COUNTRY_ID01_TEXT;
            pAppExportInfo.COUNTRY_ID02_TEXT = pAppInfo.COUNTRY_ID02_TEXT;
            pAppExportInfo.COUNTRY_ID03_TEXT = pAppInfo.COUNTRY_ID03_TEXT;
            pAppExportInfo.COUNTRY_ID04_TEXT = pAppInfo.COUNTRY_ID04_TEXT;
            pAppExportInfo.COUNTRY_ID05_TEXT = pAppInfo.COUNTRY_ID05_TEXT;
            pAppExportInfo.COUNTRY_ID06_TEXT = pAppInfo.COUNTRY_ID06_TEXT;
            pAppExportInfo.COUNTRY_ID07_TEXT = pAppInfo.COUNTRY_ID07_TEXT;
            pAppExportInfo.COUNTRY_ID08_TEXT = pAppInfo.COUNTRY_ID08_TEXT;

            pAppExportInfo.COUNTRY_ID01_CODE = pAppInfo.COUNTRY_ID01_CODE;

            pAppExportInfo.LEPHI = pAppInfo.LEPHI;
            pAppExportInfo.PAGE_REMAIN = pAppInfo.PAGE_REMAIN;
            pAppExportInfo.strNgayNopDon= pAppInfo.NGAYNOPDON.ToTimeStringN0();
            //pAppExportInfo.TOKHAI_SOTRANG = pAppInfo.TOKHAI_SOTRANG;
            //pAppExportInfo.TOKHAI_SOBAN = pAppInfo.TOKHAI_SOBAN;
            //pAppExportInfo.MAUDK_VPQT_SO = pAppInfo.MAUDK_VPQT_SO;
            //pAppExportInfo.MAUDK_VPQT_NGONNGU = pAppInfo.MAUDK_VPQT_NGONNGU;
            //pAppExportInfo.MAUDK_VPQT_SOTRANG = pAppInfo.MAUDK_VPQT_SOTRANG;
            //pAppExportInfo. = pAppInfo.;
            //pAppExportInfo. = pAppInfo.;
            //pAppExportInfo. = pAppInfo.;
            //pAppExportInfo. = pAppInfo.;


            return pAppExportInfo;

        }

        public static AppTM06DKQTInfoExport CopySysDocumentInfo(AppTM06DKQTInfoExport pAppExportInfo, SysAppDocumentInfo pAppInfo)
        {
            return pAppExportInfo;
        }
    }

}
