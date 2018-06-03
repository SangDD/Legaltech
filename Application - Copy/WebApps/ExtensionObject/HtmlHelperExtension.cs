namespace WebApps.ExtensionObject
{
	using System;
	using System.Web;
	using System.Web.Mvc;

	public static class HtmlHelperExtension
	{
		public static IHtmlString AntiForgeryTokenValue(this HtmlHelper htmlHelper)
		{
			var field = htmlHelper.AntiForgeryToken().ToHtmlString();
			var beginIndex = field.IndexOf("value=\"", StringComparison.Ordinal) + 7;
			var endIndex = field.IndexOf("\"", beginIndex, StringComparison.Ordinal);
			return new HtmlString(field.Substring(beginIndex, endIndex - beginIndex));
		}
	}
}