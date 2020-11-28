using Auth.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public UserContext db { get; set; }

        public AuthController(UserContext context)
        {
            db = context;
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            var user = AuthenticateUser(request.Email, request.Password);
            if (user != null)
            {

            }

            return Unauthorized();
        }

        private User AuthenticateUser(string email, string password)
        {
            return db.users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
