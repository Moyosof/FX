using FX.Application.Contracts.Auth;
using FX.Application.DataContext;
using FX.Domain.Enum;
using FX.DTO.OAuth;
using FX.DTO.WriteOnly.AuthDTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Infrastructure.Contracts.Auth
{
    public class UserAuth : IUserAuth
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IWebHostEnvironment env;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAuth(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IWebHostEnvironment _env,
            RoleManager<IdentityRole> roleManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            env = _env;
            _roleManager = roleManager;
        }


        public async Task<string> RegisterFXAdmin(RegisterUserDTO registerUser)
        {
            return await Register(registerUser, Roles.Admin);
        }
       

        public async Task<string> RegisterUser(RegisterUserDTO registerUser)
        {
            return await Register(registerUser, Roles.User);
        }


        public async Task<string> LoginUser(LoginDTO loginDTO)
        {
            //var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password,false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return string.Empty;
            }
            return "Invalid Login credentials";
        }
        //public async Task<AuthResponse> LoginUser(LoginDTO loginDTO)
        //{
        //    var result = await _signInManager.CheckPasswordSignInAsync(, loginDTO.Password, lockoutOnFailure: false) .PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, lockoutOnFailure: false);

        //    if (result.Succeeded)
        //    {
        //        return new AuthResponse();
        //    }
        //    else if (result.IsLockedOut)
        //    {
        //        throw new ApplicationException("Account locked. Please contact support.");
        //    }
        //    else if (result.RequiresTwoFactor)
        //    {
        //        throw new ApplicationException("Two-factor authentication is required.");
        //    }
        //    else
        //    {
        //        throw new ApplicationException("Invalid login credentials.");
        //    }
        //}

        /// <summary>
        /// Find User by Username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetByUserName(string userName) => await _userManager.FindByNameAsync(userName);
        

        public async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }


        public async Task<string> ChangePassword(string userEmail, ChangePasswordDTO passwordDTO)
        {
            var currentUser = await _userManager.FindByEmailAsync(userEmail);

            var changePassword = await _userManager.ChangePasswordAsync(currentUser, passwordDTO.CurrentPassword, passwordDTO.NewPassword);
            if (changePassword.Succeeded)
            {
                return string.Empty;
            }

            else
            {
                string error = changePassword.Errors.First().Description;
                if (error == "passwordMismatch")
                {
                    return "Current password os incorrect";
                }
                else
                {
                    return "We are not able to change your password right now. Please contact admin";
                }
            }
        }

        /// <summary>
        /// Generate password reset token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> PasswordResetToken(ApplicationUser user) => await _userManager.GeneratePasswordResetTokenAsync(user);
        
        /// <summary>
        /// Reset the user password and making sure the token is valid for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<string> ResetUserPassword(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return string.Empty;
            }
            return result.Errors.First().Description;
        }

        #region General Method For Registering User
        private async Task<string> Register(RegisterUserDTO registerUserDTO, Roles role)
        {
            ApplicationUser user = new ApplicationUser();

            user = new ApplicationUser { Email = registerUserDTO.EmailAddress, PhoneNumber = registerUserDTO.PhoneNumber, PhoneNumberConfirmed = false, EmailConfirmed = true, UserName = registerUserDTO.EmailAddress, Firstname = registerUserDTO.FirstName.ToUpper(), Lastname = registerUserDTO.LastName.ToUpper() };

            // Creates a new user and password hash
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            if (result.Succeeded)
            {
                string roleName = role.ToString();
                return await CreateRoleAndAddUserToRole(roleName, user);
            }
            return result.Errors.First().Description;
        }
        #endregion

        #region User Role Creation Method
        public async Task<string> CreateRoleAndAddUserToRole(string name, ApplicationUser user)
        {
            bool checkedRole = await _roleManager.RoleExistsAsync(name);
            if (!checkedRole)
            {
                IdentityResult role = await _roleManager.CreateAsync(new IdentityRole(name));
                if (!role.Succeeded)
                {
                    return role.Errors.First().Description;
                }
            }

            IdentityResult AddUserToRole = await _userManager.AddToRoleAsync(user, name);
            if(AddUserToRole.Succeeded)
            {
                return string.Empty;
            }

            return AddUserToRole.Errors.First().Description;
        }

        #endregion
    }
}
