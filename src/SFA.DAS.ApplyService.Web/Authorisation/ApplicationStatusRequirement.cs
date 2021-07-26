using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ApplyService.Web.Authorization
{
    public class ApplicationStatusRequirement : IAuthorizationRequirement
    {
        private List<string> _statuses { get; }

        public IReadOnlyList<string> Statuses => _statuses.AsReadOnly();

        public ApplicationStatusRequirement(string status)
        {
            _statuses = new List<string> {status};
        }

        public ApplicationStatusRequirement(params string[] statuses)
        {
            _statuses = new List<string>();
            _statuses.AddRange(statuses);
        }
    }
}