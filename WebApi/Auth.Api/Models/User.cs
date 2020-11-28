using System;

namespace Auth.Api.Models
{
    public enum Role
    {
        Admin,
        User
    }
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role[] Roles { get; set; }
    }
}
