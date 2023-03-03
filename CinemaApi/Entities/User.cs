using System.Collections.Generic;

namespace CinemaApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Role Role { get; set; }
        public int RoleId { get; set; } = 2;
        public List<CinemaSeats> CinemaSeats { get; set; }
        public int CinemaSeatsId { get; set; }
    }
}
