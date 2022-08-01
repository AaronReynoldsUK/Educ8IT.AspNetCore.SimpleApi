using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public static class IdentityDbInit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            var serviceScopeFactory = (IServiceScopeFactory)host
                .Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<T>();
                dbContext.Database.Migrate();

                if (dbContext is IdentityDbContext identityDbContext)
                    InitialiseIdentityDb(identityDbContext);
            }

            return host;
        }

        /// <summary>
        /// Delegate for a local Initialiser for the IdentityDb
        /// </summary>
        /// <param name="identityDbContext"></param>
        public delegate void DbInitialiserHandler(IdentityDbContext identityDbContext);

        private static DbInitialiserHandler _initialiser = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="initialiser"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder Initialiser(
            this DbContextOptionsBuilder builder,
            DbInitialiserHandler initialiser)
        {
            if (initialiser != null)
                _initialiser = initialiser;

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityDbContext"></param>
        public static void InitialiseIdentityDb(IdentityDbContext identityDbContext)
        {
            if (_initialiser != null)
                _initialiser.Invoke(identityDbContext);
        }
    }
}
