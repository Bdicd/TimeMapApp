using Microsoft.AspNetCore.Mvc;
using TimeMap.API.Models.Requests;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityController(
        IAvailabilityRepository availabilityRepository,
        IUserRepository userRepository) : ControllerBase
{
    //TODO FIX NAMING
    [HttpGet]
    public ActionResult<List<Availability>> GetAll()
    {
        return Ok(availabilityRepository.GetAll());
    }

    [HttpPost("{userId}")]
    public ActionResult AddAvailability(Guid userId, [FromBody] AddAvailabilityRequest request)
    {
        var user = userRepository.GetById(userId);

        if (user == null)
            return NotFound($"User with id {userId} not found");
        if (user.Password != request.Password)
            return Unauthorized("Неверный пароль");

        var availability = new Availability
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartTimeUtc = request.StartTimeUtc,
            EndTimeUtc = request.EndTimeUtc
        };

        availabilityRepository.Add(availability);
        return Ok(availability);
    }
}
