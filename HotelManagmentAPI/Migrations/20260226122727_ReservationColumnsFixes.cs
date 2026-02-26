using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagmentAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReservationColumnsFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckedOut",
                table: "Reservations",
                newName: "CheckOutDate");

            migrationBuilder.RenameColumn(
                name: "CheckedIn",
                table: "Reservations",
                newName: "CheckInDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOutDate",
                table: "Reservations",
                newName: "CheckedOut");

            migrationBuilder.RenameColumn(
                name: "CheckInDate",
                table: "Reservations",
                newName: "CheckedIn");
        }
    }
}
