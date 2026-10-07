using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pokedex.Api.Infra;
using Pokedex.Api.Models;

namespace Pokedex.Api.Tests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public FakePokemonRepository Repository { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting(
            "ConnectionStrings:Pokedex",
            "Host=localhost;Port=5432;Database=pokedex;Username=postgres;Password=test");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IPokemonRepository>();
            services.AddSingleton(Repository);
            services.AddSingleton<IPokemonRepository>(serviceProvider =>
                serviceProvider.GetRequiredService<FakePokemonRepository>());
        });
    }
}

public sealed class FakePokemonRepository : IPokemonRepository
{
    public List<Pokemon> Items { get; } =
    [
        new Pokemon("Bulbasaur", 0)
        {
            pokemonid = 1,
            url = "https://example.test/1.png"
        }
    ];

    public Task AddAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        pokemon.pokemonid = Items.Max(item => item.pokemonid) + 1;
        Items.Add(pokemon);
        return Task.CompletedTask;
    }

    public Task<List<Pokemon>> GetAsync(CancellationToken cancellationToken)
        => Task.FromResult(Items.OrderBy(pokemon => pokemon.pokemonid).ToList());

    public Task<List<Pokemon>> GetByNomeAsync(string nomepokemon, CancellationToken cancellationToken)
    {
        if (nomepokemon.Contains("explode", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("database exploded");
        }

        var matches = Items
            .Where(pokemon => pokemon.nomepokemon.Contains(nomepokemon, StringComparison.OrdinalIgnoreCase))
            .OrderBy(pokemon => pokemon.pokemonid)
            .ToList();

        return Task.FromResult(matches);
    }
}
