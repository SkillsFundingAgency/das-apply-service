using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.GetContact
{
    public class GetContactByIdRequest : IRequest<Contact>
    {
        public Guid Id { get; }

        public GetContactByIdRequest(Guid id)
        {
            Id = id;
        }
    }
}