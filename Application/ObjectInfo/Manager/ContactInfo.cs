using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos.Manager
{
    public class ContactInfo
    {
        public decimal STT { get; set; }
        public decimal ID { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string Language { get; set; }
        public decimal Status { get; set; }
        public decimal StatusText { get; set; }
        public DateTime DateSent { get; set; }

        public string ReplyContent { get; set; }
        public string ReplySubject { get; set; }
        public string ReplyBy { get; set; }
        public string Case_Code { get; set; }
        
        public DateTime ReplyDate { get; set; }

        public string URL { get; set; }
        public HttpPostedFileBase FileBase_File_Url { get; set; }

        public string URL01 { get; set; }
        public HttpPostedFileBase FileBase_File_Url02 { get; set; }
    }
}
