using System;
using System.Web;
using System.Web.Mvc;
namespace ObjectInfos
{
    public class Sys_Pages_Info
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Code { get; set; }
        public string Imageheader { get; set; }

        [AllowHtml]
        public string Header { get; set; }

        [AllowHtml]
        public string Header_En { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        [AllowHtml]
        public string Content_En { get; set; }

        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public decimal Deleted { get; set; }
        public decimal Status { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }

    }
}
