using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.WebApi.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private IEventoService _service;
        private IWebHostEnvironment _hostEnvironment;

        public EventosController(IEventoService service, IWebHostEnvironment hostEnvironment)
        {
            _service = service;
        }
        #region  CRUD
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _service.AddEventos(model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar avento.");

                return Created(new Uri(Url.Link("GetById", new { id = evento.Id })), evento);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _service.GetEventoByIdAsync(eventoId);
                if (evento == null) return BadRequest("Erro tentar fazer o upload da imagem.");

                var file = Request.Form.Files[0];
                if(file.Length > 0){
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }

                var EventoRetorno = await _service.UpdateEvento(evento);

                return Ok(EventoRetorno);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EventoDto model)
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
                var evento = await _service.GetEventoByIdAsync(id);
                if (evento == null) return NoContent();


                return await _service.DeleteEvento(id) ?
                    Ok(new { message = "Deletado" }) :
                    throw new Exception("Ocorreu um problema não especificado ao tentar deletar o Evento.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Erro ao deletar eventos. Erro: {ex.Message}");
            }
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _service.GetEventoByIdAsync(id, true);
                if (evento == null) return NoContent();
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
                if (eventos == null) return NoContent();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile) {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ','-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return "string";    
        }

        [NonAction]
        public void DeleteImage(string imageName) {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Rosources/images", imageName);

            if(System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
        }
    }
}
