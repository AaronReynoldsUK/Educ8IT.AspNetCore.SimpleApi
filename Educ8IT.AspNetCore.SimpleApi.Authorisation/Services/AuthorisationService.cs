// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationService : IAuthorisationService
    {
        private readonly IAuthorizationEvaluator _evaluator;
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly IAuthorizationHandlerProvider _handlers;
        private readonly IAuthorizationHandlerContextFactory _authorizationHandlerContextFactory;
        private readonly ILogger<AuthorisationService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="policyProvider"></param>
        /// <param name="handlers"></param>
        /// <param name="authorizationHandlerContextFactory"></param>
        /// <param name="logger"></param>
        public AuthorisationService(
            IAuthorizationEvaluator evaluator,
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            IAuthorizationHandlerContextFactory authorizationHandlerContextFactory,
            ILogger<AuthorisationService> logger)
        {
            _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            _policyProvider = policyProvider ?? throw new ArgumentNullException(nameof(policyProvider));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _authorizationHandlerContextFactory = authorizationHandlerContextFactory ?? throw new ArgumentNullException(nameof(authorizationHandlerContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public async Task<AuthorizationResult> AuthorizeAsync(
            ClaimsPrincipal user, 
            object resource, 
            IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (requirements == null)
            {
                throw new ArgumentNullException(nameof(requirements));
            }

            var authContext = _authorizationHandlerContextFactory.CreateContext(requirements, user, resource);

            var handlers = await _handlers.GetHandlersAsync(authContext);

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(authContext);
            }

            var result = _evaluator.Evaluate(authContext);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public async Task<AuthorizationResult> AuthorizeAsync(
            ClaimsPrincipal user, 
            object resource, 
            string policyName)
        {
            if (policyName == null)
            {
                throw new ArgumentNullException(nameof(policyName));
            }

            var policy = await _policyProvider.GetPolicyAsync(policyName);
            if (policy == null)
            {
                throw new InvalidOperationException($"No policy found: {policyName}.");
            }

            return await this.AuthorizeAsync(user, resource, policy);
        }
    }
}
