namespace ObjectInfos
{
    public class AllCodeInfo
    {
        public string CdName { get; set; }
        public string CdType { get; set; }
        public string CdVal { get; set; }
        public string Content { get; set; }
        public string Content_Eng { get; set; }

        public int LstOdr { get; set; }
    }

    public class Country_Info
    {
        public decimal Country_Id { get; set; }
        public string Name { get; set; } 
    }

    public class Injection_Info
    {
        public string Key { get; set; }
    }

    public class CallBack_Info
    {
        public CallBack_Info()
        {

        }

        public CallBack_Info(string p_Table_Name, string p_Message)
        {
            Table_Name = p_Table_Name;
            Message = p_Message;
        }

        public string Table_Name { get; set; }

        public string Message { get; set; }
    }

    public class Reports_Info
    {
        public string Rpt_File_Name { get; set; }
        
        public string Stored_Name { get; set; }
        
        public string Rpt_Name { get; set; }
        
        public decimal Type { get; set; }

        public decimal Sort_By { get; set; }

        public string Searchcmdsql { get; set; }

        public decimal NUMBER_CURSOR { get; set; }

        public decimal SEARCH_BY_USER_TYPE { get; set; }
    }
}
