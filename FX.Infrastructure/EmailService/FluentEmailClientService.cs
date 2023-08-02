using FluentEmail.Core;
using FX.Application.EmailService;
using FX.Domain.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Infrastructure.EmailService
{
    public class FluentEmailClientService : IFluentEmailClient
    {
        private readonly IFluentEmail _fluentEmail;
        public FluentEmailClientService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }
        //public Task SendChangePasswordEmail(string userEmail)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SendForgotPasswordEmail()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task SendOneTimeCodeEmail(object source, OneTimeCodeDTO oneTimeCodeDTO, CancellationToken cancellationToken)
        {
            var emailTemplate = "<p>Your OTP code is <i>@Model.Token</i> .</p>";
            var newEmail = _fluentEmail
                .To(oneTimeCodeDTO.Sender)
                .Subject("[TestMode] One Time Code")
                .UsingTemplate<OneTimeCodeDTO>(emailTemplate, oneTimeCodeDTO);
            await newEmail.SendAsync(cancellationToken);
        }

        //public Task SendResetPasswordEmail()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
