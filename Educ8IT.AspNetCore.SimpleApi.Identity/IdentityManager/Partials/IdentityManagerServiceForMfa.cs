using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial class IdentityManagerService
    {
        #region For ApiMfa

        /// <inheritdoc/>
        public async Task<ApiMfa> SaveMfaAsync(ApiMfa apiMfa)
        {
            if (apiMfa == null)
                return null;

            if (apiMfa.Id == null || apiMfa.Id == Guid.Empty)
                apiMfa.Id = Guid.NewGuid();

            var predicate = PredicateBuilder.New<ApiMfa>(true);

            predicate = predicate.And(p => p.Id == apiMfa.Id);

            var __dbVersion = await _identityDbContext.ApiMfas
                .AsExpandable()
                .Where(predicate)
                .FirstOrDefaultAsync();

            if (__dbVersion ==  null)
            {
                await _identityDbContext.ApiMfas.AddAsync(apiMfa);
            }
            else
            {
                _identityDbContext.Entry(__dbVersion).CurrentValues.SetValues(apiMfa);

                //__dbVersion.UpdateFrom(apiMfa);
                //_identityDbContext.Update(apiMfa);
            }
            await _identityDbContext.SaveChangesAsync();

            return apiMfa;
        }

        /// <inheritdoc/>
        public async Task<ApiMfa> GetMfaEntryByMfaIdAsync(Guid mfaId)
        {
            if (mfaId == null || mfaId == Guid.Empty)
                return null;

            var predicate = PredicateBuilder.New<ApiMfa>(true);

            predicate = predicate.And(p => p.Id == mfaId);

            return await _identityDbContext.ApiMfas
                .AsExpandable()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ApiMfa>> GetMfaEntriesByUserIdAsync(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                return null;

            var predicate = PredicateBuilder.New<ApiMfa>(true);

            predicate = predicate.And(p => p.UserId == userId);

            return await _identityDbContext.ApiMfas
                .AsExpandable()
                .Where(predicate)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteMfaAsync(Guid mfaId)
        {
            if (mfaId == null || mfaId == Guid.Empty)
                return false;

            _identityDbContext.ApiMfas
                .Remove(new ApiMfa() { Id = mfaId });
            
            var __affectedRecords = await _identityDbContext.SaveChangesAsync();

            return __affectedRecords != -1;
        }

        #endregion
    }
}
