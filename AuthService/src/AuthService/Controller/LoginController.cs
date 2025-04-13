using AuthService.Models;
using AuthService.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("auth/[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly JwtManager _jwtManager;

    public LoginController(UserManager<IdentityUser> userManager, IConfiguration configuration, JwtManager jwtManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtManager = jwtManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = _jwtManager.GenerateToken(model.Email);

            var responseSuccess = ApiResponse<LoginResponse>.Success(
                    method: "POST",
                    url: "/auth/login",
                    statusCode: StatusCodes.Status200OK,
                    message: "Login successfully",
                    data: new LoginResponse { Token = token, User = user.UserName }
                );

            return Ok(responseSuccess);
        }

        var response = ApiResponse<LoginResponse>.Error(
            method: "POST",
            url: "/auth/login",
            statusCode: StatusCodes.Status401Unauthorized,
            message: "Username o password invalid"
        );

        return Unauthorized(response);
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}