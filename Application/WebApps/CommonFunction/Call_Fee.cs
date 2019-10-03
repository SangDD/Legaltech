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
using CrystalDecisions.Shared;

namespace WebApps.CommonFunction
{
    public class Call_Fee
    {
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
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                    _AppFeeFixInfo1.Amount_Represent = _AppFeeFixInfo1.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = _AppFeeFixInfo1.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;
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
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = _AppFeeFixInfo2.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = _AppFeeFixInfo2.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo2.Number_Of_Patent;

                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                    _AppFeeFixInfo21.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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

                                _AppFeeFixInfo21.Amount_Represent = _AppFeeFixInfo21.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo21.Number_Of_Patent;
                                _AppFeeFixInfo21.Amount_Represent_Usd = _AppFeeFixInfo21.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo21.Number_Of_Patent;
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
                    _AppFeeFixInfo3.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                    _AppFeeFixInfo4.Amount_Represent = _AppFeeFixInfo4.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent_Usd = _AppFeeFixInfo4.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo4.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                    _AppFeeFixInfo5.Amount_Represent = _AppFeeFixInfo5.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent_Usd = _AppFeeFixInfo5.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo5.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                    _AppFeeFixInfo6.Amount_Represent = _AppFeeFixInfo6.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo6.Number_Of_Patent;
                    _AppFeeFixInfo6.Amount_Represent_Usd = _AppFeeFixInfo6.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo6.Number_Of_Patent;

                    _AppFeeFixInfo6.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo6.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                    _AppFeeFixInfo61.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }

                //20190811 chị Tuyến bảo bỏ đi vì cục chưa áp dụng
                _AppFeeFixInfo61.Isuse = 0;
                _AppFeeFixInfo61.Number_Of_Patent = 0;
                _AppFeeFixInfo61.Amount = 0;
                _AppFeeFixInfo61.Amount_Usd = 0;
                _AppFeeFixInfo61.Amount_Represent_Usd = 0;
                _AppFeeFixInfo61.Amount_Represent = 0;

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                //if (pLstImagePublic != null && pLstImagePublic.Count > _numberPicOver)
                //{
                //    _AppFeeFixInfo61.Isuse = 1;
                //    _AppFeeFixInfo61.Number_Of_Patent = (pLstImagePublic.Count - _numberPicOver);

                //    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //    {
                //        _AppFeeFixInfo61.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo61.Number_Of_Patent;
                //        _AppFeeFixInfo61.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo61.Number_Of_Patent;
                //        _AppFeeFixInfo61.Amount_Represent = _AppFeeFixInfo61.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo61.Number_Of_Patent;
                //        _AppFeeFixInfo61.Amount_Represent_Usd = _AppFeeFixInfo61.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo61.Number_Of_Patent;
                //    }
                //    else
                //        _AppFeeFixInfo61.Amount = 60000 * _AppFeeFixInfo61.Number_Of_Patent;
                //}
                //else
                //{
                //    _AppFeeFixInfo61.Isuse = 0;
                //    _AppFeeFixInfo61.Number_Of_Patent = 0;
                //    _AppFeeFixInfo61.Amount = 0;
                //}
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
                    _AppFeeFixInfo62.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                                _AppFeeFixInfo62.Amount_Represent = _AppFeeFixInfo62.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo62.Number_Of_Patent;
                                _AppFeeFixInfo62.Amount_Represent_Usd = _AppFeeFixInfo62.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo62.Number_Of_Patent;
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
                    _AppFeeFixInfo7.Amount_Represent = _AppFeeFixInfo7.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo7.Number_Of_Patent;
                    _AppFeeFixInfo7.Amount_Represent_Usd = _AppFeeFixInfo7.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo7.Number_Of_Patent;
                    _AppFeeFixInfo7.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo7.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                    _AppFeeFixInfo71.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                                    _AppFeeFixInfo71.Amount_Represent = _AppFeeFixInfo71.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo71.Number_Of_Patent;
                                    _AppFeeFixInfo71.Amount_Represent_Usd = _AppFeeFixInfo71.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo71.Number_Of_Patent;
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
                    _AppFeeFixInfo72.Amount_Represent = _AppFeeFixInfo72.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo72.Number_Of_Patent;
                    _AppFeeFixInfo72.Amount_Represent_Usd = _AppFeeFixInfo72.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo72.Number_Of_Patent;
                    _AppFeeFixInfo72.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo72.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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

