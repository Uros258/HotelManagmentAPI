using HotelManagmentAPI.DTOs.Guest;

namespace HotelManagmentAPI.Services.Guest
{
    public interface IGuestService
    {
        Task<IEnumerable<GuestDto>> GetAllGuestsAsync();
        Task<GuestDto?> GetGuestByIdAsync(int id);
        Task<GuestDto> CreateGuestAsync(CreateGuestDto dto);
        Task<GuestDto?> UpdateGuestAsync(int id, UpdateGuestDto dto);
        Task<bool> DeleteGuestAsync(int id);
    }
}