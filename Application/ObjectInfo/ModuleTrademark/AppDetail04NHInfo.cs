using System;
using System.Web;

namespace ObjectInfos
{
    public class AppDetail04NHInfo : ApplicationHeaderInfo
    {
        public AppDetail04NHInfo()
        {
            App_Header_Id = 0;
            Request = "";
            Language_Code = "";
            Appno = "";
            Nuocnopdon_Ut2 = "";
            ThoaThuanKhac = "";
            Huongquyenuutien2 = "";
            Sodon_Ut2 = "";
            LoaiNhanHieu = "";
            Dactinhkhac = "";
            Chatluong = "";
            Nguongocdialy = "";
            Nuocnopdon_Ut = "";
            Sodon_Ut = "";
            Huongquyenuutien = "";
            Description = "";
            Color = "";
            Dactichhanghoa =0;
        }
        //public decimal Id { get; set; }
        public decimal App_Header_Id { get; set; }
        //public string Appcode { get; set; }
        public string Request { get; set; }
        public string Language_Code { get; set; }
        /// <summary>
        /// So don 
        /// </summary>
        public string Appno { get; set; }
        /// <summary>
        /// Ngay nop don
        /// </summary>
        public DateTime Duadate { get; set; }

        public string DuadateExp { get; set; }

        public string LogourlOrg { get; set; }
        public string Logourl { get; set; }
        public string CodeLogo { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }
        public int isChuLogo { get; set; }
        public string ChuLogo { get; set; }
        public int Dactichhanghoa { get; set; }
        public string Color { get; set; }

        public string Description { get; set; }
        public string Huongquyenuutien { get; set; }

        public int Used_Special { get; set; }
        public string Sodon_Ut { get; set; }
        //Ngay nop don uu tien 
        public DateTime Ngaynopdon_Ut { get; set; }
        public string Ngaynopdon_UtExp { get; set; }
        public string Nuocnopdon_Ut { get; set; }
        public string Nuocnopdon_Ut_Display { get; set; }
        public string Nguongocdialy { get; set; }
        public string Chatluong { get; set; }
        public string Dactinhkhac { get; set; }

        public string Cdk_Name_1 { get; set; }
        public string Cdk_Address_1 { get; set; }
        public string Cdk_Phone_1 { get; set; }
        public string Cdk_Fax_1 { get; set; }
        public string Cdk_Email_1 { get; set; }

        public string Cdk_Name_2 { get; set; }
        public string Cdk_Address_2 { get; set; }
        public string Cdk_Phone_2 { get; set; }
        public string Cdk_Fax_2 { get; set; }
        public string Cdk_Email_2 { get; set; }
        public string Cdk_Name_3 { get; set; }
        public string Cdk_Address_3 { get; set; }
        public string Cdk_Phone_3 { get; set; }
        public string Cdk_Fax_3 { get; set; }
        public string Cdk_Email_3 { get; set; }
        public string Cdk_Name_4 { get; set; }
        public string Cdk_Address_4 { get; set; }
        public string Cdk_Phone_4 { get; set; }
        public string Cdk_Fax_4 { get; set; }
        public string Cdk_Email_4 { get; set; }
        public string LoaiNhanHieu { get; set; }
        public string ListFileAttachOtherDel { get; set; }

        public string Sodon_Ut2 { get; set; }
        //Ngay nop don uu tien 
        public DateTime Ngaynopdon_Ut2 { get; set; }
        public string Ngaynopdon_UtExp2 { get; set; }
        public string Nuocnopdon_Ut2 { get; set; }
        public string Huongquyenuutien2 { get; set; }

        public string YCCapPho1 { get; set; }
        public string YCCapPho2 { get; set; }
        public string YCCapPho3 { get; set; }
        public string YCCapPho4 { get; set; }
        public string ThoaThuanKhac { get; set; }
        public decimal VI_TRANSLATE { get; set; }
        public decimal ID_EN { get; set; }
        public decimal STATUS_EN { get; set; }
        
    }

    public class SuggestInfo
    {
        public SuggestInfo (string _label, string _value)
        {
            label = _label;
            value = _value;
        }

        public string value { get; set; }
        public string label { get; set; }
    }

    public class SuggestInfo2
    {
        public SuggestInfo2(string _label)
        {
            label = _label;
        }

        public string label { get; set; }
    }

    public class CustomerSuggestInfo
    {
        public string label { get; set; }
        public string value { get; set; }
        public string name { get; set; }

        public string name_en { get; set; }
        public string label_en { get; set; }
        public CustomerSuggestInfo()
        {
            value = "";
            label = "";
            name = "";
            label_en = "";
            name_en = "";
            Language = "";
        }
        public string Language { get; set; }
    }


    public class CustomerInfo
    {
        public CustomerInfo()
        {
            Master_Name = "";
            Master_Address = "";
            Master_Phone = "";
            Master_Fax = "";
            Master_Email = "";
            Rep_Master_Type = "";
            Rep_Master_Name = "";
            Rep_Master_Address = "";
            Rep_Master_Phone = "";
            Rep_Master_Fax = "";
            Rep_Master_Email = "";

            Cdk_Name_1 = "";
            Cdk_Address_1 = "";
            Cdk_Phone_1 = "";
            Cdk_Fax_1 = "";
            Cdk_Email_1 = "";
            Cdk_Name_2 = "";
            Cdk_Address_2 = "";
            Cdk_Phone_2 = "";
            Cdk_Fax_2 = "";
            Cdk_Email_2 = "";
            Cdk_Name_3 = "";
            Cdk_Address_3 = "";
            Cdk_Phone_3 = "";
            Cdk_Fax_3 = "";
            Cdk_Email_3 = "";
            Cdk_Name_4 = "";
            Cdk_Address_4 = "";
            Cdk_Phone_4 = "";
            Cdk_Fax_4 = "";
            Cdk_Email_4 = "";
            Language = "";
        }

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

        public string Appno { get; set; }

        public string Cdk_Name_1 { get; set; }
        public string Cdk_Address_1 { get; set; }
        public string Cdk_Phone_1 { get; set; }
        public string Cdk_Fax_1 { get; set; }
        public string Cdk_Email_1 { get; set; }
        public string Cdk_Name_2 { get; set; }
        public string Cdk_Address_2 { get; set; }
        public string Cdk_Phone_2 { get; set; }
        public string Cdk_Fax_2 { get; set; }
        public string Cdk_Email_2 { get; set; }
        public string Cdk_Name_3 { get; set; }
        public string Cdk_Address_3 { get; set; }
        public string Cdk_Phone_3 { get; set; }
        public string Cdk_Fax_3 { get; set; }
        public string Cdk_Email_3 { get; set; }
        public string Cdk_Name_4 { get; set; }
        public string Cdk_Address_4 { get; set; }
        public string Cdk_Phone_4 { get; set; }
        public string Cdk_Fax_4 { get; set; }
        public string Cdk_Email_4 { get; set; }

        public string Language { get; set; }

    }
}
