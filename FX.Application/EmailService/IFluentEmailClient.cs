using FX.DTO.WriteOnly.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.EmailService
{
    public interface IFluentEmailClient
    {
        Task SendConfirmAccountEmail(RegisterUserDTO registerUserDTO);
    }
}
