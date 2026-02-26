using HotelManagmentAPI.Data;
using HotelManagmentAPI.DTOs.Guest;
using HotelManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Services.Guest
{
    public class GuestService : IGuestService
    {
        private readonly HotelDbContext _context;

        public GuestService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuestDto>> GetAllGuestsAsync()
        {
            return await _context.Guests
                .Select(g => new GuestDto
                {
                    GuestId = g.GuestId,
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    PhoneNumber = g.PhoneNumber,
                    DocumentNumber = g.DocumentNumber,
                    CreatedAt = g.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<GuestDto?> GetGuestByIdAsync(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null) return null;

            return new GuestDto
            {
                GuestId = guest.GuestId,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                DocumentNumber = guest.DocumentNumber,
                CreatedAt = guest.CreatedAt
            };
        }

        public async Task<GuestDto> CreateGuestAsync(CreateGuestDto dto)
        {
            var guest = new Models.Guest
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DocumentNumber = dto.DocumentNumber,
                CreatedAt = DateTime.UtcNow
            };

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            return new GuestDto
            {
                GuestId = guest.GuestId,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                DocumentNumber = guest.DocumentNumber,
                CreatedAt = guest.CreatedAt
            };
        }

        public async Task<GuestDto?> UpdateGuestAsync(int id, UpdateGuestDto dto)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null) return null;

            guest.FirstName = dto.FirstName;
            guest.LastName = dto.LastName;
            guest.PhoneNumber = dto.PhoneNumber;
            guest.DocumentNumber = dto.DocumentNumber;

            await _context.SaveChangesAsync();

            return new GuestDto
            {
                GuestId = guest.GuestId,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                DocumentNumber = guest.DocumentNumber,
                CreatedAt = guest.CreatedAt
            };
        }

        public async Task<bool> DeleteGuestAsync(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null) return false;

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}