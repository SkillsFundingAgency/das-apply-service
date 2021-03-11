using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class AppealRepository : IAppealRepository
    {
        public void Add(Appeal entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Appeal> GetByOversightReviewId(Guid oversightReviewId)
        {
            throw new NotImplementedException();
        }
    }
}
