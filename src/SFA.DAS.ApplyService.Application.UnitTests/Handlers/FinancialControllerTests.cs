using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.UnitTests.Controllers
{[TestFixture]
    public  class FinancialControllerFinancialStatusCountsTests
    {
        private Mock<IMediator> _mediator;


        [SetUp]
        public void Before_each_test()
        {
           _mediator = new Mock<IMediator>();
        }

        [Test]
        public void GetStatusCounts_GetExpectedCounts()
        {
            _mediator.Setup()
        }

    }
}
