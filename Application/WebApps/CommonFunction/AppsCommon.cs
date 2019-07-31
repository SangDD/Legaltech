using Common;
using Common.CommonData;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BussinessFacade.ModuleTrademark;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography;
using ObjectInfos;
using GemBox.Document;
using BussinessFacade.ModuleUsersAndRoles;
using BussinessFacade;
using WebApps.Session;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using BussinessFacade.ModuleMemoryData;
using System.Linq;
using ObjectInfos.ModuleTrademark;

namespace WebApps.CommonFunction
{
    public class AppsCommon
    {
        static MyQueue c_QueueSendEmail = new MyQueue();

        public static void EnqueueSendEmail(Email_Info email_Info)
        {
            c_QueueSendEmail.Enqueue(email_Info);
        }

        public static Email_Info Dequeue_SendEmail()
        {
            Email_Info _Email_Info = (Email_Info)c_QueueSendEmail.Dequeue();
            if (_Email_Info != null)
                return _Email_Info;
            else return null;
        }

        public static string gKeyEncrypt = @"L2WcFveA50iHVuucn+bPUw==";

        public static string GetCurrentLang()
        {
            try
            {
                var culture = "";
                var httpCookie = HttpContext.Current.Request.Cookies["language"];
                var queryLanguage = "";
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["lang"]))
                {
                    queryLanguage = HttpContext.Current.Request.QueryString["lang"].ToString().ToUpper();
                    if (queryLanguage == "EN-GB")
                    {
                        culture = "en-GB";
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
                        culture = ConfigurationManager.AppSettings["DefaultLang"].ToString();
                    }
                }
                if (culture.ToUpper().Equals("EN-GB"))
                    culture = Language.LangEN;
                else if (culture.ToUpper().Equals("EN-US") || culture.ToUpper().Equals("VI-VN"))
                    culture = Language.LangVI;
                else culture = Language.LangVI;
                return culture.ToUpper();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Language.LangVI;
            }
        }

        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model = null)
        {
            try
            {
                controllerContext.Controller.ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                    var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                    ViewResult.View.Render(ViewContext, sw);
                    ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string CreateRandomString(int p_length)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                char c; Random rand = new Random();
                for (int i = 0; i < p_length; i++)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                    sb.Append(c);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string EncryptString(string strDataToEncrypt, string strKey = "")
        {
            if (string.IsNullOrEmpty(strKey))
                strKey = gKeyEncrypt;
            //
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(strKey));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(strDataToEncrypt);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        public static string DecryptString(string strEncryptedString, string strKey = "")
        {
            if (string.IsNullOrEmpty(strKey))
                strKey = gKeyEncrypt;
            //
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(strKey));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]

            byte[] DataToDecrypt = Convert.FromBase64String(strEncryptedString);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        public static string Encrypt(string toEncrypt)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(toEncrypt));
                StringBuilder s = new StringBuilder();
                foreach (byte _hashedByte in hashedBytes)
                {
                    s.Append(_hashedByte.ToString("x2"));
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Export_Billing(string p_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(p_case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Search_Report.doc");
                //if (_ApplicationHeaderInfo.Customer_Country != Common.Common.Country_VietNam_Id)
                //    _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Report_EN.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName_exp = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                string fileName_exp_doc = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

                string fileName = System.Web.HttpContext.Current.Server.MapPath(fileName_exp);
                string fileName_doc = System.Web.HttpContext.Current.Server.MapPath(fileName_exp_doc);

                document.MailMerge.FieldMerging += (sender, e) =>
                {
                    if (e.IsValueFound)
                    {
                        if (e.FieldName == "Text")
                            ((Run)e.Inline).Text = e.Value.ToString();
                    }
                };

                document.MailMerge.Execute(new { DateNo = DateTime.Now.ToString("dd-MM-yyyy") });
                document.MailMerge.Execute(new { Case_Name = SearchObject_Header_Info.CASE_NAME });
                document.MailMerge.Execute(new { Client_Reference = SearchObject_Header_Info.CLIENT_REFERENCE });
                document.MailMerge.Execute(new { Case_Code = SearchObject_Header_Info.CASE_CODE });
                document.MailMerge.Execute(new { Master_Name = SearchObject_Header_Info.Customer_Name });
                //document.MailMerge.Execute(new { App_No = SearchObject_Header_Info.App_No });
                document.MailMerge.Execute(new { Customer_Country_Name = SearchObject_Header_Info.Customer_Country_Name });
                document.MailMerge.Execute(new { Bill_Code = _Billing_Header_Info.Case_Code });

                document.MailMerge.Execute(new { Total_Amount = _Billing_Header_Info.Total_Amount.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Total_Pre_Tex = _Billing_Header_Info.Total_Pre_Tex.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Tex_Fee = _Billing_Header_Info.Tex_Fee.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Currency = _Billing_Header_Info.Currency });

                document.MailMerge.Execute(new { Deadline = _Billing_Header_Info.Deadline.ToString("dd/MM/yyyy") });
                document.MailMerge.Execute(new { Billing_Date = _Billing_Header_Info.Billing_Date.ToString("dd/MM/yyyy") });

                // lấy thông tin người dùng
                UserBL _UserBL = new UserBL();
                UserInfo userInfo = _UserBL.GetUserByUsername(SearchObject_Header_Info.CREATED_BY);
                if (userInfo != null)
                {
                    document.MailMerge.Execute(new { Contact_Person = userInfo.Contact_Person + " " + userInfo.FullName });
                    document.MailMerge.Execute(new { Address = userInfo.Address });
                    document.MailMerge.Execute(new { FullName = userInfo.FullName });
                }
                else
                {
                    document.MailMerge.Execute(new { Contact_Person = "" });
                    document.MailMerge.Execute(new { Address = "" });
                    document.MailMerge.Execute(new { FullName = "" });
                }

                DataTable dtDetail = new DataTable();
                dtDetail = ConvertData.ConvertToDatatable<Billing_Detail_Info>(_lst_billing_detail);
                document.MailMerge.Execute(dtDetail, "TEMP");

                document.Save(fileName, SaveOptions.PdfDefault);
                //document.Save(fileName_doc, SaveOptions.DocxDefault);

                byte[] fileContents;
                var options = SaveOptions.PdfDefault;
                // Save document to DOCX format in byte array.
                using (var stream = new MemoryStream())
                {
                    document.Save(stream, options);
                    fileContents = stream.ToArray();
                }
                Convert.ToBase64String(fileContents);

                return fileName_exp;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static List<Billing_Detail_Info> Get_LstFee_Detail(string p_case_code)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = (List<Billing_Detail_Info>)SessionData.GetDataSession(p_case_code);
                if (_lst_billing_detail == null)
                {
                    _lst_billing_detail = new List<Billing_Detail_Info>();
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                return _lst_billing_detail;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Billing_Detail_Info>();
            }
        }

        public static decimal Get_Currentcy_VCB()
        {
            try
            {
                string _url = CommonFuc.GetConfig("Link_VCB");
                var response = new WebClient().DownloadString(_url);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);


                XmlElement _ExrateList = doc["ExrateList"];
                foreach (XmlElement item in _ExrateList)
                {

                    XmlElement _XmlElement = (XmlElement)item;
                    if (_XmlElement != null)
                    {
                        string _currentcy = _XmlElement.GetAttribute("CurrencyCode");
                        if (_currentcy == "USD")
                        {
                            return Convert.ToDecimal(_XmlElement.GetAttribute("Sell"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return 0;
        }

        public static decimal Insert_Billing(string p_Case_Code, string p_note, decimal p_insert_type, ref string p_fileExport)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = AppsCommon.Get_LstFee_Detail(p_Case_Code);
                if (_lst_billing_detail.Count == 0)
                {
                    return -2;
                }

                decimal _ck = 0;

                Billing_BL _Billing_BL = new Billing_BL();
                Billing_Header_Info p_Billing_Header_Info = new Billing_Header_Info();
                p_Billing_Header_Info.Created_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Created_Date = DateTime.Now;
                p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Billing_Header_Info.Status = (decimal)CommonEnums.Billing_Status.New_Wait_Approve;
                p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.App;
                if (p_Billing_Header_Info.App_Case_Code.Contains("SEARCH"))
                    p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.Search;
                p_Billing_Header_Info.Insert_Type = p_insert_type;

                p_Billing_Header_Info.Notes = "Billing for case code " + p_Case_Code + " - " + p_note;

                p_Billing_Header_Info.Case_Code = _Billing_BL.Billing_GenCaseCode();
                p_Billing_Header_Info.App_Case_Code = p_Case_Code;

                p_Billing_Header_Info.Billing_Date = DateTime.Now;
                p_Billing_Header_Info.Deadline = DateTime.Now.AddDays(30);

                p_Billing_Header_Info.Request_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Approve_By = "";


                decimal _idBilling = _Billing_BL.Billing_Insert(p_Billing_Header_Info);
                if (_idBilling > 0 && _lst_billing_detail.Count > 0)
                {
                    _ck = _Billing_BL.Billing_Detail_InsertBatch(_lst_billing_detail, _idBilling);
                }

                p_fileExport = Export_Billing(p_Billing_Header_Info.Case_Code);

                // nếu kết xuất file thành công thì insert vào docking
                Insert_Docketing(p_Billing_Header_Info.App_Case_Code, "Report Billing", p_fileExport, true);

                return _ck;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public static void Insert_Docketing(string p_app_case_code, string p_doc_name, string p_url_File_Atachment, bool p_is_transaction = false)
        {
            try
            {
                // insert vào docking để lưu trữ
                Docking_BL _obj_docBL = new Docking_BL();
                Docking_Info p_Docking_Info = new Docking_Info();
                p_Docking_Info.Created_By = SessionData.CurrentUser.Username;
                p_Docking_Info.Created_Date = DateTime.Now;
                p_Docking_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Docking_Info.Status = (decimal)CommonEnums.Docking_Status.Completed;
                p_Docking_Info.Docking_Type = (decimal)CommonEnums.Docking_Type_Enum.In_Book;
                p_Docking_Info.Document_Type = (decimal)CommonEnums.Document_Type_Enum.Khac;
                p_Docking_Info.Document_Name = p_doc_name;
                p_Docking_Info.In_Out_Date = DateTime.Now;
                p_Docking_Info.Isshowcustomer = 1;
                p_Docking_Info.App_Case_Code = p_app_case_code;

                //
                string[] _arr = p_url_File_Atachment.Split('/');
                if (_arr.Length > 0)
                {
                    p_Docking_Info.FileName = _arr[_arr.Length - 1];
                }

                p_Docking_Info.Url = p_url_File_Atachment;

                if (p_is_transaction == false)
                {
                    _obj_docBL.Docking_Insert(p_Docking_Info);
                }
                else
                {
                    _obj_docBL.Docking_Insert_Transaction(p_Docking_Info);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static List<AllCodeInfo> AllCode_GetBy_CdTypeCdName(string p_cdname, string p_cdtype, string p_lang = "VI_VN")
        {
            try
            {
                if (MemoryData.c_hs_Allcode.ContainsKey(p_cdname + "|" + p_cdtype))
                {
                    List<AllCodeInfo> _lst = new List<AllCodeInfo>();

                    List<AllCodeInfo> _lst_tem = (List<AllCodeInfo>)MemoryData.c_hs_Allcode[p_cdname + "|" + p_cdtype];
                    string language = WebApps.CommonFunction.AppsCommon.GetCurrentLang();

                    foreach (AllCodeInfo item in _lst_tem)
                    {
                        AllCodeInfo _AllCodeInfo = new AllCodeInfo();
                        _AllCodeInfo.CdName = item.CdName;
                        _AllCodeInfo.CdType = item.CdType;
                        _AllCodeInfo.CdVal = item.CdVal;
                        _AllCodeInfo.Content = item.Content;
                        _AllCodeInfo.Content_Eng = item.Content_Eng;
                        _lst.Add(_AllCodeInfo);
                    }

                    _lst = _lst.OrderBy(m => m.LstOdr).ToList();
                    foreach (var item in _lst)
                    {
                        if (language != "VI_VN")
                        {
                            item.Content = item.Content_Eng;
                        }
                    }

                    return _lst;
                }
                else return new List<AllCodeInfo>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AllCodeInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_A01(A01_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                #region 1 Lệ phí nộp đơn
                pDetail.Appcode = "A01";
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Amount_Usd;

                    _AppFeeFixInfo1.Amount_Represent = _AppFeeFixInfo1.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = _AppFeeFixInfo1.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region 2 Phí thẩm định hình thức
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Isuse = pDetail.Point == -1 ? 0 : 1;
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.Point == -1 ? 0 : pDetail.Point;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Amount_Usd;
                    _AppFeeFixInfo2.Amount_Represent = _AppFeeFixInfo2.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = _AppFeeFixInfo2.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;

                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }
                else
                    _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region 2.1 Phí thẩm định hình thức từ trang bản mô tả thứ 7 trở đi
                AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                _AppFeeFixInfo21.Level = 1;
                _AppFeeFixInfo21.Fee_Id = 21;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                decimal _numberPicOver = 5;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo21.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                {
                    AppDocumentInfo _AppDocumentInfo = null;
                    foreach (var item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "A01_02")
                        {
                            _AppDocumentInfo = item;
                        }
                    }

                    if (_AppDocumentInfo != null)
                    {
                        if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                        {
                            _AppFeeFixInfo21.Isuse = 1;
                            _AppFeeFixInfo21.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                            if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            {
                                _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                                _AppFeeFixInfo21.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo21.Number_Of_Patent;

                                _AppFeeFixInfo21.Amount_Represent = _AppFeeFixInfo21.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                                _AppFeeFixInfo21.Amount_Represent_Usd = _AppFeeFixInfo21.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                            }
                            else
                                _AppFeeFixInfo21.Amount = 60000 * _AppFeeFixInfo21.Number_Of_Patent;
                        }
                        else
                        {
                            _AppFeeFixInfo21.Isuse = 0;
                            _AppFeeFixInfo21.Number_Of_Patent = 0;
                            _AppFeeFixInfo21.Amount = 0;
                            _AppFeeFixInfo21.Amount_Represent = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo21.Isuse = 0;
                        _AppFeeFixInfo21.Number_Of_Patent = 0;
                        _AppFeeFixInfo21.Amount = 0;
                        _AppFeeFixInfo21.Amount_Represent = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo21.Isuse = 0;
                    _AppFeeFixInfo21.Number_Of_Patent = 0;
                    _AppFeeFixInfo21.Amount = 0;
                    _AppFeeFixInfo21.Amount_Represent = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo21);

                #endregion

                #region 3 Phí phân loại quốc tế về sáng chế
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();

                if (pDetail.Class_Type == "CUC")
                {
                    _AppFeeFixInfo3.Isuse = 1;
                    _AppFeeFixInfo3.Number_Of_Patent = 1;
                }
                else
                {
                    _AppFeeFixInfo3.Isuse = 0;
                    _AppFeeFixInfo3.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent = _AppFeeFixInfo3.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent_Usd = _AppFeeFixInfo3.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo3.Number_Of_Patent;

                    _AppFeeFixInfo3.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;

                }
                else
                {
                    _AppFeeFixInfo3.Amount = 100000 * _AppFeeFixInfo3.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion

                #region 4 Quyền ưu tiên
                AppFeeFixInfo _AppFeeFixInfo4 = new AppFeeFixInfo();
                _AppFeeFixInfo4.Fee_Id = 4;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo4.Fee_Id.ToString();

                if (pUTienInfo != null && pUTienInfo.Count > 0)
                {
                    _AppFeeFixInfo4.Isuse = 1;
                    _AppFeeFixInfo4.Number_Of_Patent = pUTienInfo.Count;
                }
                else
                {
                    _AppFeeFixInfo4.Isuse = 0;
                    _AppFeeFixInfo4.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo4.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent = _AppFeeFixInfo4.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo4.Amount_Represent_Usd = _AppFeeFixInfo4.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    _AppFeeFixInfo4.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }
                else
                {
                    _AppFeeFixInfo4.Amount = 100000 * _AppFeeFixInfo4.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo4);
                #endregion

                #region 5 Phí thẩm định yêu cầu sửa đổi
                AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                _AppFeeFixInfo5.Fee_Id = 5;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();

                if (pDetail.PCT_Suadoi == 1)
                {
                    _AppFeeFixInfo5.Isuse = 1;
                    _AppFeeFixInfo5.Number_Of_Patent = pUTienInfo.Count;
                }
                else
                {
                    _AppFeeFixInfo5.Isuse = 0;
                    _AppFeeFixInfo5.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent = _AppFeeFixInfo5.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo5.Amount_Represent_Usd = _AppFeeFixInfo5.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    _AppFeeFixInfo5.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }
                else
                {
                    _AppFeeFixInfo5.Amount = 160000 * _AppFeeFixInfo5.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo5);
                #endregion

                #region 6 Phí công bố đơn

                AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                _AppFeeFixInfo6.Fee_Id = 6;
                _AppFeeFixInfo6.Isuse = 1;
                _AppFeeFixInfo6.Number_Of_Patent = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;
                    _AppFeeFixInfo6.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo6.Number_Of_Patent;
                    _AppFeeFixInfo6.Amount_Represent = _AppFeeFixInfo6.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo6.Amount_Represent_Usd = _AppFeeFixInfo6.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;

                    _AppFeeFixInfo6.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }
                else
                {
                    _AppFeeFixInfo6.Amount = 150000 * _AppFeeFixInfo6.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo6);

                #endregion

                #region 6.1 Phí công bố đơn Đơn có trên 1 hình (từ hình thứ 2 trở đi)
                AppFeeFixInfo _AppFeeFixInfo61 = new AppFeeFixInfo();
                _AppFeeFixInfo61.Level = 1;
                _AppFeeFixInfo61.Fee_Id = 61;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo61.Fee_Id.ToString();
                _numberPicOver = 1;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo61.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pLstImagePublic != null && pLstImagePublic.Count > _numberPicOver)
                {
                    _AppFeeFixInfo61.Isuse = 1;
                    _AppFeeFixInfo61.Number_Of_Patent = (pLstImagePublic.Count - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo61.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo61.Number_Of_Patent;
                        _AppFeeFixInfo61.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo61.Number_Of_Patent;
                        _AppFeeFixInfo61.Amount_Represent = _AppFeeFixInfo61.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                        _AppFeeFixInfo61.Amount_Represent_Usd = _AppFeeFixInfo61.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    }
                    else
                        _AppFeeFixInfo61.Amount = 60000 * _AppFeeFixInfo61.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo61.Isuse = 0;
                    _AppFeeFixInfo61.Number_Of_Patent = 0;
                    _AppFeeFixInfo61.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo61);

                #endregion

                #region 6.2 Phí công bố đơn Bản mô tả có trên 6 trang (từ trang thứ 7 trở đi)
                AppFeeFixInfo _AppFeeFixInfo62 = new AppFeeFixInfo();
                _AppFeeFixInfo62.Fee_Id = 62;
                _AppFeeFixInfo62.Level = 1;
                _numberPicOver = 6;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo62.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo62.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                {
                    AppDocumentInfo _AppDocumentInfo = null;
                    foreach (var item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "A01_02")
                        {
                            _AppDocumentInfo = item;
                        }
                    }

                    if (_AppDocumentInfo != null)
                    {
                        if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                        {
                            _AppFeeFixInfo62.Isuse = 1;
                            _AppFeeFixInfo62.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                            if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            {
                                _AppFeeFixInfo62.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo62.Number_Of_Patent;
                                _AppFeeFixInfo62.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo62.Number_Of_Patent;
                                _AppFeeFixInfo62.Amount_Represent = _AppFeeFixInfo62.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                                _AppFeeFixInfo62.Amount_Represent_Usd = _AppFeeFixInfo62.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                            }
                            else
                                _AppFeeFixInfo62.Amount = 8000 * _AppFeeFixInfo62.Number_Of_Patent;
                        }
                        else
                        {
                            _AppFeeFixInfo62.Isuse = 0;
                            _AppFeeFixInfo62.Number_Of_Patent = 0;
                            _AppFeeFixInfo62.Amount = 0;
                            _AppFeeFixInfo62.Amount_Represent = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo62.Isuse = 0;
                        _AppFeeFixInfo62.Number_Of_Patent = 0;
                        _AppFeeFixInfo62.Amount = 0;
                        _AppFeeFixInfo62.Amount_Represent = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo62.Isuse = 0;
                    _AppFeeFixInfo62.Number_Of_Patent = 0;
                    _AppFeeFixInfo62.Amount = 0;
                    _AppFeeFixInfo62.Amount_Represent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo62.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo62.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo62.Amount = 600000 * _AppFeeFixInfo62.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo62);

                #endregion

                #region 7 Phí thẩm định nội dung

                AppFeeFixInfo _AppFeeFixInfo7 = new AppFeeFixInfo();
                _AppFeeFixInfo7.Fee_Id = 7;

                if (pDetail.ThamDinhNoiDung == "TDND")
                {
                    _AppFeeFixInfo7.Isuse = 1;
                    _AppFeeFixInfo7.Number_Of_Patent = pDetail.Point == -1 ? 1 : pDetail.Point;
                }
                else
                {
                    _AppFeeFixInfo7.Isuse = 0;
                    _AppFeeFixInfo7.Number_Of_Patent = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo7.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo7.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo7.Number_Of_Patent;
                    _AppFeeFixInfo7.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo7.Number_Of_Patent;
                    _AppFeeFixInfo7.Amount_Represent = _AppFeeFixInfo7.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo7.Amount_Represent_Usd = _AppFeeFixInfo7.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    _AppFeeFixInfo7.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }
                else
                {
                    _AppFeeFixInfo7.Amount = 720000 * _AppFeeFixInfo7.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo7);

                #endregion

                #region 7.1 Phí thẩm định hình thức từ trang bản mô tả thứ 7 trở đi
                AppFeeFixInfo _AppFeeFixInfo71 = new AppFeeFixInfo();
                _AppFeeFixInfo71.Fee_Id = 71;
                _numberPicOver = 5;
                _AppFeeFixInfo71.Level = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo71.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo71.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }

                if (pDetail.ThamDinhNoiDung == "TDND")
                {
                    // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                    if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                    {
                        AppDocumentInfo _AppDocumentInfo = null;
                        foreach (var item in pAppDocumentInfo)
                        {
                            if (item.Document_Id == "A01_02")
                            {
                                _AppDocumentInfo = item;
                            }
                        }

                        if (_AppDocumentInfo != null)
                        {
                            if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                            {
                                _AppFeeFixInfo71.Isuse = 1;
                                _AppFeeFixInfo71.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                                {
                                    _AppFeeFixInfo71.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo71.Number_Of_Patent;
                                    _AppFeeFixInfo71.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo71.Number_Of_Patent;
                                    _AppFeeFixInfo71.Amount_Represent = _AppFeeFixInfo71.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                                    _AppFeeFixInfo71.Amount_Represent_Usd = _AppFeeFixInfo71.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                                }
                                else
                                    _AppFeeFixInfo71.Amount = 60000 * _AppFeeFixInfo71.Number_Of_Patent;
                            }
                            else
                            {
                                _AppFeeFixInfo71.Isuse = 0;
                                _AppFeeFixInfo71.Number_Of_Patent = 0;
                                _AppFeeFixInfo71.Amount = 0;
                                _AppFeeFixInfo71.Amount_Represent = 0;
                            }
                        }
                        else
                        {
                            _AppFeeFixInfo71.Isuse = 0;
                            _AppFeeFixInfo71.Number_Of_Patent = 0;
                            _AppFeeFixInfo71.Amount = 0;
                            _AppFeeFixInfo71.Amount_Represent = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo71.Isuse = 0;
                        _AppFeeFixInfo71.Number_Of_Patent = 0;
                        _AppFeeFixInfo71.Amount = 0;
                        _AppFeeFixInfo71.Amount_Represent = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo71.Isuse = 0;
                    _AppFeeFixInfo71.Number_Of_Patent = 0;
                    _AppFeeFixInfo71.Amount = 0;
                    _AppFeeFixInfo71.Amount_Represent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo71.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo71.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo71.Amount = 32000 * _AppFeeFixInfo71.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo71);

                #endregion

                #region 72 Phí tra cứu thông tin nhằm phục vụ việc thẩm định
                AppFeeFixInfo _AppFeeFixInfo72 = new AppFeeFixInfo();
                _AppFeeFixInfo72.Fee_Id = 72;
                _AppFeeFixInfo72.Level = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo72.Fee_Id.ToString();
                if (pDetail.ThamDinhNoiDung == "TDND")
                {
                    _AppFeeFixInfo72.Isuse = pDetail.Point == -1 ? 0 : 1;
                    _AppFeeFixInfo72.Number_Of_Patent = pDetail.Point == -1 ? 0 : pDetail.Point;
                }
                else
                {
                    _AppFeeFixInfo72.Isuse = 0;
                    _AppFeeFixInfo72.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo72.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo72.Number_Of_Patent;
                    _AppFeeFixInfo72.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo72.Number_Of_Patent;
                    _AppFeeFixInfo72.Amount_Represent = _AppFeeFixInfo72.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo72.Amount_Represent_Usd = _AppFeeFixInfo72.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    _AppFeeFixInfo72.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }
                else
                    _AppFeeFixInfo72.Amount = 600000 * _AppFeeFixInfo72.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo72);
                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static void Prepare_Data_Export_A01(ref A01_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
            List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos,
            List<AuthorsInfo> _lst_authorsInfos, List<Other_MasterInfo> _lst_Other_MasterInfo,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> _LstDocumentOthersInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                // copy Header
                A01_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                if (app_Detail.Source_PCT == null)
                {
                    app_Detail.PCT_Date = DateTime.MinValue;
                    app_Detail.PCT_Filling_Date_Qt = DateTime.MinValue;
                    app_Detail.PCT_VN_Date = DateTime.MinValue;
                }

                if (app_Detail.Source_DQSC == null)
                {
                    app_Detail.DQSC_Filling_Date = DateTime.MinValue;
                }

                if (app_Detail.Source_GPHI== null)
                {
                    app_Detail.GPHI_Filling_Date = DateTime.MinValue;
                }

                if (app_Detail.Source_PCT == "Y")
                {
                    app_Detail.DQSC_Filling_Date = DateTime.MinValue;
                    app_Detail.GPHI_Filling_Date = DateTime.MinValue;

                    //if (app_Detail.PCT_Date)
                    //{

                    //}
                }
                else if (app_Detail.Source_DQSC == "Y")
                {
                    app_Detail.GPHI_Filling_Date = DateTime.MinValue;
                    app_Detail.PCT_Date = DateTime.MinValue;
                    app_Detail.PCT_Filling_Date_Qt = DateTime.MinValue;
                    app_Detail.PCT_VN_Date = DateTime.MinValue;
                }
                else if (app_Detail.Source_GPHI == "Y")
                {
                    app_Detail.DQSC_Filling_Date = DateTime.MinValue;
                    app_Detail.PCT_Date = DateTime.MinValue;
                    app_Detail.PCT_Filling_Date_Qt = DateTime.MinValue;
                    app_Detail.PCT_VN_Date = DateTime.MinValue;
                }

                // copy tác giả
                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 0)
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[0], 0);
                }

                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 1)
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[1], 1);
                    app_Detail.Author_Others = "Y";
                }
                else
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 1);
                    app_Detail.Author_Others = "N";
                }

                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 2)
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[2], 2);
                }
                else
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 2);
                }

                // copy chủ đơn khác
                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 1)
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 2)
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }

                // copy đơn ưu tiên
                if (pUTienInfo != null && pUTienInfo.Count > 0)
                {
                    A01_Info_Export.CopyUuTienInfo(ref app_Detail, pUTienInfo[0]);
                }
                else
                {
                    A01_Info_Export.CopyUuTienInfo(ref app_Detail, null);
                }

                #region Tài liệu có trong đơn

                if (_LstDocumentOthersInfo != null)
                {
                    foreach (var item in _LstDocumentOthersInfo)
                    {
                        app_Detail.strDanhSachFileDinhKem += item.Documentname + " ; ";
                    }

                    app_Detail.strDanhSachFileDinhKem = app_Detail.strDanhSachFileDinhKem.Substring(0, app_Detail.strDanhSachFileDinhKem.Length - 2);
                }

                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "A01_01")
                    {
                        app_Detail.Doc_Id_1 = item.CHAR01;
                        app_Detail.Doc_Id_102 = item.CHAR02;
                        app_Detail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_02")
                    {
                        app_Detail.Doc_Id_2 = item.CHAR01;
                        app_Detail.Doc_Id_202 = item.CHAR02;

                        app_Detail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_03")
                    {
                        app_Detail.Doc_Id_3_Check = item.Isuse;
                        app_Detail.Doc_Id_3 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_04")
                    {
                        app_Detail.Doc_Id_4 = item.CHAR01;
                        app_Detail.Doc_Id_402 = item.CHAR02;
                        app_Detail.Doc_Id_4_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_05")
                    {
                        app_Detail.Doc_Id_5_Check = item.Isuse;
                        app_Detail.Doc_Id_5 = item.CHAR01;
                    }

                    else if (item.Document_Id == "A01_06")
                    {
                        app_Detail.Doc_Id_6_Check = item.Isuse;
                        app_Detail.Doc_Id_6 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_07")
                    {
                        app_Detail.Doc_Id_7_Check = item.Isuse;
                        app_Detail.Doc_Id_7 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_07")
                    {
                        app_Detail.Doc_Id_8_Check = item.Isuse;
                        app_Detail.Doc_Id_8 = item.CHAR01;
                    }

                    else if (item.Document_Id == "A01_09")
                    {
                        app_Detail.Doc_Id_9 = item.CHAR01;
                        app_Detail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_10")
                    {
                        app_Detail.Doc_Id_10_Check = item.Isuse;
                        app_Detail.Doc_Id_10 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }

                    // quyền ưu tiên A01_12
                    else if (item.Document_Id == "1_TLCMQUT")
                    {
                        app_Detail.Doc_Id_12 = item.CHAR01;
                        app_Detail.Doc_Id_12_Check = item.Isuse;
                    }

                    //A01_13 
                    else if (item.Document_Id == "1_BanSaoDauTien")
                    {
                        app_Detail.Doc_Id_13 = item.CHAR01;
                        app_Detail.Doc_Id_13_Check = item.Isuse;
                    }

                    //A01_14
                    else if (item.Document_Id == "1_GiayChuyenNhuong")
                    {
                        app_Detail.Doc_Id_14 = item.CHAR01;
                        app_Detail.Doc_Id_14_Check = item.Isuse;
                    }

                    // end quyền ưu tiên

                    else if (item.Document_Id == "A01_15")
                    {
                        app_Detail.Doc_Id_15 = item.CHAR01;
                        app_Detail.Doc_Id_15_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_16")
                    {
                        app_Detail.Doc_Id_16 = item.CHAR01;
                        app_Detail.Doc_Id_16_Check = item.Isuse;
                    }
                }

                // nếu ko dùng đơn ưu tiên thì ko có tài liệu quyền ưu tiên
                if (pUTienInfo == null)
                {
                    app_Detail.Doc_Id_12 = "";
                    app_Detail.Doc_Id_13 = "";
                    app_Detail.Doc_Id_14 = "";
                }

                #endregion

                #region Fee
                if (_lst_appFeeFixInfos.Count > 0)
                {
                    foreach (var item in _lst_appFeeFixInfos)
                    {
                        if (item.Fee_Id == 1)
                        {
                            app_Detail.Fee_Id_1 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_1_Check = item.Isuse;

                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 21)
                        {
                            app_Detail.Fee_Id_21 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_21_Check = item.Isuse;
                            app_Detail.Fee_Id_21_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 3)
                        {
                            app_Detail.Fee_Id_3 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_3_Check = item.Isuse;
                            app_Detail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 4)
                        {
                            app_Detail.Fee_Id_4 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_4_Check = item.Isuse;
                            app_Detail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 5)
                        {
                            app_Detail.Fee_Id_5 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_5_Check = item.Isuse;
                            app_Detail.Fee_Id_5_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 6)
                        {
                            app_Detail.Fee_Id_6 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_6_Check = item.Isuse;
                            app_Detail.Fee_Id_6_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 61)
                        {
                            app_Detail.Fee_Id_61 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_61_Check = item.Isuse;
                            app_Detail.Fee_Id_61_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 62)
                        {
                            app_Detail.Fee_Id_62 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_62_Check = item.Isuse;
                            app_Detail.Fee_Id_62_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 7)
                        {
                            app_Detail.Fee_Id_7 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_7_Check = item.Isuse;
                            app_Detail.Fee_Id_7_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 71)
                        {
                            app_Detail.Fee_Id_71 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_71_Check = item.Isuse;
                            app_Detail.Fee_Id_71_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 72)
                        {
                            app_Detail.Fee_Id_72 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_72_Check = item.Isuse;
                            app_Detail.Fee_Id_72_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static List<AppFeeFixInfo> CallFee_3B(App_Detail_PLB01_SDD_Info pDetail)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;

                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Amount_Usd;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                    _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo1);


                #region Phí công bố thông tin đơn sửa đổi
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Isuse = 1;
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;

                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Amount_Usd;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                    _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region Đơn có trên 1 hình (từ hình thứ hai trở đi)
                AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                _AppFeeFixInfo21.Level = 1;
                _AppFeeFixInfo21.Fee_Id = 21;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                decimal _numberPicOver = 1;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo21.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                }

                // nếu số hình > số hình quy định
                if (pDetail.Number_Pic > _numberPicOver)
                {
                    _AppFeeFixInfo21.Isuse = 1;
                    _AppFeeFixInfo21.Number_Of_Patent = (pDetail.Number_Pic - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;

                        _AppFeeFixInfo21.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Amount_Usd;
                        _AppFeeFixInfo21.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                        _AppFeeFixInfo21.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    }
                    else
                        _AppFeeFixInfo21.Amount = 60000 * _AppFeeFixInfo21.Number_Of_Patent;

                }
                else
                {
                    _AppFeeFixInfo21.Isuse = 0;
                    _AppFeeFixInfo21.Number_Of_Patent = 0;
                    _AppFeeFixInfo21.Amount = 0;
                }

                _lstFeeFix.Add(_AppFeeFixInfo21);
                #endregion

                #region Bản mô tả sáng chế có trên 6 trang (từ trang thứ 7 trở đi)  
                AppFeeFixInfo _AppFeeFixInfo22 = new AppFeeFixInfo();
                _AppFeeFixInfo22.Level = 1;
                _AppFeeFixInfo22.Fee_Id = 22;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo22.Fee_Id.ToString();
                decimal _numberPageOver = 6;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo22.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _numberPageOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > số hình quy định
                if (pDetail.Number_Page > _numberPageOver)
                {
                    _AppFeeFixInfo22.Isuse = 1;
                    _AppFeeFixInfo22.Number_Of_Patent = (pDetail.Number_Page - _numberPageOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo22.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo22.Number_Of_Patent;
                        _AppFeeFixInfo22.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Amount_Usd;
                        _AppFeeFixInfo22.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                        _AppFeeFixInfo22.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    }
                    else
                        _AppFeeFixInfo22.Amount = 10000 * _AppFeeFixInfo22.Number_Of_Patent;

                }
                else
                {
                    _AppFeeFixInfo22.Isuse = 0;
                    _AppFeeFixInfo22.Number_Of_Patent = 0;
                    _AppFeeFixInfo22.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo22);
                #endregion
                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static void Prepare_Data_Export_3B(ref App_Detail_PLB01_SDD_Info pDetail, ApplicationHeaderInfo pInfo,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                // copy Header
                App_Detail_PLB01_SDD_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "01_SDD_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;
                        }
                        else if (item.Document_Id == "01_SDD_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "01_SDD_06")
                        {
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_07")
                        {
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_08")
                        {
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "01_SDD_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = AppsCommon.CallFee_3B(pDetail);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }

                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_2 = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_2_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                if (_lstFeeFix.Count > 2)
                {
                    pDetail.Fee_Id_21 = _lstFeeFix[2].Number_Of_Patent;
                    pDetail.Fee_Id_21_Check = _lstFeeFix[2].Isuse;
                    pDetail.Fee_Id_21_Val = _lstFeeFix[2].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[2].Amount;
                }

                if (_lstFeeFix.Count > 3)
                {
                    pDetail.Fee_Id_22 = _lstFeeFix[3].Number_Of_Patent;
                    pDetail.Fee_Id_22_Check = _lstFeeFix[3].Isuse;
                    pDetail.Fee_Id_22_Val = _lstFeeFix[3].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[3].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static int CaculatorFee(List<AppClassDetailInfo> pAppClassInfo, string NumberAppNo, string p_case_code, ref List<AppFeeFixInfo> _lstFeeFix, int pPrviewOrInsert = 0)
        {
            try
            {
                if (NumberAppNo == null)
                {
                    NumberAppNo = "";
                }
                string _keyFee = "";
                int TongSoNhom = 1;
                int SoDongTinhQua = 0;
                int TongSoTinhPhi = 0;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_2011";
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    SoDongTinhQua = CommonFuc.ConvertToInt(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                else
                {
                    SoDongTinhQua = 6;
                }
                if (pAppClassInfo != null && pAppClassInfo.Count > 0)
                {
                    TongSoNhom = CommonFuc.ConvertToInt(pAppClassInfo[0].TongSoNhom);
                    string[] arrSoSanPham = pAppClassInfo[0].TongSanPham.Split('|');
                    for (int i = 0; i < arrSoSanPham.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(arrSoSanPham[i]))
                        {
                            int TotalItemOnGroup = CommonFuc.ConvertToInt(arrSoSanPham[i]);
                            if (TotalItemOnGroup > SoDongTinhQua)
                            {
                                TongSoTinhPhi = TongSoTinhPhi + (TotalItemOnGroup - SoDongTinhQua);
                            }
                        }
                    }
                }
                if (TongSoTinhPhi < 1) TongSoTinhPhi = 0;

                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();

                #region Phí Nộp hồ sơ
                //1.Phí nộp hồ sơ 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 200;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 1; //default là 1 
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 150000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //2.Phí phân loại quốc tế về Nhãn hiệu
                //20.02.2019 SUA THEO YC CHI TUYEN, CHUA RO TINH KHI NAO 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 201;
                _AppFeeFixInfo.Isuse = 0;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 0;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Isuse == 0 ? 0 :   MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                //Tạm thời ram vào =0 
                //if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                //else
                //    _AppFeeFixInfo.Amount = 200000 * _AppFeeFixInfo.Number_Of_Patent;

                _AppFeeFixInfo.Amount = 0;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //3.Tổng số sản phẩm tren nhom 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2011;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                //So sanh vs Char01 neu >Char01 thi moi tinh phi phan tang
                if (_AppFeeFixInfo.Number_Of_Patent > CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01))
                {
                    _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    else
                        _AppFeeFixInfo.Amount = 22000 * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }
                else
                {
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Amount = 0;
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                //4.Số đơn ưu tiên  pDetail.Used_Special
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 203;
                _AppFeeFixInfo.Isuse = 0;
                if (!string.IsNullOrEmpty(NumberAppNo))
                    _AppFeeFixInfo.Isuse = 1;

                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = _AppFeeFixInfo.Isuse;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Isuse == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Isuse;
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Isuse;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //5.Lệ phí công bố đơn
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 204;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 1; //default là 1 
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //6. Phí tra cứu phục vụ thẩm định 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 205;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = TongSoNhom;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = TongSoNhom == 0 ? 0 :  MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 360000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //7.Tổng số sản phẩm tren nhom 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2051;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                //So sanh vs Char01 neu >Char01 thi moi tinh phi phan tang
                if (_AppFeeFixInfo.Number_Of_Patent > CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01))
                {
                    _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    else
                        _AppFeeFixInfo.Amount = 30000 * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    _lstFeeFix.Add(_AppFeeFixInfo);

                }
                else
                {
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Amount = 0;
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }


                //8.Phí thẩm định đơn
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 207;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 550000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //9.Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2071;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
               
                //So sanh vs Char01 neu >Char01 thi moi tinh phi phan tang
                if (_AppFeeFixInfo.Number_Of_Patent > CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01))
                {
                    _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    else
                        _AppFeeFixInfo.Amount = 120000 * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }
                else
                {
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Amount = 0;
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                }

                //Xem trước privew thì ko làm gì cả chỉ tính đẩy vào list thôi 
                if (pPrviewOrInsert != 0)
                {
                    return 0;
                }

                AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                string language = AppsCommon.GetCurrentLang();
                _AppFeeFixBL.AppFeeFixDelete(p_case_code, language);
                return _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -3;
            }
        }

    }


    public enum LangCode
    {
        VI_VN,
        EN_US
    }

    public static class FileAutoVersioningHelper
    {
        private readonly static string _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GeneratePath(string fileName)
        {
            try
            {
                return string.Format("{0}?v={1}", fileName, _version);
            }
            catch
            {
                return string.Format("{0}?v={1}", fileName, DateTime.Now.ToString("dd-MM-yyyy HH"));
            }
        }
    }

    public static class UrlHelperExtensions
    {
        public static string Stylesheet(this UrlHelper helper, string fileName)
        {

            return helper.Content(string.Format("{0}", FileAutoVersioningHelper.GeneratePath(fileName)));
        }

        public static string Script(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("{0}", FileAutoVersioningHelper.GeneratePath(fileName)));
        }
    }


    public class DaiDienChuDon
    {
        public static string DaiDienPL = "DDPL";
        public static string DaiDienSH = "DDSH";
        public static string DaiDienUyQuyen = "DDUQ";
    }


    public class ConvertData
    {

        #region Định dạng datetime

        public const string strDate = "dd/MM/yyyy";
        public const string strDate_Time = "dd/MM/yyyy HH:mm";
        public const string strTimeFormat = "HH:mm:ss";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static DateTime ConvertStringTime2Date(string strTime)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(CorrectStringTime(strTime), strTimeFormat, provider);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }


        /// <summary>
        /// Chuan hoa chuoi ky tu Time truyen vao dam bao phai dung dinh dang HH:mm:ss
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        private static string CorrectStringTime(string strTime)
        {
            try
            {
                string _return = "00:00:00";
                string[] _arr = strTime.Split(':');

                if (_arr.Length >= 3)
                    _return = _arr[0].PadLeft(2, '0') + ":" + _arr[1].PadLeft(2, '0') + ":" + _arr[2].PadLeft(2, '0');
                return _return;
            }
            catch
            {
                return "00:00:00";
            }
        }

        public static string ConvertDate2String(DateTime p_date)
        {
            try
            {
                return p_date.ToString(strDate);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static DateTime ConvertString2Date(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, strDate, provider); // strDate 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        public static DateTime Convert_String_2DateTime(string p_str_datetime)
        {
            try
            {
                string[] _arr_au_date = p_str_datetime.Trim().Split('-');
                string[] _arr_time = _arr_au_date[0].Split(':');
                DateTime _dt_au = ConvertData.ConvertString2Date(_arr_au_date[1].Trim());
                DateTime _dt = new DateTime(_dt_au.Year, _dt_au.Month, _dt_au.Day, Convert.ToInt16(_arr_time[0]), Convert.ToInt16(_arr_time[1]), 0);
                return _dt;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        public static DateTime ConvertString2Date_dd_MM_yyyy(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, "dd-MM-yyyy", provider); // strDate 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert string to datetime format dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="str">ví dụ 17/01/2015 09:10</param>
        /// <returns></returns>
        public static DateTime ConvertString2DateWithTime(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, strDate_Time, provider);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }


        public const string strDate_1 = "yyyy/MM/dd";
        public static string ConvertDate2String_yyyyMMdd(DateTime p_date)
        {
            try
            {
                return p_date.ToString(strDate_1);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
        #endregion

        #region Convert DataTable

        public static void ConvertArrayListToDataTable(ArrayList arrayList, ref DataTable p_dt)
        {
            //DataTable dt = new DataTable();

            if (arrayList.Count != 0)
            {
                ConvertObjectToDataTableSchema(arrayList[0], ref p_dt);
                FillData(arrayList, p_dt);
            }

            //return p_dt;
        }

        public static void ConvertObjectToDataTableSchema(Object o, ref DataTable dt)
        {
            //DataTable dt = new DataTable();
            PropertyInfo[] properties = o.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                DataColumn dc = new DataColumn(property.Name);
                dc.DataType = property.PropertyType; dt.Columns.Add(dc);
            }
            //return dt;
        }

        private static void FillData(ArrayList arrayList, DataTable dt)
        {
            foreach (Object o in arrayList)
            {
                DataRow dr = dt.NewRow();
                PropertyInfo[] properties = o.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    dr[property.Name] = property.GetValue(o, null);
                }
                dt.Rows.Add(dr);
            }
        }

        public static DataTable ConvertToDatatable<T>(IList<T> data, bool p_isAdd_STT = true)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                int SoThuTu = 0;
                object[] values;
                if (p_isAdd_STT == true)
                {
                    table.Columns.Add("STT");
                    values = new object[props.Count + 1];
                }
                else
                    values = new object[props.Count];

                foreach (T item in data)
                {
                    SoThuTu++;
                    for (int i = 0; i < values.Length - 1; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    if (p_isAdd_STT == true)
                    {
                        values[values.Length - 1] = SoThuTu.ToString();
                    }

                    table.Rows.Add(values);
                }
                return table;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataTable();
            }

        }

        public static DataSet ConvertToDataSet<T>(IList<T> data, bool p_isAdd_STT = true)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                int SoThuTu = 0;
                object[] values;
                if (p_isAdd_STT == true)
                {
                    table.Columns.Add("STT");
                    values = new object[props.Count + 1];
                }
                else
                {
                    values = new object[props.Count];
                }

                foreach (T item in data)
                {
                    SoThuTu++;
                    for (int i = 0; i <= values.Length - 1; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    if (p_isAdd_STT == true)
                    {
                        values[values.Length - 1] = SoThuTu.ToString();
                    }

                    table.Rows.Add(values);
                }

                DataSet _ds = new DataSet();
                _ds.Tables.Add(table);
                return _ds;
            }
            catch (Exception)
            {
                return new DataSet();
            }

        }

        #endregion
    }
}