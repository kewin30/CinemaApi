using CinemaApi.DTO.Actors;
using CinemaApi.Entities;
using FluentValidation;

namespace CinemaApi.DTO.Validation
{
    public class ActorValidation : AbstractValidator<ActorsDto>
    {
        public ActorValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().Length(5,30);
            RuleFor(x => x.Age.ToString()).NotEmpty().Length(1,3);
            RuleFor(x => x.Gender).Must(x => x == "Male" || x == "Female")
               .WithMessage("Gender must be Male or Female");
            RuleFor(x => x.PlayingAs).NotEmpty().Length(2,30);
        }
    }
}
