﻿using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Dtos;
using System.Threading.Tasks;

namespace OA.Api.Common.HttpClientsBase
{
    public interface IHttpClientBase
    {
        /// <summary>
        /// This mathod send sms messages by sms get way
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="Lable"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<string> SendSMSCode(string Phone, string Lable, string body);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="MailerType"></param>
        /// <returns></returns>
        FeedBack Mailer(MailerDominModel data, EnumMailer MailerType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Response"></param>
        /// <returns></returns>
        bool GoogleRecaptcha(string Response);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <param name="Title"></param>
        /// <param name="Body"></param>
        /// <param name="Data"></param>
        /// <param name="AndroidChannelId"></param>
        /// <returns></returns>
        FeedBack PushNotification(string DeviceId, string Title, string Body, object Data = null, string AndroidChannelId = null);
    }
}