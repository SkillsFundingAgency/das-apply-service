﻿using SFA.DAS.ApplyService.Web.Constants;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public record HomeIndexViewModel(bool UseGovSignIn)
    {
        public string ApplyNowLink =>
            UseGovSignIn 
                ? RouteNames.SignIn 
                : RouteNames.ExistingAccount;
    }
}
