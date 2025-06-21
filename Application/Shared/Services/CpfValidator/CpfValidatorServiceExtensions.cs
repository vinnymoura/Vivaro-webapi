// filepath: e:\projetos\VivaroBravea\vivaro-webapi\Application\Shared\Services\CpfValidator\CpfValidatorServiceExtensions.cs
using Application.Shared.Interfaces.CpfValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Application.Shared.Services.CpfValidator
{
    public static class CpfValidatorServiceExtensions
    {
        public static IServiceCollection AddCpfValidatorService(this IServiceCollection services, IConfiguration configuration)
        {
            // Configura o cliente HTTP para comunicação com a API da Receita Federal
            services.AddHttpClient("ReceitaFederal", client =>
            {
                // Obtém a URL base da API da Receita Federal da configuração
                var baseUrl = configuration["ExternalServices:ReceitaFederal:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl ?? "https://api.receitafederal.gov.br/");
                
                // Configura cabeçalhos padrão, se necessário
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                
                // Configura timeout
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            
            // Registra o serviço de validação de CPF
            services.AddScoped<ICpfValidatorService, CpfValidatorService>();
            
            return services;
        }
    }
}
