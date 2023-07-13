﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Constants;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Models
{
    public class WhenBuildingHomeViewModel
    {
        [TestCase(true, RouteNames.SignIn)]
        [TestCase(false, RouteNames.ExistingAccount)]
        public void Then_The_ApplyNowLink(bool useGovSignIn, string applyNowLink)
        {
            var actual = new HomeIndexViewModel(useGovSignIn);

            actual.ApplyNowLink.Should().Be(applyNowLink);
        }


        [TestCase(true, "Start now")]
        [TestCase(false, "Apply now")]
        public void Then_The_ApplyNowBtnText(bool useGovSignIn, string buttonText)
        {
            var actual = new HomeIndexViewModel(useGovSignIn);

            actual.ApplyNowBtnText.Should().Be(buttonText);
        }
    }
}
