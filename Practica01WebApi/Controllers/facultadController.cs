using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public facultadController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]

        public IActionResult getAll()
        {
            var facultad = (from db in _equiposContext.facultades
                         select db).ToList();

            if (facultad.Count == 0) { return NotFound(); }

            return Ok(facultad);
        }

        [HttpGet]
        [Route("getbyid/{id}")]

        public IActionResult get(int id)
        {
            var facultad = (from db in _equiposContext.facultades
                         where db.facultad_id == id
                         select db).FirstOrDefault();

            if (facultad == null)
            {
                return NotFound();
            }
            return Ok(facultad);
        }

        [HttpGet]
        [Route("find")]

        public IActionResult find(string filtro)
        {
            var facultad = (from db in _equiposContext.facultades
                         where db.nombre_facultad == filtro
                         select db).ToList();

            if (facultad.Any())
            {
                return Ok(facultad);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult post([FromBody] facultades facuNew)
        {
            try
            {

                _equiposContext.facultades.Add(facuNew);
                _equiposContext.SaveChanges();
                return Ok(facuNew);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult update(int id, [FromBody] facultades modificar)
        {
            facultades? facultad = (from e in _equiposContext.facultades
                             where e.facultad_id == id
                             select e).FirstOrDefault();

            if (facultad == null)
            {
                return NotFound();
            }

           facultad.nombre_facultad= modificar.nombre_facultad;

            _equiposContext.facultades.Entry(facultad).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(facultad);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            facultades? facultad = (from e in _equiposContext.facultades
                             where e.facultad_id == id
                             select e).FirstOrDefault();
            if (facultad == null) { return NotFound(); }
            _equiposContext.facultades.Attach(facultad);
            _equiposContext.facultades.Remove(facultad);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
