using Microsoft.EntityFrameworkCore;

public class ConnectionContext : DbContext
{
    public DbSet<Pokemon> pokemon {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost;"+
            "Port=5432;Database=pokedex;"+
            "User Id=postgres;"+
            "Password=995736;");

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                
                modelBuilder.Entity<Pokemon>()
                    .HasKey(h => h.pokemonid);// Definindo HabilidadeId como chave primária

                base.OnModelCreating(modelBuilder);
            }
}