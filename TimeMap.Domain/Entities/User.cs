namespace TimeMap.Domain.Entities;

public class User
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public List<Availability> Availabilities { get; init; } = [];
}