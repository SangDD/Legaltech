using System;

namespace ObjectInfos
{
    public class AppClassInfo
    {
        public string Code { get; set; }
        public string Name_Vi { get; set; }
        public string Name_En { get; set; }

        public string KeySearch { get; set; }

        public string DisplayValue { get; set; }
    }

    public class ApplicationHeaderInfo
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Appcode { get; set; }
        public string Master_Name { get; set; }
        public string Master_Address { get; set; }
        public string Master_Phone { get; set; }
        public string Master_Fax { get; set; }
        public string Master_Email { get; set; }

        public string Rep_Master_Type { get; set; }
        public string Rep_Master_Name { get; set; }
        public string Rep_Master_Address { get; set; }
        public string Rep_Master_Phone { get; set; }
        public string Rep_Master_Fax { get; set; }
        public string Rep_Master_Email { get; set; }
        public string Relationship { get; set; }
        public DateTime Send_Date { get; set; }
        public decimal Status { get; set; }
        public decimal Status_Form { get; set; }
        public decimal Status_Content { get; set; }
        public DateTime Filing_Date { get; set; }
        public DateTime Accept_Date { get; set; }
        public DateTime Public_Date { get; set; }
        public DateTime Accept_Content_Date { get; set; }
        public DateTime Grant_Date { get; set; }
        public DateTime Grant_Public_Date { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Languague_Code { get; set; }
        public string Remark { get; set; }
        public string Status_Nname { get; set; }
        public string Status_Formm_Name { get; set; }
        public string Status_Content_Name { get; set; }
        public string AppName { get; set; }
        public string Address { get; set; }
        public string DateNo { get; set; }
        public string Months { get; set; }
        public string Years { get; set; }
    }
}
