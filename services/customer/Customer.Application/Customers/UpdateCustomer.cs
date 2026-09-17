using Customer.Application.Abstractions;
using Customer.Application.Common;
using FluentValidation;

namespace Customer.Application.Customers;

public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber);

public sealed class UpdateCustomerHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCustomerCommand> _validator;

    public UpdateCustomerHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateCustomerCommand> validator)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<CustomerResult>> HandleAsync(
        UpdateCustomerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult =
            await _validator.ValidateAsync(
                command,
                cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result<CustomerResult>.Failure(
                new Error(
                    "Customer.ValidationFailed",
                    string.Join(
                        ", ",
                        validationResult.Errors.Select(x => x.ErrorMessage))));
        }

        var customer =
            await _customerRepository.GetByIdAsync(
                command.CustomerId,
                cancellationToken);

        if (customer is null)
        {
            return Result<CustomerResult>.Failure(
                CustomerErrors.NotFound);
        }

        var normalizedEmail =
            command.Email.Trim().ToLowerInvariant();

        if (customer.Email != normalizedEmail)
        {
            var existingCustomer =
                await _customerRepository.GetByEmailAsync(
                    normalizedEmail,
                    cancellationToken);

            if (existingCustomer is not null)
            {
                return Result<CustomerResult>.Failure(
                    CustomerErrors.EmailAlreadyExists);
            }
        }

        customer.Update(
            command.FirstName,
            command.LastName,
            normalizedEmail,
            command.PhoneNumber);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<CustomerResult>.Success(
            new CustomerResult(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber,
                customer.CreatedAt));
    }
}

public sealed class UpdateCustomerValidator
    : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30);
    }
}
