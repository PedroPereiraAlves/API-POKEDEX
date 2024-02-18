using WebApi.Model;

namespace WebApi.Infra
{
    public class HabilidadePokemonRepository : IHabilidadePokemon
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void Add(HabilidadePokemon habilidadePokemon)
        {
        _context.HabilidadePokemon.Add(habilidadePokemon);

        _context.SaveChanges();
        }

        public List<HabilidadePokemon> Get()
            => _context.HabilidadePokemon.ToList();
    }
}