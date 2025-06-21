namespace Application.Shared.Interfaces.CpfValidator;

public interface ICpfValidatorService
{
    Task<bool> IsValidCpfAsync(string cpf);
}