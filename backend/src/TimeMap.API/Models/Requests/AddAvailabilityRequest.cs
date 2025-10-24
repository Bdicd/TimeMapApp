namespace TimeMap.API.Models.Requests
{
    public sealed record AddAvailabilityRequest
    {
        public required string Password { get; init; }
        public required DateTime StartTimeUtc { get; init; }
        public required DateTime EndTimeUtc { get; init; }
    }
}
