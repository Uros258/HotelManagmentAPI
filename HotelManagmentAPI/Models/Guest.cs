namespace HotelManagmentAPI.Models
{
    public class Guest
    {
        public int GuestId {  get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty ;
        public string PhoneNumber {  get; set; } = string.Empty ;
        public string DocumentNumber {  get; set; } = string.Empty ;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
