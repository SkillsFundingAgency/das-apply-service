//using Microsoft.Extensions.Options;
//using FluentAssertions;
//using Moq;
//using Newtonsoft.Json.Linq;
//using NUnit.Framework;
//using SFA.DAS.ApplyService.Session;
//using SFA.DAS.ApplyService.Web.Configuration;
//using SFA.DAS.ApplyService.Web.Infrastructure;
//using SFA.DAS.ApplyService.Web.Services;
//using System;
//using System.Collections.Generic;
//using SFA.DAS.ApplyService.Application.Apply.Roatp;
//using SFA.DAS.ApplyService.Application.UnitTests;
//using AutoMapper;
//using SFA.DAS.ApplyService.Web.AutoMapper;
//using NotRequiredOverrideConfiguration = SFA.DAS.ApplyService.Web.Configuration.NotRequiredOverrideConfiguration;

//namespace SFA.DAS.ApplyService.Web.UnitTests
//{
//    [TestFixture]
//    public class NotRequiredOverridesServiceTests
//    {
//        private Mock<IOptions<List<NotRequiredOverrideConfiguration>>> _config;
//        private Mock<IApplicationApiClient> _applicationApiClient;
//        private Mock<IQnaApiClient> _qnaApiClient;
//        private Mock<ISessionService> _sessionService;
//        private NotRequiredOverridesService _service;
//        private Guid _applicationId;
//        private string _sessionKey;

//        [SetUp]
//        public void Before_each_test()
//        {
//            Mapper.Reset();

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<NotRequiredOverridesProfile>();
//                cfg.AddProfile<NotRequiredConditionsProfile>();
//            });

//            Mapper.AssertConfigurationIsValid();

//            _config = new Mock<IOptions<List<NotRequiredOverrideConfiguration>>>();
//            _applicationApiClient = new Mock<IApplicationApiClient>();
//            _qnaApiClient = new Mock<IQnaApiClient>();
//            _sessionService = new Mock<ISessionService>();
//            _applicationId = Guid.NewGuid();
//            _sessionKey = string.Format("NotRequiredConfiguration_{0}", _applicationId);
            
//            _service = new NotRequiredOverridesService(_config.Object, _applicationApiClient.Object, _qnaApiClient.Object, _sessionService.Object);
//        }

//        [Test]
//        public void Not_required_overrides_retrieved_from_config_if_not_available_in_apply_data()
//        {
//            var configuration = new List<NotRequiredOverrideConfiguration>
//            {
//                new NotRequiredOverrideConfiguration
//                {
//                    Conditions = new List<Configuration.NotRequiredCondition>
//                    {
//                        new Configuration.NotRequiredCondition
//                        {
//                            ConditionalCheckField = "Field2",
//                            MustEqual = "Value"
//                        }
//                    },
//                    SectionId = 1,
//                    SequenceId = 2
//                }
//            };

//            _config.Setup(x => x.Value).Returns(configuration);

//            List<NotRequiredOverrideConfiguration> sessionConfig = null;
//            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(sessionConfig);

//            Application.Apply.Roatp.NotRequiredOverrideConfiguration applyDataConfig = null;
//            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(applyDataConfig);

//            var overrides = _service.GetNotRequiredOverrides(_applicationId); //MFCMFC.GetAwaiter().GetResult();

//            overrides.Should().NotBeNull();
//            _applicationApiClient.Verify(x => x.GetNotRequiredOverrides(_applicationId), Times.Once);
//            _applicationApiClient.Verify(x => x.UpdateNotRequiredOverrides(_applicationId, It.IsAny<Application.Apply.Roatp.NotRequiredOverrideConfiguration>()), Times.Once);
//        }

//        [Test]
//        public void Not_required_overrides_retrieved_from_apply_data()
//        {
//            List<NotRequiredOverrideConfiguration> sessionConfig = null;
//            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(sessionConfig);

//            IEnumerable<NotRequiredOverride> applyDataConfig = new List<NotRequiredOverride> 
//            {
//                new NotRequiredOverride
//                {
//                    Conditions = new List<Application.Apply.Roatp.NotRequiredCondition>
//                    {
//                        new Application.Apply.Roatp.NotRequiredCondition
//                        {
//                            ConditionalCheckField = "Field2",
//                            MustEqual = "Value"
//                        }
//                    },
//                    SectionId = 1,
//                    SequenceId = 2
//                }
//            };
//            var configuration = new Application.Apply.Roatp.NotRequiredOverrideConfiguration { NotRequiredOverrides = applyDataConfig };
//            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(configuration);

//            var overrides = _service.GetNotRequiredOverrides(_applicationId); //MFCMFC .GetAwaiter().GetResult();

//            overrides.Should().NotBeNull();
//            _config.Verify(x => x.Value, Times.Never);
//            _applicationApiClient.Verify(x => x.GetNotRequiredOverrides(_applicationId), Times.Once);
//            _applicationApiClient.Verify(x => x.UpdateNotRequiredOverrides(_applicationId, It.IsAny<Application.Apply.Roatp.NotRequiredOverrideConfiguration>()), Times.Never);
//        }

//        [Test]
//        public void Not_required_overrides_unchanged_if_no_matching_tags_in_application_data()
//        {
//            Application.Apply.Roatp.NotRequiredOverrideConfiguration applyDataConfig = null;
//            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(applyDataConfig);

