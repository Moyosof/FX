using FX.Application.Contracts.Auth;
using FX.Application.DataContext;
using FX.Domain.Enum;
using FX.DTO.WriteOnly.AuthDTO;
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
        

        #region General Method For Registering User
        private async Task<string> Register(RegisterUserDTO registerUserDTO, Roles role)
        {
            ApplicationUser user = new ApplicationUser();

            user = new ApplicationUser { Email = registerUserDTO.EmailAddress, PhoneNumber = registerUserDTO.PhoneNumber, PhoneNumberConfirmed = false, EmailConfirmed = false, UserName = registerUserDTO.EmailAddress, Firstname = registerUserDTO.FirstName.ToUpper(), Lastname = registerUserDTO.LastName.ToUpper() };

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
