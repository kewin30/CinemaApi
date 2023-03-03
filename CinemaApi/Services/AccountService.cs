using AutoMapper;
using CinemaApi.DTO.Users;
using CinemaApi.Entities;
using CinemaApi.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CinemaApi.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly CinemaDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly IMapper _mapper;

        public AccountService(CinemaDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
            _mapper = mapper;
        }
        public string GenerateJwt(LoginDto dto)
        {
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
                new Claim(ClaimTypes.Name,$"{user.FirstName}{user.LastName}"),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);
            JwtSecurityToken token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
              authenticationSettings.JwtIssuer,
              claims,
              expires: expires,
              signingCredentials: cred);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            string hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            _context.User.Add(user);
            _context.SaveChanges();
        }
    }
}
