using Microsoft.EntityFrameworkCore;

namespace Ghost.Models;

public class PrimaryDbContext : DbContext
{
    public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Letter> Letters { get; set; }
}