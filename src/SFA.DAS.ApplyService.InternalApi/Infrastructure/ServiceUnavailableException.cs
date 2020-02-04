using System;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() : base()
        {

        }

        public ServiceUnavailableException(string message) : base(message)
        {

        }

        public ServiceUnavailableException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
