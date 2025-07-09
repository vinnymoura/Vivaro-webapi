using Application.Shared.Entities;
using Application.Shared.Models.Errors;
using Application.Shared.Notifications;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Abstractions;
using Application.UseCases.PersonUseCase.v1.CreatePerson.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vivaro_webapi.Controllers.IndividualCustomers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CreateIndividualCustomer(ICreatePersonUseCase useCase, Notification notification)
    : ControllerBase, IOutputPort
{
    private IActionResult? _viewModel;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CreatePersonRequest request,
        CancellationToken cancellationToken)
    {
        useCase.SetOutputPort(this);
        await useCase.ExecuteAsync(request, cancellationToken);
        return _viewModel!;
    }

    void IOutputPort.InvalidRequest()
        => _viewModel = NotFound(new ValidationError(notification, HttpContext));

    void IOutputPort.NaturalPersonAlreadyExists()
        => _viewModel = Conflict(new ConflictError("Já existe um cliente individual com este CPF.", HttpContext));

    void IOutputPort.NaturalPersonCreated(NaturalPerson naturalPerson)
        => _viewModel = Created($"api/v1/PersonUseCase/{naturalPerson.Id}", naturalPerson);

    void IOutputPort.LegalPersonAlreadyExists()
        => _viewModel = Conflict(new ConflictError("Já existe um cliente com esse CNPJ.", HttpContext));

    void IOutputPort.LegalPersonCreated(LegalPerson legalPerson)
        => _viewModel = Created($"api/v1/PersonUseCase/{legalPerson.Id}", legalPerson);

    void IOutputPort.KeycloakCreationFailed()
        => _viewModel = Conflict(new ValidationError(notification, HttpContext));

    void IOutputPort.PersonTypeNotSupported() =>
        _viewModel = BadRequest(new ValidationError(notification, HttpContext));
}