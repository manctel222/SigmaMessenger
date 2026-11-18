using Microsoft.EntityFrameworkCore;
using SigmaMessenger.Models;

namespace SigmaMessenger.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }
}
