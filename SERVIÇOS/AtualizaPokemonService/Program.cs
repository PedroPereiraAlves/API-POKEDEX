using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokedex.ImageSync.Data;
using Pokedex.ImageSync.Services;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Pokedex");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'Pokedex' is not configured. Set ConnectionStrings:Pokedex or the ConnectionStrings__Pokedex environment variable.");
}

var pokeApiBaseUrl = builder.Configuration["PokeApi:BaseUrl"] ?? "https://pokeapi.co/api/v2/";
if (!Uri.TryCreate(pokeApiBaseUrl, UriKind.Absolute, out var pokeApiUri))
{
    throw new InvalidOperationException("PokeApi:BaseUrl must be an absolute URL.");
}

if (!pokeApiUri.AbsoluteUri.EndsWith('/'))
{
    pokeApiUri = new Uri(pokeApiUri.AbsoluteUri + "/");
}

builder.Services.AddDbContext<ConnectionContext>(options =>
    options.UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure()));

builder.Services.AddHttpClient<IPokeApiClient, PokeApiClient>(client =>
{
    client.BaseAddress = pokeApiUri;
    client.Timeout = Timeout.InfiniteTimeSpan;
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("PokedexImageSync/1.0");
})
.AddStandardResilienceHandler();

builder.Services.AddScoped<AtualizacaoPokemonService>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<AtualizacaoPokemonService>();

using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellation.Cancel();
};

await service.AtualizarUrlsImagemPokemonsAsync(cancellation.Token);
