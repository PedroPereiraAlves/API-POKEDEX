using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pokedex.Api.Tests;

public class PokemonEndpointTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;

    public PokemonEndpointTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsUiFieldNames()
    {
        var response = await _client.GetAsync("/api/v1/pokemon/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var pokemon = document.RootElement[0];
        Assert.Equal("Bulbasaur", pokemon.GetProperty("nomepokemon").GetString());
        Assert.Equal("https://example.test/1.png", pokemon.GetProperty("url").GetString());
        Assert.Equal(1, pokemon.GetProperty("pokemonid").GetInt32());
        Assert.False(pokemon.TryGetProperty("habilidadePokemon", out _));
    }

    [Fact]
    public async Task GetByNome_ReturnsEmptyListWhenNothingMatches()
    {
        var response = await _client.GetAsync("/api/v1/pokemon/missing");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.Equal(0, document.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task GetByNome_RejectsBlankName()
    {
        var response = await _client.GetAsync("/api/v1/pokemon/%20");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Post_RejectsInvalidSexo()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/pokemon", new
        {
            nomePokemon = "Pikachu",
            sexoPokemon = 2
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Post_RejectsBlankName()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/pokemon", new
        {
            nomePokemon = "   ",
            sexoPokemon = 0
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ReturnsCreatedPokemon()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/pokemon", new
        {
            nomePokemon = "  Charmander  ",
            sexoPokemon = 1
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("Charmander", document.RootElement.GetProperty("nomepokemon").GetString());
        Assert.Equal(1, document.RootElement.GetProperty("sexopokemon").GetInt32());
        Assert.Contains(_factory.Repository.Items, pokemon => pokemon.nomepokemon == "Charmander");
    }

    [Fact]
    public async Task UnhandledException_ReturnsProblemDetailsWithoutConnectionString()
    {
        var response = await _client.GetAsync("/api/v1/pokemon/explode");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("Ocorreu um erro inesperado.", body);
        Assert.DoesNotContain("Password=test", body);
    }

    [Fact]
    public async Task Cors_AllowsConfiguredFrontendOriginOnly()
    {
        using var allowed = new HttpRequestMessage(HttpMethod.Get, "/api/v1/pokemon");
        allowed.Headers.Add("Origin", "http://localhost:3000");
        var allowedResponse = await _client.SendAsync(allowed);
        Assert.Equal(
            "http://localhost:3000",
            allowedResponse.Headers.GetValues("Access-Control-Allow-Origin").Single());

        using var blocked = new HttpRequestMessage(HttpMethod.Get, "/api/v1/pokemon");
        blocked.Headers.Add("Origin", "http://evil.example");
        var blockedResponse = await _client.SendAsync(blocked);
        Assert.False(blockedResponse.Headers.Contains("Access-Control-Allow-Origin"));
    }
}
