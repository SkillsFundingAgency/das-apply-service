﻿using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class UkprnWhitelistValidator : IUkprnWhitelistValidator
    {
        private readonly IWhitelistedProvidersApiClient _whitelistedProvidersApiClient;

        public UkprnWhitelistValidator(IWhitelistedProvidersApiClient whitelistedProvidersApiClient)
        {
            _whitelistedProvidersApiClient = whitelistedProvidersApiClient;
        }

        public bool IsWhitelistedUkprn(long longUkprnToCheck)
        {
            int ukprn;
            if (int.TryParse(longUkprnToCheck.ToString(), out ukprn))
            {
                return _whitelistedProvidersApiClient.CheckIsWhitelistedUkprn(ukprn).Result;
            }
            else
            {
                return false;
            }
        }
    }
}
