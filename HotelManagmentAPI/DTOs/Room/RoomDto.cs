using HotelManagmentAPI.Models;

namespace HotelManagmentAPI.DTOs.Room
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreateRoomDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public RoomType Type { get; set; }
    }

    public class UpdateRoomStatusDto
    {
        public RoomStatus Status { get; set; }
    }
}