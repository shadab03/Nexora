using Customer.Application.Abstractions;
using Customer.Application.Common;
using Customer.Domain.Customers;
using FluentValidation;

namespace Customer.Application.Customers.CreateCustomer;

public sealed class CreateCustomerHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCustomerCommand> _validator;

    public CreateCustomerHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateCustomerCommand> validator)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<CreateCustomerResult>> HandleAsync(
        CreateCustomerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult =
            await _validator.ValidateAsync(
                command,
                cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(
                ", ",
                validationResult.Errors.Select(x => x.ErrorMessage));

            return Result<CreateCustomerResult>.Failure(
                new Error(
                    "Customer.ValidationFailed",
                    errorMessage));
        }

        var normalizedEmail =
            command.Email.Trim().ToLowerInvariant();

        var existingCustomer =
            await _customerRepository.GetByEmailAsync(
                normalizedEmail,
                cancellationToken);

        if (existingCustomer is not null)
        {
            return Result<CreateCustomerResult>.Failure(
                CustomerErrors.EmailAlreadyExists);
        }

        var customer = CustomerEntity.Create(
            command.FirstName,
            command.LastName,
            normalizedEmail,
            command.PhoneNumber);

        await _customerRepository.AddAsync(
            customer,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<CreateCustomerResult>.Success(
            new CreateCustomerResult(customer.Id));
    }
}