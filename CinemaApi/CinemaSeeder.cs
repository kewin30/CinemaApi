using CinemaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaApi
{
    public class CinemaSeeder
    {
        private readonly CinemaDbContext _context;

        public CinemaSeeder(CinemaDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Cinemas.Any())
                {
                    IEnumerable<Cinema> cinemas = GetCinemas();
                    _context.Cinemas.AddRange(cinemas);
                    _context.SaveChanges();
                }
                if (!_context.Directors.Any())
                {
                    IEnumerable<Director> directors = GetDirectors();
                    _context.Directors.AddRange(directors);
                    _context.SaveChanges();
                }
                if (!_context.Films.Any())
                {
                    IEnumerable<Films> films = GetFilms();
                    _context.Films.AddRange(films);
                    _context.SaveChanges();
                }
                if (!_context.Actors.Any())
                {
                    IEnumerable<Actors> actors = GetActors();
                    _context.Actors.AddRange(actors);
                    _context.SaveChanges();
                }
                if (!_context.CinemaHalls.Any())
                {
                    IEnumerable<CinemaHall> halls = GetCinemaHalls();
                    _context.CinemaHalls.AddRange(halls);
                    _context.SaveChanges();
                }

                if (!_context.CinemaSeats.Any())
                {
                    IEnumerable<CinemaSeats> seats = GetCinemaSeats();
                    _context.CinemaSeats.AddRange(seats);
                    _context.SaveChanges();
                }
                if (!_context.Roles.Any())
                {
                    IEnumerable<Role> roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
            }
        }
        private IEnumerable<Actors> GetActors()
        {
            List<Actors> actors = new List<Actors>()
            {
                new Actors()
                {
                    FullName = "Robert Downey Junior",
                    Age = 50,
                    Gender = "Male",
                    PlayingAs = "Tony Stark",
                    filmId = 1
                },
                new Actors()
                {
                    FullName = "Chris Evans",
                    Age = 40,
                    Gender = "Male",
                    PlayingAs = "Steve Rogers",
                    filmId = 1
                },
                new Actors()
                {
                    FullName = "Tom Holland",
                    Age = 50,
                    Gender = "Male",
                    PlayingAs = "Peter Parker",
                    filmId = 1
                }
            };
            return actors;
        }
        private IEnumerable<Cinema> GetCinemas()
        {
            DateTime openTime = new DateTime(2023, 1, 13);
            DateTime closeTime = new DateTime(2023, 1, 13);
            List<Cinema> cinemas = new List<Cinema>()
            {
                new Cinema()
                {
                    Name = "Filharmonia Kaszubka",
                    City="Wejherowo",
                    OpenTime = openTime,
                    ClosingTime = closeTime
                },
                new Cinema()
                {
                    Name = "Multikino",
                    City="Gdynia",
                    OpenTime = openTime,
                    ClosingTime = closeTime
                }
            };
            return cinemas;
        }
        private IEnumerable<CinemaHall> GetCinemaHalls()
        {
            List<CinemaHall> cinemaHalls = new List<CinemaHall>()
            {
                new CinemaHall()
                {
                    Name = "Wojskowa",
                    HowManySeats = 250,
                    Row = 10,
                    Column = 25,
                    FilmsId = 1
                },
                new CinemaHall()
                {
                    Name = "Piracia Sala Jacka Sparrowa",
                    HowManySeats = 80,
                    Row = 10,
                    Column = 8,
                    FilmsId = 2
                }
            };
            return cinemaHalls;
        }
        private IEnumerable<CinemaSeats> GetCinemaSeats()
        {
            List<CinemaSeats> cinemaSeats = new List<CinemaSeats>()
            {
                new CinemaSeats()
                {
                    Name = "Wojskowa",
                    Row = 5,
                    Column = 4,
                    IsBought = true,
                    CinemaHallId = 2
                },
                new CinemaSeats()
                {
                    Name = "Wojskowa",
                    Row = 6,
                    Column = 4,
                    IsBought = true,
                    CinemaHallId = 2
                },
            };
            return cinemaSeats;
        }
        private IEnumerable<Director> GetDirectors()
        {
            List<Director> directors = new List<Director>()
            {
                new Director()
                {
                    FullName = "Andrzej Wajda",
                    Age = 50,
                    Gender = "Male"
                },
                new Director()
                {
                    FullName = "Joe Russo",
                    Age = 51,
                    Gender = "Male"
                },
                new Director()
                {
                    FullName = "Anthony Russo",
                    Age = 51,
                    Gender = "Male"
                }
            };
            return directors;
        }
        private IEnumerable<Films> GetFilms()
        {
            List<Films> films = new List<Films>()
            {
                new Films()
                {
                    Name = "Avengers Endgame",
                    Description = "Nice film",
                    PathToVideo = "XXX",
                    PathToImage = "XXX",
                    IsRecommended = 10,
                    DirectorId = 2,
                    CinemaId = 1,
                    Genre = "Science Fiction"
                },
                new Films()
                {
                    Name = "Spider-Man HomeComming",
                    Description = "Amazing film",
                    PathToVideo = "XXX",
                    PathToImage = "XXX",
                    IsRecommended = 10,
                    DirectorId = 2,
                    CinemaId = 1,
                    Genre = "Science Fiction"
                }
            };
            return films;
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name="User"
                },
                new Role()
                {
                    Name="Admin"
                }
            };
            return roles;
        }
    }
}
