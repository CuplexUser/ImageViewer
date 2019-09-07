using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;
using NSubstitute.Extensions;
using NSubstitute.ReceivedExtensions;
using Serilog;
using Serilog.Core;

namespace ImageViewer.UnitTests.Configuration
{
    public class MockLogger
    {
        private readonly MockRepository _mockRepository;
        private bool _isActive = false;
        // Returned instance after config
        private readonly Mock<Logger> _logger;
        private readonly Mock<LoggerConfiguration> _loggerConfigMock;

        public MockLogger()
        {
 
            _mockRepository = new MockRepository(MockBehavior.Loose);
            _logger = _mockRepository.Create<Logger>(MockBehavior.Loose);
            _loggerConfigMock = _mockRepository.Create<LoggerConfiguration>();
        }

        public DefaultValue DefaultValue
        {
            get => _mockRepository.DefaultValue;
            set => _mockRepository.DefaultValue = value;
        }

        public DefaultValueProvider DefaultValueProvider
        {
            get => _mockRepository.DefaultValueProvider;
            set => _mockRepository.DefaultValueProvider = value;
        }

        public void VerifyAll()
        {
            _loggerConfigMock.VerifyAll();
        }

        public void SetReturnsDefault<TReturn>(TReturn value)
        {
            _loggerConfigMock.SetReturnsDefault(value);
        }

        public IInvocationList Invocations => _loggerConfigMock.Invocations;

        public ISetup<LoggerConfiguration, TResult> Setup<TResult>(Expression<Func<LoggerConfiguration, TResult>> expression)
        {
            return _loggerConfigMock.Setup(expression);
        }

        public Mock<LoggerConfiguration> SetupAllProperties()
        {
            return _loggerConfigMock.SetupAllProperties();
        }

        public ISetupConditionResult<LoggerConfiguration> When(Func<bool> condition)
        {
            return _loggerConfigMock.When(condition);
        }

        public void VerifySet(Action<LoggerConfiguration> setterExpression)
        {
            _loggerConfigMock.VerifySet(setterExpression);
        }

        public ISetup<LoggerConfiguration, TResult> Expect<TResult>(Expression<Func<LoggerConfiguration, TResult>> expression)
        {
            return _loggerConfigMock.Setup(expression);
        }

        public ISetupGetter<LoggerConfiguration, TProperty> ExpectGet<TProperty>(Expression<Func<LoggerConfiguration, TProperty>> expression)
        {
            return _loggerConfigMock.SetupGet(expression);
        }

        public MockRepository MockRepository => _mockRepository;

        public bool IsActive => _isActive;

        public Mock<Logger> Logger => _logger;

        public Mock<LoggerConfiguration> LoggerConfigMock => _loggerConfigMock;

        public LoggerConfiguration Object => _loggerConfigMock.Object;



        public void ConfigureMockLoggerBehavior()
        {
            _loggerConfigMock.Object.CreateLogger().Returns<Logger>(GetMockedLogger()).AndDoes(LoggerCreated);
        }

        private void LoggerCreated(CallInfo obj)
        {
            _isActive = true;
            obj.Received();
        }


        public  LoggerConfiguration GetMockedLoggerConfiguration()
        {
            return _loggerConfigMock.Object;
        }

        public Logger GetMockedLogger()
        {
            return _logger.Object;
        }


    }
}
