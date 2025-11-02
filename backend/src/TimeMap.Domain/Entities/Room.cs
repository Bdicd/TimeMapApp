using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMap.Domain.Entities;

public class Room
{
    public required string Name { get; init; }
    public required string Password { get; init; }
    public required Guid Id { get; init; }
    public List<User> Users { get; set; } = [];
}
