using System.Web.Mvc;

namespace WebApps.Areas.DKQT
{
    public class DKQTAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DKQT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DKQT_default",
                "DKQT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}