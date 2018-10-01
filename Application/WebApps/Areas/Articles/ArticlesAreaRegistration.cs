using System.Web.Mvc;

namespace WebApps.Areas.Articles
{
    public class ArticlesAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Articles";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Articles_default",
                "Articles/{controller}/{action}/{id}/{id2}",
                new { action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional }, new[] { "WebApps.Controllers" }
            );
        }
    }
}