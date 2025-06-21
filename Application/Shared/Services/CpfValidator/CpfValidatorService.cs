// filepath: e:\projetos\VivaroBravea\vivaro-webapi\Application\Shared\Services\CpfValidator\CpfValidatorService.cs

using System.Net.Http.Json;
using Application.Shared.Interfaces.CpfValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Shared.Services.CpfValidator
{
    public class CpfValidatorService(IHttpClientFactory httpClientFactory, ILogger<CpfValidatorService> logger, IConfiguration configuration)
        : ICpfValidatorService
    {
        public async Task<bool> IsValidCpfAsync(string cpf)
        {
            try
            {
                // Primeiro, valide o formato do CPF localmente
                if (!IsValidCpfFormat(cpf))
                {
                    logger.LogInformation("CPF {Cpf} inválido no formato local", cpf);
                    return false;
                }

                // Verifique se estamos em modo de desenvolvimento ou se a validação da Receita está desabilitada
                var useReceitaValidation = configuration.GetValue<bool>("ExternalServices:ReceitaFederal:Enabled", false);
                
                if (!useReceitaValidation)
                {
                    logger.LogInformation("Usando apenas validação local para CPF {Cpf}", cpf);
                    return true; // CPF já foi validado localmente
                }

                // Se a validação da Receita está habilitada, tente usar a API
                try
                {
                    // Limpe o CPF para enviar para a API (sem pontos ou hífens)
                    var cleanCpf = cpf.Trim().Replace(".", "").Replace("-", "").Replace(" ", "");
                    
                    // Cria um cliente HTTP para fazer a requisição à API da Receita Federal
                    var client = httpClientFactory.CreateClient("ReceitaFederal");
                    
                    // Tenta fazer a requisição com timeout reduzido
                    var timeoutTask = Task.Delay(3000); // 3 segundos de timeout
                    var requestTask = client.GetAsync($"api/validacpf/{cleanCpf}");
                    
                    var completedTask = await Task.WhenAny(requestTask, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        logger.LogWarning("Timeout ao validar CPF na API da Receita Federal. Usando validação local.");
                        return true; // CPF já foi validado localmente
                    }
                    
                    var response = await requestTask;
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<CpfValidationResponse>();
                        logger.LogInformation("CPF validado pela Receita Federal: {IsValid}", result?.IsValid);
                        return result?.IsValid ?? false;
                    }
                    
                    logger.LogWarning("Falha na validação do CPF na Receita Federal. Status code: {StatusCode}. Usando validação local.", response.StatusCode);
                    return true; // CPF já foi validado localmente
                }
                catch (Exception ex)
                {
                    // Em caso de erro na API, confie na validação local que já foi feita
                    logger.LogError(ex, "Exceção ao validar CPF na Receita Federal. Usando validação local.");
                    return true; // CPF já foi validado localmente
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exceção ao validar CPF");
                return false;
            }
        }

        private bool IsValidCpfFormat(string cpf)
        {
            // Retira os caracteres não numéricos caso existam
            cpf = cpf?.Trim().Replace(".", "").Replace("-", "").Replace(" ", "") ?? string.Empty;
            
            // Verifica se o CPF possui 11 dígitos
            if (cpf.Length != 11)
                return false;
            
            // Verifica se todos os dígitos são iguais
            bool allDigitsEqual = true;
            for (int i = 1; i < cpf.Length; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    allDigitsEqual = false;
                    break;
                }
            }
            
            if (allDigitsEqual)
                return false;
            
            // Validação do primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);
            
            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;
            
            if (int.Parse(cpf[9].ToString()) != digit1)
                return false;
            
            // Validação do segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
            
            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;
            
            return int.Parse(cpf[10].ToString()) == digit2;
        }
    }

    // Classe para deserializar a resposta da API da Receita Federal
    public class CpfValidationResponse
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
    }
}
