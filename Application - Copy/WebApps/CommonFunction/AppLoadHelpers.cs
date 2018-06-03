using Common;
using System;
using System.Web;

namespace WebApps
{
    public class AppUpload
    {
        public static string Logo = "Logo";
        public static string FileAttact = "FileAttact";
        public static string Document = "Document";
    }

    public class   AppLoadHelpers
    {
        /// <summary>
        /// Upload file 
        /// </summary>
        /// <param name="pFiles">HttpPostedFileBase </param>
        /// <param name="pType">AppUpload.Logo</param>
        /// <returns>path file save database </returns>
        public  static string PushFileToServer(System.Web.HttpPostedFileBase pFiles, string pType)
        {
            try
            {
                if (pFiles == null) return "";
                var name = pFiles.FileName;
                name = System.IO.Path.GetExtension(pFiles.FileName);
                string path = "~/Content/Archive/" + pType + "/";
                var f_part = HttpContext.Current.Server.MapPath(path) + pFiles.FileName;
                pFiles.SaveAs(f_part);
                return path + pFiles.FileName;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }

        }
    }
}