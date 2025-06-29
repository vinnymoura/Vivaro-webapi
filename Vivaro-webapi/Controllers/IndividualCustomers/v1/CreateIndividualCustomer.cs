using Application.Shared.Entities;
using Application.Shared.Models.Errors;
using Application.Shared.Notifications;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Abstractions;
using Application.UseCases.IndividualCustomers.v1.CreateIndividualCustomer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vivaro_webapi.Controllers.IndividualCustomers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CreateIndividualCustomer(ICreateIndividualCustomersUseCase useCase, Notification notification)
    : ControllerBase, IOutputPort
{
    private IActionResult? _viewModel;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CreateIndividualCustomerRequest request,
        CancellationToken cancellationToken)
    {
        useCase.SetOutputPort(this);
        await useCase.ExecuteAsync(request, cancellationToken);
        return _viewModel!;
    }

    void IOutputPort.InvalidRequest()
        => _viewModel = NotFound(new ValidationError(notification, HttpContext));

    void IOutputPort.IndividualCustomerAlreadyExists()
        => _viewModel = Conflict(new ConflictError("Já existe um cliente individual com este CPF.", HttpContext));

    void IOutputPort.IndividualCustomerCreated(IndividualCustomer individualCustomer)
        => _viewModel = Created($"api/v1/IndividualCustomers/{individualCustomer.Id}", individualCustomer);

    void IOutputPort.KeycloakCreationFailed()
        => _viewModel = Conflict(new ValidationError(notification, HttpContext));
}