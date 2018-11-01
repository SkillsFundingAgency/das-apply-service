using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            if (!string.IsNullOrEmpty(searchString))
            {
                ModelState.AddModelError(nameof(searchString), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }

            var searchResults = await _apiClient.Search(searchString);
            var searchViewModel = new OrganisationSearchViewModel
            {
                Organisations = searchResults,
                SearchString = searchString,
            };

            return View(searchViewModel);
        }

        [HttpPost]
        public IActionResult Results(OrganisationSearchViewModel viewModel)
        {
            if (viewModel?.SelectedOrganisation == null)
            {
                if (!string.IsNullOrEmpty(viewModel?.SearchString))
                {
                    ModelState.AddModelError(nameof(viewModel.SelectedOrganisation), "Select an organisation");
                    return RedirectToAction(nameof(Results), new { searchString = viewModel.SearchString });
                }

                return RedirectToAction(nameof(Index));
            }

            // PRG pattern!!
            _sessionService.Set("OrganisationSearchViewModel", JsonConvert.SerializeObject(viewModel));

            if (viewModel.SelectedOrganisation.Type != null)
            {
                return RedirectToAction(nameof(Type));
            }
            else
            {
                return RedirectToAction(nameof(Done));
            }
        }



        [HttpGet]
        public async Task<IActionResult> Type()
        {
            var viewModel = _sessionService.Get<OrganisationSearchViewModel>("OrganisationSearchViewModel");

            if (viewModel?.SelectedOrganisation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Type(OrganisationSearchViewModel viewModel)
        {
            if (viewModel?.SelectedOrganisation.Type == null)
            {
                if(viewModel?.SelectedOrganisation != null)
                {
                    ModelState.AddModelError(nameof(viewModel.SelectedOrganisation), "Select an organisation type");
                    return RedirectToAction(nameof(Type));
                }
                else if (!string.IsNullOrEmpty(viewModel?.SearchString))
                {
                    ModelState.AddModelError(nameof(viewModel.SelectedOrganisation), "Select an organisation");
                    return RedirectToAction(nameof(Results), new { searchString = viewModel.SearchString });
                }

                return RedirectToAction(nameof(Index));
            }

            // PRG pattern!!
            _sessionService.Set("OrganisationSearchViewModel", JsonConvert.SerializeObject(viewModel));

            return RedirectToAction(nameof(Done));
        }




        [HttpGet]
        public IActionResult Done()
        {
            var viewModel = _sessionService.Get<OrganisationSearchViewModel>("OrganisationSearchViewModel");
            return View(viewModel);
        }
    }
}
