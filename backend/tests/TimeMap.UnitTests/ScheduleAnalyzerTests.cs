using TimeMap.Core.Interfaces;
using TimeMap.Core.Services;
using TimeMap.Domain.Entities;

namespace TimeMap.UnitTests
{
    class InMemoryAvailabilityRepoForAnalyzer : IAvailabilityRepository
    {
        private readonly List<Availability> _items = new();
        public void Add(Availability availability) => _items.Add(availability);
        public List<Availability> GetAll() => _items;
        public List<Availability> GetByUserId(Guid userId) => _items.Where(a => a.UserId == userId).ToList();
    }

    public class ScheduleAnalyzerTests
    {
        [Fact]
        public void NoAvailabilities_ReturnsEmpty()
        {
            var repo = new InMemoryAvailabilityRepoForAnalyzer();
            var analyzer = new ScheduleAnalyzer(repo);
            var res = analyzer.FindBestCommonSlots(DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            Assert.Empty(res);
        }


        [Fact]
        public void TwoUsers_ThresholdOneOverHalf_ReturnsSlots()
        {
            var repo = new InMemoryAvailabilityRepoForAnalyzer();
            var u1 = Guid.NewGuid();
            var u2 = Guid.NewGuid();
            var start = DateTime.UtcNow.Date.AddHours(9);
            repo.Add(new Availability { Id = Guid.NewGuid(), UserId = u1, StartTimeUtc = start, EndTimeUtc = start.AddHours(2) });
            repo.Add(new Availability { Id = Guid.NewGuid(), UserId = u2, StartTimeUtc = start, EndTimeUtc = start.AddHours(2) });
            var analyzer = new ScheduleAnalyzer(repo);
            var res = analyzer.FindBestCommonSlots(start, start.AddHours(2));
            Assert.NotEmpty(res);
            Assert.All(res, s => Assert.Contains(u1, s.UserIds));
            Assert.All(res, s => Assert.Contains(u2, s.UserIds));
        }

        [Fact]
        public void MergeAdjacentSlots_WithSameUsers_Merges()
        {
            var repo = new InMemoryAvailabilityRepoForAnalyzer();
            var u1 = Guid.NewGuid();
            var u2 = Guid.NewGuid();
            var start = DateTime.UtcNow.Date.AddHours(9);
            repo.Add(new Availability { Id = Guid.NewGuid(), UserId = u1, StartTimeUtc = start, EndTimeUtc = start.AddHours(1) });
            repo.Add(new Availability { Id = Guid.NewGuid(), UserId = u2, StartTimeUtc = start, EndTimeUtc = start.AddHours(1) });
            var analyzer = new ScheduleAnalyzer(repo);
            var res = analyzer.FindBestCommonSlots(start, start.AddHours(1));
            Assert.Single(res);
            var slot = res.First();
            Assert.Equal(start, slot.StartUtc);
            Assert.Equal(start.AddHours(1), slot.EndUtc);
        }
    }
}
