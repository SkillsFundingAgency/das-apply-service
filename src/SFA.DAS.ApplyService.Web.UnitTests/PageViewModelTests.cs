using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Controllers;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class PageViewModelTests
    {
        private List<QnaPageOverrideConfiguration> _config;
        private Guid _applicationId;
        private int _sequenceId;
        private int _sectionId;
        private string _pageContext;
        private string _redirectAction;
        private string _returnUrl;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _sequenceId = 1;
            _sectionId = 2;
            _pageContext = "page context";
            _redirectAction = "TaskList";
            _returnUrl = "/return";

            _config = new List<QnaPageOverrideConfiguration>
            {
                new QnaPageOverrideConfiguration
                {
                    PageId = "100",
                    CTAButtonText = "Override text",
                    HideCTA = false
                },
                new QnaPageOverrideConfiguration
                {
                    PageId = "110",
                    HideCTA = true
                },
                new QnaPageOverrideConfiguration
                {
                    PageId = "120",
                    CTAButtonText = string.Empty,
                    HideCTA = false
                }
            };

        }

        [Test]
        public void Page_view_model_sets_button_text_to_default_if_page_not_listed_in_config()
        {
            var pageId = "130";
            
            var model = new PageViewModel(
                _applicationId,
                _sequenceId,
                _sectionId,
                pageId,
                CreateTestPage(pageId, _sectionId.ToString()),
                _pageContext,
                _redirectAction,
                _returnUrl,
                new List<ValidationErrorDetail>(),
                _config);

            model.CTAButtonText.Should().Be(PageViewModel.DefaultCTAButtonText);
        }

        [Test]
        public void Page_view_model_sets_button_text_to_default_if_page_listed_but_not_set_to_hide_or_override_text()
        {
            var pageId = "120";

            var model = new PageViewModel(
                _applicationId,
                _sequenceId,
                _sectionId,
                pageId,
                CreateTestPage(pageId, _sectionId.ToString()),
                _pageContext,
                _redirectAction,
                _returnUrl,
                new List<ValidationErrorDetail>(),
                _config);

            model.CTAButtonText.Should().Be(PageViewModel.DefaultCTAButtonText);
        }
        
        [Test]
        public void Page_view_model_sets_button_text_to_specified_value_if_matching_page_id_in_config()
        {
            var pageId = "100";
            
            var model = new PageViewModel(
                _applicationId,
                _sequenceId,
                _sectionId,
                pageId,
                CreateTestPage(pageId, _sectionId.ToString()),
                _pageContext,
                _redirectAction,
                _returnUrl,
                new List<ValidationErrorDetail>(),
                _config);
            
            model.CTAButtonText.Should().Be(_config[0].CTAButtonText);
        }

        [Test]
        public void Page_view_model_hides_button_if_specified_in_config()
        {
            var pageId = "110";

            var model = new PageViewModel(
                _applicationId,
                _sequenceId,
                _sectionId,
                pageId,
                CreateTestPage(pageId, _sectionId.ToString()),
                _pageContext,
                _redirectAction,
                _returnUrl,
                new List<ValidationErrorDetail>(),
                _config);

            model.HideCTA.Should().BeTrue();
        }
        
        private Page CreateTestPage(string pageId, string sectionId)
        {
            return new Page
            {
                PageId = pageId,
                SectionId = sectionId,
                PageOfAnswers = new List<PageOfAnswers>(),
                Questions = new List<Question>()
            };
        }
    }
}
