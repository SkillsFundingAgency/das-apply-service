using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AssessorReviewHandlerTests
{
    [TestFixture]
    public class AssessorEvaluateSectionHandlerTests
    {
        private Mock<ILogger<AssessorEvaluateSectionHandler>> _logger;
        private Mock<IApplyRepository> _repository;
        private AssessorEvaluateSectionHandler _handler;
        private const int SequenceId = 2;
        private const int SectionId = 1;
        private const string Reviewer = "Review User";

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<AssessorEvaluateSectionHandler>>();
            _repository = new Mock<IApplyRepository>();
            _handler = new AssessorEvaluateSectionHandler(_repository.Object, _logger.Object);
        }

        [Test]
        public void Review_section_returns_false_if_no_apply_data_present()
        {
            ApplyData applyData = null;
            _repository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);

            var request = new AssessorEvaluateSectionRequest(Guid.NewGuid(), SequenceId, SectionId, true, Reviewer);
            
            var result = _handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            result.Should().BeFalse();
        }

        [Test]
        public void Review_section_returns_false_if_sequence_not_found()
        {
            var applyData = new ApplyData
            {
                Sequences = new List<ApplySequence>
                {
                    new ApplySequence
                    {
                        SequenceNo = 1
                    }
                }
            };

            _repository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);

            var request = new AssessorEvaluateSectionRequest(Guid.NewGuid(), SequenceId, SectionId, true, Reviewer);

            var result = _handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            result.Should().BeFalse();
        }

        [Test]
        public void Review_section_returns_false_if_section_not_found_in_sequence()
        {
            var applyData = new ApplyData
            {
                Sequences = new List<ApplySequence>
                {
                    new ApplySequence
                    {
                        SequenceNo = 1
                    },
                    new ApplySequence
                    {
                        SequenceNo = 2,
                        Sections = new List<ApplySection>
                        {
                            new ApplySection
                            {
                                SectionNo = 0
                            },
                            new ApplySection
                            {
                                SectionNo = 2
                            }
                        }
                    }
                }
            };

            _repository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);

            var request = new AssessorEvaluateSectionRequest(Guid.NewGuid(), SequenceId, SectionId, true, Reviewer);

            var result = _handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            result.Should().BeFalse();
        }

        [TestCase(AssessorReviewStatus.Approved)]
        [TestCase(AssessorReviewStatus.Declined)]
        public void Review_section_returns_false_if_section_already_reviewed(string sectionStatus)
        {
            var applyData = new ApplyData
            {
                Sequences = new List<ApplySequence>
                {
                    new ApplySequence
                    {
                        SequenceNo = 2,
                        Sections = new List<ApplySection>
                        {
                            new ApplySection
                            {
                                SectionNo = 1,
                                Status = sectionStatus
                            }
                        }
                    }
                }
            };

            _repository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);

            var request = new AssessorEvaluateSectionRequest(Guid.NewGuid(), SequenceId, SectionId, true, Reviewer);

            var result = _handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            result.Should().BeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Review_section_returns_true_when_section_review_initiated(bool sectionCompleted)
        {
            var applyData = new ApplyData
            {
                Sequences = new List<ApplySequence>
                {
                    new ApplySequence
                    {
                        SequenceNo = 2,
                        Sections = new List<ApplySection>
                        {
                            new ApplySection
                            {
                                SectionNo = 1,
                                Status = AssessorReviewStatus.InProgress
                            }
                        }
                    }
                }
            };

            _repository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);

            _repository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            var request = new AssessorEvaluateSectionRequest(Guid.NewGuid(), SequenceId, SectionId, sectionCompleted, Reviewer);

            var result = _handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            result.Should().BeTrue();
        }
    }
}
