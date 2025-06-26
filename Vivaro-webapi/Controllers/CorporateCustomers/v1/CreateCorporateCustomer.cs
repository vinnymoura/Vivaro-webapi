using Application.Shared.Entities;
using Application.Shared.Models.Errors;
using Application.Shared.Notifications;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Abstractions;
using Application.UseCases.CorporateCustomers.v1.CreateCorporateCustomer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Vivaro_webapi.Controllers.CorporateCustomers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CreateCorporateCustomer(ICreateCorporateCustomersUseCase useCase, Notification notification)
    : ControllerBase, IOutputPort
{
    private IActionResult? _viewModel;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCorporateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        useCase.SetOutputPort(this);
        await useCase.ExecuteAsync(request, cancellationToken);
        return _viewModel!;
    }

    void IOutputPort.InvalidRequest()
        => _viewModel = NotFound(new ValidationError(notification, HttpContext));

    void IOutputPort.CorporateCustomerAlreadyExists()
        => _viewModel = Conflict(new ConflictError("Já existe um cliente corporativo com este CNPJ.", HttpContext));

    void IOutputPort.CorporateCustomerCreated(CorporateCustomer corporateCustomer)
        => _viewModel = Created($"api/v1/CorporateCustomers/{corporateCustomer.Id}", corporateCustomer);
}