using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Payloads;
using Hotel_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_System.Repository.Implementation
{
    public class ReceptionistRepository : IReceptionistRepository
    {
        private readonly HMSDbContext _context;

        public ReceptionistRepository(HMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuestPayload>> SearchGuestsAsync(string name, string email, string phoneNumber, string memberCode)
        {
            var query = _context.Guests.Include(g => g.Reservation).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(g => g.Name.Contains(name));
            if (!string.IsNullOrEmpty(email))
                query = query.Where(g => g.Email.Contains(email));
            if (!string.IsNullOrEmpty(phoneNumber))
                query = query.Where(g => g.PhoneNumber.Contains(phoneNumber));
            if (!string.IsNullOrEmpty(memberCode))
                query = query.Where(g => g.MemberCode.Contains(memberCode));



           
            return await query.Select(g => new GuestPayload
            {
                GuestId = g.GuestId,
                Name = g.Name,
                Email = g.Email,
                Gender = g.Gender,
                Address = g.Address,
                PhoneNumber = g.PhoneNumber,
                MemberCode = g.MemberCode,
                ReservationId = g.ReservationId ?? 0
            }).ToListAsync();
        }

        public async Task<GuestPayload> GetGuestByIdAsync(int id)
        {
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.GuestId == id);
            if (guest == null)
                return null;

            return new GuestPayload
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                Gender = guest.Gender,
                Address = guest.Address,
                PhoneNumber = guest.PhoneNumber,
                MemberCode = guest.MemberCode
            };
        }

        public async Task<GuestPayload> CreateGuestAsync(GuestPayload guestDto)
        {

            var guest = new Guest
            {
                Name = guestDto.Name,
                Email = guestDto.Email,
                Gender = guestDto.Gender,
                Address = guestDto.Address,
                PhoneNumber = guestDto.PhoneNumber,
                MemberCode = guestDto.MemberCode
            };
           

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            guestDto.GuestId = guest.GuestId;
            return guestDto;
        }

        public async Task<GuestPayload> UpdateGuestAsync(int id, GuestPayload guestDto)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
                return null;

            guest.Name = guestDto.Name;
            guest.Email = guestDto.Email;
            guest.Gender = guestDto.Gender;
            guest.Address = guestDto.Address;
            guest.PhoneNumber = guestDto.PhoneNumber;
            guest.MemberCode = guestDto.MemberCode;

            _context.Entry(guest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return guestDto;
        }

        public async Task<bool> DeleteGuestAsync(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
                return false;

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.GuestId == id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            var reservations =  await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Rate)
                .Include(r => r.Payment)
                .Include(r => r.Bill)
                .ToListAsync();

            //return reservations.Select(reservations => new ReservationResponsePayload
            //{
            //    ReservationId = reservations.ReservationId,
            //    Status = reservations.Status,
            //    TotalAmount = reservations.Rate?.TotalCharges ?? 0,
            //    BillId = reservations.BillId,
            //    RoomId = reservations.RoomId
            //});
            return reservations;
        }

        public async Task<IEnumerable<BillPayload>> GetBills()
        {
            var bills = await _context.Bills
                .Select(b => new BillPayload
                {
                    BillId = b.BillId,
                    BillingNumber = b.BillingNumber,
                    StayDates = b.StayDates,
                    Price = b.Price,
                    Taxes = b.Taxes,
                    Services = b.Services
                })
                .ToListAsync();

            foreach (var bill in bills)
            {
                var reservation = _context.Reservations.Where(rev => rev.BillId == bill.BillId).FirstOrDefault();
                bill.ReservationId = reservation.ReservationId;
                bill.Status = reservation.Status;
            }

            var presentBills = bills.Where(b =>
            {
                var guest = _context.Guests.Where(g => g.ReservationId == b.ReservationId).FirstOrDefault();
                return guest != null;
            });

            foreach (var bill in presentBills)
            {
                var guest = _context.Guests.Where(g => g.ReservationId == bill.ReservationId).FirstOrDefault();
                bill.GuestId = guest.GuestId;
            }
            return presentBills;
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Rate)
                .Include(r => r.Payment)
                .Include(r => r.Bill)
                .FirstOrDefaultAsync(r => r.ReservationId == id);
        }
        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }

        public async Task<IEnumerable<Rate>> GetRatesAsync()
        {
            return await _context.Rates.ToListAsync();
        }

        public async Task<Rate> GetRateByIdAsync(int id)
        {
            return await _context.Rates.FindAsync(id);
        }

        public async Task<Rate> CreateRateAsync(Rate rate)
        {
            _context.Rates.Add(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

        public async Task<Rate> UpdateRateAsync(int id, Rate rate)
        {
            if (id != rate.RateId)
            {
                throw new ArgumentException("ID mismatch");
            }

            _context.Entry(rate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return rate;
        }

        public async Task<bool> DeleteRateAsync(int id)
        {
            var rate = await _context.Rates.FindAsync(id);
            if (rate == null)
            {
                return false;
            }

            _context.Rates.Remove(rate);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool RateExists(int id)
        {
            return _context.Rates.Any(e => e.RateId == id);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
    }
}
