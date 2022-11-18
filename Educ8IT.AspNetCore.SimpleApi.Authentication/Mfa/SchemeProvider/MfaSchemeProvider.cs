using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaSchemeProvider : IMfaSchemeProvider
    {
        #region Fields

        private readonly MfaOptions _options;

        private readonly object _lock = new object();

        private readonly IDictionary<string, MfaScheme> _schemes;

        private readonly List<MfaScheme> _requestHandlers;

        private IEnumerable<MfaScheme> _schemesCopy = Array.Empty<MfaScheme>();

        private IEnumerable<MfaScheme> _requestHandlersCopy = Array.Empty<MfaScheme>();

        #endregion

        #region Properties

        #endregion

        #region IMfaSchemeProvider

        /// <inheritdoc/>
        public virtual Task<IEnumerable<MfaScheme>> GetAllSchemesAsync()
            => Task.FromResult(_schemesCopy);

        /// <inheritdoc/>
        public virtual Task<MfaScheme> GetSchemeForMethodAsync(SimpleApi.Identity.EMfaMethod method)
            => Task.FromResult(_schemesCopy.FirstOrDefault(s => s.Method == method));

        /// <inheritdoc/>
        public virtual Task<MfaScheme> GetSchemeAsync(string name)
            => Task.FromResult(_schemes.ContainsKey(name) ? _schemes[name] : null);
        
        /// <inheritdoc/>
        public Task<MfaScheme> GetDefaultSchemeAsync()
            => _options.DefaultScheme != null
            ? GetSchemeAsync(_options.DefaultScheme)
            : Task.FromResult<MfaScheme>(null);

        /// <inheritdoc/>
        public virtual void AddScheme(MfaScheme scheme)
        {
            if (_schemes.ContainsKey(scheme.Name))
            {
                throw new InvalidOperationException("Scheme already exists: " + scheme.Name);
            }
            lock (_lock)
            {
                if (_schemes.ContainsKey(scheme.Name))
                {
                    throw new InvalidOperationException("Scheme already exists: " + scheme.Name);
                }
                if (typeof(IMfaHandler).IsAssignableFrom(scheme.HandlerType))
                {
                    _requestHandlers.Add(scheme);
                    _requestHandlersCopy = _requestHandlers.ToArray();
                }
                _schemes[scheme.Name] = scheme;
                _schemesCopy = _schemes.Values.ToArray();
            }
        }

        /// <inheritdoc/>
        public virtual void RemoveScheme(string name)
        {
            if (!_schemes.ContainsKey(name))
            {
                return;
            }
            lock (_lock)
            {
                if (_schemes.ContainsKey(name))
                {
                    var scheme = _schemes[name];
                    if (_requestHandlers.Remove(scheme))
                    {
                        _requestHandlersCopy = _requestHandlers.ToArray();
                    }
                    _schemes.Remove(name);
                    _schemesCopy = _schemes.Values.ToArray();
                }
            }
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<MfaScheme>> GetRequestHandlerSchemesAsync()
            => Task.FromResult(_requestHandlersCopy);

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public MfaSchemeProvider(IOptions<MfaOptions> options)
            : this(options, new Dictionary<string, MfaScheme>(StringComparer.Ordinal))
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="schemes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MfaSchemeProvider(IOptions<MfaOptions> options, IDictionary<string, MfaScheme> schemes)
        {
            _options = options.Value;

            _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _requestHandlers = new List<MfaScheme>();

            foreach (var builder in _options.Schemes)
            {
                var scheme = builder.Build();
                AddScheme(scheme);
            }
        }

        #endregion
    }
}
