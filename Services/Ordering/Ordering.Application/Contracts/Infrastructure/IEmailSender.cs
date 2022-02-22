using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastructure;

public interface IEmailSender
{
    Task<bool> SendEmail(Email email);
}
