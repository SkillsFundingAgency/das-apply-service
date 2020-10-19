using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Application.UnitTests;
using SFA.DAS.ApplyService.Web.AutoMapper;
using NotRequiredOverride = SFA.DAS.ApplyService.Domain.Entities.NotRequiredOverride;
using NotRequiredCondition = SFA.DAS.ApplyService.Domain.Entities.NotRequiredCondition;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class NotRequiredOverridesServiceTests
    {
        private Guid _applicationId;
        private string _sessionKey;
        private JObject _qnaApplicationData;
        private List<NotRequiredOverride> _notRequiredOverrides;
        private List<Configuration.NotRequiredOverride> _configurationNotRequiredOverrides;

        private Mock<IOptions<List<Configuration.NotRequiredOverride>>> _iOptions;
        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<ISessionService> _sessionService;
        private NotRequiredOverridesService _notRequiredOverridesService;
        

        [SetUp]
        public void Before_each_test()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<NotRequiredOverrideProfile>();
                cfg.AddProfile<NotRequiredConditionProfile>();
            });

            Mapper.AssertConfigurationIsValid();

            _applicationId = Guid.NewGuid();
            _sessionKey = string.Format("NotRequiredConfiguration_{0}", _applicationId);

            _qnaApplicationData = new JObject
            {
                ["Field1"] = "Test"
            };

            _notRequiredOverrides = new List<NotRequiredOverride>
            {
                new NotRequiredOverride
                {
                    Conditions = new List<NotRequiredCondition>
                    {
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "Field1",
                            MustEqual = "Test"
                        }
                    },
                    SectionId = 1,
                    SequenceId = 2
                }
            };

            _configurationNotRequiredOverrides = new List<Configuration.NotRequiredOverride>
            {
                new Configuration.NotRequiredOverride
                {
                    Conditions = new List<Configuration.NotRequiredCondition>
                    {
                        new Configuration.NotRequiredCondition
                        {
                            ConditionalCheckField = "Field1",
                            MustEqual = "Test"
                        }
                    },
                    SectionId = 1,
                    SequenceId = 2
                }
            };

            _iOptions = new Mock<IOptions<List<Configuration.NotRequiredOverride>>>();
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _sessionService = new Mock<ISessionService>();

            _notRequiredOverridesService = new NotRequiredOverridesService(_iOptions.Object, _applicationApiClient.Object, _qnaApiClient.Object, _sessionService.Object);

            _iOptions.Setup(x => x.Value).Returns(_configurationNotRequiredOverrides);

            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(_notRequiredOverrides);
            _applicationApiClient.Setup(x => x.UpdateNotRequiredOverrides(_applicationId, _notRequiredOverrides)).ReturnsAsync(true);

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);
        }

        [Test]
        public async Task RefreshNotRequiredOverrides_clears_cache_and_replaces_with_latest_overrides()
        {
            await _notRequiredOverridesService.RefreshNotRequiredOverrides(_applicationId);

            _sessionService.Verify(x => x.Remove(_sessionKey), Times.Once);
            _sessionService.Verify(x => x.Set(_sessionKey, _notRequiredOverrides), Times.Once);
        }

        [Test]
        public async Task GetNotRequiredOverrides_when_persisted_in_cache_returns_expected_overrides()
        {
            _sessionService.Setup(x => x.Get<List<NotRequiredOverride>>(_sessionKey)).Returns(_notRequiredOverrides);

            var result = await _notRequiredOverridesService.GetNotRequiredOverrides(_applicationId);

            result.Should().BeSameAs(_notRequiredOverrides);

            _sessionService.Verify(x => x.Get<List<NotRequiredOverride>>(_sessionKey), Times.Once);
            _sessionService.Verify(x => x.Remove(_sessionKey), Times.Never);
        }

        [Test]
        public async Task GetNotRequiredOverrides_when_not_cached_refreshs_overrides_and_saves_to_cache()
        {
            _sessionService.Setup(x => x.Get<List<NotRequiredOverride>>(_sessionKey)).ReturnsInOrder(null, _notRequiredOverrides);

            var result = await _notRequiredOverridesService.GetNotRequiredOverrides(_applicationId);

            result.Should().BeSameAs(_notRequiredOverrides);

            _sessionService.Verify(x => x.Get<List<NotRequiredOverride>>(_sessionKey), Times.Exactly(2));
            _sessionService.Verify(x => x.Remove(_sessionKey), Times.Once);
            _sessionService.Verify(x => x.Set(_sessionKey, _notRequiredOverrides), Times.Once);
        }
    }
}
