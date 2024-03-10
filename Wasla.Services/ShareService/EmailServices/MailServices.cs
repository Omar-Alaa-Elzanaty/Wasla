using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Wasla.Model.Helpers;
using Wasla.Services.Exceptions;
using Microsoft.Extensions.Localization;

namespace Wasla.Services.ShareService.EmailServices
{
    public class MailServices : IMailServices
    {
        private readonly SmtpSettings _settings;
        private readonly IStringLocalizer<MailServices> _localization;
        public MailServices(IOptions<SmtpSettings> settings, IStringLocalizer<MailServices> localization)
        {
            _settings = settings.Value;
            _localization = localization;
        }
        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachments = null)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_settings.Username),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();
            if (attachments is not null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Username));

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Sender, _settings.Port);
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch
            {
                throw new BadHttpRequestException(_localization["errorInSendEmail"].Value);
            }
        }
    }
}
