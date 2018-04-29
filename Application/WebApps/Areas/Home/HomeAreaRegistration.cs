namespace WebApps.Areas.Home
{
    using System.Web.Mvc;

    public class HomeAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Home";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Home_default",
                "Home/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "WebApps.Controllers" });
        }
    }
}