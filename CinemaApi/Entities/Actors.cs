namespace CinemaApi.Entities
{
    public class Actors
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PlayingAs { get; set; }
        public virtual Films Films { get; set; }
        public int? filmId { get; set; }

    }
}
