namespace HotelManagmentAPI.Models
{
    public enum UserRole {Admin, Receptionist, Housekeeping }
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty ;
        public UserRole Role { get; set; }


    }
}
