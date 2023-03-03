using CinemaApi.DTO.Films;
using FluentValidation;

namespace CinemaApi.DTO.Validation
{
    public class CreateFilmValidation : AbstractValidator<CreateFilmDto>
    {
        public CreateFilmValidation()
        {
            RuleFor(c => c.Name).NotEmpty().Length(5,35);
            RuleFor(c => c.Description).NotEmpty().Length(10,100);
            RuleFor(c => c.IsRecommended.ToString()).NotEmpty().Length(1,2);
            RuleFor(c => c.Genre).NotEmpty().Length(4,15);
            RuleForEach(x => x.Actors).SetValidator(new ActorValidation());
        }
    }
}
