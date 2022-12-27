using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : ControllerBase
{
  private readonly ILogger<PingController> _logger;

  public PingController(ILogger<PingController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public IActionResult Ping()
  {
    return Ok(new { message = "I'm still alive"} );
  }
}