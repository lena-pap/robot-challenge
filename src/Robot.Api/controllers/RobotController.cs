using Microsoft.AspNetCore.Mvc;
using Robot.Application;

namespace Robot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RobotController(MotionService motionService) : ControllerBase
{
    [HttpPost("move")]
    [ProducesResponseType(typeof(RobotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Move([FromBody] RobotRequest request)
    {
        try
        {
            var result = motionService.ExecuteMotion(request);
            
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred processing the robot movement.");
        }
    }
}