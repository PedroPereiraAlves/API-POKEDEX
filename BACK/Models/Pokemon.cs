using System.ComponentModel.DataAnnotations.Schema;

[Table("pokemon")]
public class Pokemon
{
    public int pokemonid {get; set;}
    public string nomepokemon {get; set;}
    public int sexopokemon {get; set;}//0 homen 1 mulher


    #region relacionamentos
    public IEnumerable<HabilidadePokemon> HabilidadePokemon {get; set;}

    #endregion relacionamentos


    #region construtores

     public Pokemon() { }
     
    public Pokemon(string nomePokemon, int sexoPokemon)
    {
        nomepokemon = nomePokemon;
        sexopokemon = sexoPokemon;
    }

    #endregion construtores
}