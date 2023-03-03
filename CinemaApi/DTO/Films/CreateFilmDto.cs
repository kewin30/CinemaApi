using CinemaApi.DTO.Actors;
using System.Collections.Generic;

namespace CinemaApi.DTO.Films
{
    public class CreateFilmDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathToVideo { get; set; }
        public string PathToImage { get; set; }
        public int IsRecommended { get; set; }
        public string Genre { get; set; }
        public List<ActorsDto> Actors { get; set; }
    }
}
