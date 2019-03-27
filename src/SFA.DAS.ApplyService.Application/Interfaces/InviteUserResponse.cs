using System;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public class InviteUserResponse
    {
        public InviteUserResponse()
        {
            IsSuccess = true;
        }
        public bool IsSuccess { get; set; }
        public bool UserExists { get; set; }
        public Guid ExistingUserId { get; set; }
    }
}