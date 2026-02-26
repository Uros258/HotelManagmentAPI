using System;

namespace HotelManagmentAPI.Models
{
        public enum ReservationStatus {Pending, Confirmed, CheckedIn, CheckedOut, Canceled}

        public class Reservation
        {
            public int ReservationId { get; set; }
            public int GuestId {get; set; }
            public int RoomId {  get; set; }
            public DateTime CheckedIn { get; set; }
            public DateTime CheckedOut { get; set; }

            public decimal PricePerNight {  get; set; }

            public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public Guest Guest { get; set; } = null!;
            public Room Room { get; set; } = null!;
            public Bill? Bill { get; set; }
    }
}
