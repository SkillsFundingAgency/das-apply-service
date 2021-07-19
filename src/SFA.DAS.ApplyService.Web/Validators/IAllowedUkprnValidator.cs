using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public interface IAllowedUkprnValidator
    {
        Task<bool> IsUkprnOnAllowedList(int ukprn);
    }
}
