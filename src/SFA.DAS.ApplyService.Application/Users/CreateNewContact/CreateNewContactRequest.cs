using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.CreateNewContact
{
    public class CreateNewContactRequest : IRequest
    {
        public Guid Id { get; set; }
        public Guid OrganisationId { get; set; }
        public string Email { get; set; }
        public string GivenNames { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
        public string SignInType { get; set; }
    }
}