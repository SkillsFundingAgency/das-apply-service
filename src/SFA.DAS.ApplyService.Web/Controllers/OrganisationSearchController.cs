using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;
using System;
using System.Collections.Generic;
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
        private readonly ISessionService _sessionService;

        public OrganisationSearchController(IUsersApiClient usersApiClient, OrganisationApiClient organisationApiClient,
            OrganisationSearchApiClient apiClient, ISessionService sessionService)
        {
            _usersApiClient = usersApiClient;
            _organisationApiClient = organisationApiClient;
            _apiClient = apiClient;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            if (user.ApplyOrganisationId != null)
            {
                return RedirectToAction("Applications", "Application");
            }

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
        public async Task<IActionResult> Results(OrganisationSearchViewModel viewModel)
        {
            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            if (user.ApplyOrganisationId != null)
            {
                return RedirectToAction("Applications", "Application");
            }
            else if (string.IsNullOrEmpty(viewModel.SearchString) || viewModel.SearchString.Length < 2)
            {
                ModelState.AddModelError(nameof(viewModel.SearchString), "Enter a valid search string");
                TempData["ShowErrors"] = true;
                return RedirectToAction(nameof(Index));
            }

            viewModel.Organisations = await _apiClient.SearchOrganisation(viewModel.SearchString);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Type(OrganisationSearchViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Name) || viewModel.SearchString.Length < 2)
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a valid search string");
                TempData["ShowErrors"] = true;
                return RedirectToAction(nameof(Index));
            }

            viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(OrganisationSearchViewModel viewModel)
        {
            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            if (user.ApplyOrganisationId != null)
            {
                return RedirectToAction("Applications", "Application");
            }
            else if (string.IsNullOrEmpty(viewModel.Name) || viewModel.SearchString.Length < 2)
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a valid search string");
                TempData["ShowErrors"] = true;
                return RedirectToAction(nameof(Index));
            }
            else if (string.IsNullOrEmpty(viewModel.OrganisationType))
            {
                ModelState.AddModelError(nameof(viewModel.OrganisationType), "Select an organisation type");
                viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
                TempData["ShowErrors"] = true;
                return View(nameof(Type), viewModel);
            }

            var organisationSearchResult = await GetOrganisation(viewModel.SearchString, viewModel.Name, viewModel.Ukprn, viewModel.OrganisationType, viewModel.Postcode);

            if (organisationSearchResult != null)
            {
                if (organisationSearchResult.CompanyNumber != null)
                {
                    var isActivelyTrading = await _apiClient.IsCompanyActivelyTrading(organisationSearchResult.CompanyNumber);

                    if (!isActivelyTrading)
                    {
                        return View("~/Views/OrganisationSearch/CompanyNotActive.cshtml", viewModel);
                    }
                }

                viewModel.Organisations = new List<OrganisationSearchResult> { organisationSearchResult };
                viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganisationSearchViewModel viewModel)
        {
            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            if (user.ApplyOrganisationId != null)
            {
                return RedirectToAction("Applications", "Application");
            }
            else if (string.IsNullOrEmpty(viewModel.Name) || viewModel.SearchString.Length < 2)
            {
                ModelState.AddModelError(nameof(viewModel.Name), "Enter a valid search string");
                TempData["ShowErrors"] = true;
                return RedirectToAction(nameof(Index));
            }
            else if (string.IsNullOrEmpty(viewModel.OrganisationType))
            {
                ModelState.AddModelError(nameof(viewModel.OrganisationType), "Select an organisation type");
                viewModel.OrganisationTypes = await _apiClient.GetOrganisationTypes();
                TempData["ShowErrors"] = true;
                return View(nameof(Type), viewModel);
            }

            var organisationSearchResult = await GetOrganisation(viewModel.SearchString, viewModel.Name, viewModel.Ukprn, viewModel.OrganisationType, viewModel.Postcode);

            if (organisationSearchResult != null)
            {
                if (organisationSearchResult.CompanyNumber != null)
                {
                    var isActivelyTrading = await _apiClient.IsCompanyActivelyTrading(organisationSearchResult.CompanyNumber);

                    if(!isActivelyTrading)
                    {
                        return View("~/Views/OrganisationSearch/CompanyNotActive.cshtml", viewModel);
                    }
                }


                var orgThatWasCreated = await _organisationApiClient.Create(organisationSearchResult, user.Id);

                return RedirectToAction("Applications", "Application");
            }
            else
            {
                // Should never get here but needed to do something as we don't have an error page!!
                return View(nameof(Results), viewModel);
            }
        }

        private async Task<OrganisationSearchResult> GetOrganisation(string searchString, string name, int? ukprn, string organisationType, string postcode)
        {
            var searchResults = await _apiClient.SearchOrganisation(searchString);

            // filter ukprn
            searchResults = searchResults.Where(sr =>
                sr.Ukprn.HasValue && ukprn.HasValue
                    ? sr.Ukprn == ukprn
                    : true);

            // filter name (identical match)
            searchResults = searchResults.Where(sr =>
                sr.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            // filter organisation type
            searchResults = searchResults.Where(sr =>
                sr.OrganisationType != null
                    ? sr.OrganisationType.Equals(organisationType, StringComparison.InvariantCultureIgnoreCase)
                    : true);

            // filter postcode
            searchResults = searchResults.Where(sr =>
                !string.IsNullOrEmpty(postcode)
                    ? (sr.Address != null
                        ? sr.Address.Postcode.Equals(postcode, StringComparison.InvariantCultureIgnoreCase)
                        : true)
                    : true);

            var organisationSearchResult = searchResults.FirstOrDefault();

            if (organisationSearchResult != null)
            {
                if (organisationSearchResult.OrganisationType == null)
                    organisationSearchResult.OrganisationType = organisationType;

                // At this point find out if org is dormant or ceased trading???
                if(organisationSearchResult.CompanyNumber != null)
                {
                    //_apiClient.IsCompanyTrading(organisationSearchResult.CompanyNumber);
                }
            }

            return organisationSearchResult;
        }
    }
}