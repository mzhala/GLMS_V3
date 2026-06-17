using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceRequests
        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ToListAsync();

            return Ok(requests);
        }

        // GET: api/ServiceRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceRequest(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(s => s.Id == id);

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

            _context.ServiceRequests.Add(serviceRequest);

            await _context.SaveChangesAsync();

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

            _context.Entry(serviceRequest).State =
                EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ServiceRequests.Any(s => s.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ServiceRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var request =
                await _context.ServiceRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            _context.ServiceRequests.Remove(request);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
