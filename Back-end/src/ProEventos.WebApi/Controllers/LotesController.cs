using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _service;

        public LotesController(ILoteService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _service.GetLotesByEventoIdAsync(eventoId);
                if (result.Length == 0) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Erro ao tentar recuperar os lotes. Erro: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{eventoId}/{loteId}")]
        public async Task<IActionResult> GetById(int eventoId, int loteId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _service.GetLoteByIdAsync(eventoId, loteId);
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Erro ao tentar recuperar o lote. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, [FromBody] LoteDto[] models)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _service.SaveLote(eventoId, models);
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Erro ao tentar salvar os lotes. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("{eventoId}")]
        public async Task<IActionResult> Post(int eventoId, [FromBody] LoteDto[] models)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _service.SaveLote(eventoId, models);
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Erro ao salvar os lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var resultValidation = await _service.GetLoteByIdAsync(eventoId, loteId);
                if (resultValidation == null) return NoContent();

                return await _service.DeleteLote(eventoId, loteId) ?
                    Ok(new { Message = "Deletado" }) :
                    throw new Exception();

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Erro ao tentar deletar o lote. Erro: {ex.Message}");
            }
        }
    }
}
