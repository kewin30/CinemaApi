using CinemaApi.DTO.Director;
using CinemaApi.Entities;
using FluentValidation;

namespace CinemaApi.DTO.Validation
{
    public class DirectorValidation : AbstractValidator<DirectorDto>
    {
        public DirectorValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().Length(5, 75);
            RuleFor(x => x.Age.ToString()).NotEmpty().MaximumLength(3);
            RuleFor(x => x.Age).NotEmpty().GreaterThan(10).WithMessage("The Age field must be greater than 10.");
            RuleFor(x => x.Gender).Must(x => x == "Male" || x == "Female")
                .WithMessage("Gender must be Male or Female");
        }
        
    }
}
