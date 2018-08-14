using System.Web.Mvc;

namespace WebApps.Areas.DaiDienSHCN
{
    public class DaiDienSHCNAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DaiDienSHCN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DaiDienSHCN_default",
                "DaiDienSHCN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}