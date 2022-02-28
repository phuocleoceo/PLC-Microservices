using Ordering.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Models;
using SendGrid.Helpers.Mail;
using System.Net;
using SendGrid;

namespace Ordering.Infrastructure.Mail;

public class EmailService : IEmailService
{
    public EmailSettings _emailSettings { get; }
    public ILogger<EmailService> _logger { get; }
    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmail(Email email)
    {
        SendGridClient client = new SendGridClient(_emailSettings.APIKey);
        string subject = email.Subject;
        EmailAddress to = new EmailAddress(email.To);
        string emailBody = email.Body;

        EmailAddress from = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName
        };

        SendGridMessage sendMessage =
                MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        Response response = await client.SendEmailAsync(sendMessage);

        if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("Email sent");
            return true;
        }

        _logger.LogError("Email send fail");
        return false;
    }
}
