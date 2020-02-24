using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public interface IUkprnWhitelistValidator
    {
        Task<bool> IsWhitelistedUkprn(long longUkprnToCheck);
    }
}
