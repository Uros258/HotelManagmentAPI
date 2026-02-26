using HotelManagmentAPI.DTOs.Reservation;

namespace HotelManagmentAPI.Services.Reservation
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
        Task<ReservationDto?> GetReservationByIdAsync(int id);
        Task<IEnumerable<ReservationDto>> GetReservationsByGuestIdAsync(int guestId);
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto);
        Task<ReservationDto?> UpdateReservationStatusAsync(int id, UpdateReservationStatusDto dto);
        Task<bool> CancelReservationAsync(int id);
        Task<bool> CheckInAsync(int id);
        Task<bool> CheckOutAsync(int id);
    }
}