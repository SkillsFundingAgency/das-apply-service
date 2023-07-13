using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplyConfig _applyConfig;

        public HomeController(IApplyConfig applyConfig)
        {
            _applyConfig = applyConfig;
        }

        public IActionResult Index()
        { 
            return View(new HomeIndexViewModel(_applyConfig.UseGovSignIn));
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Cookies()
        {
            return View();
        }

        public IActionResult CookieDetails()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Accessibility()
        {
            return View();
        }

        public IActionResult TermsOfUse()
        {
            return View();
        }

        public IActionResult SessionTimeout()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/home/error/{statusCode}")]
        public IActionResult Error(int? statusCode = null)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    return View("PageNotFound");
                case (int)HttpStatusCode.Unauthorized:
                    return View("AccessDenied");
                case (int)HttpStatusCode.ServiceUnavailable:
                    return View("ServiceUnavailable");
                case (int)HttpStatusCode.InternalServerError:
                    return View("ServiceError");
                default:
                    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}