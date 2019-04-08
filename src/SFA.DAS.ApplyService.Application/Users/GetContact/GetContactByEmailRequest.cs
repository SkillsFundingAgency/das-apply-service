using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.GetContact
{
    public class GetContactByEmailRequest:IRequest<Contact>
    {
        public GetContactByEmailRequest(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
