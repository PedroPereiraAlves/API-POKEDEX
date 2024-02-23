namespace WebApi.Model
{
    public interface IPokemon
    {
        void Add(Pokemon pokemon);
        List<Pokemon> Get();
        Pokemon? GetPokemon(string nomepokemon);
        List<Pokemon> GetPokemonsByNome(string nomepokemon);
    }

}