//            var configuration = new List<NotRequiredOverrideConfiguration> 
//            {
//                new NotRequiredOverrideConfiguration
//                {
//                    Conditions = new List<Configuration.NotRequiredCondition>
//                    {
//                        new Configuration.NotRequiredCondition
//                        {
//                            ConditionalCheckField = "Field2",
//                            MustEqual = "Value"
//                        }
//                    },
//                    SectionId = 1,
//                    SequenceId = 2
//                }
//            };

//            _config.Setup(x => x.Value).Returns(configuration);

//            var applicationData = new JObject
//            {
//                ["Field1"] = "Test"
//            };
//            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

//            List<NotRequiredOverrideConfiguration> nullConfig = null;
//            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(nullConfig);
//            // MFCMFC_sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
//            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

//            var overrides = _service.GetNotRequiredOverrides(_applicationId); //MFCMFC .GetAwaiter().GetResult();
//            //MFCMFC _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
//            // var overrides = _service.GetNotRequiredOverrides(_applicationId);

//            overrides[0].Conditions[0].Value.Should().BeEmpty();
//            overrides[0].AllConditionsMet.Should().BeFalse();
//        }

//        [Test]
//        public void Not_required_overrides_populated_with_answers_from_question_tags()
//        {
//            Application.Apply.Roatp.NotRequiredOverrideConfiguration applyDataConfig = null;
//            _applicationApiClient.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(applyDataConfig);

//            var configuration = new List<NotRequiredOverrideConfiguration>
//            {
//                new NotRequiredOverrideConfiguration
//                {
//                    Conditions = new List<Configuration.NotRequiredCondition>
//                    {
//                        new Configuration.NotRequiredCondition
//                        {
//                            ConditionalCheckField = "Field1",
//                            MustEqual = "Test"
//                        }
//                    },
//                    SectionId = 1,
//                    SequenceId = 2
//                }
//            };

//            _config.Setup(x => x.Value).Returns(configuration);

//            var applicationData = new JObject
//            {
//                ["Field1"] = "Test"
//            };
//            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

//            List<NotRequiredOverrideConfiguration> nullConfig = null;
//            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(nullConfig);
//            //MFCMFC _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).ReturnsInOrder(null, configuration);
//            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

//            var overrides = _service.GetNotRequiredOverrides(_applicationId); //MFCMFC .GetAwaiter().GetResult();
//            //MFCMFC _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
//            // var overrides = _service.GetNotRequiredOverrides(_applicationId);

//            overrides[0].Conditions[0].Value.Should().Be("Test");
//            overrides[0].AllConditionsMet.Should().BeTrue();
//        }

//        [Test]
//        public void Not_required_overrides_retrieved_from_session_cache_if_already_looked_up_previously()
//        {
//            var configuration = new List<NotRequiredOverrideConfiguration>
//            {
//                new NotRequiredOverrideConfiguration
//                {
//                    Conditions = new List<Configuration.NotRequiredCondition>
//                    {
//                        new Configuration.NotRequiredCondition
//                        {
//                            ConditionalCheckField = "Field1",
//                            MustEqual = "Test",
//                            Value = "NotTest"
//                        }
//                    },
//                    SectionId = 1,
//                    SequenceId = 2
//                }
//            };

//            _config.Setup(x => x.Value).Returns(configuration);

//            var applicationData = new JObject
//            {
//                ["Field1"] = "Test"
//            };
//            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

//            _sessionService.Setup(x => x.Get<List<NotRequiredOverrideConfiguration>>(_sessionKey)).Returns(configuration);
//            _sessionService.Setup(x => x.Set(_sessionKey, configuration));

//            var overrides = _service.GetNotRequiredOverrides(_applicationId); //MFCMFC .GetAwaiter().GetResult();

//            overrides[0].Conditions[0].Value.Should().Be("NotTest");
//            overrides[0].AllConditionsMet.Should().BeFalse();
//        }

//        ////MFCMFC not sure if this is needed
//        //[Test]
//        //public void RefreshNotRequiredOverrides_repopulates_the_session_cache_config()
//        //{
//        //    var configuration = new List<NotRequiredOverrideConfiguration>
//        //    {
//        //        new NotRequiredOverrideConfiguration
//        //        {
//        //            Conditions = new List<NotRequiredCondition>
//        //            {
//        //                new NotRequiredCondition
//        //                {
//        //                    ConditionalCheckField = "Field1",
//        //                    MustEqual = "Test"
//        //                }
//        //            },
//        //            SectionId = 1,
//        //            SequenceId = 2
//        //        }
//        //    };

//        //    _config.Setup(x => x.Value).Returns(configuration);

//        //    var applicationData = new JObject
//        //    {
//        //        ["Field1"] = "Test"
//        //    };

//        //    _apiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(applicationData);

//        //    _service = new NotRequiredOverridesService(_config.Object, _apiClient.Object, _sessionService.Object);
//        //    _service.RefreshNotRequiredOverrides(_applicationId);

//        //    _sessionService.Verify(x => x.Remove(_sessionKey), Times.Once);
//        //    _sessionService.Verify(x => x.Set(_sessionKey, configuration), Times.Once);
//        //}
//    }
//}
