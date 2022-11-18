namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial interface IIdentityManagerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMfa"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<ApiMfa> SaveMfaAsync(ApiMfa apiMfa);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfaId"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<ApiMfa> GetMfaEntryByMfaIdAsync(System.Guid mfaId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<System.Collections.Generic.List<ApiMfa>> GetMfaEntriesByUserIdAsync(System.Guid userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfaId"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<bool> DeleteMfaAsync(System.Guid mfaId);
    }
}
