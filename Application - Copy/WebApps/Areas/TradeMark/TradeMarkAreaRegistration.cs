
namespace WebApps.Areas.TradeMark
{
    using System.Web.Mvc;

    public class TradeMarkAreaRegistration : AreaRegistration
    {
        public override string AreaName => "TradeMark";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TradeMark_default",
                "TradeMark/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "WebApps.Controllers" }
            );
        }
    }
}
