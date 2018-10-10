using System;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class DeveloperEnvironmentException : Exception
    {
        public DeveloperEnvironmentException(string message) : base(message)
        {
            
        }
    }
}