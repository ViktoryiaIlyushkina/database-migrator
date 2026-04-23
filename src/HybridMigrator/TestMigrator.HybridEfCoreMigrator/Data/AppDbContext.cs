using Microsoft.EntityFrameworkCore;
using TestMigrator.HybridShared.Models;

namespace TestMigrator.HybridEfCoreMigrator.Data;
public class AppDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=my_test_db;Username=postgres;Password=yourpassword");
    }
}
