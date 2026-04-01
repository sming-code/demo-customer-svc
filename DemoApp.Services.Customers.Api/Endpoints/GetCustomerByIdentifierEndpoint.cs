namespace DemoApp.Services.Customers.Api.Endpoints;
using Models;

public class GetCustomerByIdentifierEndpoint : IMinimalEndpoint
{
    public void MapEndpoint(WebApplication app) =>
        app.MapGet(
            "customer/{customerIdentifier}",
            async (
                Guid customerIdentifier,
                [FromServices] ICustomerService customerService
            ) =>
            {
                // var customerDto = await customerService.GetCustomerByIdentifier(
                //     customerIdentifier
                // );

                // var customerModel = customerDto.ToModel();

                var customerModel = FakeStorage.GetById(customerIdentifier);
                
                return Results.Ok(
                    customerModel
                );
            }
        )
        .WithGroupName("Customers")
        .WithName("GetCustomerByIdentifier")
        .Produces<CustomerModel>();
}
