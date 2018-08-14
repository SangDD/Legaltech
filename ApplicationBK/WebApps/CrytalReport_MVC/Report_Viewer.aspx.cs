using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using Common.CommonData;
using WebApps.Session;
using Common;
using ObjectInfos;
using BussinessFacade.ModuleTrademark;
using WebApps.CommonFunction;
using ObjectInfos.ModuleTrademark;
using BussinessFacade.ModuleMemoryData;

namespace WebApps.CrytalReport_MVC
{
    public partial class Report_Viewer : System.Web.UI.Page
    {
        // 
        string c_strRptFileName = "";                           // tên file báo cáo trong thư mục Report
        string gv_strRptName = "";                              // tên STORED_NAME
        string gv_strReportName = "";

        Dictionary<string, string> htParam = new Dictionary<string, string>();

        DataSet mv_dsData;
        decimal mv_AppHeader_Id;
        string mv_PaperSize = "";

        // Thông tin header
        ApplicationHeaderInfo c_ApplicationHeaderInfo = new ApplicationHeaderInfo();

        //string Appcode = "", Master_Name = "", Master_Address = "", Master_Phone = "", Master_Fax = "", Master_Email = "";
        //string Rep_Master_Type = "", Rep_Master_Name = "", Rep_Master_Address = "", Rep_Master_Phone = "", Rep_Master_Fax = "", Rep_Master_Email = "", Relationship = "";

        //string Str_Send_Date = "", Str_Filing_Date = "", Str_Accept_Date;


        

        #region Fomulator

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                gv_strRptName = Request.QueryString[Report_Enums.gc_RPT_NAME];
                gv_strReportName = Request.QueryString[Report_Enums.gc_TITLE_NAME];
                mv_AppHeader_Id = Convert.ToDecimal(Request.QueryString[Report_Enums.gc_FilterAppHeaderId]);
                if (gv_strRptName == null)
                {
                    Response.Redirect("/home/index");
                }
                else
                {
                    DoReport();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cmdExit_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;
                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        oRpt.Close();
                        oRpt.Dispose();
                        rptViewer.Dispose();
                    }
                    _hs_tem.Remove(gv_strRptName);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;

                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        rptViewer.ReportSource = oRpt;
                        int _zoom = Convert.ToInt16(cboZoom.SelectedValue.ToString());
                        rptViewer.Zoom(_zoom);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cmdExportPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gv_strRptName = Get_Request_string(Report_Enums.gc_RPT_NAME);
                gv_strReportName = Get_Request_string(Report_Enums.gc_TITLE_NAME);

                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;
                string _FileName = gv_strReportName + "_" + DateTime.Now.ToString("yyyyMMdd");
                _FileName = _FileName.Replace(",", "");

                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        ExportOptions crExportOptions = oRpt.ExportOptions;
                        crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                        crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        crExportOptions.FormatOptions = new PdfFormatOptions();

                        // size 
                        if (mv_PaperSize.ToLower() == "a3")
                        {
                            oRpt.PrintOptions.PaperSize = PaperSize.PaperA3;
                        }

                        // xoay dọc hay ngang
                        if (cboOrientation.SelectedValue.ToString() == "1")
                            oRpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                        else
                            oRpt.PrintOptions.PaperOrientation = PaperOrientation.Landscape;

