using System.Net;
using System.Net.Mail;
using LOGIN.Services.Interfaces;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string token)
    {
        try
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = smtpSettings["SmtpServer"];
            var smtpPort = smtpSettings["SmtpPort"];
            var smtpUsername = smtpSettings["SmtpUsername"];
            var smtpPassword = smtpSettings["SmtpPassword"];

            if (string.IsNullOrWhiteSpace(smtpServer) || string.IsNullOrWhiteSpace(smtpPort) ||
                string.IsNullOrWhiteSpace(smtpUsername) || string.IsNullOrWhiteSpace(smtpPassword))
            {
                throw new InvalidOperationException("SMTP settings are not configured properly.");
            }

            using (var smtpClient = new SmtpClient
            {
                Host = smtpServer,
                Port = int.Parse(smtpPort),
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            })
            {
                var body = $"Su codigo de restablecimiento de contraseña es: {token}";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }

            _logger.LogInformation($"Email sent to {to} with subject {subject}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send email to {to}. Exception: {ex.Message}");
            throw;
        }
    }

}
