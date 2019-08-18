using System.Web.Mvc;

namespace WebApps.Areas.ThietKeBanDan
{
    public class ThietKeBanDanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ThietKeBanDan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ThietKeBanDan_default",
                "ThietKeBanDan/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}