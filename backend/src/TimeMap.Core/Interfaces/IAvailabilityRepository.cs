using TimeMap.Domain.Entities;

namespace TimeMap.Core.Interfaces;

public interface IAvailabilityRepository
{
    void Add(Availability availability);
    List<Availability> GetAll();
    List<Availability> GetByUserId(Guid userId);
}
