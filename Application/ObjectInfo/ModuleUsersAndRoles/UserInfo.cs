namespace ObjectInfos
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Common.Converters;

    public class UserInfo
    {
        private int _numberDayPasswordOutOfDate;

        public UserInfo()
        {
            this._numberDayPasswordOutOfDate = 90; // default 90 days
            this.AccountRoleChanged = false;
            this.AllAccountRoles = new List<FunctionInfo>();
            this.chashFile = new Hashtable();
            this.chashFileOther = new Hashtable();
            FilePreview = "";
        }

        // main field mapping with database
        public int Stt { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Lưu trong allcode với cdname='SEX_TYPE'
        /// </summary>
        public string Sex { get; set; }
        public string SexDisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int Type { get; set; }
        public string Type_Name { get; set; }


        public int Status { get; set; }
        public string StatusDisplayName { get; set; }

        public string Language { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime LastTimeUpdated { get; set; }
        public int Deleted { get; set; }

        public decimal Lawer_Id { get; set; }


        public string Fax { get; set; }
        public decimal Country { get; set; }
        public string Country_Name { get; set; }
        public string Company_Name { get; set; }
        public string Main_Business { get; set; }
        public string Title { get; set; }
        public string Copyto { get; set; }
        public string Face_Link { get; set; }
        public string Linkedin_Link { get; set; }
        public string Wechat_Link { get; set; }
        public string Other_Link { get; set; }
        public decimal Reason_Select { get; set; }
        public string Reason_Select_Name { get; set; }

        public decimal Request_Credit { get; set; }
        public string Request_Credit_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Contact_Person { get; set; }


        public decimal Other_Type { get; set; }
        public string Lawer_Type_Name { get; set; }

        public decimal Hourly_Rate { get; set; }
        public decimal Hourly_Rate_USD { get; set; }

        // additional info
        public DateTime LastTimeUpdatePassword { get; set; }
        public DateTime LastTimeRecoverPassword { get; set; }
        public string HtmlMenu { get; set; }
        public string DefaultHomePage { get; set; }
        public DateTime LoginTime { get; set; }
        public bool AccountRoleChanged { get; set; }
        public List<FunctionInfo> AllAccountRoles { get; set; }

        public List<int> GroupSelectedCollection { get; set; }

        public void SetRoleSuperAdmin()
        {
            this.Username = "SuperAdmin";
            this.Id = -99;
        }

        public bool IsSuperAdmin()
        {
            return this.Id == -99;
        }

        public bool IsFirstTimeLogin()
        {
            return this.LastTimeUpdatePassword == DateTime.MinValue;
        }

        public bool IsPasswordOutOfDate()
        {
            return DateTime.Now.Subtract(this.LastTimeUpdatePassword).TotalDays > this._numberDayPasswordOutOfDate;
        }

        public bool IsActive()
        {
            return this.Status == 1;
        }

        public void SetTimePasswordOutOfDate(int numberOfDay)
        {
            this._numberDayPasswordOutOfDate = numberOfDay;
        }

        public string GroupSelectedCollectionToString()
        {
            if (this.GroupSelectedCollection == null) return string.Empty;
            var groupsString = string.Empty;
            foreach (var group in this.GroupSelectedCollection)
            {
                groupsString += group + ",";
            }

            groupsString = groupsString.TrimEnd(',');

			return groupsString;
		}
	 
        public DateTime CurrentDate { get; set; }
        /// <summary>
        /// Lưu danh sách các tài liệu upload trong đơn 
        /// //1user mà làm 2 đơn giống nhau thì khả năng bị trùng
        /// 
        /// </summary>
        public Hashtable chashFile { get; set; }

        public Hashtable chashFileOther { get; set; }


        public string FilePreview { get; set; }

      
    }
}
