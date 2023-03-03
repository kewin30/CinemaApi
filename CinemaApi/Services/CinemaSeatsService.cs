using AutoMapper;
using CinemaApi.DTO.CinemaSeats;
using CinemaApi.Entities;
using CinemaApi.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CinemaApi.Services
{
    public interface ICinemaSeatsService
    {
        List<GetByUserId> GetById(int userId);
        void Create(ReservationOfSeat dto);
    }
    public class CinemaSeatsService : ICinemaSeatsService
    {
        private readonly CinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CinemaSeatsService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public CinemaSeatsService(CinemaDbContext context, IMapper mapper, ILogger<CinemaSeatsService> logger, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public List<GetByUserId> GetById(int userId)
        {
            _logger.LogInformation($"User with id: {userId} GetByID action invoked");
            List<User> user = _context.User.Include(x=>x.CinemaSeats).Where(x=>x.Id==userId).ToList();
            if (user.Count==0)
            {
                throw new NotFoundException("User not found!");
            }
            var userDto = _mapper.Map<List<GetByUserId>>(user);
            return userDto;
        }
        private int ReturnUserId(ReservationOfSeat dto)
        {
            _logger.LogWarning("User ReturnUserId method invoked!");
            var user = _context.User.Include(u => u.Role).FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
            string userId = "";
            foreach (var item in claims)
            {
                userId = item.Value;
            }
            return Convert.ToInt32(userId);
        }
        public void Create(ReservationOfSeat dto)
        {
            _logger.LogInformation($"Creating Reservation Action Invoked! ");
            int userId = ReturnUserId(dto);
            CinemaHall cinemaHall = _context.CinemaHalls.FirstOrDefault(c => c.Name == dto.Name);
            if(cinemaHall is null)
            {
                _logger.LogError($"Cinema with name: {dto.Name} not found ");
                throw new NotFoundException("Cinema hall not found! ");
            }
            else
            {
                if(dto.Row<= 0 || cinemaHall.Row < dto.Row)
                {
                    _logger.LogError($"Wrong cinemaSeat Row! Hall max row: {cinemaHall.Row} | {dto.Row}");
                    throw new BadRequestException("Wrong row!");
                }
                if(dto.Column<=0 || cinemaHall.Column < dto.Column)
                {
                    _logger.LogError($"Wrong cinemaSeat Column! Hall max column: {cinemaHall.Column} | {dto.Column}");
                    throw new BadRequestException("Wrong column! ");
                }
            }
            IQueryable<CinemaSeats> seatsList = _context
                                                .CinemaSeats
                                                .Where(x=>x.Row == dto.Row 
                                                 && x.Column == dto.Column 
                                                 && x.CinemaHallId == cinemaHall.Id);
            if (seatsList.Any())
            {
                throw new BadRequestException("Seat has been taken! ");
            }
            CinemaSeats seats = new CinemaSeats()
            {
                Name = cinemaHall.Name,
                Row = dto.Row,
                Column = dto.Column,
                IsBought = true,
                UserId = userId,
                CinemaHallId = cinemaHall.Id
            };
            _context.CinemaSeats.Add(seats);
            _context.SaveChanges();
        }
    }
}
