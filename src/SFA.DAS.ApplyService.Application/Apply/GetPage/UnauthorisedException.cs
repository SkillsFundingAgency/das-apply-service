using System;

namespace SFA.DAS.ApplyService.Application.Apply.GetPage
{
    public class UnauthorisedException : Exception
    {
        public UnauthorisedException(string message): base(message) {}
    }
}