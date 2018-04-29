namespace CommonData
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Net.Sockets;

	public static class HttpHelper
	{
		public static string GetClientIPAddress(string ipCollection)
		{
			try
			{
				if (ipCollection.Length >= 5) return ipCollection;
				var host = Dns.GetHostEntry(Dns.GetHostName());
				foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
				{
					return ip.ToString();
				}

				return ipCollection;
			}
			catch (Exception)
			{
				return "unknown";
			}
		}
	}
}
