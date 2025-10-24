namespace TimeMap.API.Models.Requests
{
    public sealed record CreateUserRequest
    {
        public required string Name { get; init; }
        public required string Password { get; init; }
    }
}