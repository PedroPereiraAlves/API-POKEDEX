using System;
using System.Net.Http.Json;
using System.Text.Json;

public class AtualizacaoPokemonService
{
    private readonly HttpClient _httpClient;
    private readonly ConnectionContext _dbContext;

    public AtualizacaoPokemonService(HttpClient httpClient, ConnectionContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task AtualizarUrlsImagemPokemonsAsync()
    {
        try
        {

        var pokemons = _dbContext.pokemon.ToList().OrderBy(x => x.pokemonid);

        foreach (var pokemon in pokemons)
        {
            string urlImagem = await ObterUrlImagemPokemonAsync(pokemon.nomepokemon);

            if (urlImagem != null)
                pokemon.url = urlImagem;

            await _dbContext.SaveChangesAsync();
        }
        
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro na desserialização do JSON: " + ex.Message);
        }
    }

    private async Task<string> ObterUrlImagemPokemonAsync(string nomePokemon)
    {
        for(var i = 1; i < 151; i++)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>("https://pokeapi.co/api/v2/pokemon/" + i + "/");

                if (response?.sprites != null)
                {
                    return response.sprites.FrontDefault;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Erro na desserialização do JSON: " + ex.Message);
            }

            
        }

        return nomePokemon;
    }
}
