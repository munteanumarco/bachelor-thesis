using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.EventContracts;
using DataAccessLayer.Interfaces;
using MassTransit;
using Serilog;

namespace BusinessLayer.RabbitMQ.Consumers;

public class EmergencyReportedEventConsumer : IConsumer<EmergencyReportedEvent>
{
    private readonly IMailSendingService _mailSendingService;
    private readonly Serilog.ILogger _logger;
    private readonly IChatRepository _chatRepository;

    public EmergencyReportedEventConsumer(IMailSendingService mailSendingService, ILogger logger, IChatRepository chatRepository)
    {
        _mailSendingService = mailSendingService;
        _logger = logger;
        _chatRepository = chatRepository;
    }

    public async Task Consume(ConsumeContext<EmergencyReportedEvent> context)
    {
        try
        {
            _logger.Information($"Consuming EmergencyReportedEventConsumer: Id: {context.Message.Id}," +
                                $"Location: {context.Message.Location}," +
                                $" Description: {context.Message.Description}"+
                                $" Latitude: {context.Message.Latitude}"+
                                $" Longitude: {context.Message.Longitude}"+
                                $" Type: {context.Message.Type}"+
                                $" Severity: {context.Message.Severity}"+
                                $" Status: {context.Message.Status}"+
                                $" ReportedBy: {context.Message.ReportedBy}"+
                                $" ReportedByUsername: {context.Message.ReportedByUsername}"+
                                $" ReportedAt: {context.Message.ReportedAt}"+
                                $" UpdatedAt: {context.Message.UpdatedAt}");
            
           await _chatRepository.AddChatEventAsync(context.Message.Id, $"Emergency at {context.Message.Location}");
           
           //TODO: Send email to relevant users about the emergency
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}