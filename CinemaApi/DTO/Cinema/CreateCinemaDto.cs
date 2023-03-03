using CinemaApi.DTO.Films;
using System;
using System.Collections.Generic;

namespace CinemaApi.DTO.Cinema
{
    public class CreateCinemaDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public List<FilmsIdDto> FilmId{get; set;}
    }
}
