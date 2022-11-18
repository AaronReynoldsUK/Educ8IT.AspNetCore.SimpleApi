using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SettingsKey = "MfaOptions";

        private readonly IList<MfaSchemeBuilder> _schemes = new List<MfaSchemeBuilder>();

        /// <summary>
        /// Returns the schemes in the order they were added (important for request handling priority)
        /// </summary>
        public IEnumerable<MfaSchemeBuilder> Schemes => _schemes;

        /// <summary>
        /// Maps schemes by name.
        /// </summary>
        public IDictionary<string, MfaSchemeBuilder> SchemeMap { get; } = new Dictionary<string, MfaSchemeBuilder>(StringComparer.Ordinal);

        /// <summary>
        /// Adds an <see cref="MfaScheme"/>.
        /// </summary>
        /// <param name="name">The name of the scheme being added.</param>
        /// <param name="configureBuilder">Configures the scheme.</param>
        public void AddScheme(string name, Action<MfaSchemeBuilder> configureBuilder)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (configureBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureBuilder));
            }
            if (SchemeMap.ContainsKey(name))
            {
                throw new InvalidOperationException("Scheme already exists: " + name);
            }

            var builder = new MfaSchemeBuilder(name);
            configureBuilder(builder);
            _schemes.Add(builder);
            SchemeMap[name] = builder;
        }

        /// <summary>
        /// Adds an <see cref="MfaScheme"/>.
        /// </summary>
        /// <typeparam name="THandler">The <see cref="IMfaHandler"/> responsible for the scheme.</typeparam>
        /// <param name="name">The name of the scheme being added.</param>
        /// <param name="method"></param>
        /// <param name="displayName">The display name for the scheme.</param>
        public void AddScheme<THandler>(string name, SimpleApi.Identity.EMfaMethod method, string displayName) where THandler : IMfaHandler
            => AddScheme(name, b =>
            {
                b.DisplayName = displayName;
                b.Method = method;
                b.HandlerType = typeof(THandler);
            });

        /// <summary>
        /// Used as the default scheme if none selected by user.
        /// </summary>
        public string DefaultScheme { get; set; }
    }
}
