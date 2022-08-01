using Educ8IT.AspNetCore.SimpleApi.Identity;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationIdentityManagerService : IdentityManagerService
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <param name="identityDbContext"></param>
        public AuthenticationIdentityManagerService(
            ILoggerFactory logger,
            IOptions<IdentityManagerOptions> options,
            IdentityDbContext identityDbContext)
            : base(logger, options, identityDbContext)
        {

        }

        #endregion

        #region Overrides

        /// <inheritdoc/>
        public override ClaimsPrincipal GetClaimsPrinciple(ApiUser apiUser, string schemeName, List<Claim> extraClaims = null)
        {
            var __claimsList = new List<Claim>
            {
                new Claim(AuthenticationClaimTypes.UserId, apiUser.Id.ToString()),
                new Claim(AuthenticationClaimTypes.UserName, apiUser.UserName)
            };

            if (extraClaims != null)
                __claimsList.AddRange(extraClaims);

            return base.GetClaimsPrinciple(apiUser, schemeName, __claimsList);
        }

        #endregion
    }
}
