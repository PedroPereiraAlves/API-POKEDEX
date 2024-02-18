using Microsoft.EntityFrameworkCore;

namespace WebApi.Infra
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Pokemon> pokemon {get; set;}
        public DbSet<HabilidadePokemon> HabilidadePokemon {get; set;}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost;"+
            "Port=5432;Database=pokedex;"+
            "User Id=postgres;"+
            "Password=995736;");

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<HabilidadePokemon>()
                    .HasKey(h => h.HabilidadeId);// Definindo HabilidadeId como chave primária

                modelBuilder.Entity<Pokemon>()
                    .HasKey(h => h.pokemonid);// Definindo HabilidadeId como chave primária

                modelBuilder.Entity<HabilidadePokemon>()
                    .HasOne(h => h.Pokemon)  // Indicando a relação com a tabela Pokemon
                    .WithMany(p => p.HabilidadePokemon)  // Indicando a relação de um-para-muitos
                    .HasForeignKey(h => h.PokemonId);  // Indicando que PokemonId é a chave estrangeira

                base.OnModelCreating(modelBuilder);
            }
    }

}