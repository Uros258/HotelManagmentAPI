using HotelManagmentAPI.DTOs.Room;

namespace HotelManagmentAPI.Services.Room
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync();
        Task<RoomDto?> GetRoomByIdAsync(int id);
        Task<RoomDto> CreateRoomAsync(CreateRoomDto dto);
        Task<RoomDto?> UpdateRoomStatusAsync(int id, UpdateRoomStatusDto dto);
        Task<bool> DeleteRoomAsync(int id);
    }
}