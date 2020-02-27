using System;
using System.Web;

namespace ObjectInfos
{

    public class App_Detail_F04_Info : ApplicationHeaderInfo
    {
        public static void CopyAppHeaderInfo(ref App_Detail_F04_Info p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
            p_appDetail.Id = pAppInfo.Id;
            p_appDetail.Appcode = pAppInfo.Appcode;
            p_appDetail.Master_Name = pAppInfo.Master_Name;
            p_appDetail.Master_Address = pAppInfo.Master_Address;
            p_appDetail.Master_Phone = pAppInfo.Master_Phone;
            p_appDetail.Master_Fax = pAppInfo.Master_Fax;
            p_appDetail.Master_Email = pAppInfo.Master_Email;
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

        public App_Detail_F04_Info()
        {

        }

        public App_Detail_F04_Info(App_Detail_F04_Info p_app_detail, ApplicationHeaderInfo p_app_header)
        {
            this.Language_Code = p_app_detail.Language_Code;
            this.App_Header_Id = p_app_detail.App_Header_Id;
            this.Appcode = p_app_detail.Appcode;
            this.Applicant_Type = p_app_detail.Applicant_Type;
            this.Business_Line = p_app_detail.Business_Line;
            this.Description = p_app_detail.Description;
            this.Codelogo = p_app_detail.Codelogo;
            this.Loainhanhieu = p_app_detail.Loainhanhieu;
            this.Sodon_ut = p_app_detail.Sodon_ut;
            this.Ngaynopdon_ut = p_app_detail.Ngaynopdon_ut;
            this.Ngaynopdon_ut_text = p_app_detail.Ngaynopdon_ut.ToString("dd/MM/yyyy");
            this.Nuocnopdon_ut = p_app_detail.Nuocnopdon_ut;
            this.Nuocnopdon_ut_text = p_app_detail.Nuocnopdon_ut_text;
            this.Translation_Of_Word = p_app_detail.Translation_Of_Word;
            this.Color = p_app_detail.Color;
            this.pfileLogo = p_app_detail.pfileLogo;

            this.LogourlOrg = p_app_detail.LogourlOrg;
            this.Logourl = p_app_detail.Logourl;
            this.ClassNo = p_app_detail.ClassNo;
            this.Duadate = p_app_detail.Duadate;
            this.DuadateText = p_app_detail.DuadateText;
            this.Number_Pic = p_app_detail.Number_Pic;
            this.Number_Page = p_app_detail.Number_Page;

            this.STT = p_app_header.STT;
            this.Appcode = p_app_header.Appcode;
            this.Master_Name = p_app_header.Master_Name;
            this.Master_Address = p_app_header.Master_Address;
            this.Master_Phone = p_app_header.Master_Phone;
            this.Master_Fax = p_app_header.Master_Fax;
            this.Master_Email = p_app_header.Master_Email;
            this.Rep_Master_Type = p_app_header.Rep_Master_Type;
            this.Rep_Master_Name = p_app_header.Rep_Master_Name;
            this.Rep_Master_Address = p_app_header.Rep_Master_Address;
            this.Rep_Master_Phone = p_app_header.Rep_Master_Phone;
            this.Rep_Master_Fax = p_app_header.Rep_Master_Fax;
            this.Rep_Master_Email = p_app_header.Rep_Master_Email;
            this.Relationship = p_app_header.Relationship;
            this.Send_Date = p_app_header.Send_Date;
            this.Status = p_app_header.Status;
            this.Status_Form = p_app_header.Status_Form;
            this.Status_Content = p_app_header.Status_Content;
            this.Remark = p_app_header.Remark;
            this.AppName = p_app_header.AppName;
            this.Address = p_app_header.Address;
            this.DateNo = p_app_header.DateNo;
            this.Months = p_app_header.Months;
            this.Years = p_app_header.Years;
        }
       
        public decimal Id { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Appno { get; set; }
        public string Case_Code { get; set; }
        public string Language_Code { get; set; }
        public decimal Applicant_Type { get; set; }
        public string Business_Line { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Translation_Of_Word { get; set; }
        public string Codelogo { get; set; }
        public string Loainhanhieu { get; set; }
        public string Sodon_ut { get; set; }
        public string Ngaynopdon_ut_text { get; set; }
        public DateTime Ngaynopdon_ut { get; set; }

        public DateTime Duadate { get; set; }

        public string DuadateText { get; set; }
        public decimal Nuocnopdon_ut { get; set; }
        public string Nuocnopdon_ut_text { get; set; }
        public string LogourlOrg { get; set; }
        public string Logourl { get; set; }
        public string ClassNo { get; set; }
        public HttpPostedFileBase pfileLogo { get; set; }

        public int Number_Pic { get; set; }
        public int Number_Page { get; set; }
        

        // các tài liệu trong đơn
        public string Doc_Id_1 { get; set; }
        public decimal Doc_Id_1_Check { get; set; }

        public string Doc_Id_2 { get; set; }
        public decimal Doc_Id_2_Check { get; set; }

        public string Doc_Id_3 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }

        public string Doc_Id_4 { get; set; }
        public decimal Doc_Id_4_Check { get; set; }

        public string Doc_Id_5 { get; set; }
        public decimal Doc_Id_5_Check { get; set; }

        public string Doc_Id_6 { get; set; }
        public decimal Doc_Id_6_Check { get; set; }

        public string Doc_Id_7 { get; set; }
        public decimal Doc_Id_7_Check { get; set; }

        public string Doc_Id_8 { get; set; }
        public decimal Doc_Id_8_Check { get; set; }

        public string Doc_Id_9 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }

        
        public string Doc_Id_101 { get; set; }
        public string Doc_Id_102 { get; set; }
        public string Doc_Id_103 { get; set; }
        public decimal Doc_Id_10_Check { get; set; }

        public string Doc_Id_11 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }

        public string Doc_Id_12 { get; set; }
        public decimal Doc_Id_12_Check { get; set; }


        public string Doc_Id_13 { get; set; }
        public decimal Doc_Id_13_Check { get; set; }

        public string Doc_Id_14 { get; set; }
        public decimal Doc_Id_14_Check { get; set; }

        public string Doc_Id_151 { get; set; }
        public string Doc_Id_152 { get; set; }
        public string Doc_Id_153 { get; set; }
       
        public decimal Doc_Id_15_Check { get; set; }

        public string Doc_Id_16 { get; set; }
        public decimal Doc_Id_16_Check { get; set; }

        public string Doc_Id_17 { get; set; }
        public decimal Doc_Id_17_Check { get; set; }

        public string Doc_Id_18 { get; set; }
        public decimal Doc_Id_18_Check { get; set; }

        public string Doc_Id_19 { get; set; }
        public decimal Doc_Id_19_Check { get; set; }

        public string Doc_Id_20 { get; set; }
        public decimal Doc_Id_20_Check { get; set; }
    }
}
