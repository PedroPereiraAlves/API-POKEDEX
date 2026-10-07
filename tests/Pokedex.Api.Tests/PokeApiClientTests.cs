using System.Net;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Pokedex.ImageSync.Services;

namespace Pokedex.Api.Tests;

public class PokeApiClientTests
{
    [Fact]
    public async Task ObterUrlImagemAsync_ReadsFrontDefaultSprite()
    {
        var handler = new StubHandler(HttpStatusCode.OK, """
            {"name":"bulbasaur","sprites":{"front_default":"https://example.test/sprite.png"}}
            """);
        using var httpClient = CreateHttpClient(handler);
        var client = new PokeApiClient(httpClient, NullLogger<PokeApiClient>.Instance);

        var url = await client.ObterUrlImagemAsync(1, CancellationToken.None);

        Assert.Equal("https://example.test/sprite.png", url);
        Assert.Equal(new Uri("https://pokeapi.co/api/v2/pokemon/1/"), handler.RequestUri);
        Assert.Equal("application/json", handler.Accept);
    }

    [Fact]
    public async Task ObterUrlImagemAsync_ReturnsNullWhenPokeApiFails()
    {
        var handler = new StubHandler(HttpStatusCode.NotFound, """{"detail":"not found"}""");
        using var httpClient = CreateHttpClient(handler);
        var client = new PokeApiClient(httpClient, NullLogger<PokeApiClient>.Instance);

        var url = await client.ObterUrlImagemAsync(9999, CancellationToken.None);

        Assert.Null(url);
    }

    private static HttpClient CreateHttpClient(StubHandler handler)
        => new(handler)
        {
            BaseAddress = new Uri("https://pokeapi.co/api/v2/")
        };

    private sealed class StubHandler(HttpStatusCode statusCode, string body) : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }
        public string? Accept { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            Accept = request.Headers.Accept.ToString();
            return Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                RequestMessage = request
            });
        }
    }
}
