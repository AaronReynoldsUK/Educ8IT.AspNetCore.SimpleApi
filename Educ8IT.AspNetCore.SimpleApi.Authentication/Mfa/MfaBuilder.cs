using System;
using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public MfaBuilder(IServiceCollection services) => Services = services;

        // <summary>
        /// The services being configured.
        /// </summary>
        public virtual IServiceCollection Services { get; }

        private MfaBuilder AddSchemeHelper<TOptions, THandler>(
            string mfaScheme, string displayName, EMfaMethod method, Action<TOptions> configureOptions)
            where TOptions: MfaSchemeOptions, new()
            where THandler: class, IMfaHandler
        {
            Services.Configure<MfaOptions>(o =>
            {
                o.AddScheme(mfaScheme, scheme =>
                {
                    scheme.HandlerType = typeof(THandler);
                    scheme.DisplayName = displayName;
                    scheme.Method = method;
                });
            });

            if (configureOptions != null)
            {
                Services.Configure(mfaScheme, configureOptions);
            }

            Services.AddOptions<TOptions>(mfaScheme).Validate(o =>
            {
                o.Validate(mfaScheme);
                return true;
            });
            Services.AddTransient<THandler>();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="mfaScheme"></param>
        /// <param name="displayName"></param>
        /// <param name="method"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public virtual MfaBuilder AddScheme<TOptions, THandler>(
            string mfaScheme, string displayName, EMfaMethod method, Action<TOptions> configureOptions)
            where TOptions : MfaSchemeOptions, new()
            where THandler : class, IMfaHandler
            => AddSchemeHelper<TOptions, THandler>(mfaScheme, displayName, method, configureOptions);


    }
}
