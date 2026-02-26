using HotelManagmentAPI.Data;
using HotelManagmentAPI.DTOs.Bill;
using HotelManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Services.Bill
{
    public class BillService : IBillService
    {
        private readonly HotelDbContext _context;

        public BillService(HotelDbContext context)
        {
            _context = context;
        }

        private static BillDto MapToDto(Models.Bill b)
        {
            return new BillDto
            {
                BillId = b.BillId,
                ReservationId = b.ReservationId,
                GuestName = b.Reservation?.Guest != null
                    ? $"{b.Reservation.Guest.FirstName} {b.Reservation.Guest.LastName}"
                    : string.Empty,
                RoomNumber = b.Reservation?.Room?.RoomNumber ?? string.Empty,
                RoomCharges = b.RoomCharges,
                ExtraCharges = b.ExtraCharges,
                TotalAmount = b.TotalAmount,
                IsPaid = b.IsPaid,
                CreatedAt = b.CreatedAt
            };
        }

        public async Task<IEnumerable<BillDto>> GetAllBillsAsync()
        {
            var bills = await _context.Bills
                .Include(b => b.Reservation)
                    .ThenInclude(r => r.Guest)
                .Include(b => b.Reservation)
                    .ThenInclude(r => r.Room)
                .ToListAsync();

            return bills.Select(MapToDto);
        }

        public async Task<BillDto?> GetBillByIdAsync(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Reservation)
                    .ThenInclude(r => r.Guest)
                .Include(b => b.Reservation)
                    .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null) return null;
            return MapToDto(bill);
        }

        public async Task<BillDto?> GetBillByReservationIdAsync(int reservationId)
        {
            var bill = await _context.Bills
                .Include(b => b.Reservation)
                    .ThenInclude(r => r.Guest)
                .Include(b => b.Reservation)
                    .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(b => b.ReservationId == reservationId);

            if (bill == null) return null;
            return MapToDto(bill);
        }

        public async Task<BillDto> CreateBillAsync(CreateBillDto dto)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == dto.ReservationId);

            if (reservation == null)
                throw new Exception("Reservation not found.");

            var existingBill = await _context.Bills
                .AnyAsync(b => b.ReservationId == dto.ReservationId);

            if (existingBill)
                throw new Exception("A bill already exists for this reservation.");

            var nights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            var roomCharges = reservation.PricePerNight * nights;
            var totalAmount = roomCharges + dto.ExtraCharges;

            var bill = new Models.Bill
            {
                ReservationId = dto.ReservationId,
                RoomCharges = roomCharges,
                ExtraCharges = dto.ExtraCharges,
                TotalAmount = totalAmount,
                IsPaid = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return await GetBillByIdAsync(bill.BillId)
                ?? throw new Exception("Failed to retrieve created bill.");
        }

        public async Task<BillDto?> UpdateBillAsync(int id, UpdateBillDto dto)
        {
            var bill = await _context.Bills
                .Include(b => b.Reservation)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null) return null;

            var nights = (bill.Reservation.CheckOutDate - bill.Reservation.CheckInDate).Days;
            bill.ExtraCharges = dto.ExtraCharges;
            bill.RoomCharges = bill.Reservation.PricePerNight * nights;
            bill.TotalAmount = bill.RoomCharges + dto.ExtraCharges;
            bill.IsPaid = dto.IsPaid;

            await _context.SaveChangesAsync();

            return await GetBillByIdAsync(id);
        }
    }
}