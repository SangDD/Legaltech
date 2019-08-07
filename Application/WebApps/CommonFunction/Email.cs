using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace WebApps
{

    public class EmailInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string EmailCC { get; set; }

        public string EMailFrom { get; set; }
        public string PassWord { get; set; }
        public string DisplayName { get; set; }

        public string EMailFrom_Business { get; set; }
        public string PassWord_Business { get; set; }
        public string DisplayName_Business { get; set; }

        
        public bool IsSsl { get; set; }
        //public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

    }

    public class EmailHelper
    {
        public static EmailInfo EmailOriginal { get; set; }

        /// <summary>
        /// Hàm gửi email
        /// </summary>
        /// <param name="p_Subject">Chủ đề</param>
        /// <param name="p_content">Nội dung gửi</param>
        /// <param name="p_emailTo">người nhận, cách nhau = dấu ;</param>
        /// <param name="p_EmailCC">người cc, cách nhau = dấu ;</param>
        /// <param name="oMsg"></param>
        /// <returns></returns>
        public static bool SendMail(string p_emailFrom, string p_pass, string p_display_name, string p_emailTo, string p_EmailCC, string p_Subject, string p_content, List<string> p_LstAttachment)
        {
            //EmailInfo pEmail = EmailHelper.EmailOriginal;
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(p_emailFrom, p_display_name);
                    foreach (var emailTo in p_emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        mail.To.Add(emailTo);
                    mail.Subject = p_Subject;
                    mail.Body = p_content;
                    mail.IsBodyHtml = true;

                    // cc cho chị tuyến
                    if (EmailHelper.EmailOriginal.EmailCC != null && EmailHelper.EmailOriginal.EmailCC != "")
                        mail.CC.Add(EmailHelper.EmailOriginal.EmailCC);

                    if (!string.IsNullOrEmpty(p_EmailCC))
                    {
                        foreach (var emailCC in p_EmailCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                            mail.CC.Add(emailCC);
                    }

                    if (p_LstAttachment != null)
                    {
                        var strAttachMent = p_LstAttachment.ToArray();
                        if (strAttachMent.Length > 0)
                            foreach (var vAttach in strAttachMent)
                                mail.Attachments.Add(new Attachment(vAttach));
                    }

                    using (var smtp = new SmtpClient(EmailHelper.EmailOriginal.Host, EmailHelper.EmailOriginal.Port))
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential(p_emailFrom, p_pass);
                        smtp.EnableSsl = EmailHelper.EmailOriginal.IsSsl;
                        try
                        {
                            smtp.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
    }
}