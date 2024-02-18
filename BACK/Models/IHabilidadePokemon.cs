namespace WebApi.Model
{
    public interface IHabilidadePokemon
    {
        void Add(HabilidadePokemon habilidadePokemon);
        List<HabilidadePokemon> Get();
    }
}