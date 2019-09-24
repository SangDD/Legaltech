using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class C08_Info : ApplicationHeaderInfo
    {
        public decimal C08_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string SO_DON_DK_QTNH { get; set; }
        public string SO_DK_QTNH { get; set; }
        public DateTime NGAYNOPDON_DKQTNH { get; set; }
        public string LOAI_DK { get; set; }
 
 
    }

    public class C08_Info_Export : ApplicationHeaderInfo
    {

        public decimal C08_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string SO_DON_DK_QTNH { get; set; }
        public string SO_DK_QTNH { get; set; }
        public DateTime NGAYNOPDON_DKQTNH { get; set; }
        public string LOAI_DK { get; set; }
        #region Chủ đơn khác

        public string Master_Name_1 { set; get; }
        public string Master_Address_1 { set; get; }
        public string Master_Phone_1 { set; get; }
        public string Master_Fax_1 { set; get; }
        public string Master_Email_1 { set; get; }


        public string Master_Name_2 { set; get; }
        public string Master_Address_2 { set; get; }
        public string Master_Phone_2 { set; get; }
        public string Master_Fax_2 { set; get; }
        public string Master_Email_2 { set; get; }

        #endregion


        #region Phí
        public string Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }
        public string Fee_Id_11 { get; set; }
        public decimal Fee_Id_11_Check { get; set; }
        public string Fee_Id_11_Val { get; set; }
        public string Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }

        public string Fee_Id_21 { get; set; }
        public decimal Fee_Id_21_Check { get; set; }
        public string Fee_Id_21_Val { get; set; }

        public string Fee_Id_3 { get; set; }
        public decimal Fee_Id_3_Check { get; set; }
        public string Fee_Id_3_Val { get; set; }
        public string Fee_Id_31 { get; set; }
        public decimal Fee_Id_31_Check { get; set; }
        public string Fee_Id_31_Val { get; set; }
        public string Fee_Id_4 { get; set; }
        public decimal Fee_Id_4_Check { get; set; }
        public string Fee_Id_4_Val { get; set; }

        public string Fee_Id_41 { get; set; }
        public decimal Fee_Id_41_Check { get; set; }
        public string Fee_Id_41_Val { get; set; }

        public string Fee_Id_5 { get; set; }
        public decimal Fee_Id_5_Check { get; set; }
        public string Fee_Id_5_Val { get; set; }
        public string Fee_Id_51 { get; set; }
        public decimal Fee_Id_51_Check { get; set; }
        public string Fee_Id_51_Val { get; set; }

        public string Fee_Id_6 { get; set; }
        public decimal Fee_Id_6_Check { get; set; }
        public string Fee_Id_6_Val { get; set; }

        public string Fee_Id_61 { get; set; }
        public decimal Fee_Id_61_Check { get; set; }
        public string Fee_Id_61_Val { get; set; }

        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        public string strListClass { get; set; }
        
        #endregion

        #region các tài liệu trong đơn

        string _strDsFile = "";
        public string strDanhSachFileDinhKem
        { get { return _strDsFile; } set { _strDsFile = value; } }
        public string Doc_Id_0 { get; set; }
        public string Doc_Id_002 { get; set; }

        public decimal Doc_Id_0_Check { get; set; }

        public string Doc_Id_1 { get; set; }
        public string Doc_Id_102 { get; set; }

        public decimal Doc_Id_1_Check { get; set; }

        public string Doc_Id_2 { get; set; }
        public string Doc_Id_202 { get; set; }

        public decimal Doc_Id_2_Check { get; set; }

        public string Doc_Id_3 { get; set; }
        public string Doc_Id_302 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }
        public string Doc_Id_4 { get; set; }
        public string Doc_Id_402 { get; set; }

        public decimal Doc_Id_4_Check { get; set; }
        public string Doc_Id_5 { get; set; }
        public string Doc_Id_502 { get; set; }

        public decimal Doc_Id_5_Check { get; set; }
        public decimal Doc_Id_6_Check { get; set; }

        public string Doc_Id_6 { get; set; }
        public string Doc_Id_602 { get; set; }
        public decimal Doc_Id_7_Check { get; set; }
        public string Doc_Id_702 { get; set; }

        public string Doc_Id_7 { get; set; }
        public decimal Doc_Id_8_Check { get; set; }
        public string Doc_Id_802 { get; set; }

        public string Doc_Id_8 { get; set; }

        public string Doc_Id_9 { get; set; }
        public string Doc_Id_902 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }

        public string Doc_Id_10 { get; set; }
        public string Doc_Id_1002 { get; set; }
        public decimal Doc_Id_10_Check { get; set; }
        public string Doc_Id_11 { get; set; }
        public string Doc_Id_1102 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }

        public string Doc_Id_12 { get; set; }
        public string Doc_Id_1202 { get; set; }
        public decimal Doc_Id_12_Check { get; set; }

        public string Doc_Id_13 { get; set; }
        public string Doc_Id_1302 { get; set; }
        public decimal Doc_Id_13_Check { get; set; }

        public string Doc_Id_14 { get; set; }
        public string Doc_Id_1402 { get; set; }
        public decimal Doc_Id_14_Check { get; set; }

        public string Doc_Id_15 { get; set; }
        public string Doc_Id_1502 { get; set; }
        public decimal Doc_Id_15_Check { get; set; }

        public string Doc_Id_16 { get; set; }
        public string Doc_Id_1602 { get; set; }
        public decimal Doc_Id_16_Check { get; set; }

        public string Doc_Id_17 { get; set; }
        public string Doc_Id_1702 { get; set; }
        public decimal Doc_Id_17_Check { get; set; }

        public string Doc_Id_18 { get; set; }
        public string Doc_Id_1802 { get; set; }
        public decimal Doc_Id_18_Check { get; set; }

        public string Doc_Id_19 { get; set; }
        public string Doc_Id_1902 { get; set; }
        public decimal Doc_Id_19_Check { get; set; }

        public string Doc_Id_20 { get; set; }
        public string Doc_Id_2002 { get; set; }
        public decimal Doc_Id_20_Check { get; set; }
 


        #endregion

        public static void CopyC08_Info(ref C08_Info_Export p_appDetail, C08_Info p_C08_Info)
        {
            p_appDetail.C08_Id = p_C08_Info.C08_Id;
            p_appDetail.Language_Code = p_C08_Info.Language_Code;
            p_appDetail.SO_DON_DK_QTNH = p_C08_Info.SO_DON_DK_QTNH;
            p_appDetail.SO_DK_QTNH = p_C08_Info.SO_DK_QTNH;
            p_appDetail.NGAYNOPDON_DKQTNH = p_C08_Info.NGAYNOPDON_DKQTNH;
            p_appDetail.LOAI_DK = p_C08_Info.LOAI_DK;
          

    }
        public static void CopyAppHeaderInfo(ref C08_Info_Export p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
            p_appDetail.Appcode = pAppInfo.Appcode;
            p_appDetail.Master_Name = pAppInfo.Master_Name;
            p_appDetail.Master_Address = pAppInfo.Master_Address;
            p_appDetail.Master_Phone = pAppInfo.Master_Phone;
            p_appDetail.Master_Fax = pAppInfo.Master_Fax;
            p_appDetail.Master_Email = pAppInfo.Master_Email;

            p_appDetail.Master_Type = pAppInfo.Master_Type == null ? "" : pAppInfo.Master_Type;
            p_appDetail.Customer_Code = pAppInfo.Customer_Code == null ? "239" : pAppInfo.Customer_Code;

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
       
        public static void CopyOther_MasterInfo(ref C08_Info_Export p_appDetail, Other_MasterInfo pAppInfo, int p_position)
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
                }
                else
                {
                    p_appDetail.Master_Name_1 = "";
                    p_appDetail.Master_Address_1 = "";
                    p_appDetail.Master_Phone_1 = "";
                    p_appDetail.Master_Fax_1 = "";
                    p_appDetail.Master_Email_1 = "";
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

                }
                else
                {
                    p_appDetail.Master_Name_2 = "";
                    p_appDetail.Master_Address_2 = "";
                    p_appDetail.Master_Phone_2 = "";
                    p_appDetail.Master_Fax_2 = "";
                    p_appDetail.Master_Email_2 = "";

                }
            }
        }

    }
}
