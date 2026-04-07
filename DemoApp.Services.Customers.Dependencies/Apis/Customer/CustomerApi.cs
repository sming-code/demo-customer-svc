namespace DemoApp.Services.Customers.Dependencies.Apis.Customer;

internal class CustomerApi(
    CustomerHttpClient _httpClient
) : ICustomerApi
{
    public async Task<CustomerDto[]> GetAll() => await _httpClient.GetAll();
}