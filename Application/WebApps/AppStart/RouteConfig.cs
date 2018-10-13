namespace WebApps.AppStart
{
	using System.Web.Mvc;
	using System.Web.Routing;

	public class RouteConfig
	{
		public const string KnHome = "/";

		public const string KnLogin = "/";

		public const string KnAccountSessionInvalid = "/account-session-invalid";

		public const string KnHttpNotFound = "/page-not-found";

		public const string KnAccessDenied = "/access-denied";

		public const string KnAccessDeniedShortern = "/access-denied-shortern";

		public const string KnReLoginWhenAccountDataChanged = "/re-login";

		public const string KnFilterRequestNotIdentity = "/filter-request-not-identity";

		public const string KnLoggedHome = "/home";

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapMvcAttributeRoutes();

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "WebApps.Controllers" });

        }
	}
}
