using HotelManagmentAPI.Data;
using HotelManagmentAPI.DTOs.Reservation;
using HotelManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Services.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly HotelDbContext _context;

        public ReservationService(HotelDbContext context)
        {
            _context = context;
        }

        private static ReservationDto MapToDto(Models.Reservation r)
        {
            var nights = (r.CheckOutDate - r.CheckInDate).Days;
            return new ReservationDto
            {
                ReservationId = r.ReservationId,
                GuestId = r.GuestId,
                GuestName = r.Guest != null ? $"{r.Guest.FirstName} {r.Guest.LastName}" : string.Empty,
                RoomId = r.RoomId,
                RoomNumber = r.Room != null ? r.Room.RoomNumber : string.Empty,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                PricePerNight = r.PricePerNight,
                TotalPrice = r.PricePerNight * nights,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt
            };
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .ToListAsync();

            return reservations.Select(MapToDto);
        }

        public async Task<ReservationDto?> GetReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null) return null;
            return MapToDto(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsByGuestIdAsync(int guestId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .Where(r => r.GuestId == guestId)
                .ToListAsync();

            return reservations.Select(MapToDto);
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto)
        {
            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null)
                throw new Exception("Room not found.");

            var guest = await _context.Guests.FindAsync(dto.GuestId);
            if (guest == null)
                throw new Exception("Guest not found.");

            if (dto.CheckInDate >= dto.CheckOutDate)
                throw new Exception("Check-in date must be before check-out date.");

            var overlap = await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == dto.RoomId &&
                    r.Status != ReservationStatus.Cancelled &&
                    r.CheckInDate < dto.CheckOutDate &&
                    r.CheckOutDate > dto.CheckInDate);

            if (overlap)
                throw new Exception("Room is already booked for the selected dates.");

            var reservation = new Models.Reservation
            {
                GuestId = dto.GuestId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                PricePerNight = dto.PricePerNight,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return await GetReservationByIdAsync(reservation.ReservationId)
                ?? throw new Exception("Failed to retrieve created reservation.");
        }

        public async Task<ReservationDto?> UpdateReservationStatusAsync(int id, UpdateReservationStatusDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return null;

            reservation.Status = dto.Status;
            await _context.SaveChangesAsync();

            return await GetReservationByIdAsync(id);
        }

        public async Task<bool> CancelReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            reservation.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckInAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null) return false;
            if (reservation.Status != ReservationStatus.Confirmed)
                throw new Exception("Only confirmed reservations can be checked in.");

            reservation.Status = ReservationStatus.CheckedIn;
            reservation.Room.Status = RoomStatus.Occupied;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckOutAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null) return false;
            if (reservation.Status != ReservationStatus.CheckedIn)
                throw new Exception("Only checked-in reservations can be checked out.");

            reservation.Status = ReservationStatus.CheckedOut;
            reservation.Room.Status = RoomStatus.Cleaning;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}