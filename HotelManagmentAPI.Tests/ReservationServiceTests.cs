using HotelManagmentAPI.Data;
using HotelManagmentAPI.DTOs.Reservation;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Services.Reservation;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Tests.Services
{
    public class ReservationServiceTests
    {
        private HotelDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new HotelDbContext(options);
        }

        private async Task<(Guest guest, Room room)> SeedBasicData(HotelDbContext context)
        {
            var guest = new Guest { FirstName = "Marko", LastName = "Petrovic", PhoneNumber = "123", DocumentNumber = "ABC" };
            var room = new Room { RoomNumber = "101", Type = RoomType.Single, Status = RoomStatus.Available };

            context.Guests.Add(guest);
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            return (guest, room);
        }

        [Fact]
        public async Task CreateReservation_Succeeds_WhenRoomIsAvailable()
        {
            var context = GetInMemoryContext();
            var (guest, room) = await SeedBasicData(context);
            var service = new ReservationService(context);

            var dto = new CreateReservationDto
            {
                GuestId = guest.GuestId,
                RoomId = room.RoomId,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(5),
                PricePerNight = 50
            };

            var result = await service.CreateReservationAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(guest.GuestId, result.GuestId);
        }

        [Fact]
        public async Task CreateReservation_Throws_WhenRoomIsDoubleBooked()
        {
            var context = GetInMemoryContext();
            var (guest, room) = await SeedBasicData(context);
            var service = new ReservationService(context);

            var firstReservation = new CreateReservationDto
            {
                GuestId = guest.GuestId,
                RoomId = room.RoomId,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(5),
                PricePerNight = 50
            };

            await service.CreateReservationAsync(firstReservation);

            var overlappingReservation = new CreateReservationDto
            {
                GuestId = guest.GuestId,
                RoomId = room.RoomId,
                CheckInDate = DateTime.Today.AddDays(3),
                CheckOutDate = DateTime.Today.AddDays(7),
                PricePerNight = 50
            };

            await Assert.ThrowsAsync<Exception>(() =>
                service.CreateReservationAsync(overlappingReservation));
        }

        [Fact]
        public async Task CreateReservation_Throws_WhenCheckInIsAfterCheckOut()
        {
            var context = GetInMemoryContext();
            var (guest, room) = await SeedBasicData(context);
            var service = new ReservationService(context);

            var dto = new CreateReservationDto
            {
                GuestId = guest.GuestId,
                RoomId = room.RoomId,
                CheckInDate = DateTime.Today.AddDays(5),
                CheckOutDate = DateTime.Today.AddDays(1),
                PricePerNight = 50
            };

            await Assert.ThrowsAsync<Exception>(() =>
                service.CreateReservationAsync(dto));
        }

        [Fact]
        public async Task CheckIn_UpdatesRoomStatusToOccupied()
        {
            var context = GetInMemoryContext();
            var (guest, room) = await SeedBasicData(context);
            var service = new ReservationService(context);

            var reservation = new Reservation
            {
                GuestId = guest.GuestId,
                RoomId = room.RoomId,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(5),
                PricePerNight = 50,
                Status = ReservationStatus.Confirmed
            };

            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            await service.CheckInAsync(reservation.ReservationId);

            var updatedRoom = await context.Rooms.FindAsync(room.RoomId);
            Assert.Equal(RoomStatus.Occupied, updatedRoom!.Status);
        }

        [Fact]
        public async Task CheckOut_SetsRoomStatusToCleaning()
        {
            var context = GetInMemoryContext();
            var (guest, room) = await SeedBasicData(context);
            var service = new ReservationService(context);

            var reservation = new Reservation
            {
                GuestId = guest.GuestId,
                RoomId = room.RoomId,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(5),
                PricePerNight = 50,
                Status = ReservationStatus.CheckedIn
            };

            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            room.Status = RoomStatus.Occupied;
            await context.SaveChangesAsync();

            await service.CheckOutAsync(reservation.ReservationId);

            var updatedRoom = await context.Rooms.FindAsync(room.RoomId);
            Assert.Equal(RoomStatus.Cleaning, updatedRoom!.Status);
        }
    }
}