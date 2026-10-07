using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Models;

namespace Pokedex.Api.Infra;

public class ConnectionContext(DbContextOptions<ConnectionContext> options) : DbContext(options)
{
    public DbSet<Pokemon> pokemon => Set<Pokemon>();
    public DbSet<HabilidadePokemon> HabilidadePokemon => Set<HabilidadePokemon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HabilidadePokemon>()
            .HasKey(h => h.HabilidadeId);

        modelBuilder.Entity<Pokemon>()
            .HasKey(p => p.pokemonid);

        modelBuilder.Entity<HabilidadePokemon>()
            .HasOne(h => h.Pokemon)
            .WithMany(p => p.HabilidadePokemon)
            .HasForeignKey(h => h.PokemonId);

        base.OnModelCreating(modelBuilder);
    }
}
