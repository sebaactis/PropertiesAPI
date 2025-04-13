using Microsoft.AspNetCore.Identity;

namespace AuthService.Models
{
    public class LoginResponse
    {
        public string User { get; set; }
        public string Token { get; set; }
    }
}
