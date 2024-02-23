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

            int i = 1;

        foreach (var pokemon in pokemons)
        {
            if (pokemon.url != null)
            {
                continue; // Se a URL já estiver definida, passe para o próximo Pokémon
            }

            while (i < 151)
            {
                string urlImagem = await ObterUrlImagemPokemonAsync(i);

                if (urlImagem != null)
                {
                    pokemon.url = urlImagem;
                    await _dbContext.SaveChangesAsync();
                    i++; // Atualiza o índice para o próximo Pokémon
                    break; // Sai do loop interno após encontrar a URL
                }

                i++; // Atualiza o índice para verificar o próximo Pokémon
            }
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro na desserialização do JSON: " + ex.Message);
        }
    }

    private async Task<string> ObterUrlImagemPokemonAsync(int numeroPokemon)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse>("https://pokeapi.co/api/v2/pokemon/" + numeroPokemon + "/");

            if (response?.sprites != null)
            {
                return response.sprites.FrontDefault;
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Erro na desserialização do JSON: " + ex.Message);
        }

        return null;
    }
}
