using SmingCode.Utilities.ServiceApiClient;

namespace DemoApp.Services.Customers.Dependencies.Apis.ReservationService;

internal class ReservationServiceApi(
    IServiceApiClient<ReservationServiceApi> _reservationApiClient
)
{
    internal async Task UpdateReservation(
        string reservationSourceIdentifier
    )
    {
        var reservationUpdateModel = new { Value = "test" };

        await _reservationApiClient.Post(
            $"reservation/{reservationSourceIdentifier}/assertions",
            reservationUpdateModel
        );
    }
}
