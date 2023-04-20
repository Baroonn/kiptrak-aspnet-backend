using Domain.Models;

namespace Service.ServiceContracts;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}