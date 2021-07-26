using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitApplicationHandlerTests
{
    public class When_submitting_application : SubmitApplicationHandlerTestsBase
    {
        [Test]
        public async Task Then_Handler_Returns_False_If_Not_Allowed_To_Submit()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<Guid>())).ReturnsAsync(false);

            var request = new SubmitApplicationRequest { ApplicationId = Guid.NewGuid(), SubmittingContactId = Guid.NewGuid() };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsFalse(result);
            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<FinancialData>(), It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task Then_Handler_Returns_True_If_Allowed_To_Submit()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<Guid>())).ReturnsAsync(true);

            var request = new SubmitApplicationRequest
            {
                ApplicationId = Guid.NewGuid(),
                SubmittingContactId = Guid.NewGuid(),
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>
                    {
                        new ApplySequence
                        {
                            SequenceNo = 1,
                            Sections = new List<ApplySection>
                            {
                                new ApplySection
                                {
                                    SectionNo = 1
                                }
                            }
                        }
                    }
                }
            };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsTrue(result);
            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<FinancialData>(), It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task Then_FinancialData_Is_Persisted()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<Guid>())).ReturnsAsync(true);

            var request = new SubmitApplicationRequest
            {
                ApplicationId = Guid.NewGuid(),
                SubmittingContactId = Guid.NewGuid(),
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>
                    {
                        new ApplySequence
                        {
                            SequenceNo = 1,
                            Sections = new List<ApplySection>
                            {
                                new ApplySection
                                {
                                    SectionNo = 1
                                }
                            }
                        }
                    }
                },
                FinancialData = new FinancialData
                {
                    TurnOver = 1,
                    Depreciation = 2,
                    ProfitLoss = 3,
                    Dividends = 4,
                    IntangibleAssets = 5,
                    Assets = 6,
                    Liabilities = 7,
                    ShareholderFunds = 8,
                    Borrowings = 9,
                    AccountingReferenceDate = new DateTime(2021, 1, 1),
                    AccountingPeriod = 10,
                    AverageNumberofFTEEmployees = 11
                }
            };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsTrue(result);
            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.Is<FinancialData>(
                x => x.TurnOver == 1
                     && x.Depreciation == 2
                     && x.ProfitLoss == 3
                     && x.Dividends == 4
                     && x.IntangibleAssets == 5
                     && x.Assets == 6
                     && x.Liabilities == 7
                     && x.ShareholderFunds == 8
                     && x.Borrowings == 9
                     && x.AccountingReferenceDate == new DateTime(2021, 1, 1)
                     && x.AccountingPeriod == 10
                     && x.AverageNumberofFTEEmployees == 11
                ), It.IsAny<Guid>()), Times.Once);
        }
    }
}
