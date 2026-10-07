# API Pokedex

ASP.NET Core API for the Pokémon stored in PostgreSQL, plus a one-shot tool that fills missing sprite URLs from [PokéAPI](https://pokeapi.co/).

The React app in `FRONT/pokedex-front` calls `http://localhost:5195/api/v1/pokemon` and reads `nomepokemon` and `url`.

## Projects

- `BACK` — HTTP API
- `SERVIÇOS/AtualizaPokemonService` — writes `pokemon.url` for rows that do not have one yet
- `FRONT/pokedex-front` — UI (Create React App, port 3000)

Requires the .NET 10 SDK and a PostgreSQL database named `pokedex` with the existing `pokemon` table.

## Configuration

The database password is not stored in the repository. Both .NET projects read the same connection string.

```bash
export ConnectionStrings__Pokedex="Host=localhost;Port=5432;Database=pokedex;Username=postgres;Password=YOUR_PASSWORD"
```

For the API, user secrets work as well:

```bash
dotnet user-secrets set "ConnectionStrings:Pokedex" "Host=localhost;Port=5432;Database=pokedex;Username=postgres;Password=YOUR_PASSWORD" --project BACK/API_POKEDEX.csproj
```

`Cors:AllowedOrigins` defaults to `http://localhost:3000`. The image sync reads `PokeApi:BaseUrl` (default `https://pokeapi.co/api/v2/`).

## Run

```bash
dotnet run --project BACK/API_POKEDEX.csproj
dotnet run --project "SERVIÇOS/AtualizaPokemonService/AtualizaPokemonService.csproj"
```

In Development, Swagger UI is the API site root (`http://localhost:5195`). HTTPS redirection is enabled outside Development so the local UI can keep calling plain HTTP.

## API

| Method | Path | Behavior |
| --- | --- | --- |
| `GET` | `/api/v1/pokemon` | All Pokémon, ordered by `pokemonid` |
| `GET` | `/api/v1/pokemon/{nomepokemon}` | Case-insensitive partial name. Returns `200` and a JSON array, empty when nothing matches |
| `POST` | `/api/v1/pokemon` | Body `{ "nomePokemon": "Pikachu", "sexoPokemon": 0 }`. `sexoPokemon` is `0` or `1`. Returns `201` |

Invalid input returns `400` with `application/problem+json`. Unexpected failures return the same problem-details shape and do not include exception text outside Development.
