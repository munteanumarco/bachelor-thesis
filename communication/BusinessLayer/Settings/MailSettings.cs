namespace BusinessLayer.Settings;

public class MailSettings
{
    public string Mail { get; set; } = "mail@example.com";
    public string DisplayName { get; set; } = "display name";
    public string Password { get; set; } = "password";
    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;

}