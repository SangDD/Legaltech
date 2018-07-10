using System;
using System.Web;

namespace ObjectInfos
{
    public class AppDetail04NHInfo : ApplicationHeaderInfo
    {
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
    }


    public class CustomerInfo 
    {

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
         
    }
}
