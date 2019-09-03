namespace BussinessFacade.ModuleUsersAndRoles
{
	using System;
	using System.Collections.Generic;
	using ObjectInfos;

	public class AccountManagerBL : RepositoriesBL
    {
	    private static readonly object s_lockerAccountIdForceReLoginCollection = new object();
	    private static readonly object s_lockerDicAccountLogin                 = new object();
	    private static readonly List<int> s_accountIdForceReLoginCollection    = new List<int>();
	    private static readonly Dictionary<int, DateTime> s_dicAccountsLogin   = new Dictionary<int, DateTime>();

		public static bool IsAccountRoleDataChanged(int accountId)
        {
            lock (s_lockerAccountIdForceReLoginCollection)
            {
                return s_accountIdForceReLoginCollection.Contains(accountId);
            }
        }

        public static void AddToAccountForceReLoginCollection(int accountId)
        {
            lock (s_lockerAccountIdForceReLoginCollection)
            {
                if (!s_accountIdForceReLoginCollection.Contains(accountId))
                {
                    s_accountIdForceReLoginCollection.Add(accountId);
                }
            }
        }

        public static void AddAccountInGroupToAccountForceReLoginCollection(int groupId)
        {
        //   var lstAccountIdInGroup = UserBL.GetAllUserIdByGroupId(groupId);
        //if (!(lstAccountIdInGroup?.Count > 0)) return;
        //foreach (var accountId in lstAccountIdInGroup)
        //{
        // AddToAccountForceReLoginCollection(accountId);
        //}
        }

        public static void RemoveFromAccountForceReLoginCollection(int accountId)
        {
            lock (s_lockerAccountIdForceReLoginCollection)
            {
                if (s_accountIdForceReLoginCollection.Contains(accountId))
                {
                    s_accountIdForceReLoginCollection.Remove(accountId);
                }
            }
        }

		public static void UpdateDicAccountLogin(UserInfo userInfo)
		{
			lock (s_lockerDicAccountLogin)
			{
				if (s_dicAccountsLogin.ContainsKey(userInfo.Id))
				{
					s_dicAccountsLogin[userInfo.Id] = userInfo.LoginTime;
				}
				else
				{
					s_dicAccountsLogin.Add(userInfo.Id, userInfo.LoginTime);
				}
			}
		}

		public static bool ValidAccountSession(UserInfo userInfo)
		{
			lock (s_lockerDicAccountLogin)
			{
				if (userInfo.LoginTime == s_dicAccountsLogin[userInfo.Id])
					return true;
			}

			return false;
		}
	}
}
