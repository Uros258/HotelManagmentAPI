namespace HotelManagmentAPI.Models
{
        public enum RoomType { Single, Double, Suite}
        public enum RoomStatus { Available, Occupied, Cleaning, Maintenance}

        public class Room
        {
            public int RoomId { get; set; }
            public string RoomNumber {  get; set; } = string.Empty;
            public RoomType Type { get; set; }
            public RoomStatus Status { get; set; } = RoomStatus.Available;

            public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
