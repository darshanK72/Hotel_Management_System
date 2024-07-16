using HMS_API.Models;
using HMS_API.Payload;
using Hotel_Management_System.Payloads;
using Hotel_Management_System.Repository.Implementation;
using Hotel_Management_System.Repository.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS_API.Controllers
{
    [Route("api/receptionist")]
    [ApiController]
    [Authorize(Roles = "Receptionist,Manager,Owner")]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistRepository _receptionistRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IConfiguration _configuration;

        public ReceptionistController(IReceptionistRepository receptionistRepository, ICommonRepository commonRepository, IConfiguration configuration)
        {
            _receptionistRepository = receptionistRepository;
            _commonRepository = commonRepository;
            _configuration = configuration;
        }

        [HttpGet("guest/search")]
        public async Task<ActionResult<IEnumerable<GuestPayload>>> SearchGuests(
            [FromQuery] string? name,
            [FromQuery] string? email,
            [FromQuery] string? phoneNumber,
            [FromQuery] string? memberCode)
        {
            var guests = await _receptionistRepository.SearchGuestsAsync(name, email, phoneNumber, memberCode);
            return Ok(guests);
        }

        [HttpGet("guest/{id}")]
        public async Task<ActionResult<GuestPayload>> GetGuest(int id)
        {
            var guest = await _receptionistRepository.GetGuestByIdAsync(id);

            if (guest == null)
            {
                return NotFound();
            }

            return guest;
        }

        [HttpPost("guest")]
        public async Task<ActionResult<GuestPayload>> CreateGuest(GuestPayload guestDto)
        {
            var createdGuest = await _receptionistRepository.CreateGuestAsync(guestDto);

            var subject = "Welcome to My Home";
            var body = $"<h1>Welcome, {guestDto.Name}!</h1><p>Thank you for registering with us!.</p>";
            await SendEmailAsync(guestDto.Email, subject, body);

            return CreatedAtAction(nameof(GetGuest), new { id = createdGuest.GuestId }, createdGuest);
        }

        [HttpPut("guest/{id}")]
        public async Task<IActionResult> UpdateGuest(int id, GuestPayload guestDto)
        {
            if (id != guestDto.GuestId)
            {
                return BadRequest();
            }

            var updatedGuest = await _receptionistRepository.UpdateGuestAsync(id, guestDto);
            if (updatedGuest == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("guest/{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var result = await _receptionistRepository.DeleteGuestAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("reservations")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return Ok(await _receptionistRepository.GetReservationsAsync());
        }

        [HttpGet("bills")]
        public async Task<ActionResult<IEnumerable<BillPayload>>> GetBills()
        {
            return Ok(await _receptionistRepository.GetBills());
        }

        [HttpGet("reservation/{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _receptionistRepository.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPost("reservation/create")]
        public async Task<ActionResult<ReservationResponsePayload>> CreateReservation(ReservationPayload request)
        {
            try
            {
                var result = await _commonRepository.CreateReservationAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reservation/complete-payment")]
        public async Task<ActionResult<ReservationResponsePayload>> CompletePayment(PaymentPayload request)
        {
            try
            {
                var result = await _commonRepository.CompletePaymentAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    [   HttpPut("reservation/{id}")]
        public async Task<IActionResult> UpdateReservation(int id, Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return BadRequest();
            }

            try
            {
                await _receptionistRepository.UpdateReservationAsync(id, reservation);
            }
            catch (Exception)
            {
                if (!_receptionistRepository.ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("reservation/{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var result = await _receptionistRepository.DeleteReservationAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return Ok(await _receptionistRepository.GetRoomsAsync());
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["SmtpSettings:Server"], int.Parse(_configuration["SmtpSettings:Port"]), false);
                client.Authenticate(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
