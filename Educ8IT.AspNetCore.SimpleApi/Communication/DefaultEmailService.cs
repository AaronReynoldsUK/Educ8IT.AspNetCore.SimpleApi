using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultEmailService : IEmailService
    {
        #region Fields

        private readonly ILogger _iLogger;
        private readonly EmailServiceOptions _emailServiceOptions;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefaultEmailService(
            ILogger<DefaultEmailService> logger,
            IOptions<EmailServiceOptions> options)
        {
            _iLogger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailServiceOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        #region IEmailService

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHtml"></param>
        /// <param name="To"></param>
        /// <param name="Cc"></param>
        /// <param name="Bcc"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SendEmailAsync(
            string subject, string body, bool isHtml,
            List<MailAddress> To,
            List<MailAddress> Cc,
            List<MailAddress> Bcc)
        {
            if (String.IsNullOrEmpty(subject))
                return false;

            if (String.IsNullOrEmpty(body))
                return false;

            if ((To == null || To.Count == 0)
                && (Cc == null || Cc.Count == 0)
                && (Bcc == null || Bcc.Count == 0))
            {
                return false;
            }

            if (!_emailServiceOptions.OK)
                return false;

            try
            {
                SmtpClient __client = new SmtpClient(
                    _emailServiceOptions.SmtpServerHostName,
                    _emailServiceOptions.SmtpServerHostPort);

                if (_emailServiceOptions.HasCredentials)
                {
                    __client.Credentials = _emailServiceOptions.GetCredentials();
                }

                MailMessage __message = new MailMessage();

                // Recipients
                if (To != null)
                    foreach (MailAddress __to in To)
                        __message.To.Add(__to);

                // Cc
                if (Cc != null)
                    foreach (MailAddress __cc in Cc)
                        __message.CC.Add(__cc);

                // Bcc
                if (Bcc != null)
                    foreach (MailAddress __bcc in Bcc)
                        __message.Bcc.Add(__bcc);

                // Subject
                __message.Subject = subject;

                // Body
                __message.Body = body;
                __message.BodyEncoding = Encoding.UTF8;
                __message.IsBodyHtml = isHtml;

                // From / Sender
                __message.Sender = new MailAddress(_emailServiceOptions.SenderEmail, _emailServiceOptions.SenderName ?? _emailServiceOptions.SenderEmail);
                __message.From = new MailAddress(_emailServiceOptions.SenderEmail, _emailServiceOptions.SenderName ?? _emailServiceOptions.SenderEmail);

                // TLS ??
                __client.EnableSsl = _emailServiceOptions.UseTls;

                // Send
                __client.Timeout = _emailServiceOptions.EmailSendTimeoutMS;

                await __client.SendMailAsync(__message);

                return true;
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex,
                    "Email sending error with subject {subject}",
                    new string[] { subject });
            }

            return false;
        }

        #endregion
    }
}
