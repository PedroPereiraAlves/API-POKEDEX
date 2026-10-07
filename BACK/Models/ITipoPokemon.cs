namespace Pokedex.Api.Models;

public interface ITipoPokemon
{
    void Add(TipoPokemon tipoPokemon);
    List<TipoPokemon> Get();
}
