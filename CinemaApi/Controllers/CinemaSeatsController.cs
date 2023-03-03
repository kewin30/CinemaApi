using CinemaApi.DTO;
using CinemaApi.DTO.CinemaSeats;
using CinemaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CinemaApi.Controllers
{
    [Route("api/seats")]
    [ApiController]
    public class CinemaSeatsController : ControllerBase
    {
        private readonly ICinemaSeatsService _cinemaSeatsService;

        public CinemaSeatsController(ICinemaSeatsService cinemaSeatsService)
        {
            _cinemaSeatsService = cinemaSeatsService;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<GetByUserId>> GetById([FromRoute] int id)
        {
            List<GetByUserId> users = _cinemaSeatsService.GetById(id);
            return Ok(users);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Post([FromBody] ReservationOfSeat dto)
        {
            _cinemaSeatsService.Create(dto);
            return Ok();
        }
    }
}
