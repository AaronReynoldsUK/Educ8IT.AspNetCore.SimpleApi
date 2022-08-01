// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authentication;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> PreExecution(EndpointContext endpointContext)
        {
            if (endpointContext == null)
                throw new ArgumentNullException(nameof(endpointContext));

            if (endpointContext.HttpContext == null)
                throw new InvalidOperationException($"Missing HttpContext for this request");

            if (endpointContext.Endpoint == null)
                throw new InvalidOperationException("There is no Endpoint object for this request");

            // This might not work if type is not exactly ApiMethodItem, experiment with this
            if (endpointContext.ApiMethodItem == null)
                throw new InvalidOperationException("There is no ApiMethodItem object for this request");

            // Authentication
            if (endpointContext.ApiMethodItem is AuthenticationAwareApiMethodItem authenticationAwareApiMethodItem)
            {
                if (authenticationAwareApiMethodItem.AuthenticateAttribute != null)
                {
                    var __authenticationRequired = authenticationAwareApiMethodItem.AuthenticateAttribute?.AuthenticationTypeRequired ?? EAuthenticationType.None;
                    
                    if (__authenticationRequired == EAuthenticationType.None)
                    {
                        // OK, no Auth required
                    }
                    else
                    {
                        if (__authenticationRequired.HasFlag(EAuthenticationType.Identity) && !endpointContext.HttpContext.User.HasClaim(AuthenticationClaimTypes.Authenticated))
                        {
                            // Challenge
                            var __ = new AuthenticationProperties();
                            __.SetRequiredClaim(AuthenticationClaimTypes.Authenticated);
                            await endpointContext.HttpContext.ChallengeAsync(__);
                            return true;
                        }
                        else if (__authenticationRequired.HasFlag(EAuthenticationType.Email) && !endpointContext.HttpContext.User.HasClaim(AuthenticationClaimTypes.EmailConfirmed))
                        {
                            // Challenge
                            var __ = new AuthenticationProperties();
                            __.SetRequiredClaim(AuthenticationClaimTypes.EmailConfirmed);
                            await endpointContext.HttpContext.ChallengeAsync(__);
                            return true;
                        }
                        else if (__authenticationRequired.HasFlag(EAuthenticationType.MFA) && !endpointContext.HttpContext.User.HasClaim(AuthenticationClaimTypes.Mfa))
                        {
                            // Challenge
                            var __ = new AuthenticationProperties();
                            __.SetRequiredClaim(AuthenticationClaimTypes.Mfa);
                            await endpointContext.HttpContext.ChallengeAsync(__);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointContext"></param>
        /// <returns></returns>
        public async Task<bool> PostExecution(EndpointContext endpointContext)
        {
            return await Task.FromResult<bool>(false);
            //throw new NotImplementedException();
        }
    }
}
