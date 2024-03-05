using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{

    public class PeliculaController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;


        public PeliculaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Pelicula>>> Get()
        {
            var Pelicula = await _unitOfWork.Peliculas.GetAllAsync();
            return Ok(Pelicula);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pelicula>> Get(int id)
        {
            try
            {
                var pelicula = await _unitOfWork.Peliculas.GetByIdAsync(id);
                
                if (pelicula == null)
                {
                    return NotFound($"No se encontró ninguna película con el ID {id}.");
                }
                
                return Ok(pelicula);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error interno en el servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pelicula>> Post(Pelicula pelicula)
        {
            
           _unitOfWork.Peliculas.Add(pelicula);
            await _unitOfWork.SaveAsync();

            if (pelicula == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            return CreatedAtAction(nameof(Post), new { id = pelicula.Id }, pelicula);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pelicula>> Put(int id, [FromBody] Pelicula peliculaActualizada)
        {
            if (peliculaActualizada == null || peliculaActualizada.Id != id)
            {
                return BadRequest($"No de encontró ninguna pelicula que coincida con el id: {id}");
            }

            var peliculaExistente = await _unitOfWork.Peliculas.GetByIdAsync(id);

            if (peliculaExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(peliculaActualizada.Titulo))
            {
                peliculaExistente.Titulo = peliculaActualizada.Titulo;
            }
            if (!string.IsNullOrEmpty(peliculaActualizada.Director))
            {
                peliculaExistente.Director = peliculaActualizada.Director;
            }
            if (peliculaActualizada.Anio != 0)
            {
                peliculaExistente.Anio = peliculaActualizada.Anio;
            }
            if (!string.IsNullOrEmpty(peliculaActualizada.Genero))
            {
                peliculaExistente.Genero = peliculaActualizada.Genero;
            }

            _unitOfWork.Peliculas.Update(peliculaExistente);
            await _unitOfWork.SaveAsync();

            return Ok(peliculaExistente);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pelicula>> Delete(int id)
        {
            var Pelicula = await _unitOfWork.Peliculas.GetByIdAsync(id);

            if (Pelicula == null)
            {
                return NotFound();
            }

            _unitOfWork.Peliculas.Remove(Pelicula);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}