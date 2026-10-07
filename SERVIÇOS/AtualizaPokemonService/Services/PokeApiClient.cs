using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Pokedex.ImageSync.Models;

namespace Pokedex.ImageSync.Services;

public interface IPokeApiClient
{
    Task<string?> ObterUrlImagemAsync(int numeroPokemon, CancellationToken cancellationToken);
}

public sealed class PokeApiClient(HttpClient httpClient, ILogger<PokeApiClient> logger) : IPokeApiClient
{
    public async Task<string?> ObterUrlImagemAsync(int numeroPokemon, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"pokemon/{numeroPokemon}/");
            request.Headers.Accept.ParseAdd("application/json");
            using var response = await httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "PokeAPI returned {StatusCode} for pokemon {Number}",
                    (int)response.StatusCode,
                    numeroPokemon);
                return null;
            }

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken);
            return payload?.Sprites?.FrontDefault;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or TaskCanceledException)
        {
            logger.LogWarning(exception, "PokeAPI request failed for pokemon {Number}", numeroPokemon);
            return null;
        }
    }
}
