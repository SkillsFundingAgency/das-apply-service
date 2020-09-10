using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.NotRequiredOverrideHandlerTests
{
    [TestFixture]
    public class GetNotRequiredOverridesHandlerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<IApplyRepository> _repository;

        private GetNotRequiredOverridesHandler _handler;

        [SetUp]
        public void Before_each_test()
        {
            _repository = new Mock<IApplyRepository>();

            _handler = new GetNotRequiredOverridesHandler(_repository.Object);
        }

        [Test]
        public async Task Handler_returns_null_if_NotRequiredOverrides_not_persisted()
        {
            List<NotRequiredOverride> notRequiredOverrides = null;

            _repository.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(notRequiredOverrides);

            var result = await _handler.Handle(new GetNotRequiredOverridesRequest(_applicationId), new CancellationToken());

            _repository.Verify(x => x.GetNotRequiredOverrides(_applicationId), Times.Once);
            result.Should().BeNull();
        }

        [Test]
        public async Task Handler_returns_NotRequiredOverrides_if_persisted()
        {
            var notRequiredOverrides = new List<NotRequiredOverride>
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
                };

            _repository.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(notRequiredOverrides);

            var result = await _handler.Handle(new GetNotRequiredOverridesRequest(_applicationId), new CancellationToken());

            _repository.Verify(x => x.GetNotRequiredOverrides(_applicationId), Times.Once);
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
        }
    }
}
