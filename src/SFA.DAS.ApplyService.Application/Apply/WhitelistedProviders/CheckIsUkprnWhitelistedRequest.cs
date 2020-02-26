using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.WhitelistedProviders
{
    public class CheckIsUkprnWhitelistedRequest : IRequest<bool>
    {
        public int UKPRN { get; set; }

        public CheckIsUkprnWhitelistedRequest(int ukprn)
        {
            UKPRN = ukprn;
        }
    }
}
