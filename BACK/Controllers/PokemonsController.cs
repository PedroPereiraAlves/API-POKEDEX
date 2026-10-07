using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Contracts;
using Pokedex.Api.Infra;
using Pokedex.Api.Models;

namespace Pokedex.Api.Controllers;

[ApiController]
[Route("api/v1/pokemon")]
public class PokemonsController(IPokemonRepository pokemonRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PokemonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PokemonResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var pokemons = await pokemonRepository.GetAsync(cancellationToken);
        return Ok(pokemons.Select(PokemonResponse.FromEntity).ToList());
    }

    [HttpGet("{nomepokemon}")]
    [ProducesResponseType(typeof(IReadOnlyList<PokemonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<PokemonResponse>>> GetByNome(
        string nomepokemon,
        CancellationToken cancellationToken)
    {
        var nome = nomepokemon.Trim();
        if (nome.Length == 0)
        {
            ModelState.AddModelError(nameof(nomepokemon), "O nome do pokemon é obrigatório.");
            return ValidationProblem(ModelState);
        }

        if (nome.Length > 100)
        {
            ModelState.AddModelError(nameof(nomepokemon), "O nome do pokemon deve ter no máximo 100 caracteres.");
            return ValidationProblem(ModelState);
        }

        // The UI renders this payload as a list, including when the search matches nothing.
        var pokemons = await pokemonRepository.GetByNomeAsync(nome, cancellationToken);
        return Ok(pokemons.Select(PokemonResponse.FromEntity).ToList());
    }

    [HttpPost]
    [ProducesResponseType(typeof(PokemonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PokemonResponse>> AdicionarPokemon(
        PokemonAddViewModel pokemon,
        CancellationToken cancellationToken)
    {
        var novoPokemon = new Pokemon(pokemon.NomePokemon.Trim(), pokemon.SexoPokemon);
        await pokemonRepository.AddAsync(novoPokemon, cancellationToken);

        var response = PokemonResponse.FromEntity(novoPokemon);
        return CreatedAtAction(nameof(GetByNome), new { nomepokemon = response.nomepokemon }, response);
    }
}
