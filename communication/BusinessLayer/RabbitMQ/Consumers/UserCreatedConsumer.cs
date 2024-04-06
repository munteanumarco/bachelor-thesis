using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.EventContracts;
using MassTransit;
using Serilog;

namespace BusinessLayer.RabbitMQ.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IMailSendingService _mailSendingService;
    private readonly Serilog.ILogger _logger;

    public UserCreatedConsumer(ILogger logger, IMailSendingService mailSendingService)
    {
        _logger = logger;
        _mailSendingService = mailSendingService;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        try 
        {
            var userCreatedEvent = context.Message;
            _logger.Information("Consuming UserCreatedEvent:" +
                                $"Email: {userCreatedEvent.Email}, " +
                                $"Username: {userCreatedEvent.Username}, " +
                                $"ConfirmationLink: {userCreatedEvent.ConfirmationLink}");
            await _mailSendingService.SendEmailAsync(MailRequest.ConfirmAccount(userCreatedEvent.Email,
                userCreatedEvent.Username,
                userCreatedEvent.ConfirmationLink));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while sending email.");
            throw;
        }
    }
}