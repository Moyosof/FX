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
        Task<string> LoginUser(LoginDTO loginDTO);
        Task<ApplicationUser> GetByUserName(string userName);
        Task<List<string>> GetUserRoles(ApplicationUser user);
        Task<string> ChangePassword(string userEmail, ChangePasswordDTO passwordDTO);
        Task<string> PasswordResetToken(ApplicationUser user);
        Task<string> ResetUserPassword(ApplicationUser user, string token, string newPassword);
    }
}
