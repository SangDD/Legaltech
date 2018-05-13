namespace ObjectInfos.ModuleUsersAndRoles
{
    using System;
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
        }

        // main field mapping with database
        public int Stt { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
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

		public bool CanViewOtherBranch()
		{
			return Converter.ToBoolean(this.ViewOtherBranch);
		}

		public bool CanSeeProductTypeS()
		{
			return Converter.ToBoolean(this.SeeProductTypeS);
		}

		public bool CanChangeInstanceWhenOutStock()
		{
			return Converter.ToBoolean(this.ChangeInstanceWhenOutStock);
		}

        public DateTime CurrentDate { get; set; }
    }
}
