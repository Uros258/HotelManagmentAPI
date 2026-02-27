using FluentValidation;
using HotelManagmentAPI.DTOs.Reservation;

namespace HotelManagmentAPI.Validators.Reservation
{
    public class CreateReservationValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationValidator()
        {
            RuleFor(x => x.GuestId)
                .GreaterThan(0).WithMessage("A valid guest must be selected.");

            RuleFor(x => x.RoomId)
                .GreaterThan(0).WithMessage("A valid room must be selected.");

            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("Check-in date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date cannot be in the past.");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThan(x => x.CheckInDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("Price per night must be more than zero.");
        }
    }
}