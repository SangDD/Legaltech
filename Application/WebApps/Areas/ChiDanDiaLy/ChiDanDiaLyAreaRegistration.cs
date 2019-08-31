using System.Web.Mvc;

namespace WebApps.Areas.ChiDanDiaLy
{
    public class ChiDanDiaLyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ChiDanDiaLy";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ChiDanDiaLy_default",
                "ChiDanDiaLy/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}