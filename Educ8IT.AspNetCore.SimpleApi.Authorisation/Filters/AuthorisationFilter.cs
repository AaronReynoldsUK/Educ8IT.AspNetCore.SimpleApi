// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointContext"></param>
        /// <returns></returns>
        public async Task<bool> PreExecution(EndpointContext endpointContext)
        {
            if (endpointContext == null)
                throw new ArgumentNullException(nameof(endpointContext));

            if (endpointContext.HttpContext == null)
                throw new InvalidOperationException($"Missing HttpContext for this request");

            if (endpointContext.Endpoint == null)
                throw new InvalidOperationException("There is no Endpoint object for this request");

            // Authorisation depends upon Authentication.
            if (!endpointContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // This might not work if type is not exactly ApiMethodItem, experiment with this
            if (endpointContext.ApiMethodItem == null)
                throw new InvalidOperationException("There is no ApiMethodItem object for this request");

            // Authorisation
            if (endpointContext.ApiMethodItem is AuthorisationAwareApiMethodItem authorisationAwareApiMethodItem)
            {
                if (authorisationAwareApiMethodItem.AuthoriseAttribute != null)
                {
                    var authCheck = await endpointContext.HttpContext
                        .AuthorisationCheck(authorisationAwareApiMethodItem);

                    if (!authCheck.Succeeded)
                    {
                        if (authCheck.Failure.FailCalled)
                        {
                            // Log that Fail was called
                        }
                        else
                        {
                            foreach (var __failedRequirement in authCheck.Failure.FailedRequirements)
                            {
                                if (__failedRequirement is IAuthorisationRequirement authorisationRequirement)
                                {
                                    if (!String.IsNullOrEmpty(authorisationRequirement.SchemeName))
                                    {
                                        await endpointContext.HttpContext.ChallengeAsync(authorisationRequirement.SchemeName);
                                    }
                                    else
                                    {
                                        await endpointContext.HttpContext.ChallengeAsync();
                                    }                                    
                                    return false;
                                }

                                // Log which requirements failed...
                                //__failedRequirement.GetType().Name;

                                //if (__failedRequirement.)
                            }
                        }

                        await endpointContext.HttpContext.ForbidAsync();
                        throw new CustomHttpException("Not Authorised", HttpStatusCode.Forbidden);
                    }
                }
            }

            return false;
        }
    }
}
