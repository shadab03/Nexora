namespace Customer.Application.Common;

public sealed record Error(
    string Code,
    string Description);