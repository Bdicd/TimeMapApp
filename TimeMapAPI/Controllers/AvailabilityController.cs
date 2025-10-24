using Microsoft.AspNetCore.Mvc;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityController(
        IAvailabilityRepository availabilityRepository,
        IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Availability>> GetAll()
    {
        return Ok(availabilityRepository.GetAll());
    }

    [HttpPost("{userId}")]
    public ActionResult AddAvailability(Guid userId, string password, DateTime startTimeUtc, DateTime endTimeUtc)
    {
        var user = userRepository.GetById(userId);

        if (user == null)
            return NotFound($"User with id {userId} not found");
        if (user.Password != password)
            return Unauthorized("Неверный пароль");

        var availability = new Availability
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartTimeUtc = startTimeUtc,
            EndTimeUtc = endTimeUtc
        };

        availabilityRepository.Add(availability);
        return Ok(availability);
    }
}
