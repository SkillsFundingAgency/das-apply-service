﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using SFA.DAS.ApplyService.Application;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfigurationService _configurationService;

        public EmailService(ILogger<EmailService> logger, IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }
        
        public async Task SendEmail(string toAddress, int emailId, dynamic replacements)
        {
            var config = await _configurationService.GetConfig();
            
            _logger.LogInformation($"Sending email to {toAddress} with replacements :");
            foreach (var property in replacements.GetType().GetProperties())
            {
                _logger.LogInformation($"Property: {property.Name}, Value: {property.GetValue(replacements)}");
            }
            
            var client = new SendGridClient(config.Email.SendGridApiKey);
            var from = new EmailAddress("david.gouge@digital.education.gov.uk", "Apply Service");
            var subject = "Your Sign In Account";
            var to = new EmailAddress(toAddress);
            var htmlContent = $@"<p>Hi, [GivenName] [FamilyName]</p>
                                <p>You've already got an account with us.</p>
                                <p><a href='{config.SignInPage}'>Click here to to Sign In</a></p>";

            foreach (var property in replacements.GetType().GetProperties())
            {
                htmlContent = htmlContent.Replace($"[{property.Name}]", $"{property.GetValue(replacements)}");
            }
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendPreAmbleEmail(string toAddress, int emailId, dynamic replacements)
        {
            // TODO: Merge this with above at some point!
            var config = await _configurationService.GetConfig();

            _logger.LogInformation($"Sending email to {toAddress} with replacements :");
            foreach (var property in replacements.GetType().GetProperties())
            {
                _logger.LogInformation($"Property: {property.Name}, Value: {property.GetValue(replacements)}");
            }

            var client = new SendGridClient(config.Email.SendGridApiKey);
            var from = new EmailAddress(toAddress, "Apply Service");
            var subject = "Pre-Amble Notification";
            var to = new EmailAddress(toAddress);
            var htmlContent = $@"<p>Dear [OrganisationName],</p>
                                <p>There has been activity on your account.</p>
                                <p><a href='{config.SignInPage}'>Click here to view more</a></p>";

            foreach (var property in replacements.GetType().GetProperties())
            {
                htmlContent = htmlContent.Replace($"[{property.Name}]", $"{property.GetValue(replacements)}");
            }

            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}