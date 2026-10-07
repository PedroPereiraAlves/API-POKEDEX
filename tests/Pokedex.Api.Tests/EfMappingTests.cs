using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Infra;
using Pokedex.Api.Models;

namespace Pokedex.Api.Tests;

public class EfMappingTests
{
    [Fact]
    public void Pokemon_KeepsExistingTableAndColumnNames()
    {
        var options = new DbContextOptionsBuilder<ConnectionContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=pokedex;Username=postgres;Password=test")
            .Options;

        using var context = new ConnectionContext(options);
        var pokemon = context.Model.FindEntityType(typeof(Pokemon));
        var habilidade = context.Model.FindEntityType(typeof(HabilidadePokemon));

        Assert.NotNull(pokemon);
        Assert.Equal("pokemon", pokemon.GetTableName());
        Assert.Equal("pokemonid", pokemon.FindProperty(nameof(Pokemon.pokemonid))!.GetColumnName());
        Assert.Equal("nomepokemon", pokemon.FindProperty(nameof(Pokemon.nomepokemon))!.GetColumnName());
        Assert.Equal("sexopokemon", pokemon.FindProperty(nameof(Pokemon.sexopokemon))!.GetColumnName());
        Assert.Equal("url", pokemon.FindProperty(nameof(Pokemon.url))!.GetColumnName());

        Assert.NotNull(habilidade);
        Assert.Equal("HabilidadePokemon", habilidade.GetTableName());
        Assert.Equal("HabilidadeId", habilidade.FindPrimaryKey()!.Properties.Single().Name);
    }
}
