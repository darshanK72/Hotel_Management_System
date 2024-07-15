namespace Hotel_Management_System.Payloads
{
    public class ReservationResponsePayload
    {
        public int ReservationId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int? BillId { get; set; }
        public int? RoomId { get; set; }
        public int? GuestId {  get; set; }
    }
}
