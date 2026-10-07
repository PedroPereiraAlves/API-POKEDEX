using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Errors;
using Pokedex.Api.Infra;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Pokedex");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'Pokedex' is not configured. Set ConnectionStrings:Pokedex, the ConnectionStrings__Pokedex environment variable, or user secrets.");
}

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?
    .Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray() ?? [];

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "API_POKEDEX",
        Version = "1.0"
    });
});

builder.Services.AddDbContext<ConnectionContext>(options =>
    options.UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure()));

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<IHabilidadePokemonRepository, HabilidadePokemonRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        if (allowedOrigins.Length == 0)
        {
            return;
        }

        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (allowedOrigins.Length == 0)
{
    app.Logger.LogWarning("Cors:AllowedOrigins is empty. Browser clients will be blocked.");
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API_POKEDEX 1.0");
        options.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
