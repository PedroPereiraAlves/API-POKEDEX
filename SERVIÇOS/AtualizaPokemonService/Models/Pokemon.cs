namespace Pokedex.ImageSync.Models;

public class Pokemon
{
    public int pokemonid { get; set; }
    public string nomepokemon { get; set; } = "";

    // 0 homem, 1 mulher
    public int sexopokemon { get; set; }
    public string? url { get; set; }
}
