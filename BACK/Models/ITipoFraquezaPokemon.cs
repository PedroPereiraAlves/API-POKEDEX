namespace Pokedex.Api.Models;

public interface ITipoFraquezaPokemon
{
    void Add(TipoFraquezaPokemon tipoFraquezaPokemon);
    List<TipoFraquezaPokemon> Get();
}
