using InstrumentationSample.Infrastructure;
using Moq;
using NUnit.Framework;

namespace InstrumentationSample.UnitTests
{
    [TestFixture]
    public sealed class AuthorizationServiceTests
    {
        private Mock<ILogger> _loggerMock;
        private AuthorizationService _sut;  //System Under Test.

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger>();
            var logFactoryMock = new Mock<ILogFactory>();
            logFactoryMock.Setup(lf => lf.CreateLogger()).Returns(_loggerMock.Object);
            _sut = new AuthorizationService(logFactoryMock.Object);
        }

        [Test]
        public void When_Authoriztion_Called_Should_Trace_the_method()
        {
            var traceMock = new Mock<ITrace>();
            _loggerMock.Setup(l => l.StartTrace("Authorise")).Returns(traceMock.Object);

            _sut.Authorise(new AuthorizationRequest());

            traceMock.Verify(t=>t.Dispose(), Times.Once());
        }
    }
}
