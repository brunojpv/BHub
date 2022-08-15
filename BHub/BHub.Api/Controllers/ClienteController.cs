using BHub.Domain.Dtos;
using BHub.Domain.Models;
using BHub.Infra.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IPostQueueService postQueueService;

        public ClienteController(IPostQueueService postQueueService)
        {
            this.postQueueService = postQueueService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MigrationPrice([FromBody] TemplateRabbitClienteDto request)
        {
            try
            {
                var data = await postQueueService.ExecutePostQueue(request);

                var response = new ReturnDefaultService
                {
                    Data = data,
                    Message = "Inserido com sucesso na fila."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
