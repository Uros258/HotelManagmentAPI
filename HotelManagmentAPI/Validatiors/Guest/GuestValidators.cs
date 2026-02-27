using FluentValidation;
using HotelManagmentAPI.DTOs.Guest;

namespace HotelManagmentAPI.Validators.Guest
{
    public class CreateGuestValidator : AbstractValidator<CreateGuestDto>
    {
        public CreateGuestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("Document number is required.")
                .MaximumLength(20).WithMessage("Document number cannot be longer than 20 characters.");
        }
    }

    public class UpdateGuestValidator : AbstractValidator<UpdateGuestDto>
    {
        public UpdateGuestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("Document number is required.")
                .MaximumLength(20).WithMessage("Document number cannot be longer than 20 characters.");
        }
    }
}