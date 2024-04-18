using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GBC_Travel_Group_40.Services
{
    public class EmailSender : IEmailSender
    {

        private readonly string _sendGridKey;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _sendGridKey = configuration["SendGrid:ApiKey"];
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try 
            { 

            var client = new SendGridClient(_sendGridKey);
            var from = new EmailAddress("101424041@georgebrown.ca", "Welcome to GBC Travel Group 40");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent to {Email}", email);
                }
                else
                {
                    _logger.LogError("Failed to send email to {Email}. Status code: {StatusCode}", email, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred when sending an email to {Email}", email);
            }
        }
    }
}
