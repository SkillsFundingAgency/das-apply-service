using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application;

namespace SFA.DAS.ApplyService.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        
        public async Task SendEmail(string toAddress, int emailId, object replacements)
        {
            _logger.LogInformation($"Sending email to {toAddress} with replacements :");
            foreach (var property in replacements.GetType().GetProperties())
            {
                _logger.LogInformation($"Property: {property.Name}, Value: {property.GetValue(replacements)}");
            }
        }
    }
}