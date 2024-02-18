using Microsoft.EntityFrameworkCore;

public class ConnectionContext : DbContext
{
    public DbSet<Pokemon> Pokemon {get; set;}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql(
        "Server=localhost;"+
        "Port=5432;Database=pokedex;"+
        "User Id=postgres;"+
        "Password=995736;");
    
}