namespace WebApps.Areas.Account
{
    using System.Web.Mvc;

    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Account";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Login", id = UrlParameter.Optional } );
        }
    }
}