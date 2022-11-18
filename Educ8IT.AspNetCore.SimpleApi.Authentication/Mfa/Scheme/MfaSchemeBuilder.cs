using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// Used to build <see cref="MfaScheme"/>s.
    /// </summary>
    public class MfaSchemeBuilder
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the scheme being built.</param>
        public MfaSchemeBuilder(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the scheme being built.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The display name for the scheme being built.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleApi.Identity.EMfaMethod Method { get; set; }

        /// <summary>
        /// The <see cref="IMfaHandler"/> type responsible for this scheme.
        /// </summary>
        public Type HandlerType { get; set; }

        /// <summary>
        /// Builds the <see cref="MfaScheme"/> instance.
        /// </summary>
        /// <returns></returns>
        public MfaScheme Build() => new MfaScheme(Name, Method, DisplayName, HandlerType);
    }
}