                        // Kết xuất không dùng chữ ký
                        Export_PDF_Non_Signal(oRpt, _FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cmdNext_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;

                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        rptViewer.ReportSource = oRpt;
                        rptViewer.ShowNextPage();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cmdBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;

                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        rptViewer.ReportSource = oRpt;
                        rptViewer.ShowPreviousPage();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cmdFirst_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;

                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        rptViewer.ReportSource = oRpt;
                        rptViewer.ShowFirstPage();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        protected void cmdLast_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                Hashtable _hs_tem = _hs_opt;

                if (_hs_tem.ContainsKey(gv_strRptName))
                {
                    ReportDocument oRpt = (ReportDocument)_hs_tem[gv_strRptName];
                    if (oRpt != null)
                    {
                        rptViewer.ReportSource = oRpt;
                        rptViewer.ShowLastPage();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion

        #region Methods

        void DoReport()
        {
            try
            {
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new ReportDocument();
                //c_strRptFileName = "";
                //if (MemoryData.c_dic_report.ContainsKey(gv_strRptName.ToUpper()) == true)
                //{
                //    // lấy tên file báo cáo
                //    c_strRptFileName = MemoryData.c_dic_report[gv_strRptName.ToUpper()].Rpt_File_Name;
                //}
                //else return;

                c_strRptFileName = "TM_PLB01SDD.rpt";

                // lấy thông tin về đơn chung
                Get_AppHeaderbyId();

                Create_Each_Report(gv_strRptName);

                string mv_strLocation = Server.MapPath("~/Report/") + c_strRptFileName;
                if (System.IO.File.Exists(mv_strLocation) == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", "alert(\"Không tồn tại File báo cáo!\");", true);
                    return;
                }

                // load rpt
                oRpt.Load(mv_strLocation);
                if (mv_dsData != null)
                {
                    oRpt.SetDataSource(mv_dsData);
                }
                // tạo mới báo cáo .
                oRpt.Refresh();

                // Truyền các tham số Formular vào báo cáo
                Puth_Data2_Fomulator(ref oRpt);

                DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExportOptions crExportOptions = oRpt.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crExportOptions.FormatOptions = new PdfFormatOptions();

                // xoay dọc hay ngang
                if (cboOrientation.SelectedValue.ToString() == "1")
                    oRpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                else
                    oRpt.PrintOptions.PaperOrientation = PaperOrientation.Landscape;

                // Kết xuất không dùng chữ ký
                string _FileName = gv_strRptName + "_" + DateTime.Now.ToString("yyyyMMdd");
                Export_PDF_Non_Signal(oRpt, _FileName);

                //// View báo cáo  
                //rptViewer.ReportSource = oRpt;
                //rptViewer.ShowFirstPage();

                //// phần này lâu rồi ko nhớ để làm gì

                //if (SessionData.GetDataSession("oRpt") == null)
                //{
                //    Hashtable _hs_opt_khoitao = new Hashtable();
                //    SessionData.SetDataSession("oRpt", _hs_opt_khoitao);
                //}
                //Hashtable _hs_opt = (Hashtable)SessionData.GetDataSession("oRpt");
                //Hashtable _hs_tem = _hs_opt;
                //_hs_tem[gv_strRptName] = oRpt;

                //SessionData.SetDataSession("oRpt", _hs_tem);
            }
            catch (CrystalDecisions.CrystalReports.Engine.LogOnException ex1)
            {
                rptViewer.Dispose();
                Logger.Log().Error(ex1.ToString());
            }
            catch (Exception ex)
            {
                rptViewer.Dispose();
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// Truyền các tham số Formular vào báo cáo
        /// </summary>
        void Puth_Data2_Fomulator(ref ReportDocument oRpt)
        {
            try
            {
                #region Truyền các tham số Formular vào báo cáo
                // 
                FormulaFieldDefinitions crFFieldDefinitions = oRpt.DataDefinition.FormulaFields;
                FormulaFieldDefinition crFFieldDefinition;
                string strFormulaName;

                for (int i = 0; i < crFFieldDefinitions.Count; i++)
                {
                    crFFieldDefinition = crFFieldDefinitions[i];
                    strFormulaName = crFFieldDefinition.Name.ToUpper();
                    #region Thông tin đơn chung
                    if (strFormulaName == "F_APPHEADER_ID")
                    {
                        crFFieldDefinition.Text = "'" + mv_AppHeader_Id + "'";
                    }
                    else if (strFormulaName == "F_MASTER_NAME")
                    {
                        crFFieldDefinition.Text = "'" + c_ApplicationHeaderInfo.Master_Name + "'";
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        string Get_Request_string(string p_name)
        {
            if (Request.QueryString[p_name] != null)
            {
                return Request.QueryString[p_name];
            }
            else return "";
        }

        /// <summary>
        /// put dữ liệu vào dataset của báo cáo
        /// </summary>
        void PutData_2DataSet_Report(DataSet ds, int p_index_table, string p_table_name = "")
        {
            try
            {
                if (ds.Tables.Count >= p_index_table + 1)
                {
                    DataSet _ds = new DataSet();
                    DataTable _dt = ds.Tables[p_index_table].Copy();
                    if (p_table_name == "")
                        _dt.TableName = "Table";
                    else
                        _dt.TableName = p_table_name;
                    _ds.Tables.Add(_dt);
                    mv_dsData = _ds;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void Get_AppHeaderbyId(decimal p_AppHeaderId = 0)
        {
            try
            {
                Application_Header_BL _Application_Header_BL = new Application_Header_BL();
                c_ApplicationHeaderInfo = _Application_Header_BL.GetApplicationHeader_ById(p_AppHeaderId, AppsCommon.GetCurrentLang());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void Export_PDF_Non_Signal(ReportDocument oRpt, string _FileName)
        {
            try
            {
                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                string _filePathSrc = Server.MapPath("/Content/Export" + _FileName + "_Src.pdf");
                File.WriteAllBytes(_filePathSrc, byteArray.ToArray()); // Requires System.Linq

                // đọc file
                MemoryStream stream = new MemoryStream();
                using (FileStream file = new FileStream(_filePathSrc, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    stream.Write(bytes, 0, (int)file.Length);
                }

                HttpContext curContext = HttpContext.Current;
                _FileName = _FileName.Replace(",", "");
                curContext.Response.Clear();
                curContext.Response.ContentType = "application/pdf";//vnd.ms-word | vnd.openxmlformats-officedocument.wordprocessingml.document
                curContext.Response.AppendHeader("Content-Disposition", "attachment; filename=" + _FileName + ".pdf" + "");

                stream.WriteTo(curContext.Response.OutputStream);

                curContext.Response.Flush();
                curContext.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();


                // --- XÓA THÔNG TIN MẤY CÁI FIEL VỪA LÀM SONG
                if (System.IO.File.Exists(_filePathSrc))
                {
                    try
                    {
                        System.IO.File.Delete(_filePathSrc);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion

        #region Xử lý từng báo cáo 1

        void Create_Each_Report(string pv_strReportName)
        {
            try
            {
                // tạm dừng đợt đấu giá
                if (pv_strReportName == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
                {
                    TM_PLB01SDD();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void TM_PLB01SDD()
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                App_Detail_PLB01_SDD_Info app_Detail = new App_Detail_PLB01_SDD_Info();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();

                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                mv_dsData = objBL.GetByID_DS(mv_AppHeader_Id, language);

                // copy Header
                App_Detail_PLB01_SDD_Info.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                string _strCml = TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD + DateTime.Now.ToString("ddMMyyyHH24mm") + ".xml";
                mv_dsData.WriteXml(Server.MapPath(_strCml), XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion
    }
}