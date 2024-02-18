
public class PokemonRepository : IPokemon
{
    private readonly ConnectionContext _context = new ConnectionContext();

    public void Add(Pokemon pokemon)
    {
        _context.Pokemon.Add(pokemon);

        _context.SaveChanges();
    } 
    
    public List<Pokemon> Get()
        => _context.Pokemon.ToList();
    
}