using System;
using System.Diagnostics;
using InstrumentationSample.Infrastructure;

namespace InstrumentationSample
{

    //Written by Lee Campbell
    // Sample program to show Logging and instrumentation of a fictional "AuthorizationService".
    // Goal is to be non-blocking but still thread safe. 
    // Aggregating of instrumentation is abstracted away from the consumer of the Instrumentation/Logging API.
    class Program
    {
        static void Main(string[] args)
        {
            //Calls an Authorise method which logs the average time calls take to run.
            var logFactory = new FakeLogFactory(new SchedulerProvider());
            var auth = new InstrumentedAuthorizationService( new AuthorizationService(), logFactory);
            for (int i = 0; i < 20; i++)
            {
                auth.Authorise(new AuthorizationRequest());
            }
            Console.ReadLine();
        }
    }

    public class FakeLogFactory : ILogFactory
    {
        private readonly ISchedulerProvider _schedulerProvider;

        public FakeLogFactory(ISchedulerProvider schedulerProvider)
        {
            _schedulerProvider = schedulerProvider;
        }

        public ILogger CreateLogger()
        {
            var callersStackFrame = new StackFrame(1);
            var callerMethod = callersStackFrame.GetMethod();
            var callingType = callerMethod.ReflectedType;
            return new AggregatingLogger(new ConsoleLogger(callingType), _schedulerProvider);
        }
    }
}
