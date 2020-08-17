using Microsoft.Extensions.Options;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Application.UnitTests;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class NotRequiredOverridesServiceTests
    {
        private Mock<IOptions<List<NotRequiredOverrideConfiguration>>> _config;
        private Mock<IQnaApiClient> _apiClient;
        private Mock<ISessionService> _sessionService;
        private NotRequiredOverridesService _service;
        private Guid _applicationId;
        private string _sessionKey;

        [SetUp]
        public void Before_each_test()
        {
            _config = new Mock<IOptions<List<NotRequiredOverrideConfiguration>>>();
            _apiClient = new Mock<IQnaApiClient>();
            _sessionService = new Mock<ISessionService>();
            _applicationId = Guid.NewGuid();
            _sessionKey = string.Format("NotRequiredConfiguration_{0}", _applicationId);
        }

        [Test]
        public void Not_required_overrides_unchanged_if_no_matching_tags_in_application_data()
        {
            var configuration = new List<NotRequiredOverrideConfiguration> 
            {
                new NotRequiredOverrideConfiguration
                {
                    Conditions = new List<NotRequiredCondition>
                    {
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "Field2",
                            MustEqual = "Value"
                        }
                    },
                    SectionId = 1,
                    SequenceId = 2
                }
            };

            _config.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };
            _apiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

            _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
            var overrides = _service.GetNotRequiredOverrides(_applicationId);

            overrides[0].Conditions[0].Value.Should().BeEmpty();
            overrides[0].AllConditionsMet.Should().BeFalse();
        }

        [Test]
        public void Not_required_overrides_populated_with_answers_from_question_tags()
        {
            var configuration = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
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

            _config.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };
            _apiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

            _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
            var overrides = _service.GetNotRequiredOverrides(_applicationId);

            overrides[0].Conditions[0].Value.Should().Be("Test");
            overrides[0].AllConditionsMet.Should().BeTrue();
        }

        [Test]
        public void Not_required_overrides_retrieved_from_session_cache_if_already_looked_up_previously()
        {
            var configuration = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
                {
                    Conditions = new List<NotRequiredCondition>
                    {
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "Field1",
                            MustEqual = "Test",
                            Value = "NotTest"
                        }
                    },
                    SectionId = 1,
                    SequenceId = 2
                }
            };

            _config.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };
            _apiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(configuration);
            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

            _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
            var overrides = _service.GetNotRequiredOverrides(_applicationId);

            overrides[0].Conditions[0].Value.Should().Be("NotTest");
            overrides[0].AllConditionsMet.Should().BeFalse();
        }

        [Test]
        public void RefreshNotRequiredOverrides_repopulates_the_session_cache_config()
        {
            var configuration = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
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

            _config.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };

            _apiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
            _service.RefreshNotRequiredOverrides(_applicationId);

            _sessionService.Verify(x => x.Remove(_sessionKey), Times.Once);
            _sessionService.Verify(x => x.Set(_sessionKey, configuration), Times.Once);
        }
    }
}
