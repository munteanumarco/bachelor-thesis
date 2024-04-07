using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.EventContracts;
using MassTransit;
using Serilog;

namespace BusinessLayer.RabbitMQ.Consumers;

public class ResetPasswordEventConsumer : IConsumer<ResetPasswordEvent>
{
    private readonly IMailSendingService _mailSendingService;
    private readonly Serilog.ILogger _logger;

    public ResetPasswordEventConsumer(IMailSendingService mailSendingService, ILogger logger)
    {
        _mailSendingService = mailSendingService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ResetPasswordEvent> context)
    {
        try
        {
            _logger.Information($"Consuming ResetPasswordEvent: Email: {context.Message.Email}," +
                                                $"Username: {context.Message.Username}," +
                                                $" ResetLink: {context.Message.ResetLink}");
            await _mailSendingService.SendEmailAsync(MailRequest.ResetPassword(context.Message.Email,
                context.Message.Username,
                context.Message.ResetLink));
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}