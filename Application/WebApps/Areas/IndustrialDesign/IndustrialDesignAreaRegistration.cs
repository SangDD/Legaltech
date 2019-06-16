using System.Web.Mvc;

namespace WebApps.Areas.IndustrialDesign
{
    public class IndustrialDesignAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "IndustrialDesign";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IndustrialDesign_default",
                "IndustrialDesign/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}