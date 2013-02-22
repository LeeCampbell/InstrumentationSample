using System;
using InstrumentationSample.Infrastructure;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace InstrumentationSample.UnitTests
{
    //Grouping tests by use case I find makes it easier to name and navigate the tests. 

    public abstract class Given_a_constructed_AggregatingLogger
    {
        private Mock<ILogger> _underlyingLoggerMock;
        private AggregatingLogger _sut;
        private Mock<ISchedulerProvider> _schedulerProviderMock;
        private TestScheduler _eventLoopScheduler;

        private Given_a_constructed_AggregatingLogger()
        { }


        [SetUp]
        public virtual void SetUp()
        {
            _underlyingLoggerMock = new Mock<ILogger>();
            _schedulerProviderMock = new Mock<ISchedulerProvider>();
            _eventLoopScheduler = new TestScheduler();
            _schedulerProviderMock.Setup(sp => sp.CreateEventLoopScheduler("AggregatingLogger", true))
                                  .Returns(_eventLoopScheduler);
            _sut = new AggregatingLogger(_underlyingLoggerMock.Object, _schedulerProviderMock.Object);
        }

        [TestFixture]
        public sealed class When_Write_is_called : Given_a_constructed_AggregatingLogger
        {
            [TestCase(LogLevel.Debug)]
            [TestCase(LogLevel.Error)]
            [TestCase(LogLevel.Fatal)]
            [TestCase(LogLevel.Info)]
            [TestCase(LogLevel.Trace)]
            [TestCase(LogLevel.Verbose)]
            [TestCase(LogLevel.Warn)]
            public void Should_delegate_to_underlying_logger(LogLevel level)
            {
                const string expectedMessage = "Some message";
                var expectedException = new Exception("Some exception");
                _sut.Write(level, expectedMessage, expectedException);

                _underlyingLoggerMock.Verify(l => l.Write(level, expectedMessage, expectedException), Times.Once());
            }
        }

        [TestFixture]
        public sealed class When_performing_a_trace : Given_a_constructed_AggregatingLogger
        {
            const string traceName = "traceName";
            private const LogLevel expectedInstrumentationLevel = LogLevel.Debug;

            [Test]
            public void Should_log_to_underlying_on_completion()
            {
                var trace = _sut.StartTrace(traceName);
                trace.Dispose();
                var expectedMessage = string.Format("Call Count :'1'. Mean average execution time: '{0}'", trace.Elapsed);

                //This will drain all the actions that have scheduled to be performed on our virtual thread.
                _eventLoopScheduler.Start();

                _underlyingLoggerMock.Verify(l => l.Write(expectedInstrumentationLevel, expectedMessage, null), Times.Once());
            }

            [Test]
            public void Should_log_aggergate_mean_elapsed_time_to_underlying()
            {
                var trace1 = _sut.StartTrace(traceName);
                trace1.Dispose();
                var expectedMessage1 = string.Format("Call Count :'1'. Mean average execution time: '{0}'", trace1.Elapsed);

                var trace2 = _sut.StartTrace(traceName);
                trace2.Dispose();
                var meanAverageElapsedTicks = (trace1.Elapsed.Ticks + trace2.Elapsed.Ticks) / 2;
                var meanAverageElapsedTime = TimeSpan.FromTicks(meanAverageElapsedTicks);
                var expectedMessage2 = string.Format("Call Count :'2'. Mean average execution time: '{0}'", meanAverageElapsedTime);

                //This will drain all the actions that have scheduled to be performed on our virtual thread.
                _eventLoopScheduler.Start();

                _underlyingLoggerMock.Verify(l => l.Write(expectedInstrumentationLevel, expectedMessage1, null), Times.Once());
                _underlyingLoggerMock.Verify(l => l.Write(expectedInstrumentationLevel, expectedMessage2, null), Times.Once());
            }

            [Test]//Note here we are performing a trace on two different trace names, hence they should have separate running averages.
            public void Should_aggergate_independently_on_trace_name()
            {
                var trace1 = _sut.StartTrace(traceName);
                trace1.Dispose();
                var expectedMessage1 = string.Format("Call Count :'1'. Mean average execution time: '{0}'", trace1.Elapsed);

                var trace2 = _sut.StartTrace("OtherTraceName");
                trace2.Dispose();
                var expectedMessage2 = string.Format("Call Count :'1'. Mean average execution time: '{0}'", trace2.Elapsed);

                //This will drain all the actions that have scheduled to be performed on our virtual thread.
                _eventLoopScheduler.Start();

                _underlyingLoggerMock.Verify(l => l.Write(expectedInstrumentationLevel, expectedMessage1, null), Times.Once());
                _underlyingLoggerMock.Verify(l => l.Write(expectedInstrumentationLevel, expectedMessage2, null), Times.Once());
            }
        }
    }
}
// ReSharper restore InconsistentNaming