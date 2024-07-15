using HMS_API.Models;
using Hotel_Management_System.Payloads;

namespace Hotel_Management_System.Repository.Interfaces
{
    public interface ICommonRepository
    {
        Task<ReservationResponsePayload> CreateReservationAsync(ReservationPayload reservationPayload);
        Task<ReservationResponsePayload> CompletePaymentAsync(PaymentPayload paymentPayload);
    }
}
