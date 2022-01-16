using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;

namespace ProEventos.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private IEventoService _service;

        public EventosController(IEventoService service)
        {
            _service = service;
        }
        #region  CRUD
        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                var evento = await _service.AddEventos(model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar avento.");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Evento model)
        {
            try
            {
                var evento = await _service.UpdateEvento(model);
                if (evento == null) return BadRequest("Erro ao tentar alterar avento.");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await _service.DeleteEvento(id) ?
                    Ok("Deletado") :
                    BadRequest("Evento não deletado");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(true);
                if (eventos == null) return NotFound("Nenhum evento encontrado.");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _service.GetEventosByIdAsync(id, true);
                if (evento == null) return NotFound("Nenhum evento encontrado.");
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _service.GetAllEventosByTemaAsync(tema, true);
                if (eventos == null) return NotFound("Eventos por tema foram encontrados..");
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
