using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IStateService
    {
        Dictionary<string, object> GetState(object item);
    }
}
