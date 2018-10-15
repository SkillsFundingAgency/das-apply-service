using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateSignInId
{
    public class UpdateSignInRequestHandler : IRequestHandler<UpdateSignInIdRequest>
    {
        private readonly IContactRepository _contactRepository;

        public UpdateSignInRequestHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(UpdateSignInIdRequest request, CancellationToken cancellationToken)
        {
            await _contactRepository.UpdateSignInId(request.ContactId, request.SignInId); 
            
            return Unit.Value;
        }
    }
}