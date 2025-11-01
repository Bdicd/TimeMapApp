using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMap.Domain.Entities;

public class Room
{
    public required string Name { get; set; }
    public required Guid RoomId { get; set; }
    public List<User> Users { get; set; }
}
