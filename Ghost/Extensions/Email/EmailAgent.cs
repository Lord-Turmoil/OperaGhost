using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Tonisoft.AspExtensions.Email;

public class EmailAgent : ICanSetToEmail, ICanSetToName, ICanSetSubject, ICanSetBody, ICanSend
{
    private readonly EmailOptions _options;
    private string _toEmail = null!;
    private string _toName = null!;
    private string _subject = null!;
    private string _body = null!;

    private EmailAgent(EmailOptions options)
    {
        _options = options;
    }

    public ICanSetToName To(string email)
    {
        _toEmail = email;
        return this;
    }

    public ICanSetSubject Of(string name)
    {
        _toName = name;
        return this;
    }

    public ICanSetBody WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public ICanSend WithBody(string body)
    {
        _body = body;
        return this;
    }

    public void Send()
    {
        try
        {
            using var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
            email.To.Add(new MailboxAddress(_toName, _toEmail));
            email.Subject = _subject;
            var builder = new BodyBuilder { HtmlBody = _body };
            email.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            client.Connect(_options.Server, _options.Port, SecureSocketOptions.StartTls);
            client.Authenticate(_options.UserName, _options.Password);
            client.Send(email);
            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Email sending failed", ex);
        }
    }

    public static ICanSetToEmail Draft(EmailOptions options)
    {
        return new EmailAgent(options);
    }
}