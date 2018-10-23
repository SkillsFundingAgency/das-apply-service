using System;

namespace SFA.DAS.ApplyService.Application.Apply.GetPage
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message): base(message) {}
    }
}