namespace DemoApp.Services.Customers.Domain.Dtos;

public record CustomerDto(
    Guid CustomerIdentifier,
    string FirstName,
    string Surname
);