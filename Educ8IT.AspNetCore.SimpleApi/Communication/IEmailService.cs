using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Replace all occurrences of markers with values in the template.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ReplaceIn(string template, Dictionary<string, string> parameters)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return template.ReplaceIn(parameters);
        }

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
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        // we will later add methods for attachments + image embedding

    }
}
