using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecureTestController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly()
    {
        return Ok("Welcome Admin");
    }

    [HttpGet("manager")]
    [Authorize(Roles = "Manager,Admin")]
    public IActionResult ManagerOrAdmin()
    {
        return Ok("Welcome Manager or Admin");
    }

    [HttpGet("user")]
    [Authorize(Roles = "User,Manager,Admin")]
    public IActionResult AnyUser()
    {
        return Ok("Welcome Authenticated User");
    }
}
