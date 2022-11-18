using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial class IdentityManagerService
    {
        #region For Users

        /// <inheritdoc/>
        public async Task<ApiUser> GetUserByIdAsync(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                return null;

            var __authUser = await _identityDbContext.ApiUsers
                //.Include(a => a.LinkedClaims).ThenInclude(b => b.Claim)
                //.Include(a => a.LinkedRoles).ThenInclude(b => b.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (__authUser == null)
            {
                return null;
            }

            if (__authUser.LinkedClaims != null)
            {
                foreach (var __linkedClaim in __authUser.LinkedClaims)
                {
                    var __claim = __linkedClaim.Claim;
                }
            }
            if (__authUser.LinkedRoles != null)
            {
                foreach (var __linkedClaim in __authUser.LinkedRoles)
                {
                    var __role = __linkedClaim.Role;
                    if (__role.LinkedClaims != null)
                    { }
                }
            }
            return __authUser;
        }

        /// <inheritdoc/>
        public async Task<ApiUser> GetUserByUserNameAsync(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                return null;

            userName = userName.ToLowerInvariant();

            return await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        /// <inheritdoc/>
        public async Task<ApiUser> GetUserByEmailAddressAsync(string emailAddress)
        {
            if (String.IsNullOrEmpty(emailAddress))
                return null;

            emailAddress = emailAddress.ToLowerInvariant();

            return await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
        }

        #endregion
    }
}
