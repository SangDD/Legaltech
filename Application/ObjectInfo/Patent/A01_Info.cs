using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class A01_Info : ApplicationHeaderInfo
    {
        public A01_Info()
        {

        }
        //public decimal ID { set; get; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string Appno { get; set; }
        public string Patent_Type { get; set; }
        public string Patent_Name { get; set; }
        public string Source_PCT { get; set; }
        public string PCT_Number { get; set; }
        public DateTime PCT_Filling_Date_Qt { get; set; }
        public string PCT_Number_Qt { get; set; }
        public DateTime PCT_Date { get; set; }
        public DateTime PCT_VN_Date { get; set; }
        public decimal PCT_Suadoi { get; set; }
        public string PCT_Suadoi_Name { get; set; }
        public string PCT_Suadoi_Address { get; set; }

        public string PCT_Suadoi_Others { get; set; }

        public string PCT_Suadoi_Content { get; set; }
        public string Source_DQSC { get; set; }
        public string DQSC_Origin_App_No { get; set; }
        public DateTime DQSC_Filling_Date { get; set; }
        public decimal DQSC_Valid_Before { get; set; }
        public decimal DQSC_Valid_After { get; set; }
        public string Source_GPHI { get; set; }
        public string GPHI_Origin_App_No { get; set; }
        public DateTime GPHI_Filling_Date { get; set; }
        public decimal GPHI_Valid_Before { get; set; }
        public decimal GPHI_Valid_After { get; set; }

        public string ThamDinhNoiDung { get; set; }
        public string ChuyenDoiDon { get; set; }
        public decimal Point { get; set; }

        public string Class_Type { get; set; }
        public string Class_Content { get; set; }
        public decimal Used_Special { set; get; }
    }

    public class A01_Info_Export : ApplicationHeaderInfo
    {
        public static void CopyAppHeaderInfo(ref A01_Info_Export p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
            //p_appDetail.ID = pAppInfo.Id;
            p_appDetail.Appcode = pAppInfo.Appcode;
            p_appDetail.Master_Name = pAppInfo.Master_Name;
            p_appDetail.Master_Address = pAppInfo.Master_Address;
            p_appDetail.Master_Phone = pAppInfo.Master_Phone;
            p_appDetail.Master_Fax = pAppInfo.Master_Fax;
            p_appDetail.Master_Email = pAppInfo.Master_Email;
            p_appDetail.Master_Type = pAppInfo.Master_Type;

            p_appDetail.Rep_Master_Type = pAppInfo.Rep_Master_Type;
            p_appDetail.Rep_Master_Name = pAppInfo.Rep_Master_Name;
            p_appDetail.Rep_Master_Address = pAppInfo.Rep_Master_Address;
            p_appDetail.Rep_Master_Phone = pAppInfo.Rep_Master_Phone;
            p_appDetail.Rep_Master_Fax = pAppInfo.Rep_Master_Fax;
            p_appDetail.Rep_Master_Email = pAppInfo.Rep_Master_Email;
            p_appDetail.Relationship = pAppInfo.Relationship;
            p_appDetail.Send_Date = pAppInfo.Send_Date;
            p_appDetail.Status = pAppInfo.Status;
            p_appDetail.Status_Form = pAppInfo.Status_Form;
            p_appDetail.Status_Content = pAppInfo.Status_Content;
            p_appDetail.Remark = pAppInfo.Remark;
            p_appDetail.AppName = pAppInfo.AppName;
            p_appDetail.Address = pAppInfo.Address;
            p_appDetail.DateNo = pAppInfo.DateNo;
            p_appDetail.Months = pAppInfo.Months;
            p_appDetail.Years = pAppInfo.Years;
        }

        public static void CopyUuTienInfo(ref A01_Info_Export p_appDetail, UTienInfo pAppInfo)
        {
            if (pAppInfo != null)
            {
                p_appDetail.UT_SoDon = pAppInfo.UT_SoDon;
                p_appDetail.UT_NgayNopDon = pAppInfo.UT_NgayNopDon;
                p_appDetail.UT_QuocGia_Display = pAppInfo.UT_QuocGia_Display;
                p_appDetail.UT_Type = pAppInfo.UT_Type;
                p_appDetail.UT_ThoaThuanKhac = pAppInfo.UT_ThoaThuanKhac;
            }
            else
            {
                p_appDetail.UT_SoDon = "";
                p_appDetail.UT_NgayNopDon = DateTime.MinValue;
                p_appDetail.UT_QuocGia_Display = "";
                p_appDetail.UT_Type = "";
                p_appDetail.UT_ThoaThuanKhac = "";
            }
        }

        public static void CopyAuthorsInfo(ref A01_Info_Export p_appDetail, AuthorsInfo pAppInfo, int p_position)
        {
            if (p_position == 0)
            {
                p_appDetail.Author_Name = pAppInfo.Author_Name;
                p_appDetail.Author_Address = pAppInfo.Author_Address;
                p_appDetail.Author_Phone = pAppInfo.Author_Phone;
                p_appDetail.Author_Fax = pAppInfo.Author_Fax;
                p_appDetail.Author_Email = pAppInfo.Author_Email;
                p_appDetail.Author_Country_Display = pAppInfo.Author_Country_Display;

            }
            else if (p_position == 1)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Author_Name_1 = pAppInfo.Author_Name;
                    p_appDetail.Author_Address_1 = pAppInfo.Author_Address;
                    p_appDetail.Author_Phone_1 = pAppInfo.Author_Phone;
                    p_appDetail.Author_Fax_1 = pAppInfo.Author_Fax;
                    p_appDetail.Author_Email_1 = pAppInfo.Author_Email;
                    p_appDetail.Author_Country_Display_1 = pAppInfo.Author_Country_Display;
                }
                else
                {
                    p_appDetail.Author_Name_1 = "";
                    p_appDetail.Author_Address_1 = "";
                    p_appDetail.Author_Phone_1 = "";
                    p_appDetail.Author_Fax_1 = "";
                    p_appDetail.Author_Email_1 = "";
                    p_appDetail.Author_Country_Display_1 = "";
                }
            }
            else if (p_position == 2)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Author_Name_2 = pAppInfo.Author_Name;
                    p_appDetail.Author_Address_2 = pAppInfo.Author_Address;
                    p_appDetail.Author_Phone_2 = pAppInfo.Author_Phone;
                    p_appDetail.Author_Fax_2 = pAppInfo.Author_Fax;
                    p_appDetail.Author_Email_2 = pAppInfo.Author_Email;
                    p_appDetail.Author_Country_Display_2 = pAppInfo.Author_Country_Display;
                }
                else
                {
                    p_appDetail.Author_Name_2 = "";
                    p_appDetail.Author_Address_2 = "";
                    p_appDetail.Author_Phone_2 = "";
                    p_appDetail.Author_Fax_2 = "";
                    p_appDetail.Author_Email_2 = "";
                    p_appDetail.Author_Country_Display_2 = "";
                }

            }
        }

        public static void CopyOther_MasterInfo(ref A01_Info_Export p_appDetail, Other_MasterInfo pAppInfo, int p_position)
        {
            if (p_position == 0)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Master_Name_1 = pAppInfo.Master_Name;
                    p_appDetail.Master_Address_1 = pAppInfo.Master_Address;
                    p_appDetail.Master_Phone_1 = pAppInfo.Master_Phone;
                    p_appDetail.Master_Fax_1 = pAppInfo.Master_Fax;
                    p_appDetail.Master_Email_1 = pAppInfo.Master_Email;
                    p_appDetail.TacGiaDongThoi_1 = pAppInfo.TacGiaDongThoi;
                    p_appDetail.PhoBan_1 = pAppInfo.PhoBan;
                }
                else
                {
                    p_appDetail.Master_Name_1 = "";
                    p_appDetail.Master_Address_1 = "";
                    p_appDetail.Master_Phone_1 = "";
                    p_appDetail.Master_Fax_1 = "";
                    p_appDetail.Master_Email_1 = "";
                    p_appDetail.TacGiaDongThoi_1 = "";
                    p_appDetail.PhoBan_1 = "";
                }
            }
            else if (p_position == 1)
            {
                if (pAppInfo != null)
                {
                    p_appDetail.Master_Name_2 = pAppInfo.Master_Name;
                    p_appDetail.Master_Address_2 = pAppInfo.Master_Address;
                    p_appDetail.Master_Phone_2 = pAppInfo.Master_Phone;
                    p_appDetail.Master_Fax_2 = pAppInfo.Master_Fax;
                    p_appDetail.Master_Email_2 = pAppInfo.Master_Email;

                    p_appDetail.TacGiaDongThoi_2 = pAppInfo.TacGiaDongThoi;
                    p_appDetail.PhoBan_2 = pAppInfo.PhoBan;
                }
                else
                {
                    p_appDetail.Master_Name_2 = "";
                    p_appDetail.Master_Address_2 = "";
                    p_appDetail.Master_Phone_2 = "";
                    p_appDetail.Master_Fax_2 = "";
                    p_appDetail.Master_Email_2 = "";

                    p_appDetail.TacGiaDongThoi_2 = "";
                    p_appDetail.PhoBan_2 = "";
                }
            }
        }

        public decimal ID { set; get; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string Appno { get; set; }
        public string Patent_Type { get; set; }
        public string Patent_Name { get; set; }
        public string Source_PCT { get; set; }
        public string PCT_Number { get; set; }
        public DateTime PCT_Filling_Date_Qt { get; set; }
        public string PCT_Number_Qt { get; set; }
        public DateTime PCT_Date { get; set; }
        public DateTime PCT_VN_Date { get; set; }
        public decimal PCT_Suadoi { get; set; }
        public string PCT_Suadoi_Name { get; set; }
        public string PCT_Suadoi_Address { get; set; }

        public string PCT_Suadoi_Others { get; set; }

        public string PCT_Suadoi_Content { get; set; }
        public string Source_DQSC { get; set; }
        public string DQSC_Origin_App_No { get; set; }
        public DateTime DQSC_Filling_Date { get; set; }
        public decimal DQSC_Valid_Before { get; set; }
        public decimal DQSC_Valid_After { get; set; }
        public string Source_GPHI { get; set; }
        public string GPHI_Origin_App_No { get; set; }
        public DateTime GPHI_Filling_Date { get; set; }
        public decimal GPHI_Valid_Before { get; set; }
        public decimal GPHI_Valid_After { get; set; }

        public string ThamDinhNoiDung { get; set; }
        public string ChuyenDoiDon { get; set; }
        public decimal Point { get; set; }

        public string Class_Type { get; set; }
        public string Class_Content { get; set; }

        #region Chủ đơn khác

        public string Master_Name_1 { set; get; }
        public string Master_Address_1 { set; get; }
        public string Master_Phone_1 { set; get; }
        public string Master_Fax_1 { set; get; }
        public string Master_Email_1 { set; get; }

        public string TacGiaDongThoi_1 { set; get; }
        public string PhoBan_1 { set; get; }

        public string Master_Name_2 { set; get; }
        public string Master_Address_2 { set; get; }
        public string Master_Phone_2 { set; get; }
        public string Master_Fax_2 { set; get; }
        public string Master_Email_2 { set; get; }

        public string TacGiaDongThoi_2 { set; get; }
        public string PhoBan_2 { set; get; }

        #endregion

        #region Đơn ưu tiên
        public decimal Used_Special { set; get; }
        public string UT_SoDon { set; get; }
        public DateTime UT_NgayNopDon { set; get; }
        public decimal UT_QuocGia { set; get; }

        public string UT_QuocGia_Display { set; get; }
        public string UT_Type { set; get; }
        public string UT_ThoaThuanKhac { set; get; }
        #endregion

        #region Tác giả

        public string Author_Name { set; get; }

        public string Author_Address { set; get; }

        public string Author_Phone { set; get; }

        public string Author_Fax { set; get; }
        public string Author_Email { set; get; }

        public decimal Author_Country { set; get; }
        public string Author_Country_Display { set; get; }

        string _Author_Others;
        public string Author_Others
        {
            get
            {
                return _Author_Others;
            }
            set
            {
                _Author_Others = value;
            }
        }

        #endregion

        #region Tác giả 1

        public string Author_Name_1 { set; get; }

        public string Author_Address_1 { set; get; }

        public string Author_Phone_1 { set; get; }

        public string Author_Fax_1 { set; get; }
        public string Author_Email_1 { set; get; }

        public decimal Author_Country_1 { set; get; }
        public string Author_Country_Display_1 { set; get; }

        #endregion

        #region Tác giả 2

        public string Author_Name_2 { set; get; }

        public string Author_Address_2 { set; get; }

        public string Author_Phone_2 { set; get; }

        public string Author_Fax_2 { set; get; }
        public string Author_Email_2 { set; get; }

        public decimal Author_Country_2 { set; get; }
        public string Author_Country_Display_2 { set; get; }

        #endregion

        #region Phí

        public decimal Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }

        public decimal Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }

        public decimal Fee_Id_21 { get; set; }
        public decimal Fee_Id_21_Check { get; set; }
        public string Fee_Id_21_Val { get; set; }

        public decimal Fee_Id_3 { get; set; }
        public decimal Fee_Id_3_Check { get; set; }
        public string Fee_Id_3_Val { get; set; }

        public decimal Fee_Id_4 { get; set; }
        public decimal Fee_Id_4_Check { get; set; }
        public string Fee_Id_4_Val { get; set; }

        public decimal Fee_Id_5 { get; set; }
        public decimal Fee_Id_5_Check { get; set; }
        public string Fee_Id_5_Val { get; set; }

        public decimal Fee_Id_6 { get; set; }
        public decimal Fee_Id_6_Check { get; set; }
        public string Fee_Id_6_Val { get; set; }

        public decimal Fee_Id_61 { get; set; }
        public decimal Fee_Id_61_Check { get; set; }
        public string Fee_Id_61_Val { get; set; }

        public decimal Fee_Id_62 { get; set; }
        public decimal Fee_Id_62_Check { get; set; }
        public string Fee_Id_62_Val { get; set; }

        public decimal Fee_Id_7 { get; set; }
        public decimal Fee_Id_7_Check { get; set; }
        public string Fee_Id_7_Val { get; set; }

        public decimal Fee_Id_71 { get; set; }
        public decimal Fee_Id_71_Check { get; set; }
        public string Fee_Id_71_Val { get; set; }

        public decimal Fee_Id_72 { get; set; }
        public decimal Fee_Id_72_Check { get; set; }
        public string Fee_Id_72_Val { get; set; }

        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        #endregion

        #region các tài liệu trong đơn
        public string strDanhSachFileDinhKem { get; set; }

        public string Doc_Id_1 { get; set; }
        public string Doc_Id_102 { get; set; }

        public decimal Doc_Id_1_Check { get; set; }

        public string Doc_Id_2 { get; set; }
        public string Doc_Id_202 { get; set; }

        public decimal Doc_Id_2_Check { get; set; }

        public string Doc_Id_3 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }

        public string Doc_Id_4 { get; set; }
        public string Doc_Id_402 { get; set; }

        public decimal Doc_Id_4_Check { get; set; }

        public string Doc_Id_5 { get; set; }
        public decimal Doc_Id_5_Check { get; set; }

        public decimal Doc_Id_6_Check { get; set; }
        public string Doc_Id_6 { get; set; }

        public decimal Doc_Id_7_Check { get; set; }
        public string Doc_Id_7 { get; set; }

        public decimal Doc_Id_8_Check { get; set; }
        public string Doc_Id_8 { get; set; }


        public string Doc_Id_9 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }

        public string Doc_Id_10 { get; set; }
        public decimal Doc_Id_10_Check { get; set; }

        public string Doc_Id_11 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }

        public string Doc_Id_12 { get; set; }
        public decimal Doc_Id_12_Check { get; set; }

        public string Doc_Id_13 { get; set; }
        public decimal Doc_Id_13_Check { get; set; }

        public string Doc_Id_14 { get; set; }
        public decimal Doc_Id_14_Check { get; set; }

        public string Doc_Id_15 { get; set; }
        public decimal Doc_Id_15_Check { get; set; }

        public string Doc_Id_16 { get; set; }
        public decimal Doc_Id_16_Check { get; set; }

        public string Doc_Id_17 { get; set; }
        public decimal Doc_Id_17_Check { get; set; }
        #endregion

        #region add thêm 1 số cột tùy biến để sau này cần thì thêm vào đây
        public string Extent_fld01 { get; set; }

        public string Extent_fld02 { get; set; }

        public string Extent_fld03 { get; set; }

        public string Extent_fld04 { get; set; }

        public string Extent_fld05 { get; set; }

        public string Extent_fld06 { get; set; }

        public string Extent_fld07 { get; set; }

        public string Extent_fld08 { get; set; }

        public string Extent_fld09 { get; set; }

        public string Extent_fld10 { get; set; }


        #endregion
    }
}
