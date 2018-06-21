using System;

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
        public string Address { get; set; }
        public string DateNo { get; set; }
        public string Months { get; set; }
        public string Years { get; set; }
    }


    public class AppInfoExport : AppDetail04NHInfo
    {

       
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
    }


}
