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

        public OrganisationSearchController(IUsersApiClient usersApiClient, OrganisationApiClient organisationApiClient, OrganisationSearchApiClient apiClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
        {
            _usersApiClient = usersApiClient;
            _organisationApiClient = organisationApiClient;
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var user = await _usersApiClient.GetUserBySignInId(_httpContextAccessor.HttpContext.User.FindFirstValue("sub"));

            if(user != null)
            {
                // var ukprn = get from security claim <-- this doesn't exist right now!
                var org = await _organisationApiClient.GetByContactEmail(user.Email);

                if (org != null)
                {
                    if (org.OrganisationType is null)
                    {
                        // org found but doesn't have Organisation Type
                        var viewModel = new OrganisationSearchViewModel
                        {
                            SearchString = org.Name,
                            Name = org.Name,
                            Ukprn = org.OrganisationUkprn,
                            OrganisationType = org.OrganisationType,
                            Postcode = org.OrganisationDetails?.Postcode,
                            OrganisationTypes = await _apiClient.GetOrganisationTypes()
                        };

                        return View(nameof(Type), viewModel);
                    }
                    else
                    {
                        // Got everything, set user to approved
                        if (!user.IsApproved)
                        {
                            await _usersApiClient.ApproveUser(user.Id);
                        }

                        return Ok();
                    }
                }
            }

            // Nothing found, go to search
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Results(string searchString, string organisationTypeFilter = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                ModelState.AddModelError(nameof(searchString), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }

            var searchResults = await _apiClient.SearchOrganisation(searchString);
            var organisationTypes = await _apiClient.GetOrganisationTypes();

            if (organisationTypes != null && organisationTypes.Any(ot => ot.Equals(organisationTypeFilter, StringComparison.InvariantCultureIgnoreCase)))
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
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }

            viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Done(OrganisationSearchViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a search string");
                return RedirectToAction(nameof(Index));
            }
            else if (string.IsNullOrEmpty(viewModel.OrganisationType))
            {
                ModelState.AddModelError(nameof(viewModel.OrganisationType), "Select an organisation type");
                viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
                return View(nameof(Type), viewModel);
            }

            // make sure we got everything
            var searchResults = await _apiClient.SearchOrganisation(viewModel.Name);

            // filter name
            searchResults = searchResults.Where(sr => sr.Name.Equals(viewModel.Name, StringComparison.InvariantCultureIgnoreCase));

            // filter organisation type
            searchResults = searchResults.Where(sr => sr.OrganisationType != null ? sr.OrganisationType.Equals(viewModel.OrganisationType, StringComparison.InvariantCultureIgnoreCase) : true);

            // filter ukprn
            searchResults = searchResults.Where(sr => viewModel.Ukprn.HasValue ? sr.Ukprn == viewModel.Ukprn : true);

            // filter postcode
            searchResults = searchResults.Where(sr => !string.IsNullOrEmpty(viewModel.Postcode) ? (sr.Address != null ? sr.Address.Postcode.Equals(viewModel.Postcode, StringComparison.InvariantCultureIgnoreCase) : true) : true);

            var organisation = searchResults.FirstOrDefault();

            if (organisation.OrganisationType == null) organisation.OrganisationType = viewModel.OrganisationType;

            // TODO: save (with appropriate object) and consider refactoring the above if it makes sense to
            var orgThatWasCreated = await _organisationApiClient.Create(null);

            // TODO: Notify Primary contact of activity

            return View(orgThatWasCreated);
        }
    }
}
