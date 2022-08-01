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
    public class MockEmailService : IEmailService
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
        public MockEmailService(
            ILogger<MockEmailService> logger,
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

            await Task.CompletedTask;

            return true;
        }

        #endregion
    }
}
