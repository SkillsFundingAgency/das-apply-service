using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class OrganisationSearchController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;
        private readonly OrganisationApiClient _organisationApiClient;
        private readonly OrganisationSearchApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;

        public OrganisationSearchController(IUsersApiClient usersApiClient, OrganisationApiClient organisationApiClient,
            OrganisationSearchApiClient apiClient, IHttpContextAccessor httpContextAccessor,
            ISessionService sessionService)
        {
            _usersApiClient = usersApiClient;
            _organisationApiClient = organisationApiClient;
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _usersApiClient.GetUserBySignInId(
                _httpContextAccessor.HttpContext.User.FindFirstValue("UserId"));


            // Can get details from UkPrn?
            // var ukprn = await _apiClient.GetOrganisationByUkprn(user.Ukprn); <-- this doesn't exist right now!

            // Can get details from Email?
            var org = await _apiClient.GetOrganisationByEmail(user.Email);

            if (org != null)
            {
                if (org.OrganisationType is null)
                {
                    // org found but need to ask for Organisation Type
                    var viewModel = new OrganisationSearchViewModel
                    {
                        SearchString = org.Name,
                        Name = org.Name,
                        Ukprn = org.Ukprn,
                        OrganisationType = org.OrganisationType,
                        Postcode = org.Address?.Postcode,
                        OrganisationTypes = await _apiClient.GetOrganisationTypes()
                    };

                    return View(nameof(Type), viewModel);
                }
                else
                {
                    // Got everything, set user to approved
                    await _organisationApiClient.Create(org, user.Id);

                    if (!user.IsApproved)
                    {
                        await _usersApiClient.ApproveUser(user.Id);
                    }

                    return RedirectToAction("Applications", "Application");
                }
            }

            // Nothing found, go to search
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Results(string searchString, string organisationTypeFilter = null)
        {
            if (string.IsNullOrEmpty(searchString) || searchString.Length < 2)
            {
                ModelState.AddModelError(nameof(searchString), "Enter a valid search string");
                return RedirectToAction(nameof(Index));
            }

            var searchResults = await _apiClient.SearchOrganisation(searchString);
            var organisationTypes = await _apiClient.GetOrganisationTypes();

            if (organisationTypes != null && organisationTypes.Any(ot =>
                    ot.Type.Equals(organisationTypeFilter, StringComparison.InvariantCultureIgnoreCase)))
            {
                searchResults = searchResults.Where(sr => sr.OrganisationType == organisationTypeFilter).AsEnumerable();
            }

            var viewModel = new OrganisationSearchViewModel
            {
                SearchString = searchString,
                OrganisationTypeFilter = organisationTypeFilter,
                Organisations = searchResults,
                OrganisationTypes = organisationTypes
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Type(OrganisationSearchViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Name) || viewModel.SearchString.Length < 2)
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a valid search string");
                return RedirectToAction(nameof(Index));
            }

            viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganisationSearchViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Name) || viewModel.SearchString.Length < 2)
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a valid search string");
                return RedirectToAction(nameof(Index));
            }
            else if (string.IsNullOrEmpty(viewModel.OrganisationType))
            {
                ModelState.AddModelError(nameof(viewModel.OrganisationType), "Select an organisation type");
                viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
                return View(nameof(Type), viewModel);
            }

            // make sure we got everything before trying to create organisation
            var searchResults =
                await _apiClient.SearchOrganisation(viewModel.SearchString);

            // filter ukprn
            searchResults = searchResults.Where(sr =>
                sr.Ukprn.HasValue && viewModel.Ukprn.HasValue
                    ? sr.Ukprn == viewModel.Ukprn
                    : true);

            // filter name
            searchResults = searchResults.Where(sr =>
                sr.Name.Equals(viewModel.Name,
                    StringComparison.InvariantCultureIgnoreCase)); // Name has to be identical match

            // filter organisation type
            searchResults = searchResults.Where(sr =>
                sr.OrganisationType != null
                    ? sr.OrganisationType.Equals(viewModel.OrganisationType,
                        StringComparison.InvariantCultureIgnoreCase)
                    : true);

            // filter postcode
            searchResults = searchResults.Where(sr =>
                !string.IsNullOrEmpty(viewModel.Postcode)
                    ? (sr.Address != null
                        ? sr.Address.Postcode.Equals(viewModel.Postcode, StringComparison.InvariantCultureIgnoreCase)
                        : true)
                    : true);

            var organisationSearchResult = searchResults.FirstOrDefault();
            
            if (organisationSearchResult != null)
            {
                if (organisationSearchResult.OrganisationType == null)
                    organisationSearchResult.OrganisationType = viewModel.OrganisationType;

                var user = await _usersApiClient.GetUserBySignInId(_httpContextAccessor.HttpContext.User.FindFirstValue("sub"));

                var orgThatWasCreated = await _organisationApiClient.Create(organisationSearchResult, user.Id);

                return RedirectToAction("Applications", "Application");
            }
            else
            {
                // Should never get here but needed to do something as we don't have an error page!!
                viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
                return View(nameof(Type), viewModel);
            }
        }
    }
}