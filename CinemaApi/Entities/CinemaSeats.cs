using System.Collections.Generic;

namespace CinemaApi.Entities
{
    public class CinemaSeats
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsBought { get; set; }
        public virtual CinemaHall CinemaHall { get; set; }
        public int CinemaHallId { get; set; }
        public User Users { get; set; }
        public int? UserId { get; set; }
    }
}
