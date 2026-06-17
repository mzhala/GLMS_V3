using GLMS.Models;
using GLMS_V3.API.Interfaces;
using GLMS_V3_API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;

    public ClientsController(IClientService service)
        {
            _service = service;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _service.GetAllAsync();

            return Ok(clients);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _service.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<IActionResult> CreateClient(
            [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.CreateAsync(client);

            return CreatedAtAction(
                nameof(GetClient),
                new { id = client.Id },
                client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(
            int id,
            [FromBody] Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(client);

            return NoContent();
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _service.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }
    }

}
