using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IDiffService
    {
        IReadOnlyList<DiffItem> GenerateDiff(Dictionary<string, object> initial, Dictionary<string, object> updated);
    }
}
