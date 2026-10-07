using Pokedex.Api.Models;

namespace Pokedex.Api.Infra;

public interface IHabilidadePokemonRepository
{
    Task AddAsync(HabilidadePokemon habilidadePokemon, CancellationToken cancellationToken);
    Task<List<HabilidadePokemon>> GetAsync(CancellationToken cancellationToken);
}
