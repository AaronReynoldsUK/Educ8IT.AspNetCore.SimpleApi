using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// MfaSchemes assign a name to a specific <see cref="IMfaHandler"/>
    /// handlerType.
    /// </summary>
    public class MfaScheme
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name for the MFA scheme.</param>
        /// <param name="method"></param>
        /// <param name="displayName">The display name for the MFA scheme.</param>
        /// <param name="handlerType">The <see cref="IMfaHandler"/> type that handles this scheme.</param>
        public MfaScheme(string name, SimpleApi.Identity.EMfaMethod method, string displayName, Type handlerType)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (handlerType == null)
            {
                throw new ArgumentNullException(nameof(handlerType));
            }
            if (!typeof(IMfaHandler).IsAssignableFrom(handlerType))
            {
                throw new ArgumentException("handlerType must implement IMfaHandler.");
            }

            Name = name;
            Method = method;
            HandlerType = handlerType;
            DisplayName = displayName;
        }

        /// <summary>
        /// The name of the MFA scheme.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The display name for the MFA scheme. 
        /// Null is valid and used for non-user facing schemes.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleApi.Identity.EMfaMethod Method { get; }

        /// <summary>
        /// The <see cref="IMfaHandler"/> type that handles this Scheme
        /// </summary>
        public Type HandlerType { get; }
    }
}
