using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pokedex.ImageSync.Data;

namespace Pokedex.ImageSync.Services;

public sealed class AtualizacaoPokemonService(
    IPokeApiClient pokeApiClient,
    ConnectionContext dbContext,
    ILogger<AtualizacaoPokemonService> logger)
{
    private const int PrimeiraGeracaoLimiteExclusivo = 151;

    public async Task AtualizarUrlsImagemPokemonsAsync(CancellationToken cancellationToken)
    {
        var pokemons = await dbContext.pokemon
            .OrderBy(pokemon => pokemon.pokemonid)
            .ToListAsync(cancellationToken);

        var numero = 1;

        foreach (var pokemon in pokemons)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (pokemon.url != null)
            {
                continue;
            }

            while (numero < PrimeiraGeracaoLimiteExclusivo)
            {
                var urlImagem = await pokeApiClient.ObterUrlImagemAsync(numero, cancellationToken);
                if (urlImagem != null)
                {
                    pokemon.url = urlImagem;
                    await dbContext.SaveChangesAsync(cancellationToken);
                    logger.LogInformation(
                        "Saved sprite for pokemon {PokemonId} from PokeAPI number {Number}",
                        pokemon.pokemonid,
                        numero);
                    numero++;
                    break;
                }

                numero++;
            }
        }
    }
}
