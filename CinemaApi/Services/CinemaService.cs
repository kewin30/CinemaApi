using AutoMapper;
using CinemaApi.DTO;
using CinemaApi.DTO.Cinema;
using CinemaApi.Entities;
using CinemaApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaApi.Services
{
    public interface ICinemaService
    {
        List<CinemaDto> GetAll();
        CinemaDto GetById(int id);
        void CreateCinema(CreateCinemaDto cinema);
    }
    public class CinemaService : ICinemaService
    {
        private readonly CinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CinemaService> _logger;

        public CinemaService(CinemaDbContext context, IMapper mapper, ILogger<CinemaService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public void CreateCinema(CreateCinemaDto dto)
        {
            Cinema cinema = new Cinema()
            {
                Name = dto.Name,
                City = dto.City,
                OpenTime = dto.OpenTime,
                ClosingTime = dto.ClosingTime
            };
            if (String.IsNullOrEmpty(dto.Name) || String.IsNullOrEmpty(dto.City))
            {
                _logger.LogError($"Cinema name or city can't be empty!");
                throw new BadRequestException("Cinema properties can't be empty! ");
            }
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            int cinemaId = cinema.Id;
            foreach (var item in dto.FilmId)
            {
                Films films = _context.Films.FirstOrDefault(x => x.Id == item.Id);
                if (films is null)
                {
                    throw new NotFoundException("Film not found! ");
                }
                films.CinemaId = cinemaId;
                _context.SaveChanges();
            }
        }

        public List<CinemaDto> GetAll()
        {

            List<Cinema> cinemas = _context.Cinemas
            .Include(x => x.Films)
            .ThenInclude(x => x.Director)
            .Include(x => x.Films)
            .ThenInclude(x => x.Actors)
            .ToList();
            if (cinemas.Count == 0)
            {
                throw new NotFoundException("Cinemas not found!");
            }
            List<CinemaDto> cinemasDto = _mapper.Map<List<CinemaDto>>(cinemas);
            return cinemasDto;
        }
        public CinemaDto GetById(int id)
        {
            var cinema = _context.Cinemas
                .Include(x => x.Films)
                .ThenInclude(x => x.Director)
                .Include(x => x.Films)
                .ThenInclude(x => x.Actors)
                .FirstOrDefault(x => x.Id == id);
            if (cinema is null)
            {
                _logger.LogError($"Cinema with id {id} not found! ");
                throw new NotFoundException("Cinema not found! ");
            }
            var cinemaDto = _mapper.Map<CinemaDto>(cinema);
            return cinemaDto;
        }
    }
}
