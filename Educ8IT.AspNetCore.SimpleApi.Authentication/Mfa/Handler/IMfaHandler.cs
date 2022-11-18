using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMfaHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task InitializeAsync(MfaScheme scheme, Microsoft.AspNetCore.Http.HttpContext context);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task<bool> HandleRequestAsync()
        {
            return System.Threading.Tasks.Task.FromResult(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetCurrentOTC();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<bool> IsValidOTC(Microsoft.AspNetCore.Http.HttpContext context, string scheme, string code);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetProvisionUri(Microsoft.AspNetCore.Http.HttpContext context, string scheme);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetPromptUri(Microsoft.AspNetCore.Http.HttpContext context, string scheme);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="apiMfa"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ActionResults.ActionResult> Prompt(
            Microsoft.AspNetCore.Http.HttpContext context, 
            SimpleApi.Identity.ApiUser user, 
            SimpleApi.Identity.ApiMfa apiMfa, string scheme);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="apiMfa"></param>
        /// <param name="scheme"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ActionResults.ActionResult> Verify(
            Microsoft.AspNetCore.Http.HttpContext context,
            SimpleApi.Identity.ApiUser user,
            SimpleApi.Identity.ApiMfa apiMfa, string scheme, string code);
    }
}
