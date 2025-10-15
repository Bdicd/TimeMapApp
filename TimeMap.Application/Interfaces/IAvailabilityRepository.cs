using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMap.Domain.Entities;

namespace TimeMap.Application.Interfaces;

internal interface IAvailabilityRepository
{
    void AddAvailability(List<Availability> availabilities);

    List<Availability> GetAllAvailabilities();
    List<Availability> GetAvailabilitiesByUser(User user);
}
