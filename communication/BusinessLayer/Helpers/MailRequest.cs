using System.Text;
using BusinessLayer.RabbitMQ.EventContracts;
using MassTransit;

namespace BusinessLayer.Helpers;

public class MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public string OptionalParameter { get; set; }

    public MailRequest(string toEmail, string subject, string body, string optionalParameter = null)
    {
        ToEmail = toEmail;
        Subject = subject;
        Body = body;
        OptionalParameter = optionalParameter;
    }

    public static MailRequest ResetPassword(string toEmail, string username, string resetLink)
    {
        StringBuilder mailBody = new StringBuilder();
        mailBody.AppendLine("<h1>Password Reset Requested</h1>");
        mailBody.AppendLine("<p>Dear " + username + ",</p>");
        mailBody.AppendLine("<p>We received a request to reset your password for your Sky Sentinel account.</p>");
        mailBody.AppendLine("<p>If you didn't make this request, just ignore this email. Otherwise, you can reset your password using this link:</p>");
        mailBody.AppendLine("<a href='" + resetLink + "'>Click here to reset your password</a>");
        mailBody.AppendLine("<p>If you can't click the above link, copy and paste the following URL into your browser:</p>");
        mailBody.AppendLine("<p>" + resetLink + "</p>");
        mailBody.AppendLine("<p>Thanks,</p>");
        mailBody.AppendLine("<p>The Sky Sentinel Team</p>");

        return new MailRequest(toEmail, "Password Reset Request", mailBody.ToString());
    }
    public static MailRequest ConfirmAccount(string toEmail, string username, string confirmationLink)
    {
        StringBuilder mailBody = new StringBuilder();
        mailBody.AppendLine("<h1>Account Confirmation Requested</h1>");
        mailBody.AppendLine("<p>Dear " + username + ",</p>");
        mailBody.AppendLine("<p>We sent you a link to confirm your account.</p>");
        mailBody.AppendLine("<p>If you didn't make this request, just ignore this email. Otherwise, you can confirm your account using this link:</p>");
        mailBody.AppendLine("<a href='" + confirmationLink + "'>Click here to confirm your account</a>");
        mailBody.AppendLine("<p>If you can't click the above link, copy and paste the following URL into your browser:</p>");
        mailBody.AppendLine("<p>" + confirmationLink + "</p>");
        mailBody.AppendLine("<p>Thanks,</p>");
        mailBody.AppendLine("<p>The Sky Sentinel Team</p>");

        return new MailRequest(toEmail, "Account Confirmation Request", mailBody.ToString());
    }

    public static MailRequest EmergencyReported(string toEmail, ConsumeContext<EmergencyReportedEvent> context)
    {
        
        StringBuilder mailBody = new StringBuilder();
        mailBody.AppendLine("<h1>New Emergency Reported</h1>");
        mailBody.AppendLine("<p>Dear Team,</p>");
        mailBody.AppendLine("<p>A new emergency event has been reported with the following details:</p>");
        mailBody.AppendLine("<ul>");
        mailBody.AppendLine("<li><strong>ID:</strong> " + context.Message.Id + "</li>");
        mailBody.AppendLine("<li><strong>Location:</strong> " + context.Message.Location + "</li>");
        mailBody.AppendLine("<li><strong>Description:</strong> " + context.Message.Description + "</li>");
        mailBody.AppendLine("<li><strong>Latitude:</strong> " + context.Message.Latitude + "</li>");
        mailBody.AppendLine("<li><strong>Longitude:</strong> " + context.Message.Longitude + "</li>");
        mailBody.AppendLine("<li><strong>Type:</strong> " + context.Message.Type + "</li>");
        mailBody.AppendLine("<li><strong>Severity:</strong> " + context.Message.Severity + "</li>");
        mailBody.AppendLine("<li><strong>Status:</strong> " + context.Message.Status + "</li>");
        mailBody.AppendLine("<li><strong>Reported By:</strong> " + context.Message.ReportedBy + "</li>");
        mailBody.AppendLine("<li><strong>Reported By Username:</strong> " + context.Message.ReportedByUsername + "</li>");
        mailBody.AppendLine("<li><strong>Reported At:</strong> " + context.Message.ReportedAt + "</li>");
        mailBody.AppendLine("<li><strong>Updated At:</strong> " + context.Message.UpdatedAt + "</li>");
        mailBody.AppendLine("</ul>");
        mailBody.AppendLine("<p>Please take the necessary actions.</p>");
        mailBody.AppendLine("<p>Thanks,</p>");
        mailBody.AppendLine("<p>The Sky Sentinel Team</p>");

        return new MailRequest(toEmail, "New Emergency Reported", mailBody.ToString());
    }
}
