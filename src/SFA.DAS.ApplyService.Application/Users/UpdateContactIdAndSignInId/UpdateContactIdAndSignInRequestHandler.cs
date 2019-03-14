using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactIdAndSignInId
{
    public class UpdateContactIdAndSignInRequestHandler : IRequestHandler<UpdateContactIdAndSignInIdRequest>
    {
        private readonly IContactRepository _contactRepository;

        public UpdateContactIdAndSignInRequestHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(UpdateContactIdAndSignInIdRequest request, CancellationToken cancellationToken)
        {
            await _contactRepository.UpdateContactIdAndSignInId(request.ContactId, request.SignInId, request.Email,request.UpdatedBy); 
            
            return Unit.Value;
        }
    }
}