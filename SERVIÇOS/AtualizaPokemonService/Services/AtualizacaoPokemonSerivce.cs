using System.Net.Http.Json;

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
        var pokemons = _dbContext.pokemon.ToList();

        foreach (var pokemon in pokemons)
        {
            string urlImagem = await ObterUrlImagemPokemonAsync(pokemon.nomepokemon);

            if (urlImagem != null)
                pokemon.url = urlImagem;
            
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task<string> ObterUrlImagemPokemonAsync(string nomePokemon)
    {
        for(var i = 1; i < 151; i++)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse>("https://pokeapi.co/api/v2/pokemon/" + i + "/");

            return response?.Sprites?.FrontDefault;
        }

        return nomePokemon;
    }
}
