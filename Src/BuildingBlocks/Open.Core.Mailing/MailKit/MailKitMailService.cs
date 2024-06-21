using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Cryptography;
using Open.Core.Mailing.Abstractions;
using Open.Core.Mailing.Models;
using Open.Core.Mailing.Settings;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
namespace Open.Core.Mailing.MailKit;

public class MailKitMailService(IOptions<MailSettings> options) : IMailService
{
    private readonly MailSettings _mailSettings = options.Value;

    public void SendMail(Mail mail)
    {
        if (mail?.ToList == null || mail.ToList.Count < 1)
            return;
        var (email, smtp) = EmailPrepare(mail);
        smtp.Send(email);
        smtp.Disconnect(true);
        email.Dispose();
        smtp.Dispose();
    }

    public async Task SendEmailAsync(Mail mail)
    {
        if (mail?.ToList == null || mail.ToList.Count < 1)
            return;
        var (email, smtp) = EmailPrepare(mail);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
        email.Dispose();
        smtp.Dispose();
    }

    private (MimeMessage, SmtpClient) EmailPrepare(Mail mail)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));
        email.To.AddRange(mail.ToList);
        if (mail.CcList != null && mail.CcList.Any())
            email.Cc.AddRange(mail.CcList);
        if (mail.BccList != null && mail.BccList.Any())
            email.Bcc.AddRange(mail.BccList);

        email.Subject = mail.Subject;
        if (mail.UnsubscribeLink != null)
            email.Headers.Add(field: "List-Unsubscribe", value: $"<{mail.UnsubscribeLink}>");
        BodyBuilder bodyBuilder = new() { TextBody = mail.TextBody, HtmlBody = mail.HtmlBody };

        if (mail.Attachments != null)
            foreach (MimeEntity? attachment in mail.Attachments)
                if (attachment != null)
                    bodyBuilder.Attachments.Add(attachment);

        email.Body = bodyBuilder.ToMessageBody();
        email.Prepare(EncodingConstraint.SevenBit);

        if (_mailSettings.DkimPrivateKey != null && _mailSettings.DkimSelector != null && _mailSettings.DomainName != null)
        {
            DkimSigner signer =
                new(key: ReadPrivateKeyFromPemEncodedString(), _mailSettings.DomainName, _mailSettings.DkimSelector)
                {
                    HeaderCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                    BodyCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                    AgentOrUserIdentifier = $"@{_mailSettings.DomainName}",
                    QueryMethod = "dns/txt"
                };
            HeaderId[] headers = { HeaderId.From, HeaderId.Subject, HeaderId.To };
            signer.Sign(email, headers);
        }

        SmtpClient smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Server, _mailSettings.Port);
        if (_mailSettings.AuthenticationRequired)
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);

        return (email, smtp);
    }
    
    private AsymmetricKeyParameter ReadPrivateKeyFromPemEncodedString()
    {
        AsymmetricKeyParameter result;
        string pemEncodedKey =
            "-----BEGIN RSA PRIVATE KEY-----\n" + _mailSettings.DkimPrivateKey + "\n-----END RSA PRIVATE KEY-----";
        using (StringReader stringReader = new(pemEncodedKey))
        {
            PemReader pemReader = new(stringReader);
            object? pemObject = pemReader.ReadObject();
            result = ((AsymmetricCipherKeyPair)pemObject).Private;
        }

        return result;
    }
}