                if (app_Detail.Source_GPHI == null)
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
                _AppFeeFixInfo1.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo1);


                #region Phí công bố thông tin đơn sửa đổi
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Isuse = 1;
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo2.Number_Of_Patent;
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
                    _AppFeeFixInfo21.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }

                // nếu số hình > số hình quy định
                if (pDetail.Number_Pic > _numberPicOver)
                {
                    _AppFeeFixInfo21.Isuse = 1;
                    _AppFeeFixInfo21.Number_Of_Patent = (pDetail.Number_Pic - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                        _AppFeeFixInfo21.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo21.Number_Of_Patent;
                        _AppFeeFixInfo21.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo21.Number_Of_Patent;
                        _AppFeeFixInfo21.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo21.Number_Of_Patent;
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
                    _AppFeeFixInfo22.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
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
                        _AppFeeFixInfo22.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo22.Number_Of_Patent;
                        _AppFeeFixInfo22.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo22.Number_Of_Patent;
                        _AppFeeFixInfo22.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo22.Number_Of_Patent;
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
            List<AppDocumentInfo> pAppDocumentInfo)
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
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_3B(pDetail);

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

        public static int CaculatorFee_A04(List<AppClassDetailInfo> pAppClassInfo, string NumberAppNo, string p_case_code, ref List<AppFeeFixInfo> _lstFeeFix, int pPrviewOrInsert = 0)
        {
            try
            {
                if (NumberAppNo == null)
                {
                    NumberAppNo = "";
                }
                string _keyFee = "";
                int TongSoNhom = 0;  // dangtq sửa từ 1 về 0
                int _NumberClassOver = 0;
                int TongSo_SanPham_TinhPhi = 0;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_2011";
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _NumberClassOver = CommonFuc.ConvertToInt(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                else
                {
                    _NumberClassOver = 6;
                }

                if (pAppClassInfo != null && pAppClassInfo.Count > 0)
                {
                    // tổng số nhóm
                    TongSoNhom = CommonFuc.ConvertToInt(pAppClassInfo[0].TongSoNhom);

                    string[] arrSoSanPham = pAppClassInfo[0].TongSanPham.Split('|');
                    for (int i = 0; i < arrSoSanPham.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(arrSoSanPham[i]))
                        {
                            int TotalItemOnGroup = CommonFuc.ConvertToInt(arrSoSanPham[i]);
                            if (TotalItemOnGroup > _NumberClassOver)
                            {
                                TongSo_SanPham_TinhPhi = TongSo_SanPham_TinhPhi + (TotalItemOnGroup - _NumberClassOver);
                            }
                        }
                    }
                }

                // dangtq bỏ đi
                //if (TongSo_SanPham_TinhPhi < 1) TongSo_SanPham_TinhPhi = 0;

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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Number_Of_Patent = TongSo_SanPham_TinhPhi;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                        //_AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    }
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Number_Of_Patent = TongSo_SanPham_TinhPhi;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                        //_AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    }
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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
                    _AppFeeFixInfo.Number_Of_Patent = TongSo_SanPham_TinhPhi;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * TongSo_SanPham_TinhPhi;
                        //_AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * (_AppFeeFixInfo.Number_Of_Patent - CommonFuc.ConvertToDecimalReturn0(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01));
                    }
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
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
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

        public static List<AppFeeFixInfo> CallFee_A03(A03_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                pDetail.Appcode = "A03";
                decimal _total_PhuongAn = pAppDocIndusDesign.Where(m => m.FILELEVEL == 1).Count();
                decimal _totalImage = pAppDocIndusDesign.Where(m => m.FILELEVEL == 2).Count();

                #region 1 Lệ phí nộp đơn
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region 2 Quyền ưu tiên
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();

                if (pUTienInfo != null && pUTienInfo.Count > 0)
                {
                    _AppFeeFixInfo2.Isuse = 1;
                    _AppFeeFixInfo2.Number_Of_Patent = pUTienInfo.Count;
                }
                else
                {
                    _AppFeeFixInfo2.Isuse = 0;
                    _AppFeeFixInfo2.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo2.Amount = 600000 * _AppFeeFixInfo2.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region 3 Phí tra cứu thông tin nhằm phục vụ việc thẩm định
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;

                _AppFeeFixInfo3.Isuse = 0;
                _AppFeeFixInfo3.Number_Of_Patent = 0;
                _AppFeeFixInfo3.Isuse = 1;
                _AppFeeFixInfo3.Number_Of_Patent = _total_PhuongAn;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo3.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo3.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo3.Amount = 480000 * _AppFeeFixInfo3.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion

                #region 4 phí thẩm định đơn 
                AppFeeFixInfo _AppFeeFixInfo4 = new AppFeeFixInfo();
                _AppFeeFixInfo4.Fee_Id = 4;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo4.Fee_Id.ToString();
                _AppFeeFixInfo4.Isuse = 1;
                _AppFeeFixInfo4.Number_Of_Patent = _total_PhuongAn;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo4.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo4.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo4.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo4.Amount = 700000 * _AppFeeFixInfo4.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo4);
                #endregion

                #region 5 Phí công bố đơn

                AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                _AppFeeFixInfo5.Fee_Id = 5;
                _AppFeeFixInfo5.Isuse = 1;
                _AppFeeFixInfo5.Number_Of_Patent = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo5.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo5.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo5.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo5.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo5.Amount = 120000 * _AppFeeFixInfo5.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo5);

                #endregion

                #region 51 Phí công bố đơn từ hình thứ 2 trở đi
                AppFeeFixInfo _AppFeeFixInfo51 = new AppFeeFixInfo();
                _AppFeeFixInfo51.Fee_Id = 51;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo51.Fee_Id.ToString();
                decimal _numberPicOver = 1;
                _AppFeeFixInfo51.Level = 1;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo51.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo51.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > Phí công bố đơn từ hình thứ 2 trở đi
                if (_totalImage > _numberPicOver)
                {
                    _AppFeeFixInfo51.Isuse = 1;
                    _AppFeeFixInfo51.Number_Of_Patent = (_totalImage - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo51.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo51.Number_Of_Patent;
                    }
                    else
                        _AppFeeFixInfo51.Amount = 60000 * _AppFeeFixInfo51.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo51.Isuse = 0;
                    _AppFeeFixInfo51.Number_Of_Patent = 0;
                    _AppFeeFixInfo51.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo51);

                #endregion

                #region 6 phí phân loại kiểu dáng công nghiệp

                AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                _AppFeeFixInfo6.Fee_Id = 6;
                _AppFeeFixInfo6.Isuse = 1;
                _AppFeeFixInfo6.Number_Of_Patent = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo6.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo6.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo6.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo6.Number_Of_Patent;
                    _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;

                    _AppFeeFixInfo6.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo6.Number_Of_Patent;
                    _AppFeeFixInfo6.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo6.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo6.Amount = 100000 * _AppFeeFixInfo6.Number_Of_Patent;
                }

                if (pDetail.Phanloai_Type == 2)
                {
                    // tự phân loại
                    _AppFeeFixInfo6.Isuse = 0;
                    _AppFeeFixInfo6.Number_Of_Patent = 0;
                    _AppFeeFixInfo6.Amount = 0;
                }

                _lstFeeFix.Add(_AppFeeFixInfo6);

                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        // B02
        public static List<AppFeeFixInfo> CallFee_B02(App_Detail_PLB02_CGD_Info pDetail)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                // Phí thẩm định yêu cầu sửa đổi đơn
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.Transfer_Appno == null ? 0 : pDetail.Transfer_Appno.Split(',').Length;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);


                // Phí công bố thông tin đơn sửa đổi
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 2;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.Transfer_Appno == null ? 0 : pDetail.Transfer_Appno.Split(',').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_C05(List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                string _keyFee = "";
                string _appCode = "C05";
                if (pFeeFixInfo.Count > 0)
                {

                    foreach (var item in pFeeFixInfo)
                    {
                        _keyFee = _appCode + "_" + item.Fee_Id.ToString();
                        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        {
                            item.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                            item.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                            item.Amount = item.Amount * item.Number_Of_Patent;
                            item.Amount_Usd = Math.Round(item.Amount / AppsCommon.Get_Currentcy_VCB(), 2) * item.Number_Of_Patent;
                            item.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * item.Number_Of_Patent;
                            item.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * item.Number_Of_Patent;
                        }
                        else
                            item.Amount = 160000 * item.Number_Of_Patent;
                    }

                    return pFeeFixInfo;
                }
                else
                {
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                    // Phí tra cứu thông tin phục vụ việc giải quyết khiếu nại
                    AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Fee_Id = 1;
                    _AppFeeFixInfo.Number_Of_Patent = 0;

                    _keyFee = _appCode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                        _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    }
                    else
                        _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                    _lstFeeFix.Add(_AppFeeFixInfo);


                    // Phí công bố thông tin đơn sửa đổi
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Fee_Id = 2;
                    _AppFeeFixInfo.Number_Of_Patent = 0;

                    _keyFee = _appCode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                        _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    }
                    else
                        _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                    _lstFeeFix.Add(_AppFeeFixInfo);
                    return _lstFeeFix;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_D01(App_Detail_PLD01_HDCN_Info pDetail, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                pDetail.Appcode = TradeMarkAppCode.AppCode_TM_4C2_PLD_01_HDCN;

                #region Phí thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.Object_Contract_No == null ? 0 : pDetail.Object_Contract_No.Split(';').Length;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 230000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region Phí tra cứu nhãn hiệu liên kết phục vụ việc thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.Object_Contract_No == null ? 0 : pDetail.Object_Contract_No.Split(';').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 180000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region Phí thẩm định đơn 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 3;
                _AppFeeFixInfo.Isuse = pFeeFixInfo[0].Isuse;
                if (_AppFeeFixInfo.Isuse == 1)
                {
                    _AppFeeFixInfo.Number_Of_Patent = 1;
                }
                else
                {
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo.Amount = 180000 * _AppFeeFixInfo.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region Lệ phí cấp Giấy chứng nhận đăng ký nhãn hiệu
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 4;
                _AppFeeFixInfo.Isuse = pFeeFixInfo[1].Isuse;
                if (_AppFeeFixInfo.Isuse == 1)
                {
                    _AppFeeFixInfo.Number_Of_Patent = 1;
                }
                else
                {
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region Phí đăng bạ quyết định ghi nhận chuyển nhượng quyền SHCN
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 5;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.Object_Contract_No == null ? 0 : pDetail.Object_Contract_No.Split(';').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region Phí công bố quyết định ghi nhận chuyển nhượng quyền SHCN
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 6;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_B03(B03_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                #region  Phí tra cứu thông tin nhằm phục vụ việc thẩm định
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Level = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (pDetail.Thamdinhnoidung == "TDND")
                {
                    _AppFeeFixInfo.Isuse = pDetail.Point == -1 ? 0 : 1;
                    _AppFeeFixInfo.Number_Of_Patent = pDetail.Point == -1 ? 0 : pDetail.Point;
                }
                else
                {
                    _AppFeeFixInfo.Isuse = 0;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region Phí thẩm định nội dung
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2;

                if (pDetail.Thamdinhnoidung == "TDND")
                {
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Number_Of_Patent = pDetail.Point == -1 ? 1 : pDetail.Point;
                }
                else
                {
                    _AppFeeFixInfo.Isuse = 0;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }
                else
                {
                    _AppFeeFixInfo.Amount = 720000 * _AppFeeFixInfo.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo);

                #endregion

                #region 2 Phí thẩm định hình thức từ trang bản mô tả thứ 7 trở đi
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 21;
                decimal _numberPicOver = 5;
                _AppFeeFixInfo.Level = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }

                if (pDetail.Thamdinhnoidung == "TDND")
                {
                    // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                    if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                    {
                        AppDocumentInfo _AppDocumentInfo = null;
                        foreach (var item in pAppDocumentInfo)
                        {
                            if (item.Document_Id == "B03_00")
                            {
                                _AppDocumentInfo = item;
                            }
                        }

                        if (_AppDocumentInfo != null)
                        {
                            if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                            {
                                _AppFeeFixInfo.Isuse = 1;
                                _AppFeeFixInfo.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                                {
                                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                                }
                                else
                                    _AppFeeFixInfo.Amount = 60000 * _AppFeeFixInfo.Number_Of_Patent;
                            }
                            else
                            {
                                _AppFeeFixInfo.Isuse = 0;
                                _AppFeeFixInfo.Number_Of_Patent = 0;
                                _AppFeeFixInfo.Amount = 0;
                                _AppFeeFixInfo.Amount_Represent = 0;
                            }
                        }
                        else
                        {
                            _AppFeeFixInfo.Isuse = 0;
                            _AppFeeFixInfo.Number_Of_Patent = 0;
                            _AppFeeFixInfo.Amount = 0;
                            _AppFeeFixInfo.Amount_Represent = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo.Isuse = 0;
                        _AppFeeFixInfo.Number_Of_Patent = 0;
                        _AppFeeFixInfo.Amount = 0;
                        _AppFeeFixInfo.Amount_Represent = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo.Isuse = 0;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Amount = 0;
                    _AppFeeFixInfo.Amount_Represent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 32000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_A02(A02_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
         List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                pDetail.Appcode = "A02";
                decimal _totalImage = pAppDocIndusDesign.Where(m => m.FILELEVEL == 2).Count();

                #region 1 Lệ phí nộp đơn
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region 2 phí thẩm định đơn
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Number_Of_Patent = 1;
                _AppFeeFixInfo2.Isuse = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo2.Amount = 180000 * _AppFeeFixInfo2.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region 3 phí công bố đơn
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;
                _AppFeeFixInfo3.Isuse = 1;
                _AppFeeFixInfo3.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo3.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo3.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo3.Amount = 120000 * _AppFeeFixInfo3.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion


                #region 31 Phí công bố đơn từ hình thứ 2 trở đi
                AppFeeFixInfo _AppFeeFixInfo31 = new AppFeeFixInfo();
                _AppFeeFixInfo31.Fee_Id = 31;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo31.Fee_Id.ToString();
                decimal _numberPicOver = 1;
                _AppFeeFixInfo31.Level = 1;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo31.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo31.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > Phí công bố đơn từ hình thứ 2 trở đi
                if (_totalImage > _numberPicOver)
                {
                    _AppFeeFixInfo31.Isuse = 1;
                    _AppFeeFixInfo31.Number_Of_Patent = (_totalImage - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo31.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo31.Number_Of_Patent;
                        _AppFeeFixInfo31.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo31.Number_Of_Patent;
                        _AppFeeFixInfo31.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo31.Number_Of_Patent;
                        _AppFeeFixInfo31.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo31.Number_Of_Patent;

                    }
                    else
                        _AppFeeFixInfo31.Amount = 60000 * _AppFeeFixInfo31.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo31.Isuse = 0;
                    _AppFeeFixInfo31.Number_Of_Patent = 0;
                    _AppFeeFixInfo31.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo31);

                #endregion



                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_A05(A05_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                pDetail.Appcode = "A05";

                #region 1 Lệ phí nộp đơn
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region 2 phí công bố đơn
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Number_Of_Patent = 1;
                _AppFeeFixInfo2.Isuse = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo2.Amount = 120000 * _AppFeeFixInfo2.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region 3 phí tra cứu tông tin thẩm định đơn
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;
                _AppFeeFixInfo3.Isuse = 1;
                _AppFeeFixInfo3.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo3.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo3.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo3.Amount = 180000 * _AppFeeFixInfo3.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion

                #region 4 phí thẩm định đơn
                AppFeeFixInfo _AppFeeFixInfo4 = new AppFeeFixInfo();
                _AppFeeFixInfo4.Fee_Id = 4;
                _AppFeeFixInfo4.Isuse = 1;
                _AppFeeFixInfo4.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo4.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo4.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo4.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo4.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo4.Amount = 1200000 * _AppFeeFixInfo4.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo4);
                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }
        public static List<AppFeeFixInfo> CallFee_C06(App_Detail_TM06DKQT_Info pDetail )
        {
            try
            {
                
                pDetail.Appcode = "C06";

                #region 1  Phí dịch vụ làm thủ tục đăng ký quốc tế nhãn hiệu đã nộp kèm theo
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                }
                else
                    _AppFeeFixInfo1.Amount = 2000000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                
                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_C07(C07_Info pDetail, List<AppClassDetailInfo> appClassDetailInfos)
        {
            try
            {
                Hashtable _hsGroupClass = new Hashtable();
                foreach (var item in appClassDetailInfos)
                {
                    _hsGroupClass[item.Code] = appClassDetailInfos.Count(n => n.Code == item.Code);
                }
                pDetail.Appcode = "C07";

                #region 1 Lệ phí nộp đơn
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region Phí phân loại quốc tế về nhãn hiệu
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Number_Of_Patent = 1;
                _AppFeeFixInfo2.Isuse = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo2.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo2.Amount = 120000 * _AppFeeFixInfo2.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion


                #region 21 Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)
                AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                _AppFeeFixInfo21.Fee_Id = 21;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                int _MaxClassInGroup = 1;
                _AppFeeFixInfo21.Level = 2;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo21.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo21.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _MaxClassInGroup = Convert.ToInt32(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                int _SoDoiTuongTinhPhi = CountSoSpTinhPhi(_hsGroupClass, _MaxClassInGroup);
                //  
                if (_SoDoiTuongTinhPhi > _MaxClassInGroup)
                {
                    _AppFeeFixInfo21.Isuse = 1;
                    _AppFeeFixInfo21.Number_Of_Patent = _SoDoiTuongTinhPhi;

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo21.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo21.Number_Of_Patent;
                        _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                        _AppFeeFixInfo21.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo21.Number_Of_Patent;
                        _AppFeeFixInfo21.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo21.Number_Of_Patent;

                    }
                    else
                        _AppFeeFixInfo21.Amount = 130000 * _AppFeeFixInfo21.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo21.Isuse = 0;
                    _AppFeeFixInfo21.Number_Of_Patent = 0;
                    _AppFeeFixInfo21.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo21);

                #endregion

                #region 3 Phí công bố đơn (trường hợp đăng ký quốc tế nhãn hiệu chưa được chấp nhận bảo hộ tại Việt Nam)
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;
                _AppFeeFixInfo3.Isuse = 1;
                _AppFeeFixInfo3.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo3.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo3.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo3.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo3.Amount = 140000 * _AppFeeFixInfo3.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion

                #region 4 Phí tra cứu phục vụ việc thẩm định      
                AppFeeFixInfo _AppFeeFixInfo4 = new AppFeeFixInfo();
                _AppFeeFixInfo4.Fee_Id = 4;
                _AppFeeFixInfo4.Isuse = 1;
                _AppFeeFixInfo4.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo4.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo4.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo4.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo4.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo4.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo4.Amount = 1200000 * _AppFeeFixInfo4.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo4);
                #endregion

                #region 41 Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)
                AppFeeFixInfo _AppFeeFixInfo41 = new AppFeeFixInfo();
                _AppFeeFixInfo41.Fee_Id = 41;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo41.Fee_Id.ToString();
                _MaxClassInGroup = 1;
                _AppFeeFixInfo41.Level = 2;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo41.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo41.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _MaxClassInGroup = Convert.ToInt32(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                _SoDoiTuongTinhPhi = CountSoSpTinhPhi(_hsGroupClass, _MaxClassInGroup);
                //  
                if (_SoDoiTuongTinhPhi > _MaxClassInGroup)
                {
                    _AppFeeFixInfo41.Isuse = 1;
                    _AppFeeFixInfo41.Number_Of_Patent = _SoDoiTuongTinhPhi;

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo41.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo41.Number_Of_Patent;
                        _AppFeeFixInfo41.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo41.Number_Of_Patent;
                        _AppFeeFixInfo41.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo41.Number_Of_Patent;
                        _AppFeeFixInfo41.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo41.Number_Of_Patent;

                    }
                    else
                        _AppFeeFixInfo41.Amount = 130000 * _AppFeeFixInfo41.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo41.Isuse = 0;
                    _AppFeeFixInfo41.Number_Of_Patent = 0;
                    _AppFeeFixInfo41.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo41);

                #endregion

                #region 5 Phí thẩm định đơn
                AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                _AppFeeFixInfo5.Fee_Id = 5;
                _AppFeeFixInfo5.Isuse = 1;
                _AppFeeFixInfo5.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo5.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo5.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo5.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo5.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo5.Amount = 1200000 * _AppFeeFixInfo5.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo5);
                #endregion

                #region 51 Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)
                AppFeeFixInfo _AppFeeFixInfo51 = new AppFeeFixInfo();
                _AppFeeFixInfo51.Fee_Id = 51;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo51.Fee_Id.ToString();
                _MaxClassInGroup = 1;
                _AppFeeFixInfo51.Level = 2;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo51.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo51.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _MaxClassInGroup = Convert.ToInt32(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                _SoDoiTuongTinhPhi = CountSoSpTinhPhi(_hsGroupClass, _MaxClassInGroup);
                //  
                if (_SoDoiTuongTinhPhi > _MaxClassInGroup)
                {
                    _AppFeeFixInfo51.Isuse = 1;
                    _AppFeeFixInfo51.Number_Of_Patent = _SoDoiTuongTinhPhi;

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo51.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo51.Number_Of_Patent;

                    }
                    else
                        _AppFeeFixInfo51.Amount = 130000 * _AppFeeFixInfo51.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo51.Isuse = 0;
                    _AppFeeFixInfo51.Number_Of_Patent = 0;
                    _AppFeeFixInfo51.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo51);

                #endregion


                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }
        public static List<AppFeeFixInfo> CallFee_C08(C08_Info pDetail)
        {
            try
            {
                pDetail.Appcode = "C08";

                #region Phí thẩm định sửa đổi, chuyển nhượng, gia hạn, mở rộng lãnh thổ, hạn chế danh mục hàng hóa/dịch vụ, chấm dứt, hủy bỏ hiệu lực nhãn hiệu đăng ký quốc tế có nguồn gốc Việt Nam
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo1.Number_Of_Patent;

                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion



                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }
        public static List<AppFeeFixInfo> CallFee_E01(ApplicationHeaderInfo pheader)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                #region  Phí thẩm định hồ sơ yêu cầu cấp Chứng chỉ hành nghề dịch vụ đại diện sở hữu công nghiệp
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                string _keyFee = pheader.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion


                #region  Lệ phí cấp Chứng chỉ hành nghề dịch vụ đại diện sở hữu công nghiệp
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2;
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _keyFee = pheader.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion


                #region  Lệ phí đăng bạ quyết định cấp Chứng chỉ hành nghề dịch vụ đại diện sở hữu công nghiệp 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 3;
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _keyFee = pheader.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region  Lệ phí công bố quyết định cấp Chứng chỉ hành nghề dịch vụ đại diện sở hữu công nghiệp
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 4;
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _keyFee = pheader.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();


                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion


                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_C01(App_Detail_C01_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                #region 1. Phí thẩm định
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;
                _AppFeeFixInfo.Level = 0;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 1.1 Phí thẩm định yêu cầu sửa đổi văn bằng bảo hộ
                // Phí thẩm định sửa đổi văn bằng tính theo số văn bằng
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 0;
                _AppFeeFixInfo.Fee_Id = 11;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;
                _AppFeeFixInfo.Level = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 1.2 Phí thẩm định yêu cầu thu hẹp phạm vi bảo hộ
                // Phí yêu cầu thu hẹp tính = 1
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 12;
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 2 Phí đăng bạ quyết định sửa đổi văn bằng bảo hộ
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 2;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;
                _AppFeeFixInfo.Level = 0;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 3 Phí công bố quyết định sửa đổi văn bằng bảo hộ
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 3;
                _AppFeeFixInfo.Level = 0;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 3.1 nếu có trên 1 hình (từ hình thứ 2 trở đi)
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Fee_Id = 31;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                decimal _numberPicOver = 1;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }

                _AppFeeFixInfo.Isuse = 0;
                _AppFeeFixInfo.Number_Of_Patent = 0;
                _AppFeeFixInfo.Amount = 0;
                _AppFeeFixInfo.Amount_Usd = 0;
                _AppFeeFixInfo.Amount_Represent = 0;
                _AppFeeFixInfo.Amount_Represent_Usd = 0;

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pLstImagePublic != null && pLstImagePublic.Count > _numberPicOver)
                {
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Number_Of_Patent = (pLstImagePublic.Count - _numberPicOver);
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                        _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    }
                }
                _lstFeeFix.Add(_AppFeeFixInfo);

                #endregion

                #region 3.2 bản mô tả có trên 6 trang (từ trang thứ 7 trở đi)
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Fee_Id = 32;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                decimal _number_OverPage = 5;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _number_OverPage = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }

                _AppFeeFixInfo.Isuse = 0;
                _AppFeeFixInfo.Number_Of_Patent = 0;
                _AppFeeFixInfo.Amount = 0;
                _AppFeeFixInfo.Amount_Usd = 0;
                _AppFeeFixInfo.Amount_Represent = 0;
                _AppFeeFixInfo.Amount_Represent_Usd = 0;


                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                {
                    foreach (var item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "C01_02" && item.CHAR02 != "" && Convert.ToDecimal(item.CHAR02) > _number_OverPage)
                        {
                            _AppFeeFixInfo.Isuse = 1;
                            _AppFeeFixInfo.Number_Of_Patent = Convert.ToDecimal(item.CHAR02) - _number_OverPage;
                            if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            {
                                _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                                _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;

                                _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                                _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                            }
                        }
                    }
                }

                _lstFeeFix.Add(_AppFeeFixInfo);

                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public static List<AppFeeFixInfo> CallFee_C02(App_Detail_C02_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                #region 1. Phí thẩm định yêu cầu gia hạn/duy trì hiệu lực văn bằng bảo hộ
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _AppFeeFixInfo.Level = 0;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 2 Lệ phí gia hạn/duy trì hiệu lực văn bằng bảo hộ
                // Phí thẩm định sửa đổi văn bằng tính theo số văn bằng
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    // Nhóm sản phẩm dịch vụ: Type_detai = 2
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 0;
                    _AppFeeFixInfo.Fee_Id = 2;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Level = 0;
                    if (pDetail.App_Change_Type == 2)
                    {
                        _AppFeeFixInfo.Number_Of_Patent = pDetail.App_Change_Detail == null ? 0 : pDetail.App_Change_Detail.Split(',').Length;
                    }
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo);

                    // phương án của từng sản phẩm
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 0;
                    _AppFeeFixInfo.Fee_Id = 2;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Level = 0;
                    if (pDetail.App_Change_Type == 1)
                    {
                        _AppFeeFixInfo.Number_Of_Patent = pDetail.App_Change_Detail == null ? 0 : pDetail.App_Change_Detail.Split(',').Length;
                    }
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo);

                    // điểm yêu cầu bảo hộ độc lập
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 0;
                    _AppFeeFixInfo.Fee_Id = 2;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Level = 0;
                    if (pDetail.App_Change_Type == 4)
                    {
                        _AppFeeFixInfo.Number_Of_Patent = pDetail.Number_Point;
                    }
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

            
                #endregion

                #region 21. Lệ phí gia hạn/duy trì hiệu lực muộn 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 3;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _AppFeeFixInfo.Level = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 3 Phí sử dụng văn bằng bảo hộ
                // Phí thẩm định sửa đổi văn bằng tính theo số văn bằng
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 4;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    // Nhóm sản phẩm dịch vụ: Type_detai = 2
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Fee_Id = 4;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Level = 0;
                    if (pDetail.App_Change_Type == 2)
                    {
                        _AppFeeFixInfo.Number_Of_Patent = pDetail.App_Change_Detail == null ? 0 : pDetail.App_Change_Detail.Split(',').Length;
                    }
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo);

                    // phương án của từng sản phẩm
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Fee_Id = 4;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Level = 0;
                    if (pDetail.App_Change_Type == 1)
                    {
                        _AppFeeFixInfo.Number_Of_Patent = pDetail.App_Change_Detail == null ? 0 : pDetail.App_Change_Detail.Split(',').Length;
                    }
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo);

                    // điểm yêu cầu bảo hộ độc lập
                    _AppFeeFixInfo = new AppFeeFixInfo();
                    _AppFeeFixInfo.Isuse = 1;
                    _AppFeeFixInfo.Fee_Id = 4;
                    _AppFeeFixInfo.Number_Of_Patent = 0;
                    _AppFeeFixInfo.Level = 0;
                    if (pDetail.App_Change_Type == 4)
                    {
                        _AppFeeFixInfo.Number_Of_Patent = pDetail.Number_Point;
                    }
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo);
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

            
                #endregion

                #region 4.Phí đăng bạ quyết định gia hạn/thông báo duy trì hiệu lực văn bằng bảo hộ
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 5;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _AppFeeFixInfo.Level = 0;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion
                #region 5.Phí công bố quyết định gia hạn/thông báo duy trì hiệu lực văn bằng bảo hộ
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 6;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _AppFeeFixInfo.Level = 0;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion
                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }


        public static List<AppFeeFixInfo> CallFee_C03(App_Detail_C03_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                #region 1. Phí cấp phó bản/cấp lại văn bằng bảo hộ/giấy chứng nhận
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 1;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;
                _AppFeeFixInfo.Level = 0;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 1.1 Văn bằng bảo hộ có trên 4 trang (từ trang thứ 5 trở đi)                                                                      
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Level = 1;
                _AppFeeFixInfo.Fee_Id = 11;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                decimal _number_OverPage = 4;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _number_OverPage = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                }

                _AppFeeFixInfo.Isuse = 0;
                _AppFeeFixInfo.Number_Of_Patent = 0;
                _AppFeeFixInfo.Amount = 0;
                _AppFeeFixInfo.Amount_Usd = 0;
                _AppFeeFixInfo.Amount_Represent = 0;
                _AppFeeFixInfo.Amount_Represent_Usd = 0;


                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                {
                    foreach (var item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "C03_02" && item.CHAR02 != "" && Convert.ToDecimal(item.CHAR02) > _number_OverPage)
                        {
                            _AppFeeFixInfo.Isuse = 1;
                            _AppFeeFixInfo.Number_Of_Patent = Convert.ToDecimal(item.CHAR02) - _number_OverPage;
                            if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            {
                                _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                                _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;

                                _AppFeeFixInfo.Amount_Represent = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                                _AppFeeFixInfo.Amount_Represent_Usd = _AppFeeFixInfo.Number_Of_Patent == 0 ? 0 : MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                            }
                        }
                    }
                }

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 2 Phí đăng bạ quyết định cấp phó bản/cấp lại văn bằng bảo hộ/giấy chứng nhận
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 2;
                _AppFeeFixInfo.Number_Of_Patent = pDetail.App_No_Change == null ? 0 : pDetail.App_No_Change.Split(',').Length;
                _AppFeeFixInfo.Level = 0;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                #region 3 Phí công bố quyết định cấp phó bản/cấp lại văn bằng bảo hộ/giấy chứng nhận
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Fee_Id = 3;
                _AppFeeFixInfo.Level = 0;
                _AppFeeFixInfo.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo.Fee_Name_En = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description_En;
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Usd * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent * _AppFeeFixInfo.Number_Of_Patent;
                    _AppFeeFixInfo.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd * _AppFeeFixInfo.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo.Amount = 160000 * _AppFeeFixInfo.Number_Of_Patent;

                _lstFeeFix.Add(_AppFeeFixInfo);
                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        /// <summary>
        /// HungTD Tính toán số đối tượng tính phí của hàng hóa dịch vụ
        /// </summary>
        /// <param name="pClassInGroup"></param>
        /// <param name="pMaxItemIngroup"></param>
        /// <returns></returns>
        static int CountSoSpTinhPhi(Hashtable pClassInGroup, int pMaxItemIngroup)
        {
            int _SoSpTinhPhi = 0;
            try
            {
                foreach (DictionaryEntry _item in pClassInGroup)
                {
                    int _CountClassGroup = (int)_item.Value;
                    if (_CountClassGroup > pMaxItemIngroup)
                    {
                        _SoSpTinhPhi += _CountClassGroup - pMaxItemIngroup;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return _SoSpTinhPhi;
            }
            return _SoSpTinhPhi;
        }

    }
}