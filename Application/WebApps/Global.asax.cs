namespace WebApps
{
	using System;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;

	using AppStart;

	using BussinessFacade.ModuleMemoryData;

	using Common;
	using Common.CommonData;

	using RequestFilter;

	public class MvcApplication : HttpApplication
    {
        internal void LoadWebAppDataWhenResetPool()
        {
            try
            {
	            Configuration.GetConfigAppSetting();
                Logger.Log().Info("Start Application_Start");
                log4net.Config.XmlConfigurator.Configure();
	            CommonVariables.AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
	            CommonVariables.KnFileLogin = HttpContext.Current.Server.MapPath(@"~/log/LogInApp" + DateTime.Now.ToString("MMyyyy") + ".log");
                MemoryData.LoadAllMemoryData();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected virtual void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            this.LoadWebAppDataWhenResetPool();
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
            Logger.Log().Info("----------Stop host ------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "  ------------------------");
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Logger.Log().Error(e.ToString());
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logger.Log().Error("Error at Application_BeginRequest: " + ex);
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
   //         if (HttpContext.Current.Session == null) return;

			//var identityRequest = new IdentityRequest();
			//identityRequest.Identity(this.Context.Request);
			//if (!identityRequest.IsRequestIdentity)
			//{
			//	this.Response.Redirect(RouteConfig.KnFilterRequestNotIdentity
			//		+ "?requestMethod=" + identityRequest.RequestMethod
			//		+ "&isRequestTypeAjax=" + identityRequest.IsRequestTypeAjax
			//		+ "&urlRedirect=" + identityRequest.ResponeRedirectUrl
			//		+ "&returnUrl=" + identityRequest.ReturnUrl);
			//}
		}

        protected void Application_PreSendRequestHeaders()
        {
            try
            {
                Response.Headers.Remove("Server");
            }
            catch (Exception ex)
            {
                Logger.Log().Error("Error at Application_PreSendRequestHeaders: " + ex);
            }
        }
    }

	public class MyHttpHandler : MvcHandler
	{
		public MyHttpHandler(RequestContext requestContext)
			: base(requestContext)
		{
		}
	}
}
