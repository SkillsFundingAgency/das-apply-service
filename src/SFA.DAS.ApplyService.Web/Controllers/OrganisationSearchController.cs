using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;
using System;
using System.Linq;
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
        public async Task<IActionResult> Results(string searchString, string typeFilter = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                ModelState.AddModelError(nameof(searchString), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }

            var searchResults = await _apiClient.Search(searchString);
            var organisationTypes = await _apiClient.GetOrganisationTypes();

            if (organisationTypes.Any(ot => ot.Type == typeFilter))
            {
                searchResults = searchResults.Where(sr => sr.Type?.Type == typeFilter).AsEnumerable();
            }

            var searchViewModel = new OrganisationSearchViewModel
            {
                SearchString = searchString,
                OrganisationTypeFilter = typeFilter,
                Organisations = searchResults,
                OrganisationTypes = organisationTypes
            };

            return View(searchViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Type(string name, int? ukprn, string postcode)
        {
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError(nameof(name), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }

            var organisationTypes = await _apiClient.GetOrganisationTypes();

            var searchViewModel = new OrganisationSearchViewModel
            {
                OrganisationTypes = organisationTypes,
                SearchString = name,
                Name = name,
                Ukprn = ukprn,
                Postcode = postcode
            };

            return View(searchViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Done(string name, int? ukprn, string postcode, string organisationType)
        {
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError(nameof(name), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }
            else if (string.IsNullOrEmpty(organisationType))
            {
                ModelState.AddModelError(nameof(organisationType), "Select an organisation type");
                return RedirectToAction(nameof(Type), new { name, ukprn, postcode});
            }

            // make sure we got everything
            var searchResults = await _apiClient.Search(name);

            // filter name
            searchResults = searchResults.Where(sr => sr.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            // filter organisation type
            searchResults = searchResults.Where(sr => sr.Type?.Type != null ? sr.Type.Type.Equals(postcode, StringComparison.InvariantCultureIgnoreCase) : true);

            // filter ukprn
            searchResults = searchResults.Where(sr => ukprn.HasValue ? sr.Ukprn == ukprn : true);

            // filter postcode
            searchResults = searchResults.Where(sr => !string.IsNullOrEmpty(postcode) ? (sr.Address != null ? sr.Address.Postcode.Equals(postcode, StringComparison.InvariantCultureIgnoreCase) : true) : true);

            var organisation = searchResults.FirstOrDefault();

            if (organisation.Type == null) organisation.Type = new Models.OrganisationType { Type = organisationType };

            return View();
        }
    }
}
