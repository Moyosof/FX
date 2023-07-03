using FluentEmail.Core;
using FX.Application.EmailService;
using FX.DTO.WriteOnly.AuthDTO;
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

        public async Task SendConfirmAccountEmail(RegisterUserDTO registerUserDTO)
        {
            var emailTemplate = $"<p>Hello {registerUserDTO.FirstName}</p>, <p>your account has been created</p>.";
            var newEmail = _fluentEmail
                .To(registerUserDTO.EmailAddress)
                .Subject("Account Confirmed")
                .UsingTemplate<RegisterUserDTO>(emailTemplate, registerUserDTO);
            await newEmail.SendAsync();
        }
    }
}
