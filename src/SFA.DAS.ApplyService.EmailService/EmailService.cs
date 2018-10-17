using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        
        public async Task SendEmail(string toAddress, int emailId, dynamic replacements)
        {
            _logger.LogInformation($"Sending email to {toAddress} with replacements :");
            foreach (var property in replacements.GetType().GetProperties())
            {
                _logger.LogInformation($"Property: {property.Name}, Value: {property.GetValue(replacements)}");
            }
            
            var client = new SendGridClient("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");
            var from = new EmailAddress("david.gouge@digital.education.gov.uk", "Apply Service");
            var subject = "Your Sign In Account";
            var to = new EmailAddress(toAddress);
            var htmlContent = @"<p>Hi, [GivenName] [FamilyName]</p>
<p>You've already got an account with us.</p>
<p><a href='https://localhost:6016/Users/SignIn'>Click here to to Sign In</a></p>";

            foreach (var property in replacements.GetType().GetProperties())
            {
                htmlContent = htmlContent.Replace($"[{property.Name}]", $"{property.GetValue(replacements)}");
            }
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}