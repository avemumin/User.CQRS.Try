using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using User.Infrastructure.Helpers;

namespace User.Infrastructure.Persistence.Services;

public class SmtpEmailSender : IEmailSender
{
  private readonly EmailConfiguration _emailConfiguration;
  public SmtpEmailSender(IOptions<EmailConfiguration> emailConfiguration)
  {
    _emailConfiguration = emailConfiguration.Value;
  }

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Od:", _emailConfiguration.From));
    message.To.Add(new MailboxAddress("Do:", email));
    message.Subject = subject;
    
    message.Body = new TextPart("html") { Text = htmlMessage };

    using var client = new SmtpClient();
    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.StartTls);
    await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
    await client.SendAsync(message);
    await client.DisconnectAsync(true);
  }
}
