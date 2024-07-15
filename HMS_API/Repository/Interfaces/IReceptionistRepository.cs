using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Payloads;

namespace Hotel_Management_System.Repository.Interfaces
{
    public interface IReceptionistRepository
    {
        Task<IEnumerable<GuestPayload>> SearchGuestsAsync(string name, string email, string phoneNumber, string memberCode);
        Task<GuestPayload> GetGuestByIdAsync(int id);
        Task<GuestPayload> CreateGuestAsync(GuestPayload guestDto);
        Task<GuestPayload> UpdateGuestAsync(int id, GuestPayload guestDto);
        Task<bool> DeleteGuestAsync(int id);
        bool GuestExists(int id);

        Task<IEnumerable<Rate>> GetRatesAsync();
        Task<Rate> GetRateByIdAsync(int id);
        Task<Rate> CreateRateAsync(Rate rate);
        Task<Rate> UpdateRateAsync(int id, Rate rate);
        Task<bool> DeleteRateAsync(int id);
        bool RateExists(int id);

        Task<IEnumerable<ReservationResponsePayload>> GetReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<Reservation> UpdateReservationAsync(int id, Reservation reservation);
        Task<bool> DeleteReservationAsync(int id);
        bool ReservationExists(int id);

        Task<IEnumerable<Room>> GetRoomsAsync();
    }
}
