using System;
using System.Web.Mvc;

namespace ObjectInfos
{
    public class NewsInfo
    {
        public decimal Id { get; set; }
        [AllowHtml]
        public string Title { get; set; }
        [AllowHtml]
        public string Header { get; set; }
        public string Imageheader { get; set; }
        public string Languagecode { get; set; }
        [AllowHtml]
        public string  Content { get; set; }
        public decimal Status { get; set; }
        public string Categories_Id { get; set; }
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
    }
}
