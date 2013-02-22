using InstrumentationSample.Infrastructure;

namespace InstrumentationSample
{
    public sealed class InstrumentedAuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationService _underlyingAuthService;
        private readonly ILogger _logger;

        public InstrumentedAuthorizationService(IAuthorizationService underlyingAuthService, ILogFactory logFactory)
        {
            _underlyingAuthService = underlyingAuthService;
            _logger = logFactory.CreateLogger();
        }

        public void Authorise(AuthorizationRequest request)
        {
            using (_logger.StartTrace("Authorise"))
            {
                _underlyingAuthService.Authorise(request);
            }
        }
    }
}