namespace Common.Ultilities
{
	using System;
	using System.Security.Cryptography;
	using System.Text;

	public class Encription
	{
		private const string PWD_ENCRYPT_KEY = "+&MOD%";

		private static string Encrypt(string toEncrypt)
		{
			try
			{
				var md5Hasher = new MD5CryptoServiceProvider();
				var encoder = new UTF8Encoding();
				var hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(toEncrypt));
				var s = new StringBuilder();
				foreach (var _hashedByte in hashedBytes)
				{
					s.Append(_hashedByte.ToString("x2"));
				}

				return s.ToString();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return string.Empty;
		}

		public static string EncryptAccountPassword(string userName, string password)
		{
			var accountPwdBuiltIn = new StringBuilder();
			accountPwdBuiltIn.Append(PWD_ENCRYPT_KEY).Append(userName.ToLower()).Append(password);
			return Encrypt(accountPwdBuiltIn.ToString());
		}
	}
}
