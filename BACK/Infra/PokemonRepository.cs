
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
        
        public Pokemon? GetPokemon(string nomepokemon)
            => _context.pokemon.FirstOrDefault(x => x.nomepokemon == nomepokemon);

        public List<Pokemon> GetPokemonsByNome(string nomepokemon)
        => _context.pokemon
                    .Where(p => EF.Functions.ILike(p.nomepokemon, "%" + nomepokemon + "%"))
                    .ToList();
        
    }
}