using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            throw new System.Exception();
            return View();
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult SessionTimeout()
        {
            return View();
        }

        [Route("{controller}/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("{controller}/500")]
        public IActionResult ServiceError()
        {
            return View();
        }

        [Route("{controller}/503")]
        public IActionResult ServiceUnavailable()
        {
            return View();
        }
    }
}