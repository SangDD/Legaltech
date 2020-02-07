using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class Base_Info
    {
        public string Created_By { set; get; }

        public DateTime Created_Date { set; get; }

        public string Modified_By { set; get; }

        public DateTime Modified_Date { set; get; }

    }

    public class Template_Email_Info
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Type_Name { get; set; }
        public Byte Content { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Note { get; set; }
    }

    public class Email_Info
    {
        public string EmailFrom { get; set; }
        public string Pass { get; set; }
        public string Display_Name { get; set; }

        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<string> LstAttachment { get; set; }
        public string Status { get; set; }
        public string Status_Name { get; set; }

        public DateTime Send_Time { get; set; }

        public HttpPostedFileBase File_Attach_1 { get; set; }

        public HttpPostedFileBase File_Attach_2 { get; set; }
        public string Customer_Name { get; set; }
        public string Out_Ref { get; set; }
        public string Your_Ref { get; set; }

        public string Sign { get; set; }
        public string Position { get; set; }
        public string lst_attachment { get; set; }
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Created_by { get; set; }
    }

    public class Send_Email_Info
    {
        public decimal Id { get; set; }
        public string Email_From { get; set; }
        public string Email_To { get; set; }
        public string Email_Cc { get; set; }
        public string Display_Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Lst_Attachment { get; set; }
        public string Status { get; set; }
        public DateTime Send_Time { get; set; }
    }
}
