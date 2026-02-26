namespace HotelManagmentAPI.DTOs.Bill
{
    public class BillDto
    {
        public int BillId { get; set; }
        public int ReservationId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public decimal RoomCharges { get; set; }
        public decimal ExtraCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBillDto
    {
        public int ReservationId { get; set; }
        public decimal ExtraCharges { get; set; }
    }

    public class UpdateBillDto
    {
        public decimal ExtraCharges { get; set; }
        public bool IsPaid { get; set; }
    }
}