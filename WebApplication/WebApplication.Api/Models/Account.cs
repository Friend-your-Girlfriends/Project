using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Models
{
    public enum Role
    {
        Admin,
        User
    }
    public class Account
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Role[] Roles { get; set; }
    }
}
