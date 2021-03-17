// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public static class DefaultAuthorisationPolicies
    {
        #region Authenticated Policy

        /// <summary>
        /// 
        /// </summary>
        public const string AuthenticatedPolicyName = "AuthenticatedPolicy";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AuthenticatedPolicy()
        {
            AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();
            authorizationPolicyBuilder.Requirements.Add(new AuthenticatedRequirement());

            return authorizationPolicyBuilder.Build();
        }

        #endregion

        #region RolePolicy

        /// <summary>
        /// 
        /// </summary>
        public const string RolePolicyName = "RolePolicy";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy RolePolicy()
        {
            AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();
            authorizationPolicyBuilder.Requirements.Add(new RoleRequirement());

            return authorizationPolicyBuilder.Build();
        }

        #endregion

        #region EndpointPolicy

        /// <summary>
        /// 
        /// </summary>
        public const string EndpointPolicyName = "EndpointPolicy";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy EndpointPolicy()
        {
            AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();
            authorizationPolicyBuilder.Requirements.Add(new EndpointRequirement());
            
            return authorizationPolicyBuilder.Build();
        }

        #endregion
    }
}
