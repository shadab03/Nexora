using Customer.Application.Abstractions;
using Customer.Application.Common;
using FluentValidation;

namespace Customer.Application.Customers;

public sealed record AddAddressCommand(
    Guid CustomerId,
    string Line1,
    string City,
    string Country,
    string PostalCode);

public sealed record AddressResult(
    Guid Id,
    string Line1,
    string City,
    string Country,
    string PostalCode);

public sealed class AddAddressHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddAddressCommand> _validator;

    public AddAddressHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddAddressCommand> validator)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<AddressResult>> HandleAsync(
        AddAddressCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult =
            await _validator.ValidateAsync(
                command,
                cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result<AddressResult>.Failure(
                new Error(
                    "Customer.ValidationFailed",
                    string.Join(
                        ", ",
                        validationResult.Errors.Select(x => x.ErrorMessage))));
        }

        var customer =
            await _customerRepository.GetByIdWithAddressesAsync(
                command.CustomerId,
                cancellationToken);

        if (customer is null)
        {
            return Result<AddressResult>.Failure(
                CustomerErrors.NotFound);
        }

        var address = customer.AddAddress(
            command.Line1,
            command.City,
            command.Country,
            command.PostalCode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<AddressResult>.Success(
            new AddressResult(
                address.Id,
                address.Line1,
                address.City,
                address.Country,
                address.PostalCode));
    }
}

public sealed class AddAddressValidator
    : AbstractValidator<AddAddressCommand>
{
    public AddAddressValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.Line1)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(30);
    }
}
