namespace ObjectInfos
{
    public class AllCodeInfo
    {
        public string CdName { get; set; }
        public string CdType { get; set; }
        public string CdVal { get; set; }
        public string Content { get; set; }
        public int LstOdr { get; set; }
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
}
