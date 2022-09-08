using LastRoom.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LastRoom.Api;

public class LastRoomDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; } = default!;
    
    public LastRoomDbContext(DbContextOptions options) : base(options) {}
}