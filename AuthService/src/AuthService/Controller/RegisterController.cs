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
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
