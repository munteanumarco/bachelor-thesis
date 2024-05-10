using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;

namespace BusinessLayer.Services;

public class MailSendingService : IMailSendingService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger _logger;

    public MailSendingService(MailSettings mailSettings, ILogger logger)
    {
        _mailSettings = mailSettings;
        _logger = logger;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        try
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder
            {
                HtmlBody = mailRequest.Body
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while sending email.");
            throw;
        }
    }
}