using CinemaApi.DTO;
using CinemaApi.DTO.Director;
using CinemaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/director")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }
        [HttpGet("{id}")]
        public ActionResult<DirectorDto> Get([FromRoute] int id)
        {
            DirectorDto director = _directorService.GetDirector(id);
            return Ok(director);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody] DirectorDto dto)
        {
            _directorService.CreateDirector(dto);
            return Ok();
        }
    }
}
