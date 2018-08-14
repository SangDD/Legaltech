namespace ObjectInfos
{
    using System;

    public class GroupUserInfo
    {
        public int Stt { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime LastTimeUpdated { get; set; }
        public int Deleted { get; set; }

        public int NumberUsersInGroup { get; set; }
    }
}
