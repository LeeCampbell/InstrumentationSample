using InstrumentationSample.Infrastructure;
using Moq;
using NUnit.Framework;

namespace InstrumentationSample.UnitTests
{
    [TestFixture]
    public sealed class InstrumentedAuthorizationServiceTests
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IAuthorizationService> _underlyingMock;
        private InstrumentedAuthorizationService _sut;  //System Under Test.

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger>();
            var logFactoryMock = new Mock<ILogFactory>();
            logFactoryMock.Setup(lf => lf.CreateLogger()).Returns(_loggerMock.Object);
            _underlyingMock = new Mock<IAuthorizationService>();
            _sut = new InstrumentedAuthorizationService(_underlyingMock.Object, logFactoryMock.Object);
        }

        [Test]
        public void When_Authoriztion_Called_Should_Trace_the_method()
        {
            var traceMock = new Mock<ITrace>();
            _loggerMock.Setup(l => l.StartTrace("Authorise")).Returns(traceMock.Object);

            _sut.Authorise(new AuthorizationRequest());

            traceMock.Verify(t=>t.Dispose(), Times.Once());
        }

        [Test]
        public void When_Authoriztion_Called_Should_call_underlying_Authorize_with_parameters()
        {
            var traceMock = new Mock<ITrace>();
            _loggerMock.Setup(l => l.StartTrace("Authorise")).Returns(traceMock.Object);

            var request = new AuthorizationRequest();
            _sut.Authorise(request);

            _underlyingMock.Verify(auth=>auth.Authorise(request), Times.Once());
        }
    }
}
