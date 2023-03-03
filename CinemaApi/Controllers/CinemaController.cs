using CinemaApi.DTO;
using CinemaApi.DTO.Cinema;
using CinemaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CinemaApi.Controllers
{
    [Route("api/cinema")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;

        public CinemaController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CinemaDto>> Get()
        {
            List<CinemaDto> cinemas = _cinemaService.GetAll();
            return Ok(cinemas);
        }
        [HttpGet("{id}")]
        public ActionResult<CinemaDto> GetById([FromRoute]int id)
        {
            CinemaDto cinemas = _cinemaService.GetById(id);
            return Ok(cinemas);
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult Post([FromBody] CreateCinemaDto dto)
        {
            _cinemaService.CreateCinema(dto);
            return Ok();
        }
    }
}
