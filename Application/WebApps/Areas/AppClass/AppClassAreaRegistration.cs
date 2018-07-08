using System.Web.Mvc;

namespace WebApps.Areas.AppClass
{
    public class AppClassAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AppClass";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AppClass_default",
                "AppClass/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}