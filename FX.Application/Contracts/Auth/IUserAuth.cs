using FX.Application.DataContext;
using FX.DTO.WriteOnly.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.Contracts.Auth
{
    public interface IUserAuth
    {
        Task<string> RegisterUser(RegisterUserDTO registerUser);
        Task<string> RegisterFXAdmin(RegisterUserDTO registerUser);
        Task<string> CreateRoleAndAddUserToRole(string name, ApplicationUser user);
    }
}
