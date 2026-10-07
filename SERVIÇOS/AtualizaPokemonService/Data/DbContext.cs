using Microsoft.EntityFrameworkCore;
using Pokedex.ImageSync.Models;

namespace Pokedex.ImageSync.Data;

public class ConnectionContext(DbContextOptions<ConnectionContext> options) : DbContext(options)
{
    public DbSet<Pokemon> pokemon => Set<Pokemon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>()
            .HasKey(pokemon => pokemon.pokemonid);

        base.OnModelCreating(modelBuilder);
    }
}
