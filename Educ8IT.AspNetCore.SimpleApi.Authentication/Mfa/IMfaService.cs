namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// Used to provide MFA.
    /// </summary>
    public interface IMfaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetSchemeName(Microsoft.AspNetCore.Http.HttpContext context, SimpleApi.Identity.EMfaMethod method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetCurrentOTC(Microsoft.AspNetCore.Http.HttpContext context, string scheme);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<bool> IsValidOTC(Microsoft.AspNetCore.Http.HttpContext context, string scheme, string code);

        /// <summary>
        /// used by apps (includes secret)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetProvisionUri(Microsoft.AspNetCore.Http.HttpContext context, string scheme);

        /// <summary>
        /// used by email, sms, call etc
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
        /// <returns></returns>
        System.Threading.Tasks.Task<ActionResults.ActionResult> Prompt(
            Microsoft.AspNetCore.Http.HttpContext context,
            SimpleApi.Identity.ApiUser user,
            SimpleApi.Identity.ApiMfa apiMfa);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="apiMfa"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ActionResults.ActionResult> Verify(
            Microsoft.AspNetCore.Http.HttpContext context,
            SimpleApi.Identity.ApiUser user,
            SimpleApi.Identity.ApiMfa apiMfa, string code);
    }
}
