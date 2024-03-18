using Microsoft.EntityFrameworkCore;
using DotNetEnv;

public class CookieSessionContext : DbContext
{
  public DbSet<CookieSession> CookieSessions { get; set; }
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    Env.Load("../.env");
    
    var host = "localhost:5432";

    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var name = Environment.GetEnvironmentVariable("POSTGRES_DB_NAME");
    optionsBuilder.UseNpgsql($"Host={host};Database={name};Username={user};Password={password}");
  }
}