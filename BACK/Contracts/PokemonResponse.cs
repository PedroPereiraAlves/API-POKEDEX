using System.Text.Json.Serialization;
using Pokedex.Api.Models;

namespace Pokedex.Api.Contracts;

public sealed class PokemonResponse
{
    [JsonPropertyName("pokemonid")]
    public int pokemonid { get; init; }

    [JsonPropertyName("nomepokemon")]
    public string nomepokemon { get; init; } = "";

    [JsonPropertyName("sexopokemon")]
    public int sexopokemon { get; init; }

    [JsonPropertyName("url")]
    public string? url { get; init; }

    public static PokemonResponse FromEntity(Pokemon pokemon) => new()
    {
        pokemonid = pokemon.pokemonid,
        nomepokemon = pokemon.nomepokemon,
        sexopokemon = pokemon.sexopokemon,
        url = pokemon.url
    };
}
