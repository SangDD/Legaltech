namespace WebApps.Areas.Account
{
    using System.Web.Mvc;

    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "account11";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Account_default",
                "account11/{controller}/{action}/{id}",
                new { action = "Login", id = UrlParameter.Optional },
                new[] { "WebApps.Controllers" });
        }
    }
}