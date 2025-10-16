namespace TimeMap.Domain.Entities
{
    public class Availability
    {
        public required Guid Id { get; init; }
        public required Guid UserId { get; init; }
        public required DateTime StartTimeUtc { get; init; }
        public required DateTime EndTimeUtc { get; init; }
    }

}
