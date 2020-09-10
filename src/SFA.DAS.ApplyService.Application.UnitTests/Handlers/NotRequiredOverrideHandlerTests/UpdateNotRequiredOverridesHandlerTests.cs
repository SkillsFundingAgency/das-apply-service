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
    public class UpdateNotRequiredOverridesHandlerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<IApplyRepository> _repository;

        private UpdateNotRequiredOverridesHandler _handler;

        [SetUp]
        public void Before_each_test()
        {
            _repository = new Mock<IApplyRepository>();

            _handler = new UpdateNotRequiredOverridesHandler(_repository.Object);
        }

        [Test]
        public async Task Handler_returns_true_when_NotRequiredOverrides_are_persisted()
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

            _repository.Setup(x => x.UpdateNotRequiredOverrides(_applicationId, notRequiredOverrides)).ReturnsAsync(true);

            var result = await _handler.Handle(new UpdateNotRequiredOverridesRequest(_applicationId, notRequiredOverrides), new CancellationToken());

            _repository.Verify(x => x.UpdateNotRequiredOverrides(_applicationId, notRequiredOverrides), Times.Once);
            result.Should().BeTrue();
        }
    }
}
