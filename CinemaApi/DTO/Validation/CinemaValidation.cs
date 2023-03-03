using CinemaApi.Entities;
using FluentValidation;

namespace CinemaApi.DTO.Validation
{
    public class CinemaValidation :AbstractValidator<CinemaDto>
    {
        public CinemaValidation()
        {
            RuleFor(x => x.Name).NotEmpty().Length(4,40);
            RuleFor(x => x.City).NotEmpty().Length(4,40);
        }
    }
}
