using HotelManagmentAPI.Data;
using HotelManagmentAPI.DTOs.Room;
using HotelManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Services.Room
{
    public class RoomService : IRoomService
    {
        private readonly HotelDbContext _context;

        public RoomService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type.ToString(),
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync()
        {
            return await _context.Rooms
                .Where(r => r.Status == RoomStatus.Available)
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type.ToString(),
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return null;

            return new RoomDto
            {
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber,
                Type = room.Type.ToString(),
                Status = room.Status.ToString()
            };
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto)
        {
            var room = new Models.Room
            {
                RoomNumber = dto.RoomNumber,
                Type = dto.Type,
                Status = RoomStatus.Available
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return new RoomDto
            {
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber,
                Type = room.Type.ToString(),
                Status = room.Status.ToString()
            };
        }

        public async Task<RoomDto?> UpdateRoomStatusAsync(int id, UpdateRoomStatusDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return null;

            room.Status = dto.Status;
            await _context.SaveChangesAsync();

            return new RoomDto
            {
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber,
                Type = room.Type.ToString(),
                Status = room.Status.ToString()
            };
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}