namespace TimeMap.Domain.Entities
{
    public class Availability
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime StartTimeUtc { get; init; }
        public DateTime EndTimeUtc { get; init; }
    }

}
