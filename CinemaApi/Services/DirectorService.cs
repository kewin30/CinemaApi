using AutoMapper;
using CinemaApi.DTO;
using CinemaApi.DTO.Director;
using CinemaApi.Entities;
using CinemaApi.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CinemaApi.Services
{
    public interface IDirectorService
    {
        void CreateDirector(DirectorDto dto);
        DirectorDto GetDirector(int id);
    }
    public class DirectorService : IDirectorService
    {
        private readonly CinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DirectorService> _logger;
        public DirectorService(CinemaDbContext context, IMapper mapper, ILogger<DirectorService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public void CreateDirector(DirectorDto dto)
        {
            _logger.LogInformation("Creating direction Action Invoked! ");
            Director director = _mapper.Map<Director>(dto);
            _context.Directors.Add(director);
            _context.SaveChanges();
        }

        public DirectorDto GetDirector(int id)
        {
            _logger.LogInformation($"Director with ID {id} Get action invoked! ");
            Director director = _context.Directors.FirstOrDefault(x => x.Id == id);
            if (director is null)
            {
                _logger.LogError($"Director with id: {id} not found! ");
                throw new NotFoundException("Director not found! ");
            }
            DirectorDto directorDto = _mapper.Map<DirectorDto>(director);
            return directorDto;
        }
    }
}
