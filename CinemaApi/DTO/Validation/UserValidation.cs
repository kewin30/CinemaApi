using CinemaApi.DTO.Users;
using CinemaApi.Entities;
using FluentValidation;
using System.Linq;

namespace CinemaApi.DTO.Validation
{
    public class UserValidation : AbstractValidator<RegisterUserDto>
    {
        public UserValidation(CinemaDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.Login).MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
            RuleFor(x => x.FirstName).MinimumLength(2);
            RuleFor(x => x.LastName).MinimumLength(2);
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.User.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
