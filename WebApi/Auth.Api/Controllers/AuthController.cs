using Auth.Api.Models;
using Auth.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IOptions<AuthOptions> options;

        //public UserContext db { get; set; }
        public AuthController(IOptions<AuthOptions> options)
        {
            this.options = options;
        }
        private List<User> Users => new List<User>
        {
            new User()
            {
                Id = Guid.Parse("8e7eb047-e1e0-4801-ba41-f8360a9e64a7"),
                Email = "user@email.com",
                Password = "user",
                Roles = new Role[] { Role.User }
            },
            new User()
            {
                Id = Guid.Parse("ede2fe5d-3a38-41a0-9617-842ae7417315"),
                Email = "admin@email.com",
                Password = "admin",
                Roles = new Role[] { Role.Admin }
            },
        };
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            var user = AuthenticateUser(request.Email, request.Password);

            if (user != null)
            {
                var token = GenerateJWT(user);

                return Ok(new
                {
                    access_token = token
                });
            }

            return Unauthorized();
        }

        private User AuthenticateUser(string email, string password)
        {
            return Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        private string GenerateJWT(User user)
        {
            var authParams = options.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: authParams.Issuer,
                audience: authParams.Audience, 
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                claims: claims,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
