using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class PerformValidationAttribute : Attribute, IFilterMetadata
    {
        public string ValidationEndpoint { get; set; }
    }
}