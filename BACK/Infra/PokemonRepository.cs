
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Infra
{
    public class PokemonRepository : IPokemon
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void Add(Pokemon pokemon)
        {
            _context.pokemon.Add(pokemon);

            _context.SaveChanges();
        } 
        
        public List<Pokemon> Get()
            => _context.pokemon.ToList();
        
    }
}