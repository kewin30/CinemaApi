using AutoMapper;
using CinemaApi.DTO.Cinema;
using CinemaApi.DTO.Director;
using CinemaApi.DTO.Films;
using CinemaApi.Entities;
using CinemaApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CinemaApi.Services
{
    public interface IFilmService
    {
        void CreateFilm(CreateFilmDto dto);
        GetFilmsDto GetById(int id);
        void UpdateDirector(FilmDirectorIdDto dto);
        void UpdateCinemaId(FilmCinemaId dto);
    }
    public class FilmService : IFilmService
    {
        private readonly CinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<FilmService> _logger;

        public FilmService(CinemaDbContext context, IMapper mapper, ILogger<FilmService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public void CreateFilm(CreateFilmDto dto)
        {
            Films films = new Films()
            {
                Name = dto.Name,
                Description = dto.Description,
                PathToImage = dto.PathToImage,
                PathToVideo = dto.PathToVideo,
                IsRecommended = dto.IsRecommended,
                Genre = dto.Genre
            };
            _logger.LogInformation("Creating Film action Invoked");
            if(String.IsNullOrEmpty(dto.Name)|| String.IsNullOrEmpty(dto.Description)|| String.IsNullOrEmpty(dto.Genre))
            {
                _logger.LogError("Film can't be empty! ");
                throw new BadRequestException("Name or Description or Genre can't be empty! ");
            }
            _context.Films.Add(films);
            _context.SaveChanges();
            int filmsId = films.Id;
            if(dto.Actors is null)
            {
                throw new BadRequestException("Actors can't be null! ");
            }
            foreach (var item in dto.Actors)
            {
                Actors actors = new Actors()
                {
                    FullName = item.FullName,
                    Age = item.Age,
                    Gender = item.Gender,
                    PlayingAs = item.PlayingAs,
                    filmId = filmsId
                };
                _context.Actors.Add(actors);

            }
            _context.SaveChanges();
        }

        public GetFilmsDto GetById(int id)
        {
            _logger.LogInformation($"Getting Films with id: {id}");
            var product = _context.Films
                .Include(x=>x.Actors)
                .Include(x=>x.Director)
                .FirstOrDefault(x=> x.Id == id);
            if(product is null)
            {
                _logger.LogError($"Film with id: {id} not found! ");
                throw new NotFoundException("Film not found! ");
            }
            GetFilmsDto filmsDto = _mapper.Map<GetFilmsDto>(product);
            return filmsDto;
        }
        public void UpdateDirector(FilmDirectorIdDto dto)
        {
            Director director = _context.Directors.FirstOrDefault(x => x.Id == dto.DirectorId);
            if(director is null)
            {
                throw new NotFoundException("Director not found! ");
            }
            Films film = _context.Films.FirstOrDefault(x => x.Id == dto.FilmId);
            if(film is null)
            {
                throw new NotFoundException("Film not found! ");
            }
            film.DirectorId = dto.DirectorId;
            _context.SaveChanges();
        }
        public void UpdateCinemaId(FilmCinemaId dto)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(x => x.Id == dto.CinemaId);
            if(cinema is null)
            {
                throw new NotFoundException("Cinema not found! ");
            }
            Films film = _context.Films.FirstOrDefault(x => x.Id == dto.FilmId);
            if(film is null)
            {
                throw new NotFoundException("Film not found! ");
            }
            film.CinemaId = dto.CinemaId;
            _context.SaveChanges();
        }
    }
}
