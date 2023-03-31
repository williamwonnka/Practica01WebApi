using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcaController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public marcaController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]

        public IActionResult getAll()
        {
            var marca = (from db in _equiposContext.marcas
                         select db).ToList();

            if (marca.Count == 0) { return NotFound(); }

            return Ok(marca);
        }

        [HttpGet]
        [Route("getbyid/{id}")]

        public IActionResult get(int id)
        {
            var marca = (from db in _equiposContext.marcas
                         where db.id_marcas == id
                         select db).FirstOrDefault();

            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);
        }

        [HttpGet]
        [Route("find")]

        public IActionResult find(string filtro)
        {
            var marca = (from db in _equiposContext.marcas
                         where db.nombre_marca == filtro
                         select db).ToList();

            if (marca.Any())
            {
                return Ok(marca);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult post([FromBody] Marcas marcaNew)
        {
            try
            {
                
                _equiposContext.marcas.Add(marcaNew);
                _equiposContext.SaveChanges();
                return Ok(marcaNew);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult update(int id, [FromBody] Marcas modificar)
        {
            Marcas? marca = (from e in _equiposContext.marcas
                               where e.id_marcas == id
                               select e).FirstOrDefault();

            if (marca == null)
            {
                return NotFound();
            }

            marca.nombre_marca = modificar.nombre_marca;
            

            _equiposContext.marcas.Entry(marca).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(marca);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            Marcas? marca = (from e in _equiposContext.marcas
                               where e.id_marcas == id
                               select e).FirstOrDefault();
            if (marca == null) { return NotFound(); }
            _equiposContext.marcas.Attach(marca);
            _equiposContext.marcas.Remove(marca);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
