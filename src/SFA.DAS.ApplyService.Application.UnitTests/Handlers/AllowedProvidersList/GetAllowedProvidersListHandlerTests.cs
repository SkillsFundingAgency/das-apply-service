using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AllowedProvidersList
{
    [TestFixture]
    public class GetAllowedProvidersListHandlerTests
    {
        private const string SORT_COLUMN = null;
        private const string SORT_ORDER = null;

        private List<AllowedProvider> _allowedProviderList;

        protected Mock<IAllowedProvidersRepository> _repository;
        protected GetAllowedProvidersListHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _allowedProviderList = new List<AllowedProvider>
            {
                new AllowedProvider
                {
                    Ukprn = 12345678,
                    StartDateTime = DateTime.MinValue,
                    EndDateTime = DateTime.MaxValue,
                    AddedDateTime = DateTime.Today
                }
            };

            _repository = new Mock<IAllowedProvidersRepository>();

            _repository.Setup(x => x.GetAllowedProvidersList(SORT_COLUMN, SORT_ORDER)).ReturnsAsync(_allowedProviderList);

            _handler = new GetAllowedProvidersListHandler(_repository.Object);
        }

        [Test]
        public async Task GetAllowedProvidersListHandler_returns_expected_list_of_allowed_providers()
        {
            var result = await _handler.Handle(new GetAllowedProvidersListRequest(SORT_COLUMN, SORT_ORDER), new CancellationToken());

            CollectionAssert.AreEquivalent(_allowedProviderList, result);
        }
    }
}
