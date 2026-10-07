using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Models;

namespace Pokedex.Api.Infra;

public class HabilidadePokemonRepository(ConnectionContext context) : IHabilidadePokemonRepository
{
    public async Task AddAsync(HabilidadePokemon habilidadePokemon, CancellationToken cancellationToken)
    {
        context.HabilidadePokemon.Add(habilidadePokemon);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<List<HabilidadePokemon>> GetAsync(CancellationToken cancellationToken)
        => context.HabilidadePokemon.AsNoTracking().ToListAsync(cancellationToken);
}
