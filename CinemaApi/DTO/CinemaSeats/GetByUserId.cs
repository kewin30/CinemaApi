using CinemaApi.DTO.Users;
using System.Collections.Generic;

namespace CinemaApi.DTO.CinemaSeats
{
    public class GetByUserId
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SeatData> CinemaSeats { get; set; }
    }
}
