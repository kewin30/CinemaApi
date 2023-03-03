using System;
using System.Collections.Generic;

namespace CinemaApi.Entities
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public virtual List<Films> Films { get; set; }
    }
}
