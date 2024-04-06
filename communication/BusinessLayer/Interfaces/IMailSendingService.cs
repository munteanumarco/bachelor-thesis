using BusinessLayer.Helpers;

namespace BusinessLayer.Interfaces;

public interface IMailSendingService
{
    Task SendEmailAsync(MailRequest mailRequest);
}