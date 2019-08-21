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
    }
}
