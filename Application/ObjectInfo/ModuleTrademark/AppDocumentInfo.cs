using System;
using System.Web;

namespace ObjectInfos.ModuleTrademark
{
    public class AppDocumentInfo
    {
        public decimal Id { get; set; }
        public string Language_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Document_Id { get; set; }
        public decimal Isuse { get; set; }
        public string Note { get; set; }
        public decimal Status { get; set; }
        public DateTime Document_Filing_Date { get; set; }
        public string Filename { get; set; }
        public string Url_Hardcopy { get; set; }
        public HttpPostedFileBase pfiles { get; set; }
        public string keyFileUpload { get; set; }
        public string CHAR01 { get; set; }
        public string CHAR02 { get; set; }
        public string CHAR03 { get; set; }
        public string CHAR04 { get; set; }
        public string CHAR05 { get; set; }

        public string styleCheck { get; set; }

        public string divUpLoadFile { get; set; }

    }
}
