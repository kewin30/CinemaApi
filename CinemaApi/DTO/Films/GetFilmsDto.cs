using CinemaApi.DTO.Actors;
using CinemaApi.DTO.Director;
using CinemaApi.Entities;
using System.Collections.Generic;

namespace CinemaApi.DTO.Films
{
    public class GetFilmsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathToVideo { get; set; }
        public string PathToImage { get; set; }
        public int IsRecommended { get; set; }
        public string Genre { get; set; }
        public virtual List<ActorsDto> Actors { get; set; }
        public virtual DirectorDto Director { get; set; }
    }
}
