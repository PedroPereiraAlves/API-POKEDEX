using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


class Program
{
    static async Task Main(string[] args)
    {
         // Configurar o serviço de DI
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ConnectionContext>()  // Substitua por seu DbContext
            .AddHttpClient()
            .AddTransient<AtualizacaoPokemonService>()
            .BuildServiceProvider();

        // // Obter o serviço e realizar a atualização
        // var atualizacaoService = serviceProvider.GetRequiredService<AtualizacaoPokemonService>();
        // await atualizacaoService.AtualizarUrlsImagemPokemonsAsync();
    }
}