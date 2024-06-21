using Open.Core.Mailing.Models;

namespace Open.Core.Mailing.Abstractions;

public interface IMailService
{
    void SendMail(Mail mail);
    Task SendEmailAsync(Mail mail);
}
