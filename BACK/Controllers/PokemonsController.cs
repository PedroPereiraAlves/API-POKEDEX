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
        var pokemons = _pokemonRepository.Get().OrderBy(x => x.pokemonid);
        
        return Ok(pokemons);
    }

    [HttpGet("{nomepokemon}",Name = "GetPokemon")]
    public IActionResult GetPokemon(string nomepokemon)
    {
        var pokemonEncontrado = _pokemonRepository.GetPokemonsByNome(nomepokemon);

        if(pokemonEncontrado is null)
            return BadRequest("O pokemon buscado não foi encontrado");

        return Ok(pokemonEncontrado);
                                    
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