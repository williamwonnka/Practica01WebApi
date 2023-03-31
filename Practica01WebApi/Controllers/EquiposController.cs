using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public EquiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult ObtenerEquipos()
        {
            var listadoEquipo = (from db in _equiposContext.equipos
                                           join t in _equiposContext.tipo_equipo 
                                                on db.tipo_equipo_id equals t.id_tipo_equipo
                                           join m in _equiposContext.marcas 
                                                on db.marca_id equals m.id_marcas
                                           join es in _equiposContext.estados_equipo
                                                on db.estado_equipo_id equals es.id_estados_equipo
                                                select new
                                                {
                                                    db.id_equipos,
                                                    db.nombre,
                                                    db.descripcion,
                                                    db.tipo_equipo_id,
                                                    tipo_equipo = t.descripcion,
                                                    db.marca_id,
                                                    marca = m.nombre_marca,
                                                    db.estado_equipo_id,
                                                    estado_equipo = es.descripcion,
                                                    db.estado
                                                    }).ToList();

            if (listadoEquipo.Count == 0) { return NotFound(); }

            return Ok(listadoEquipo);
        }


        /// <summary>
        /// Metodo para consultar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult get(int id)
        {
            var equipo = (from db in _equiposContext.equipos
                               join t in _equiposContext.tipo_equipo
                                    on db.tipo_equipo_id equals t.id_tipo_equipo
                               join m in _equiposContext.marcas
                                    on db.marca_id equals m.id_marcas
                               join es in _equiposContext.estados_equipo
                                    on db.estado_equipo_id equals es.id_estados_equipo
                               where db.id_equipos == id 
                          select new
                          {
                              db.id_equipos,
                              db.nombre,
                              db.descripcion,
                              db.tipo_equipo_id,
                              tipo_equipo = t.descripcion,
                              db.marca_id,
                              marca = m.nombre_marca,
                              db.estado_equipo_id,
                              estado_equipo = es.descripcion,
                              db.estado
                          }).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        /// <summary>
        /// Metodo para buscar de acuerdo a un filtro
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("find")]
        public IActionResult buscar(string filtro)
        {
            var equiposList = (from db in _equiposContext.equipos
                                         join t in _equiposContext.tipo_equipo
                                              on db.tipo_equipo_id equals t.id_tipo_equipo
                                         join m in _equiposContext.marcas
                                              on db.marca_id equals m.id_marcas
                                         join es in _equiposContext.estados_equipo
                                              on db.estado_equipo_id equals es.id_estados_equipo
                                         where db.nombre.Contains(filtro) || db.descripcion.Contains(filtro)
                                         select new
                                         {
                                             db.id_equipos,
                                             db.nombre,
                                             db.descripcion,
                                             db.tipo_equipo_id,
                                             tipo_equipo = t.descripcion,
                                             db.marca_id,
                                             marca = m.nombre_marca,
                                             db.estado_equipo_id,
                                             estado_equipo = es.descripcion,
                                             db.estado
                                         }).ToList();

            if (equiposList.Any())
            {
                return Ok(equiposList);
            }

            return NotFound();
        }

        /// <summary>
        /// Metodo para crear un nuevo registro
        /// </summary>
        /// <param name="equiposNew"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public IActionResult crear([FromBody]equipos equiposNew)
        {
            try
            {
                equiposNew.estado = "A";
                _equiposContext.equipos.Add(equiposNew);
                _equiposContext.SaveChanges();
                return Ok(equiposNew);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para actualizar un registro por id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizarEquipo(int id,[FromBody]equipos modificar)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                          where e.id_equipos == id
                          select e).FirstOrDefault();

            if(equipo == null)
            {
                return NotFound();
            }

            equipo.nombre = modificar.nombre;
            equipo.descripcion = modificar.descripcion;

            _equiposContext.equipos.Entry(equipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipo);

        }

        /// <summary>
        /// Metodo para borrar por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if(equipo == null) { return NotFound(); }
            equipo.estado = "I";
            _equiposContext.equipos.Entry(equipo).State = EntityState.Modified;   
            //_equiposContext.Attach(equipo);
            //_equiposContext.Remove(equipo);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }


}
