using Microsoft.AspNetCore.Mvc;
using TimeMap.Core.Interfaces;
using TimeMap.Core.Services;
using TimeMap.Domain.Entities;

namespace TimeMap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController(IAvailabilityRepository availabilityRepository) : ControllerBase
{
    [HttpGet("best-slots")]
    public ActionResult<List<User>> GetBestSlots([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var analyzer = new ScheduleAnalyzer(availabilityRepository);
        var bestSlots = analyzer.FindBestCommonSlots(start, end);
        return Ok(bestSlots);
    }
}
