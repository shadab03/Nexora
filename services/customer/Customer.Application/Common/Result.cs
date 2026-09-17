namespace Customer.Application.Common;

public class Result
{
    protected Result(
        bool isSuccess,
        Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result Success()
        => new(true, null);

    public static Result Failure(Error error)
        => new(false, error);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(
        T? value,
        bool isSuccess,
        Error? error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException(
                    "Cannot access value of a failed result.");

            return _value!;
        }
    }

    public static Result<T> Success(T value)
        => new(value, true, null);

    public static Result<T> Failure(Error error)
        => new(default, false, error);
}