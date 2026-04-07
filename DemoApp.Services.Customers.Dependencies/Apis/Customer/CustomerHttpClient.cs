using System.Net.Http.Json;

namespace DemoApp.Services.Customers.Dependencies.Apis.Customer;

internal class CustomerHttpClient(
    HttpClient _httpClient
)
{
    internal async Task<CustomerDto[]> GetAll()
    {
        var response = await _httpClient.GetAsync("customer");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CustomerDto[]>();
        return result!;
    }
}