using CinemaApi.DTO.Films;
using CinemaApi.Entities;
using System;
using System.Collections.Generic;

namespace CinemaApi.DTO
{
    public class CinemaDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public List<GetFilmsDto> Films { get; set; }

    }
}
