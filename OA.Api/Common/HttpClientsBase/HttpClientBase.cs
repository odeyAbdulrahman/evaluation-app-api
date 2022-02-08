
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using OA.Api.ConfigureServices;
using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Dtos;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OA.Api.Common.HttpClientsBase
{
    class HttpClientBase : IHttpClientBase
    {
        public HttpClientBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly IConfiguration Configuration;
        readonly HttpClient Http = new HttpClient();
        public bool Valid { get; private set; } = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Response"></param>
        /// <returns></returns>
        public bool GoogleRecaptcha(string Response)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create($"{Configuration.RecaptchaSettings().RecaptchaDomin}?secret={Configuration.RecaptchaSettings().RecaptchaSecretKey}&response={Response}");
            //Google recaptcha Response
            using (WebResponse wResponse = req.GetResponse())
            {
                using StreamReader readStream = new StreamReader(wResponse.GetResponseStream());
                string jsonResponse = readStream.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                GoogleRecaptchaViewModel data = js.Deserialize<GoogleRecaptchaViewModel>(jsonResponse);// Deserialize Json
                Valid = Convert.ToBoolean(data.success);
            }
            return Valid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <param name="Title"></param>
        /// <param name="Body"></param>
        /// <param name="Data"></param>
        /// <param name="AndroidChannelId"></param>
        /// <returns></returns>
        public FeedBack PushNotification(string DeviceId, string Title, string Body, object Data = null, string AndroidChannelId = null)
        {
            WebRequest tRequest = WebRequest.Create(Configuration.FireBaseSettings().FireBaseDomin);
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", Configuration.FireBaseSettings().FireBaseKey));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", Configuration.FireBaseSettings().Sender));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = DeviceId,
                priority = "high",
                content_available = true,
                notification = new { body = Body, title = Title, android_channel_id = AndroidChannelId, badge = 1 },
                data = new { Order = Data, click_action = "FLUTTER_NOTIFICATION_CLICK" }
            };
            string postbody = JsonHelper.Serialize(payload); 
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            tRequest.Timeout = 30000;
            using Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            using WebResponse tResponse = tRequest.GetResponse();
            using Stream dataStreamResponse = tResponse.GetResponseStream();
            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                {
                    String sResponseFromServer = tReader.ReadToEnd();
                    FCMResponse response = JsonHelper.Deserialize<FCMResponse>(sResponseFromServer);
                    tReader.Close(); dataStream.Close(); tResponse.Close();
                    return FeedBack.SendSuccessfully;
                }
            else
            {
                return FeedBack.ServeErrorFail;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="lable"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<string> SendSMSCode(string phone, string lable, string body)
        {
            string smsUrl = $"{Configuration.SMSGatewaySettings().SMSGatewayDomin}user={Configuration.SMSGatewaySettings().GatewayUser}&pwd={Configuration.SMSGatewaySettings().GatewayPassword}&smstext={lable}: {body}&Sender={Configuration.SMSGatewaySettings().GatewaySender}&Nums=249{phone}";
            HttpResponseMessage res = await Http.GetAsync(smsUrl);
            if (res.IsSuccessStatusCode)
            {
                // Get the response
                var customerJsonString = res.Content.ReadAsStringAsync();
                return customerJsonString.Result;
            }
            else
            {
                return "The message has not been sent. Try again";
            }
        }

        public FeedBack Mailer(MailerDominModel data, EnumMailer MailerType)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(data.Receiver);// Email-ID of Receiver  
                message.Subject = data.Title;// Subject of Email  
                message.From = new MailAddress(data.Sender);// Email-ID of Sender  
                message.IsBodyHtml = true;
                if (MailerType == EnumMailer.ContactMail)
                    message.AlternateViews.Add(ContactMailBody(data.Name, data.Email, data.Phone, data.Title, data.MessageBody));
                if (MailerType == EnumMailer.ResetPassword)
                    message.AlternateViews.Add(ResetPasswordMailBody(data.MessageBody));
                SmtpClient SmtpMail = new SmtpClient
                {
                    Host = data.Host,
                    Port = 25,
                    Credentials = new NetworkCredential(data.Sender, data.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = false
                };
                SmtpMail.ServicePoint.MaxIdleTime = 0;
                SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
                message.BodyEncoding = Encoding.Default;
                message.Priority = MailPriority.High;
                SmtpMail.Send(message); //Smtpclient to send the mail message  
                return FeedBack.SendSuccessfully;
            }
            catch (Exception)
            {
                return FeedBack.ServeErrorFail;
            }
        }
        private static AlternateView ContactMailBody(string name, string Email, string Phone, string title, string message)
        {
            string body = "<P> الاسم :" + name + " <br />" +

                          "البريد الالكتروني:" + Email + "<br /><br />" +

                          "<b> رقم الهاتف " + Phone + "</b> <br /><br /> " +

                          "<b> عنوان الموضوع " + title + "</b> <br /><br /> " +

                          "<P> الموضوع " + message + "</P> <br /><br /> ";

            string str = @"  
                <table>  
                    <tr>  
                        <td> 
                            " + body + @" 
                        </td>  
                    </tr>  
                 </table>  
                ";
            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            return AV;
        }
        private static AlternateView ResetPasswordMailBody(string Url)
        {
            string body = "<P></p>";

            string str = @"  
                <table>  
                    <tr>  
                        <td> 
                            " + body + @" 
                        </td>  
                    </tr>  
                 </table>  
                ";
            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            return AV;
        }
    }
}
