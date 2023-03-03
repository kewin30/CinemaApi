using System.Collections.Generic;

namespace CinemaApi.Entities
{
    public class Films
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathToVideo { get; set; }
        public string PathToImage { get; set; }
        public int IsRecommended { get; set; }
        public virtual List<Actors> Actors { get; set; }
        public virtual Director Director { get; set; }
        public int? DirectorId { get; set; }
        public virtual Cinema Cinema { get; set; }
        public int? CinemaId { get; set; }
        public string Genre { get; set; }
    }
}
