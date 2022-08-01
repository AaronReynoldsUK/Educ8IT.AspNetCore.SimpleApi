using System;
using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailServiceOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SettingsKey = "EmailService";

        /// <summary>
        /// 
        /// </summary>
        public string SmtpServerHostName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SmtpServerHostPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseTls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SmtpUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EmailSendTimeoutMS { get; set; } = 5000;

        /// <summary>
        /// TODO: add validation of values
        /// </summary>
        public bool OK
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCredentials
        {
            get
            {
                return !String.IsNullOrEmpty(SmtpUserName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public NetworkCredential GetCredentials()
        {
            if (string.IsNullOrEmpty(SmtpUserName))
            {
                return null;
            }
            else
            {
                return new NetworkCredential(SmtpUserName, SmtpPassword);
            }
        }
    }
}
