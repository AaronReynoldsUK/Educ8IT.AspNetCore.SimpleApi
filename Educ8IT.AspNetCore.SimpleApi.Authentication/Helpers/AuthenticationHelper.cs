using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticateAttribute"></param>
        /// <param name="secondAttribute"></param>
        public static void CombineWith(this AuthenticateAttribute authenticateAttribute, AuthenticateAttribute secondAttribute)
        {
            if (authenticateAttribute == null)
                return;

            if (secondAttribute == null)
                return;

            authenticateAttribute.AuthenticationTypeRequired = authenticateAttribute.AuthenticationTypeRequired & secondAttribute.AuthenticationTypeRequired;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="achieved"></param>
        ///// <param name="required"></param>
        ///// <returns></returns>
        //public static bool HasAuthenticationType(this EAuthenticationType achieved, EAuthenticationType required)
        //{
        //    return ((achieved & required) == required);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="authenticationProperties"></param>
        ///// <param name="authenticationType"></param>
        //public static void AddAuthenticationType(this AuthenticationProperties authenticationProperties, EAuthenticationType authenticationType)
        //{
        //    if (authenticationProperties == null)
        //        return;

        //    authenticationProperties.SetParameter("EAuthenticationType", authenticationType);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="authenticationProperties"></param>
        ///// <returns></returns>
        //public static EAuthenticationType GetAuthenticationType(this AuthenticationProperties authenticationProperties)
        //{
        //    if (authenticationProperties == null)
        //        return EAuthenticationType.None;

        //    return authenticationProperties.GetParameter<EAuthenticationType>("EAuthenticationType");
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="claimsPrincipal"></param>
        ///// <returns></returns>
        //public static EAuthenticationType GetAuthenticationType(this ClaimsPrincipal claimsPrincipal)
        //{
        //    if (claimsPrincipal == null)
        //        return EAuthenticationType.None;

        //    if (claimsPrincipal.Claims == null)
        //        return EAuthenticationType.None;

        //    var __claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimType);

        //    if (__claim == null)
        //        return EAuthenticationType.None;


        //    if (!int.TryParse(__claim.Value, out int iValue))
        //        return EAuthenticationType.None;

        //    return (EAuthenticationType)iValue;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="claim"></param>
        public static void AddClaim(this ClaimsPrincipal claimsPrincipal, Claim claim)
        {
            var __claimsIdentity = ((ClaimsIdentity)claimsPrincipal.Identity);

            if (!__claimsIdentity.HasClaim(c => c.Type == claim.Type))
                __claimsIdentity.AddClaim(claim);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static Claim GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var __claimsIdentity = ((ClaimsIdentity)claimsPrincipal.Identity);

            return (__claimsIdentity.HasClaim(c => c.Type == claimType))
                ? __claimsIdentity.Claims.FirstOrDefault(c => c.Type == claimType)
                : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static bool HasClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var __claimsIdentity = ((ClaimsIdentity)claimsPrincipal.Identity);

            return __claimsIdentity.HasClaim(c => c.Type == claimType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationProperties"></param>
        /// <param name="claimType"></param>
        public static void SetRequiredClaim(this AuthenticationProperties authenticationProperties, string claimType)
        {
            if (authenticationProperties == null)
                return;

            authenticationProperties.SetParameter(AuthenticationClaimTypes.RequiredClaim, claimType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationProperties"></param>
        public static string GetRequiredClaim(this AuthenticationProperties authenticationProperties)
        {
            if (authenticationProperties == null)
                return null;

            return authenticationProperties.GetParameter<string>(AuthenticationClaimTypes.RequiredClaim);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="claimsPrincipal"></param>
        ///// <param name="authenticationType"></param>
        //public static void SetAuthenticationType(this ClaimsPrincipal claimsPrincipal, EAuthenticationType authenticationType)
        //{
        //    if (claimsPrincipal == null)
        //        return;

        //    var __claim = new Claim(
        //        AuthenticationTypeHelper.ClaimType, 
        //        ((int)authenticationType).ToString(),
        //        typeof(int).Name);

        //    claimsPrincipal.AddClaim(__claim);
        //}
    }
}
