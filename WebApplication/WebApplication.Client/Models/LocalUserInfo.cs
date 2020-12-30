using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Client.Models
{
    public class LocalUserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Roles { get; set; }
    }
}
