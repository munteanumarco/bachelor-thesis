using System.Text;

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
}
