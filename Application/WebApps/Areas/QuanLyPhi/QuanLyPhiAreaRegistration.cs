using System.Web.Mvc;

namespace WebApps.Areas.QuanLyPhi
{
    public class QuanLyPhiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuanLyPhi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QuanLyPhi_default",
                "QuanLyPhi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}