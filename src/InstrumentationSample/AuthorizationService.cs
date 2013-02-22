using System;
using System.Threading;
using InstrumentationSample.Infrastructure;

namespace InstrumentationSample
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly ILogger _logger;

        public AuthorizationService(ILogFactory logFactory)
        {
            _logger = logFactory.CreateLogger();
        }

        public void Authorise(AuthorizationRequest request)
        {
            using (_logger.StartTrace("Authorise"))
            {
                //Do some auth.  
 
                //This is a bit crap for Unit tests, however, this is just a sample.
                Console.Write("Authorising...");
                Thread.SpinWait(new Random().Next(10000, 100000));
                Console.WriteLine("authorised.");
            }
        }
    }
}