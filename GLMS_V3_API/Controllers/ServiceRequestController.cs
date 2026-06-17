using GLMS.Models;
using GLMS_V3_API.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService _service;

    public ServiceRequestsController(IServiceRequestService service)
        {
            _service = service;
        }

        // GET: api/ServiceRequests
        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            var requests = await _service.GetAllAsync();

            return Ok(requests);
        }

        // GET: api/ServiceRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceRequest(int id)
        {
            var request = await _service.GetByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        // POST: api/ServiceRequests
        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest(
            [FromBody] ServiceRequest serviceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.CreateAsync(serviceRequest);

            return CreatedAtAction(
                nameof(GetServiceRequest),
                new { id = serviceRequest.Id },
                serviceRequest);
        }

        // PUT: api/ServiceRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceRequest(
            int id,
            [FromBody] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(serviceRequest);

            return NoContent();
        }

        // DELETE: api/ServiceRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var request = await _service.GetByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }
    }

}
