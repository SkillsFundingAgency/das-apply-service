using SFA.DAS.ApplyService.Application.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class UkprnWhitelistValidator : IUkprnWhitelistValidator
    {
        private readonly IApplyRepository _applyRepository;

        public UkprnWhitelistValidator(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public bool IsWhitelistedUkprn(long longUkprnToCheck)
        {
            int ukprn;
            if (int.TryParse(longUkprnToCheck.ToString(), out ukprn))
            {
                return _applyRepository.IsUkprnWhitelisted(ukprn).Result;
            }
            else
            {
                return false;
            }
        }
    }
}
