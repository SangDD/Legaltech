namespace WebApps
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using AppStart;
    using BussinessFacade;
    using BussinessFacade.ModuleMemoryData;
    using Common;
    using Common.CommonData;
    using GemBox.Document;
    using GemBox.Spreadsheet;
    using ObjectInfos;

    public class MvcApplication : HttpApplication
    {
        internal void LoadWebAppDataWhenResetPool()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Configuration.GetConfigAppSetting();
                EmailHelper.EmailOriginal = new EmailInfo()
                {
                    Host = CommonFuc.GetConfig("EMailHost"),
                    Port = Convert.ToInt32(CommonFuc.GetConfig("EmailPost")),
                    EMailFrom = CommonFuc.GetConfig("EMailFrom"),
                    PassWord = CommonFuc.GetConfig("EMailPass"),
                    DisplayName = CommonFuc.GetConfig("DisplayName"),
                    IsSsl = CommonFuc.GetConfig("SSL") == "Y",
                    EmailCC = CommonFuc.GetConfig("EmailCC")
                };

                Logger.Log().Info("Start Application_Start");
                CommonVariables.AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                CommonVariables.KnFileLogin = HttpContext.Current.Server.MapPath(@"~/log/LogInApp" + DateTime.Now.ToString("MMyyyy") + ".log");
                MemoryData.LoadAllMemoryData();

                // thread chuyên load dữ liệu tĩnh khi có sự thay đổi
                Thread _th1 = new Thread(ThreadReloadWhenChangeData);
                _th1.IsBackground = true;
                _th1.Start();

                // tự động change trạng thái của remind
                Thread _th2 = new Thread(ThreadChangeRemind);
                _th2.IsBackground = true;
                _th2.Start();

                Thread _th3 = new Thread(ThreadSendEmail);
                _th3.IsBackground = true;
                _th3.Start();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected virtual void Application_Start()
        {
            //Add key cho gembox 
            SpreadsheetInfo.SetLicense("ETJW-8TZ7-8IQ6-0LAD");

            ComponentInfo.SetLicense("DTFX-2TZ7-8IQ6-VTY3");

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
                string culture = "";
                var httpCookie = Request.Cookies["language"];
                var queryLanguage = "";
                string _patch = Request.CurrentExecutionFilePath;
                if (_patch.ToUpper().Contains("CKFINDER") == true)
                {
                    return;
                }
                bool isAjaxCall = string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);

                if (Request.HttpMethod == WebRequestMethods.Http.Get && !isAjaxCall)
                {
                    string _reg = @"^(.*)(/EN-GB/)(.*)$";
                    Match match = Regex.Match(Request.RawUrl.ToUpper(), _reg);
                    if (Request.RawUrl.Contains("quen-mat-khau") == true)
                    {
                        _reg = @"^(.*)(/en-gb/)(.*)$";
                        match = Regex.Match(Request.RawUrl, _reg);
                    }

                    if (!match.Success)
                    {
                        _reg = @"^(.*)(/VI-VN/)(.*)$";
                        match = Regex.Match(Request.RawUrl.ToUpper(), _reg);
                        if (Request.RawUrl.Contains("quen-mat-khau") == true)
                        {
                            _reg = @"^(.*)(/vi-vn/)(.*)$";
                            match = Regex.Match(Request.RawUrl, _reg);
                        }
                    }

                    if (match.Success && match.Groups.Count == 4)
                    {
                        if (Request.QueryString.Count > 0)
                        {
                            Context.RewritePath(match.Groups[1].Value + "/" + match.Groups[3].Value + "&lang=" + match.Groups[2].Value.Replace(@"/", ""), false);
                        }
                        else
                        {
                            Context.RewritePath(match.Groups[1].Value + "/" + match.Groups[3].Value + "?lang=" + match.Groups[2].Value.Replace(@"/", ""), false);
                        }
                    }
                    else
                    {
                        if (httpCookie != null)
                        {
                            culture = httpCookie.Value;
                            Response.Redirect("~/" + culture.ToLower() + Request.RawUrl, false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            culture = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"].ToString();
                            Response.Redirect("~/" + culture.ToLower() + Request.RawUrl, false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                }

                if (!String.IsNullOrEmpty(Request.QueryString["lang"]))
                {
                    queryLanguage = Request.QueryString["lang"].ToString().ToUpper();
                    if (queryLanguage == "EN-GB")
                    {
                        culture = "en-GB";
                    }
                    else if (queryLanguage == "VI-VN")
                    {
                        culture = "vi-VN";
                    }
                }
                else
                {
                    if (httpCookie != null)
                    {
                        culture = httpCookie.Value;
                    }
                    else
                    {
                        culture = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"].ToString();
                    }
                }
                var language = new HttpCookie("language");
                language.Value = culture;
                language.Expires = DateTime.Now.AddDays(3);
                Response.Cookies.Add(language);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
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

        /// <summary>
        /// thread chuyên load dữ liệu tĩnh khi có sự thay đổi
        /// </summary>
        private void ThreadReloadWhenChangeData()
        {
            while (true)
            {
                try
                {
                    CallBack_Info _CallBack_Info = MemoryData.Dequeue_ChangeData();
                    if (_CallBack_Info != null && _CallBack_Info.Table_Name != null && _CallBack_Info.Table_Name != "")
                    {
                        if (_CallBack_Info.Table_Name == Table_Change.GROUP_USER)
                        {
                            MemoryData.ReloadGroup();
                        }
                        else if (_CallBack_Info.Table_Name == Table_Change.APPHEADER)
                        {
                            MemoryData.GetCacheCustomerInfo();
                        }
                        else if (_CallBack_Info.Table_Name == Table_Change.APP_DDSHCN)
                        {
                            MemoryData.GetCacheDDSHCN();
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(2000);
                    Logger.Log().Error(ex.ToString());
                }
            }
        }

        private void ThreadChangeRemind()
        {
            int _timeSleep = Convert.ToInt16(CommonFuc.GetConfig("TimeSleepSendMail"));
            while (true)
            {
                try
                {
                    //if (DateTime.Now.ToString("HH:mm") == "00:55" && Common.c_is_call_change_remind == false)
                    //{
                    //    B_Remind_BL _bl = new B_Remind_BL();
                    //    _bl.Auto_change_remind();
                    //    Logger.Log().Info("ChangeRemind " + DateTime.Now.ToString("dd/MM/yyyy"));
                    //    Common.c_is_call_change_remind = true;
                    //}
                    //else if (DateTime.Now.ToString("HH:mm") != "00:55")
                    //{
                    //    Common.c_is_call_change_remind = false;
                    //}

                    // đọc thông tin cần gửi email
                    B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                    List<B_Todos_Info> _lst = _B_Todos_BL.GetSend_Email();
                    List<string> _LstAttachment = new List<string>();

                    List<B_Todos_Info> _lst_update = _B_Todos_BL.GetSend_Email();

                    foreach (B_Todos_Info item in _lst)
                    {
                        Email_Info _Email_Info = new Email_Info
                        {
                            EmailTo = item.Email_Send,
                            EmailCC = "",
                            Subject = item.CONTENT,
                            Content = item.CONTENT,
                            LstAttachment = _LstAttachment,
                        };

                        CommonFunction.AppsCommon.EnqueueSendEmail(_Email_Info);

                        // update todo-id
                        _lst_update.Add(item);

                        Thread.Sleep(_timeSleep);
                    }

                    // update todo-id
                    if (_lst_update.Count > 0)
                    {
                        _B_Todos_BL.Update_Todo_Email(_lst_update);
                    }

                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(2000);
                    Logger.Log().Error(ex.ToString());
                }
            }
        }

        void ThreadSendEmail()
        {
            while (true)
            {
                try
                {
                    Email_Info _Email_Info = WebApps.CommonFunction.AppsCommon.Dequeue_SendEmail();
                    if (_Email_Info != null)
                    {
                        bool _ck = EmailHelper.SendMail(_Email_Info.EmailTo, _Email_Info.EmailCC, _Email_Info.Subject, _Email_Info.Content, _Email_Info.LstAttachment);
                        if (_ck == false)
                        {
                            WebApps.CommonFunction.AppsCommon.EnqueueSendEmail(_Email_Info);
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(2000);
                    Logger.Log().Error(ex.ToString());
                }
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
