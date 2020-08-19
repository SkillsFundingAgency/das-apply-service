using Microsoft.Extensions.Options;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.UnitTests;
using AutoMapper;
using SFA.DAS.ApplyService.Web.AutoMapper;
using NotRequiredOverrideConfiguration = SFA.DAS.ApplyService.Web.Configuration.NotRequiredOverrideConfiguration;
using NotRequiredCondition = SFA.DAS.ApplyService.Web.Configuration.NotRequiredCondition;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class NotRequiredOverridesServiceTests
    {
        private Mock<IOptions<List<NotRequiredOverrideConfiguration>>> _notRequiredOverrideConfiguration;
        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<ISessionService> _sessionService;
        private NotRequiredOverridesService _notRequiredOverridesService;
        private Guid _applicationId;
        private string _sessionKey;

        [SetUp]
        public void Before_each_test()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<NotRequiredOverridesProfile>();
                cfg.AddProfile<NotRequiredConditionsProfile>();
            });

            Mapper.AssertConfigurationIsValid();

            _notRequiredOverrideConfiguration = new Mock<IOptions<List<NotRequiredOverrideConfiguration>>>();
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _sessionService = new Mock<ISessionService>();
            _applicationId = Guid.NewGuid();
            _sessionKey = string.Format("NotRequiredConfiguration_{0}", _applicationId);
            
            _notRequiredOverridesService = new NotRequiredOverridesService(_notRequiredOverrideConfiguration.Object, _applicationApiClient.Object, _qnaApiClient.Object, _sessionService.Object);
        }

        [Test]
        public void Not_required_overrides_unchanged_if_no_matching_tags_in_application_data()
        {
            Application.Apply.Roatp.NotRequiredOverrideConfiguration applyDataConfig = null;
            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(applyDataConfig);

            var configuration = new List<NotRequiredOverrideConfiguration> 
            {
                new NotRequiredOverrideConfiguration
                {
                    Conditions = new List<Configuration.NotRequiredCondition>
                    {
                        new Configuration.NotRequiredCondition
                        {
                            ConditionalCheckField = "Field2",
                            MustEqual = "Value"
                        }
                    },
                    SectionId = 1,
                    SequenceId = 2
                }
            };

            _notRequiredOverrideConfiguration.Setup(x => x.Value).Returns(configuration);


            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };
            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

            _notRequiredOverridesService = new NotRequiredOverridesService(_notRequiredOverrideConfiguration.Object, _applicationApiClient.Object, _qnaApiClient.Object, _sessionService.Object);
            var overrides = _notRequiredOverridesService.GetNotRequiredOverrides(_applicationId);

            overrides[0].Conditions[0].Value.Should().BeEmpty();
            overrides[0].AllConditionsMet.Should().BeFalse();
        }

        [Test]
        public void Not_required_overrides_populated_with_answers_from_question_tags()
        {
              Application.Apply.Roatp.NotRequiredOverrideConfiguration applyDataConfig = null;
            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(applyDataConfig);

            var configuration = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
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

            _notRequiredOverrideConfiguration.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };
            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

            _notRequiredOverridesService = new NotRequiredOverridesService(_notRequiredOverrideConfiguration.Object, _applicationApiClient.Object, _qnaApiClient.Object, _sessionService.Object);
            var overrides = _notRequiredOverridesService.GetNotRequiredOverrides(_applicationId);

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
                    Conditions = new List<Configuration.NotRequiredCondition>
                    {
                        new Configuration.NotRequiredCondition
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

            _notRequiredOverrideConfiguration.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };
            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(configuration);
            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

            var overrides = _notRequiredOverridesService.GetNotRequiredOverrides(_applicationId); //MFCMFC .GetAwaiter().GetResult();

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

            _notRequiredOverrideConfiguration.Setup(x => x.Value).Returns(configuration);

            var applicationData = new JObject
            {
                ["Field1"] = "Test"
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

            _notRequiredOverridesService = new NotRequiredOverridesService(_notRequiredOverrideConfiguration.Object, _applicationApiClient.Object, _qnaApiClient.Object, _sessionService.Object);
            _notRequiredOverridesService.RefreshNotRequiredOverrides(_applicationId);

            _sessionService.Verify(x => x.Remove(_sessionKey), Times.Once);
            _sessionService.Verify(x => x.Set(_sessionKey, configuration), Times.Once);
        }
    }
}
