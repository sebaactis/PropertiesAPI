using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("auth/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;

    public RegisterController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var responseSuccess = ApiResponse<RegisterResponse>.Success(
                method: "POST",
                url: "/auth/register",
                statusCode: StatusCodes.Status200OK,
                message: "User registered successfully",
                data: new RegisterResponse { Email = user.Email, Username = user.UserName }
            );

            return Ok(responseSuccess);
        }

        var response = ApiResponse<RegisterResponse>.Error(
            method: "POST",
            url: "/auth/login",
            statusCode: StatusCodes.Status400BadRequest,
            message: "Error on the register, try again later"
        );

        return BadRequest(response);
    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
