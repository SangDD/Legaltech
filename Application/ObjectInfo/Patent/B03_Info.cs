using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class B03_Info : ApplicationHeaderInfo
    {
        public B03_Info()
        {

        }
        //public decimal Id { set; get; }
        //public string AppCode { set; get; }
        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string App_Detail_Number { get; set; }
        public string Name_Evaluator { get; set; }
        public string Address_Evaluator { get; set; }
        public string Phone_Evaluator { get; set; }
        public string Fax_Evaluator { get; set; }
        public string Email_Evaluator { get; set; }
        public string Type_Evaluator { get; set; }
        public decimal Point { get; set; }
        public string Thamdinhnoidung { get; set; }
    }

    public class B03_Info_Export : ApplicationHeaderInfo
    {

        public static void CopyA01_Info(ref B03_Info_Export p_appDetail, B03_Info p_B03_Info)
        {
            p_appDetail.App_Header_Id = p_B03_Info.App_Header_Id;
            p_appDetail.Language_Code = p_B03_Info.Language_Code;
            p_appDetail.Appcode = p_B03_Info.Appcode;
            p_appDetail.Case_Code = p_B03_Info.Case_Code;
            p_appDetail.App_Header_Id = p_B03_Info.App_Header_Id;
            p_appDetail.App_Detail_Number = p_B03_Info.App_Detail_Number;
            p_appDetail.Name_Evaluator = p_B03_Info.Name_Evaluator;
            p_appDetail.Address_Evaluator = p_B03_Info.Address_Evaluator;
            p_appDetail.Phone_Evaluator = p_B03_Info.Phone_Evaluator;
            p_appDetail.Fax_Evaluator = p_B03_Info.Fax_Evaluator;
            p_appDetail.Email_Evaluator = p_B03_Info.Email_Evaluator;
            p_appDetail.Type_Evaluator = p_B03_Info.Type_Evaluator;
            p_appDetail.Thamdinhnoidung = p_B03_Info.Thamdinhnoidung == null ? "" : p_B03_Info.Thamdinhnoidung;
            p_appDetail.Point = p_B03_Info.Point;
           

        }

        public static void CopyAppHeaderInfo(ref B03_Info_Export p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
            //p_appDetail.ID = pAppInfo.Id;
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
        #region Phí

        public string Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }

        public string Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }

        public string Fee_Id_21 { get; set; }
        public decimal Fee_Id_21_Check { get; set; }
        public string Fee_Id_21_Val { get; set; }
        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        #endregion

        #region các tài liệu trong đơn
        public string strDanhSachFileDinhKem { get; set; }

        public string Doc_Id_001 { get; set; }
        public string Doc_Id_002 { get; set; }
        public decimal Doc_Id_00_Check { get; set; }

        public string Doc_Id_011 { get; set; }
        public string Doc_Id_012 { get; set; }
        public decimal Doc_Id_01_Check { get; set; }

        public string Doc_Id_021 { get; set; }
        public string Doc_Id_022 { get; set; }
        public decimal Doc_Id_02_Check { get; set; }

        public string Doc_Id_031 { get; set; }
        public string Doc_Id_032 { get; set; }
        public decimal Doc_Id_03_Check { get; set; }

        public string Doc_Id_041 { get; set; }
        public string Doc_Id_042 { get; set; }
        public decimal Doc_Id_04_Check { get; set; }

        public string Doc_Id_051 { get; set; }
        public string Doc_Id_052 { get; set; }
        public decimal Doc_Id_05_Check { get; set; }

        public string Doc_Id_061 { get; set; }
        public string Doc_Id_062 { get; set; }
        public decimal Doc_Id_06_Check { get; set; }

        public string Doc_Id_071 { get; set; }
        public string Doc_Id_072 { get; set; }
        public decimal Doc_Id_07_Check { get; set; }

        public string Doc_Id_081 { get; set; }
        public string Doc_Id_082 { get; set; }
        public decimal Doc_Id_08_Check { get; set; }
        #endregion
        public decimal ID { set; get; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string App_Detail_Number { get; set; }
        public string Name_Evaluator { get; set; }
        public string Address_Evaluator { get; set; }
        public string Phone_Evaluator { get; set; }
        public string Fax_Evaluator { get; set; }
        public string Email_Evaluator { get; set; }
        public string Type_Evaluator { get; set; }
        public decimal Point { get; set; }
        public string Thamdinhnoidung { get; set; }

    }
}
