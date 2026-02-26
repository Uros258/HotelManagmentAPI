namespace HotelManagmentAPI.Models
{
    public class Bill
    {
        public int BillId {  get; set; }

        public int ReservationId {  get; set; }
        public decimal RoomCharges {  get; set; }
        public decimal ExtraCharges {  get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Reservation Reservation { get; set; } = null!;
    }
}
