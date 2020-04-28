using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.NotRequiredOverrideHandlerTests
{
    [TestFixture]
    public class UpdateNotRequiredOverridesHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private Mock<ILogger<UpdateNotRequiredOverridesHandler>> _logger;

        private UpdateNotRequiredOverridesHandler _handler;

        private Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _repository = new Mock<IApplyRepository>();
            _logger = new Mock<ILogger<UpdateNotRequiredOverridesHandler>>();

            _handler = new UpdateNotRequiredOverridesHandler(_repository.Object, _logger.Object);
        }
        
        [Test]
        public void Handler_returns_config_if_persisted()
        {
            var config = new NotRequiredOverrideConfiguration
            {
                NotRequiredOverrides = new List<NotRequiredOverride> 
                {
                    new NotRequiredOverride
                    {
                        Conditions = new List<NotRequiredCondition>
                        {
                            new NotRequiredCondition
                            {
                                ConditionalCheckField = "FIELD1",
                                MustEqual = "Y",
                                Value = ""
                            }
                        },
                        SectionId = 1,
                        SequenceId = 4
                    }
                }
            };

            _repository.Setup(x => x.SaveNotRequiredOverrides(_applicationId, config)).ReturnsAsync(true);

            var result = _handler.Handle(new UpdateNotRequiredOverridesRequest(_applicationId, config), new System.Threading.CancellationToken()).GetAwaiter().GetResult();

            result.Should().BeTrue();
        }
    }
}
