using System.ComponentModel.DataAnnotations.Schema;

[Table("Pokemon")]
public class Pokemon
{
    public int PokemonId {get; set;}
    public string NomePokemon {get; set;}
    public int SexoPokemon {get; set;}//0 homen 1 mulher
}