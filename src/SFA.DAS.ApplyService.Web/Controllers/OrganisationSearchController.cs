using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class OrganisationSearchController : Controller
    {
        private readonly OrganisationSearchApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;

        public OrganisationSearchController(OrganisationSearchApiClient apiClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Results(string searchString)
        {
            var searchResults = await _apiClient.Search(searchString);
            var searchViewModel = new OrganisationSearchViewModel
            {
                Organisations = searchResults,
                SearchString = searchString,
            };

            return View(searchViewModel);
        }
    }
}
