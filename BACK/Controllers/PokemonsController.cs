using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.ViewModel;

[ApiController]
[Route("api/v1/pokemon")]
public class PokemonsController : ControllerBase
{
    private readonly IPokemon _pokemonRepository;

    public PokemonsController(IPokemon pokemonRepository)
    {
        _pokemonRepository = pokemonRepository;
    }

    [HttpGet]
    public IActionResult GetAllPokemons()
    {
        var pokemons = _pokemonRepository.Get();
        
        return Ok(pokemons);
    }

    [HttpPost]
    public IActionResult AdicionarPokemon(PokemonAddViewModel pokemon)
    {
        var novoPokemon = new Pokemon(
            pokemon.NomePokemon,
            pokemon.SexoPokemon
        );

        _pokemonRepository.Add(novoPokemon);
        
        return Ok();
    }
}