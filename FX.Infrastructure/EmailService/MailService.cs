using Microsoft.Extensions.Options;
using FX.Application.EmailService;
using FX.Application.EmailService.Entities;

namespace FX.Infrastructure.EmailService
{
    /// <summary>
    /// This class implements the IMailService interface
    /// </summary>
    public sealed class MailService : IMailService
    {
        private readonly IOptions<SendGridKey> sendgridApiKey;
        private readonly IOptions<SendGridFrom> sendGridClient;
        private readonly GmailOptions gmailOptions;

        public MailService(IOptions<SendGridKey> apikey, IOptions<SendGridFrom> sendGridClient, IOptions<GmailOptions> gmailOptions)
        {
            sendgridApiKey = apikey;
            this.sendGridClient = sendGridClient;
            this.gmailOptions = gmailOptions.Value;
        }

        public ISendGridClient SendGridClient => new SendGridClient(sendgridApiKey, sendGridClient);

        public IGmailClient GmailClient => new GmailClient(gmailOptions);
    }
}