<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="CKFinder.Settings.ConfigFile" %>
<%@ Import Namespace="CKFinder.Settings" %>
<script runat="server"> 
    public override bool CheckAuthentication()
    {
        if (Administrator.SessionData.CurrentUser == null)
            return false;
        return true;
    }
    /// <summary>
    /// HungTD:
    /// </summary>
    private string get_folder_name(string p_full_path)
    {
        try
        {
            if (p_full_path.Contains("\\"))
            {
                p_full_path = p_full_path.Replace('\\', '/');
            }
            int _vitri = p_full_path.LastIndexOf("/");
            int _len = p_full_path.Length;
            string _folder_name = p_full_path.Substring(_vitri + 1, _len - _vitri - 1);
            return _folder_name;
        }
        catch (Exception ex)
        {
            Administrator.Common.log.Error(ex.ToString());
            return "";
        }
    }
    /**
     * All configuration settings must be defined here.
     */
    static ArrayList _arr_true_folder = new ArrayList();

    //Hungtd: arr luu cac folder khong duoc thuc hien quyen
    static ArrayList _arr_false_folder = new ArrayList();


    /// <summary>
    ///  Khi bàn giao trên máy 30 thì mở hàm này ra
    /// </summary>
    public override void SetConfig()
    {
        //HungTD: kiem tra quyen
        //if (Administrator.SessionData.CurrentUser == null)
        //    return  ;
        var _user_info = (NaviObjectInfo.ModuleQLKHInfo.UserInfo)Administrator.SessionData.CurrentUser;
        Session["CKFinder_UserRole"] = _user_info.UserName;
        ArrayList _arr_sub_folder_in_ck = (ArrayList)Session[_user_info.UserName + "g_arr_subfolder_in_ckfinder"];
        ArrayList _arr_user_folder_rule = (ArrayList)Session[_user_info.UserName + "g_arr_user_folder_rule"];

        LicenseName = "";
        LicenseKey = "";

        //Sửa ngày 12.03.2016
        //Nếu chạy qua host thì upload vào thư mục này 
        BaseUrl = Administrator.Common.Portal_Display_Ckeditor_Admin;
        //BaseDir duong dan tuyet doi de upload anh tren server
        BaseDir = Administrator.Common.Portal_Directory_Ftp;
        if (Request.Url.ToString().Contains("http://localhost"))
        {
            //Đường dẫn hiển thị trên web khi xem sửa 
            BaseUrl = "~/Content";
            //Đường dẫn tuyệt đối của FTP nằm trong thư mục public của Portal để đẩy vào để Admin 
            BaseDir = HttpContext.Current.Server.MapPath("~/Content");
        }


        Plugins = new string[] {
        };
        // Settings for extra plugins.
        PluginSettings = new Hashtable();
        PluginSettings.Add("ImageResize_smallThumb", "90x90");
        PluginSettings.Add("ImageResize_mediumThumb", "120x120");
        PluginSettings.Add("ImageResize_largeThumb", "180x180");
        // Name of the watermark image in plugins/watermark folder
        PluginSettings.Add("Watermark_source", "logo.gif");
        PluginSettings.Add("Watermark_marginRight", "5");
        PluginSettings.Add("Watermark_marginBottom", "5");
        PluginSettings.Add("Watermark_quality", "90");
        PluginSettings.Add("Watermark_transparency", "80");
        CheckSizeAfterScaling = true;
        DisallowUnsafeCharacters = true;
        CheckDoubleExtension = true;
        ForceSingleExtension = true;
        HtmlExtensions = new string[] { "html", "htm", "xml", "js" };
        HideFolders = new string[] { ".*", "CVS" };
        HideFiles = new string[] { ".*" };
        // Perform additional checks for image files.
        SecureImageUploads = true;
        RoleSessionVar = "CKFinder_UserRole";
        //neu user khong co quyen nao 
        AccessControl acl_admin = AccessControl.Add();
        acl_admin = AccessControl.Add();
        //acl_admin.Role = _user_info.UserName;
        acl_admin.ResourceType = "*";
        acl_admin.Folder = "";
        acl_admin.FolderView = true;
        acl_admin.FolderCreate = false;
        acl_admin.FolderRename = false;
        acl_admin.FolderDelete = false;
        acl_admin.FileView = true;
        acl_admin.FileUpload = true;
        acl_admin.FileRename = true;
        acl_admin.FileDelete = true;
        //}
        //load lan thu 2 duyet cac folder con         
        if (Administrator.CommonData.g_count_load_setconfig > 0)
        {
            _arr_false_folder = new ArrayList();
            _arr_true_folder = new ArrayList();
            int i = 0;
            int j;
            foreach (string _folder_name in _arr_sub_folder_in_ck)
            {
                //NaviCommon.Common.log.Error(_folder_name);
                j = 0;
                foreach (NaviObjectInfo.ModuleUserRoles.UserFolderRuleInfo _item in _arr_user_folder_rule)
                {

                    if (_item.Folder_Href == _folder_name)
                    {
                        //luu cac folder duoc thuc hien quyen vao mot noi
                        _arr_true_folder.Add(i);
                        break;
                    }
                    j++;
                }
                //neu folder khong thuoc quyen thi add vao arr_false_folder
                if (j == _arr_user_folder_rule.Count)
                {
                    _arr_false_folder.Add(i);
                }
                i++;
            }
            NaviCommon.Common.log.Error("Item trong thang _arr_true_folder");
            foreach (int _item in _arr_true_folder)
            {

                string _str_folder_name = _arr_sub_folder_in_ck[_item].ToString();
                acl_admin = AccessControl.Add();
                acl_admin.Role = _user_info.UserName;
                acl_admin.ResourceType = "*";
                acl_admin.Folder = _str_folder_name;
                acl_admin.FolderView = true;
                acl_admin.FolderCreate = true;
                acl_admin.FolderRename = true;
                acl_admin.FolderDelete = true;
                acl_admin.FileView = true;
                acl_admin.FileUpload = true;
                acl_admin.FileRename = true;
                acl_admin.FileDelete = true;
                NaviCommon.Common.log.Error(_str_folder_name);
            }
            // han che quyen cac folder trong _arr_false_folder
            NaviCommon.Common.log.Error("Item trong thang _arr_false_folder");
            foreach (int _item in _arr_false_folder)
            {
                string _str_folder_name = _arr_sub_folder_in_ck[_item].ToString();
                acl_admin = AccessControl.Add();
                acl_admin.Role = _user_info.UserName;
                acl_admin.ResourceType = "*";
                acl_admin.Folder = get_folder_name(_str_folder_name);
                acl_admin.FolderView = false;
                acl_admin.FolderCreate = false;
                acl_admin.FolderRename = false;
                acl_admin.FolderDelete = false;
                acl_admin.FileView = false;
                acl_admin.FileUpload = false;
                acl_admin.FileRename = false;
                acl_admin.FileDelete = false;
                NaviCommon.Common.log.Error(_str_folder_name);
            }
            //doi voi thu muc Photo thi set mot so quyen khac
            acl_admin = AccessControl.Add();
            //acl_admin.Role = _user_info.UserName;
            acl_admin.ResourceType = "*";
            //acl_admin.Folder = _user_info.UserName;
            acl_admin.FolderView = true;
            acl_admin.FolderCreate = false;
            acl_admin.FolderRename = false;
            acl_admin.FolderDelete = false;
            acl_admin.FileView = true;
            acl_admin.FileUpload = true;
            acl_admin.FileRename = true;
            acl_admin.FileDelete = true;

            acl_admin = AccessControl.Add();
            //acl_admin.Role = _user_info.UserName;
            acl_admin.ResourceType = "*";
            //acl_admin.Folder = _user_info.UserName + "/News";
            acl_admin.FolderView = true;
            acl_admin.FolderCreate = true;
            acl_admin.FolderRename = false;
            acl_admin.FolderDelete = false;
            acl_admin.FileView = true;
            acl_admin.FileUpload = true;
            acl_admin.FileRename = true;
            acl_admin.FileDelete = true;
        }
        // them quyen cho cac folder da luu           

        ResourceType type;


        type = ResourceType.Add("Images");
        type.Url = BaseUrl;
        type.Dir = BaseDir;
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "bmp", "gif", "jpeg", "jpg", "png" };
        type.DeniedExtensions = new string[] { };

        type = ResourceType.Add("Files");
        type.Url = BaseUrl;
        type.Dir = BaseDir;
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "7z", "aiff", "asf", "avi", "bmp", "csv", "doc", "docx", "fla", "flv", "gif", "gz", "gzip", "jpeg", "jpg", "mid", "mov", "mp3", "mp4", "mpc", "mpeg", "mpg", "ods", "odt", "pdf", "png", "ppt", "pptx", "pxd", "qt", "ram", "rar", "rm", "rmi", "rmvb", "rtf", "sdc", "sitd", "swf", "sxc", "sxw", "tar", "tgz", "tif", "tiff", "txt", "vsd", "wav", "wma", "wmv", "xls", "xlsx", "zip" };
        type.DeniedExtensions = new string[] { };


        type = ResourceType.Add("Flash");
        type.Url = BaseUrl;
        type.Dir = BaseDir;
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "swf", "flv" };
        type.DeniedExtensions = new string[] { };
        //bien nay de kiem tra goi ham config lan bao nhieu
        Administrator.CommonData.g_count_load_setconfig++;
    }

</script>
