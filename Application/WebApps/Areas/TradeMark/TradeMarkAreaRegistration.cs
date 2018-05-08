using System.Web.Mvc;

namespace WebApps.Areas.TradeMark
{
    public class TradeMarkAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TradeMark";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TradeMark_default",
                "TradeMark/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}