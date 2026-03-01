using HotelManagmentAPI.Data;
using HotelManagmentAPI.DTOs.Guest;
using HotelManagmentAPI.Models;
using HotelManagmentAPI.Services.Guest;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Tests.Services
{
    public class GuestServiceTests
    {
        private HotelDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new HotelDbContext(options);
        }

        [Fact]
        public async Task GetAllGuests_ReturnsAllGuests()
        {
            var context = GetInMemoryContext();
            context.Guests.AddRange(
                new Guest { FirstName = "Marko", LastName = "Petrovic", PhoneNumber = "123", DocumentNumber = "ABC" },
                new Guest { FirstName = "Ana", LastName = "Jovic", PhoneNumber = "456", DocumentNumber = "DEF" }
            );
            await context.SaveChangesAsync();

            var service = new GuestService(context);

            var result = await service.GetAllGuestsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGuestById_ReturnsCorrectGuest()
        {
            var context = GetInMemoryContext();
            var guest = new Guest { FirstName = "Marko", LastName = "Petrovic", PhoneNumber = "123", DocumentNumber = "ABC" };
            context.Guests.Add(guest);
            await context.SaveChangesAsync();

            var service = new GuestService(context);

            var result = await service.GetGuestByIdAsync(guest.GuestId);

            Assert.NotNull(result);
            Assert.Equal("Marko", result.FirstName);
            Assert.Equal("Petrovic", result.LastName);
        }

        [Fact]
        public async Task GetGuestById_ReturnsNull_WhenGuestNotFound()
        {
            var context = GetInMemoryContext();
            var service = new GuestService(context);

            var result = await service.GetGuestByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateGuest_SavesAndReturnsGuest()
        {
            var context = GetInMemoryContext();
            var service = new GuestService(context);
            var dto = new CreateGuestDto
            {
                FirstName = "Marko",
                LastName = "Petrovic",
                PhoneNumber = "123456",
                DocumentNumber = "ABC123"
            };

            var result = await service.CreateGuestAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Marko", result.FirstName);
            Assert.Equal(1, context.Guests.Count());
        }

        [Fact]
        public async Task DeleteGuest_ReturnsFalse_WhenGuestNotFound()
        {
            var context = GetInMemoryContext();
            var service = new GuestService(context);

            var result = await service.DeleteGuestAsync(999);

            Assert.False(result);
        }
    }
}