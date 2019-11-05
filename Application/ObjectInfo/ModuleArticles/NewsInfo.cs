using System;
using System.Web;
using System.Web.Mvc;

namespace ObjectInfos
{
    public class NewsInfo
    {

        public decimal STT { get; set; }
        public decimal Id { get; set; }
        [AllowHtml]
        public string Title { get; set; }
        [AllowHtml]
        public string Header { get; set; }

        [AllowHtml]
        public string Title_EN { get; set; }
        [AllowHtml]
        public string Header_EN { get; set; }

        public string Imageheader { get; set; }
        public string Languagecode { get; set; }
        [AllowHtml]
        public string  Content { get; set; }
        [AllowHtml]
        public string Content_En { get; set; }

        public decimal Status { get; set; }
        public string Status_Name { get; set; }

        public string Categories_Id { get; set; }
        public string ReCategories_Id { get; set; }
        public string Articles_Type { get; set; }
        public decimal Hottype { get; set; }
        public string Createdby { get; set; }

        public DateTime Createddate { get; set; }
        public string Modifiedby { get; set; }
        public DateTime Modifieddate { get; set; }
        public DateTime Publictime { get; set; }
        public string Publicby { get; set; }
        public string Unpublicby { get; set; }
        public DateTime Unpublicdate { get; set; }
        public decimal Deleted { get; set; }
        public decimal Country_Id { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }
        public string Case_Code { get; set; }
    }
}
