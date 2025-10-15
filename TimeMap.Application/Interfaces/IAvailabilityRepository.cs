using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMap.Domain.Entities;

namespace TimeMap.Application.Interfaces;

public interface IAvailabilityRepository
{
    void AddAvailability(Availability availability);

    List<Availability> GetAllAvailabilities();
    List<Availability> GetAvailabilitiesByUser(Guid userId);
}
