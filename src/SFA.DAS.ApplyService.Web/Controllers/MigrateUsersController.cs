using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class MigrateUsersController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;

        public MigrateUsersController(IUsersApiClient usersApiClient)
        {
            _usersApiClient = usersApiClient;
        }
//
//        [HttpPost("/MigrateUsers")]
//        public async Task<IActionResult> MigrateUsers()
//        {
//            await _usersApiClient.MigrateUsers();
//            return Ok();
//        }
//
//
//        [HttpPost("/MigrateContactAndOrgs")]
//        public async Task<IActionResult> MigrateContactAndOrgs([FromBody]MigrateContactOrganisation migrateContactOrganisation)
//        {
//            await _usersApiClient.MigrateContactAndOrgs(migrateContactOrganisation);
//            return Ok();
//        }

       
    }
}