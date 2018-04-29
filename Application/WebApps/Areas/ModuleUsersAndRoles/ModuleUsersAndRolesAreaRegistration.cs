namespace WebApps.Areas.ModuleUsersAndRoles
{
	using System.Web.Mvc;

	public class ModuleUsersAndRolesAreaRegistration : AreaRegistration
	{
		public override string AreaName => "ModuleUsersAndRoles";

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"ModuleUsersAndRoles_default",
				"ModuleUsersAndRoles/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional } );
		}
 
    }
}