using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public interface IUkprnWhitelistValidator
    {
        Task<bool> IsWhitelistedUkprn(int ukprn);
    }
}
