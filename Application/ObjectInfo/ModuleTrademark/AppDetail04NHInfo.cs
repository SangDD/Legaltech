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
        public string Appno { get; set; }
        public DateTime Duadate { get; set; }
        public string Logourl { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }
         
        public int Dactichhanghoa { get; set; }
        public string Color { get; set; }

        public string Description { get; set; }
        public int Huongquyenuutien { get; set; }

        public int Used_Special { get; set; }
        public string Sodon_Ut { get; set; }
        public DateTime Ngaynopdon_Ut { get; set; }
        public string Nuocnopdon_Ut { get; set; }
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

    }
}
