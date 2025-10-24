namespace TimeMap.Domain.Models;

//TODO почему это лучший слот и не record?
public class BestSlot
{
    public required DateTime StartUtc { get; init; }
    public required DateTime EndUtc { get; init; }
    public List<Guid> UserIds { get; init; } = [];
}
