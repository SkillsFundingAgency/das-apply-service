using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.GetContact
{
    public class GetContactBySignInIdRequest : IRequest<Contact>
    {
        public Guid SignInId { get; }

        public GetContactBySignInIdRequest(Guid signInId)
        {
            SignInId = signInId;
        }
    }
}