using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Api.Models;
using WebApplication.Shared.User;

namespace WebApplication.Api.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> UserInfoAsync();
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private ApplicationDbContext db;
        public UserService(UserManager<IdentityUser> userManager,ApplicationDbContext context, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            db = context;
        }

        public async Task<UserManagerResponse> UserInfoAsync()
        {
            var email = db.Users.FirstOrDefaultAsync().Result.Email;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false
                };
            }

            var userInfo = new UserManagerResponse();

            var userDevise = await db.Devices.ToArrayAsync();

            Dictionary<string, string> devices = new Dictionary<string, string>();
            foreach (var device in userDevise)
            {
                if (user.Id == device.UserId)
                {
                    devices.Add("DeviceId", device.Id);
                    userInfo = new UserManagerResponse
                    {
                        IsSuccess = true,
                        Message = "Have this device",
                        UserInfo = new Dictionary<string, string>
                        {
                            { "Email", user.Email },
                            { "DeviceId", devices.Values.ToString() }
                        }
                    };
                }
            }

            return userInfo;
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false
                };
            }
         
            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false
                };
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role,  _userManager.GetRolesAsync(user).Result.FirstOrDefault())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(3600),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            var admin = new IdentityRole { Name = "admin" };
            var user = new IdentityRole { Name = "user" };
            
            await _roleManager.CreateAsync(admin);
            await _roleManager.CreateAsync(user);
            if (model == null)
                throw new NullReferenceException("Register model is null.");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password.",
                    IsSuccess = false,
                };

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };
            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(identityUser, "user");

                return new UserManagerResponse
                {
                    Message = "User created successfully.",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}
