using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class EmailService(IConfiguration configuration)
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings");

        using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
        {
            client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);
            client.Credentials = new System.Net.NetworkCredential(
                smtpSettings["UserName"],
                smtpSettings["Password"]
            );

            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@example.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
    public async Task SendPasswordResetEmailAsync(
        string userEmail,
        string resetLink,
        CancellationToken cancellationToken)
    {
        
        string subject = "Password Reset Request";
        string body = $"Click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

        await SendEmailAsync(userEmail, subject, body);
    }
}