using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{
    public class SearchObject_Header_Info
    {

        public decimal STT { get; set; }

        public decimal SEARCH_ID { get; set; }

        public string CASE_CODE { get; set; }

        public string CLIENT_REFERENCE { get; set; }

        public string CASE_NAME { get; set; }

        public DateTime REQUEST_DATE { get; set; }

        public DateTime RESPONSE_DATE { get; set; }

        public decimal STATUS { get; set; }

        public string STATUS_NAME { get; set; }

        public decimal LAWER_ID { get; set; }

        public string CREATED_BY { get; set; }

        public DateTime CREATED_DATE { get; set; }

        public string MODIFIED_BY { get; set; }

        public DateTime MODIFIED_DATE { get; set; }

        public string LANGUAGE_CODE { get; set; }

        public string NOTES { get; set; }

        public string LAWER_NAME { get; set; }
        public string Lawer_User_Name { get; set; }

        public string Admin_User_Name { get; set; }
        public string ADMIN_NAME { get; set; }

        public string CONTENT { get; set; }

        public decimal Country_Id { get; set; }
        public string Country_Name { get; set; }


        public string Currency_Type { get; set; }
        public decimal Customer_Country { get; set; }
        public string Customer_Country_Name { get; set; }

        public string Customer_Name { get; set; }
        public string Customer_Address { get; set; }
        public decimal Object_Search { get; set; }
        public string Object_Search_Name { get; set; }

        public HttpPostedFileBase Url_File_Up { get; set; }
        public string Url_File { get; set; }

        public decimal Billing_Id { get; set; }
        public string Url_Billing { get; set; }



        //HungTD Thêm up ảnh
        public HttpPostedFileBase pfileLogo { get; set; }

        public int Logochu { get; set; }
        public string ChuLogo { get; set; }

        public string LogourlOrg { get; set; }
        public string Logourl { get; set; }

        public int LOGO_FONT_SIZE { get; set; }

        public string FONTTYPE { get; set; }
        //End HungTD
    }

}
