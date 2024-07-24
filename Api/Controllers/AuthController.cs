using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("sign-up")]
    public IActionResult SignUp()
    {
        if (!Request.Form.TryGetValue("signUpDetails", out var signUpDetailsValue) || !Request.Form.Files.Any())
            return BadRequest("Sign up details are required.");

        var signUpDetails = JsonConvert.DeserializeObject<SignUpDetails>(signUpDetailsValue!)!;
        signUpDetails.CvAttachment = Request.Form.Files[0].OpenReadStream();

        return Ok();
    }
}