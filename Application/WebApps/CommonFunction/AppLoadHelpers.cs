using Common;
using System;
using System.Text;
using System.Web;

namespace WebApps
{
    public class AppUpload
    {
        public static string Logo = "Logo";
        public static string FileAttact = "FileAttact";
        public static string Document = "Document";
        public static string Wiki = "Wiki";
        public static string App = "App";
        public static string Search = "Search";
    }

    public class AppLoadHelpers
    {
        /// <summary>
        /// Upload file 
        /// </summary>
        /// <param name="pFiles">HttpPostedFileBase </param>
        /// <param name="pType">AppUpload.Logo</param>
        /// <returns>path file save database </returns>
        public static string PushFileToServer(System.Web.HttpPostedFileBase pFiles, string pType)
        {
            try
            {
                if (pFiles == null) return "";
                //var name = pFiles.FileName;
                string _extension = System.IO.Path.GetExtension(pFiles.FileName);
                string path = "/Content/Archive/" + pType + "/";

                string _filename = pFiles.FileName;
                _filename = convertToUnSign2(_filename);
                _filename = System.Text.RegularExpressions.Regex.Replace(_filename, "[^0-9A-Za-z.]+", "_");
                _filename = _filename + DateTime.Now.Ticks.ToString();
                //_filename = Rename_file(pFiles.FileName);

                var f_part = HttpContext.Current.Server.MapPath(path) + _filename;
                pFiles.SaveAs(f_part);
                return path + _filename;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }

        }

        public static string convertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static string Rename_file(string p_file_name)
        {
            try
            {
                string _re_file_name = p_file_name;
                var VietNamKey = "á,à,ạ,ả,ã,â,ấ,ầ,ậ,ẩ,ẫ,ă,ắ,ằ,ặ,ẳ,ẵ,é,è,ẹ,ẻ,ẽ,ê,ế,ề,ệ,ể,ễ,ó,ò,ọ,ỏ,õ,ô,ố,ồ,ộ,ổ,ỗ,ơ,ớ,ờ,ợ,ở,ỡ,ú,ù,ụ,ủ,ũ,ư,ứ,ừ,ự,ử,ữ,í,ì,ị,ỉ,ĩ,đ,ý,ỳ,ỵ,ỷ,ỹ,Á,À,Ạ,Ả,Ã,Â,Ấ,Ầ,Ậ,Ẩ,Ẫ,Ă,Ắ,Ằ,Ặ,Ẳ,Ẵ,É,È,Ẹ,Ẻ,Ẽ,Ê,Ế,Ề,Ệ,Ể,Ễ,Ó,Ò,Ọ,Ỏ,Õ,Ô,Ố,Ồ,Ộ,Ổ,Ỗ,Ơ,Ớ,Ờ,Ợ,Ở,Ỡ,Ú,Ù,Ụ,Ủ,Ũ,Ư,Ứ,Ừ,Ự,Ử,Ữ,Í,Ì,Ị,Ỉ,Ĩ,Đ,Ý,Ỳ,Ỵ,Ỷ,Ỹ";

                string[] _arr = VietNamKey.Split(',');
                foreach (var item in _arr)
                {
                    if (p_file_name.Contains(item))
                    {
                        char[] charArr = item.ToCharArray();
                        if (charArr.Length > 0)
                        {
                            p_file_name = p_file_name.Replace(charArr[0], '_');
                        }
                    }
                }

                //var char_not_allow = "\,/,:,*,?,\",<,>,|";
                _re_file_name.Replace(":", "_");
                _re_file_name.Replace("*", "_");
                _re_file_name.Replace("?", "_");
                _re_file_name.Replace(">", "_");
                _re_file_name.Replace("<", "_");
                _re_file_name.Replace("|", "_");

                string _string = @"\";
                char[] _char = _string.ToCharArray();
                if (_char.Length > 0)
                {
                    p_file_name = p_file_name.Replace(_char[0], '_');
                }

                _string = @"/";
                _char = _string.ToCharArray();
                if (_char.Length > 0)
                {
                    p_file_name = p_file_name.Replace(_char[0], '_');
                }

                p_file_name = p_file_name.Replace(' ', '_');

                return p_file_name;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
    }
}