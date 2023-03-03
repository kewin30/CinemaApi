namespace CinemaApi.Entities
{
    public class CinemaHall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HowManySeats { get; set; }
        //public virtual CinemaSeats Seats { get; set; }
        public virtual Films Films { get; set; }
        public int? FilmsId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
