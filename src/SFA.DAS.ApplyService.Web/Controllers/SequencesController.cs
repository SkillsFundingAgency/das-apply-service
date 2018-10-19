using System;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class SequencesController : Controller
    {
        public SequencesController()
        {
            
        }
        
        
        [HttpGet("/Apply/Sequence/{sequenceId}")]
        public IActionResult GetSequence(string sequenceId)
        {
            return View();
        }
    }
}