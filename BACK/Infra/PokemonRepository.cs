using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Models;

namespace Pokedex.Api.Infra;

public class PokemonRepository(ConnectionContext context) : IPokemonRepository
{
    public async Task AddAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        context.pokemon.Add(pokemon);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Pokemon>> GetAsync(CancellationToken cancellationToken)
        => context.pokemon
            .AsNoTracking()
            .OrderBy(pokemon => pokemon.pokemonid)
            .ToListAsync(cancellationToken);

    public Task<List<Pokemon>> GetByNomeAsync(string nomepokemon, CancellationToken cancellationToken)
        => context.pokemon
            .AsNoTracking()
            .Where(pokemon => EF.Functions.ILike(pokemon.nomepokemon, LikePatterns.Contains(nomepokemon), "\\"))
            .OrderBy(pokemon => pokemon.pokemonid)
            .ToListAsync(cancellationToken);
}
