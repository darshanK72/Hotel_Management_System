using HMS_API.Models;
using Hotel_Management_System.Payloads;
using Hotel_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_System.Repository.Implementation
{
    public class CommonRepository : ICommonRepository
    {
        private readonly HMSDbContext _context;

        public CommonRepository(HMSDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationResponsePayload> CreateReservationAsync(ReservationPayload reservationPayload)
        {
            var room = await _context.Rooms.FindAsync(reservationPayload.RoomId);
            if (room == null)
                throw new ArgumentException("Room not found");

            if (room.Status != "Available")
                throw new InvalidOperationException("Room is not available");

            if(reservationPayload.CheckOutDate <= reservationPayload.CheckInDate)
            {
                throw new InvalidOperationException("Invalid checkout date");
            }

            if (reservationPayload.CheckInDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Invalid check-in date");
            }

            var reservation = new Reservation
            {
                NumberOfAdults = reservationPayload.NumberOfAdults,
                NumberOfChildren = reservationPayload.NumberOfChildren,
                CheckInDate = reservationPayload.CheckInDate,
                CheckOutDate = reservationPayload.CheckOutDate,
                NumberOfNights = (reservationPayload.CheckOutDate - reservationPayload.CheckInDate).Days,
                RoomId = room.RoomId,
                Status = "Not Paid",
                Day = reservationPayload.CheckInDate.DayOfWeek.ToString()
            };

            var rate = await CalculateRate(room, reservation);
            reservation.RateId = rate.RateId;
            reservation.Rate = rate;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var guest = await _context.Guests.FindAsync(reservationPayload.GuestId);
            if (guest == null) throw new ArgumentException("Guest not found");

            guest.ReservationId = reservation.ReservationId;
            guest.Reservation = reservation;

            var bill = new Bill
            {
                BillingNumber = GenerateBillingNumber(),
                StayDates = $"{reservation.CheckInDate:yyyy-MM-dd} to {reservation.CheckOutDate:yyyy-MM-dd}",
                Price = reservation.Rate.TotalCharges,
                Taxes = CalculateTaxes(reservation.Rate.TotalCharges),
                Services = $"Room {reservation.Room.RoomNumber} for {reservation.NumberOfNights} nights"
            };

            reservation.Bill = bill;

            _context.Entry(guest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new ReservationResponsePayload
            {
                ReservationId = reservation.ReservationId,
                Status = reservation.Status,
                TotalAmount = rate.TotalCharges + bill.Taxes,
                BillId = bill.BillId,
                RoomId = room.RoomId,
                GuestId = reservationPayload.GuestId
            };
        }

        public async Task<ReservationResponsePayload> CompletePaymentAsync(PaymentPayload paymentPayload)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Rate)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == paymentPayload.ReservationId);

            if (reservation == null)
                throw new ArgumentException("Reservation not found");

            if (reservation.Status != "Not Paid")
                throw new InvalidOperationException("Reservation is not in the correct state for payment");

            var payment = new Payment
            {
                TotalAmount = paymentPayload.TotalAmount,
                PaymentTime = DateTime.UtcNow,
                CreditCardDetails = paymentPayload.CreditCardDetails
            };

            var bill = await _context.Bills.FindAsync(paymentPayload.BillId);

            payment.Bill = bill;
            reservation.Payment = payment;
            reservation.Bill = bill;
            reservation.Status = "Paid";

            await _context.SaveChangesAsync();

            return new ReservationResponsePayload
            {
                ReservationId = reservation.ReservationId,
                Status = reservation.Status,
                TotalAmount = payment.TotalAmount,
                BillId = bill.BillId,
                RoomId = reservation.RoomId
            };
        }

        private async Task<Rate> CalculateRate(Room room, Reservation reservation)
        {
            var baseRate = room.PerNightCharges;
            var totalGuests = reservation.NumberOfAdults + reservation.NumberOfChildren;
            var additionalGuestCharge = Math.Max(totalGuests - 2, 0) * 10;
            var perNightRate = baseRate + additionalGuestCharge;

            var rate = new Rate
            {
                Day = reservation.Day,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfGuests = totalGuests,
                PerNightCharges = perNightRate,
                TotalCharges = perNightRate * reservation.NumberOfNights
            };

            _context.Rates.Add(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

        private string GenerateReservationCode()
        {
            return "RES" + DateTime.UtcNow.Ticks.ToString().Substring(0, 8);
        }

        private string GenerateBillingNumber()
        {
            return "BILL" + DateTime.UtcNow.Ticks.ToString().Substring(0, 8);
        }

        private decimal CalculateTaxes(decimal amount)
        {
            return amount * 0.1m;
        }
    }
}