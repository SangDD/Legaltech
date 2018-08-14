namespace WebApps.ExtensionObject
{
	using System.IO;
	using System.Web.Mvc;

	public class ControllerExtension : Controller
	{
		public static string RenderRazorViewToString(string viewName, object model, ViewDataDictionary viewData, ControllerContext controllerContext, TempDataDictionary tempData)
		{
			viewData.Model = model;
			using (var sw = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
				var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
				viewResult.View.Render(viewContext, sw);
				viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
				return sw.GetStringBuilder().ToString();
			}
		}
	}
}