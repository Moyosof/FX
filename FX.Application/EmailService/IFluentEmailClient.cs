using FX.Domain.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.EmailService
{
    public interface IFluentEmailClient
    {
        //Task SendChangePasswordEmail(string userEmail);
        //Task SendForgotPasswordEmail();
        //Task SendResetPasswordEmail();

        /// <summary>
        /// Sends email to the sender once the event is trigered
        /// </summary>
        /// <param name="source"></param>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendOneTimeCodeEmail(Object source, OneTimeCodeDTO oneTimeCodeDTO, CancellationToken cancellationToken);
    }
}
