using Pokedex.Api.Models;

namespace Pokedex.Api.Infra;

public interface IPokemonRepository
{
    Task AddAsync(Pokemon pokemon, CancellationToken cancellationToken);
    Task<List<Pokemon>> GetAsync(CancellationToken cancellationToken);
    Task<List<Pokemon>> GetByNomeAsync(string nomepokemon, CancellationToken cancellationToken);
}
