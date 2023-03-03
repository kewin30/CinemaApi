using CinemaApi.DTO.Cinema;
using CinemaApi.DTO.Director;
using CinemaApi.DTO.Films;
using CinemaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/films")]
    [ApiController]
    public class FilmController: ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody] CreateFilmDto dto)
        {
            _filmService.CreateFilm(dto);
            return NoContent();
        }
        [HttpGet("{id}")]
        public ActionResult<GetFilmsDto> Get([FromRoute] int id)
        {
            GetFilmsDto films = _filmService.GetById(id);
            return Ok(films);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult Put([FromBody] FilmDirectorIdDto dto)
        {
            _filmService.UpdateDirector(dto);
            return NoContent();
        }
        [HttpPut("Cinema")]
        [Authorize(Roles = "Admin")]
        public ActionResult PutCinema([FromBody] FilmCinemaId dto)
        {
            _filmService.UpdateCinemaId(dto);
            return NoContent();
        }
    }
}
