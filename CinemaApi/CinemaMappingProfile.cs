using AutoMapper;
using CinemaApi.DTO;
using CinemaApi.DTO.Actors;
using CinemaApi.DTO.Cinema;
using CinemaApi.DTO.CinemaSeats;
using CinemaApi.DTO.Director;
using CinemaApi.DTO.Films;
using CinemaApi.DTO.Users;
using CinemaApi.Entities;

namespace CinemaApi
{
    public class CinemaMappingProfile : Profile
    {
        public CinemaMappingProfile()
        {
            CreateMap<Cinema, CinemaDto>();
            CreateMap<Cinema, GetFilmsDto>();
            CreateMap<Cinema, CreateCinemaDto>();

            CreateMap<Films, GetFilmsDto>();
            CreateMap<Films, FilmsIdDto>();
            CreateMap<Films, CreateFilmDto>();

            CreateMap<Director, DirectorDto>();
            CreateMap<Director, DirectorDto>();

            CreateMap<Actors, ActorsDto>();
            
            CreateMap<CreateFilmDto, Films>();
            
            CreateMap<DirectorDto , Director>();

            CreateMap<ActorsDto , Actors>();

            CreateMap<User , RegisterUserDto>();
            CreateMap<User, GetUserDto>();
            CreateMap<User, GetByUserId>();

            CreateMap<RegisterUserDto  , User>();

            CreateMap<GetByUserId, CinemaSeats>();

            CreateMap<CinemaSeats, GetByUserId>();
            CreateMap<CinemaSeats, SeatData>();

        }
    }
}
