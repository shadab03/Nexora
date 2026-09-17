using Customer.Api.Contracts.Customers;
using Customer.Application.Common;
using Customer.Application.Customers;
using Customer.Application.Customers.CreateCustomer;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Api.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    private readonly AddAddressHandler _addAddressHandler;
    private readonly CreateCustomerHandler _createCustomerHandler;
    private readonly GetCustomerAddressesHandler _getCustomerAddressesHandler;
    private readonly GetCustomerHandler _getCustomerHandler;
    private readonly UpdateCustomerHandler _updateCustomerHandler;

    public CustomersController(
        AddAddressHandler addAddressHandler,
        CreateCustomerHandler createCustomerHandler,
        GetCustomerAddressesHandler getCustomerAddressesHandler,
        GetCustomerHandler getCustomerHandler,
        UpdateCustomerHandler updateCustomerHandler)
    {
        _addAddressHandler = addAddressHandler;
        _createCustomerHandler = createCustomerHandler;
        _getCustomerAddressesHandler = getCustomerAddressesHandler;
        _getCustomerHandler = getCustomerHandler;
        _updateCustomerHandler = updateCustomerHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCustomerCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber);

        var result =
            await _createCustomerHandler.HandleAsync(
                command,
                cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error?.Code ==
                "Customer.EmailAlreadyExists")
            {
                return Conflict(new
                {
                    code = result.Error.Code,
                    message = result.Error.Description
                });
            }

            return BadRequest(new
            {
                code = result.Error?.Code,
                message = result.Error?.Description
            });
        }

        return Created(
            $"/api/customers/{result.Value.CustomerId}",
            result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result =
            await _getCustomerHandler.HandleAsync(
                new GetCustomerQuery(id),
                cancellationToken);

        if (result.IsFailure)
        {
            return ToProblemResult(result);
        }

        return Ok(ToResponse(result.Value));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await _updateCustomerHandler.HandleAsync(
                new UpdateCustomerCommand(
                    id,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber),
                cancellationToken);

        if (result.IsFailure)
        {
            return ToProblemResult(result);
        }

        return Ok(ToResponse(result.Value));
    }

    [HttpPost("{id:guid}/addresses")]
    public async Task<IActionResult> AddAddress(
        Guid id,
        AddAddressRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await _addAddressHandler.HandleAsync(
                new AddAddressCommand(
                    id,
                    request.Line1,
                    request.City,
                    request.Country,
                    request.PostalCode),
                cancellationToken);

        if (result.IsFailure)
        {
            return ToProblemResult(result);
        }

        return Created(
            $"/api/customers/{id}/addresses/{result.Value.Id}",
            ToResponse(result.Value));
    }

    [HttpGet("{id:guid}/addresses")]
    public async Task<IActionResult> GetAddresses(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result =
            await _getCustomerAddressesHandler.HandleAsync(
                new GetCustomerAddressesQuery(id),
                cancellationToken);

        if (result.IsFailure)
        {
            return ToProblemResult(result);
        }

        return Ok(result.Value.Select(ToResponse));
    }

    private static CustomerResponse ToResponse(
        CustomerResult customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            customer.CreatedAt);
    }

    private static AddressResponse ToResponse(
        AddressResult address)
    {
        return new AddressResponse(
            address.Id,
            address.Line1,
            address.City,
            address.Country,
            address.PostalCode);
    }

    private IActionResult ToProblemResult<T>(
        Result<T> result)
    {
        if (result.Error?.Code == "Customer.NotFound")
        {
            return NotFound(new
            {
                code = result.Error.Code,
                message = result.Error.Description
            });
        }

        if (result.Error?.Code == "Customer.EmailAlreadyExists")
        {
            return Conflict(new
            {
                code = result.Error.Code,
                message = result.Error.Description
            });
        }

        return BadRequest(new
        {
            code = result.Error?.Code,
            message = result.Error?.Description
        });
    }
}
