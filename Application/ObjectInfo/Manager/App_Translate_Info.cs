using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class App_Translate_Info
    {
        public decimal Id { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Case_Code { get; set; }
        public string Object_Name { get; set; }
        public string Value_Old { get; set; }
        public string Value_Translate { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
    }

    public class Sys_App_Translate_Info
    {
        public string AppCode { get; set; }
        public string Object_Name { get; set; }
        public string Type { get; set; }

        public string Object_Lable { get; set; }
        public string Object_Lable_En { get; set; }
        public string TxtId { get; set; }
    }

    public class Sys_Document_Info
    {
        public string AppCode { get; set; }
        public string Doc_Id { get; set; }
        public string Check_Box_Id { get; set; }

        public string Doc_Level { get; set; }
        public string Content_1 { get; set; }
        public string Content_2 { get; set; }
        public string Content_3 { get; set; }

        public string Content_1_En { get; set; }
        public string Content_2_En { get; set; }
        public string Content_3_En { get; set; }

        public decimal Is_Upload { get; set; }
        public decimal Doc_Group { get; set; }
        public string Width { get; set; }
    }
}
