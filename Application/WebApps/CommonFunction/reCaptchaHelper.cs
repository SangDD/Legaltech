namespace WebApps.CommonFunction
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Web.Script.Serialization;

    // ReSharper disable once StyleCop.SA1300
    public class reCaptchaHelper
    {
        public bool IsReCaptchaValid { get; set; }

       
        public void ValidateReCaptcha(string reCaptchaResponse)
        {
            try
            {
                string _RecaptchaSecretKey =  CommonFuc.GetConfig("RecaptchaSecretKey");
                if (string.IsNullOrEmpty(reCaptchaResponse) || string.IsNullOrWhiteSpace(reCaptchaResponse)) return;

                // Request to Google Server
                var req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + _RecaptchaSecretKey + "&response=" + reCaptchaResponse.Trim('"'));

                // Google recaptcha Response
                using (var wResponse = req.GetResponse())
                {
                    var responseStream = wResponse.GetResponseStream();
                    if (responseStream == null) return;
                    using (var readStream = new StreamReader(responseStream))
                    {
                        var jsonResponse = readStream.ReadToEnd();
                        var js = new JavaScriptSerializer();
                        var data = js.Deserialize<GoogleResponse>(jsonResponse); // Deserialize Json
                        this.IsReCaptchaValid = Convert.ToBoolean(data.Success);
                    }
                }
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        // object json contain data response from google for valid reCaptcha
       
        public class GoogleResponse
        {
          
            public bool Success { get; set; }
        
            public List<string> ErrorCodes { get; set; }
        }
    }
}