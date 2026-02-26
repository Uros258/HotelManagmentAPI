namespace HotelManagmentAPI.DTOs.Guest
{
    public class GuestDto
    {
        public int GuestId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateGuestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
    }

    public class UpdateGuestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
    }
}