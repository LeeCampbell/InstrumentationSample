namespace InstrumentationSample
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        public void Authorise(AuthorizationRequest request)
        {
            //Do some auth.  Potentially complex logic happens here
        }
    }
}