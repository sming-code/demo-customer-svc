namespace DemoApp.Services.Customers.Worker.Events;

public class CustomerEvent
{
    public required Guid CustomerIdentifier { get; set; }
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
}

internal static class CustomerEventExtensions
{
    internal static CustomerDto ToDto(
        this CustomerEvent eventData
    ) => new(
        eventData.CustomerIdentifier,
        eventData.FirstName,
        eventData.Surname
    );
}