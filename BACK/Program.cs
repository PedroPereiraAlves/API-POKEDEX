using Microsoft.OpenApi.Models;
using WebApi.Model;
using WebApi.Infra;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_POKEDEX", Version = "1.0" });
});

builder.Services.AddTransient<IPokemon, PokemonRepository>();
builder.Services.AddTransient<IHabilidadePokemon, HabilidadePokemonRepository>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_POKEDEX 1.0");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
