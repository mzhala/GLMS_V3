using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContractsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetContracts()
    {
        var contracts = _context.Contracts.ToList();
        return Ok(contracts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContract(int id)
    {
        var contract = await _context.Contracts
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contract == null)
        {
            return NotFound();
        }

        return Ok(contract);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] Contract contract)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        _context.Contracts.Add(contract);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetContract),
            new { id = contract.Id },
            contract);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContract(int id, [FromBody] Contract contract)
    {
        if (id != contract.Id)
        {
            return BadRequest();
        }

        _context.Entry(contract).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Contracts.Any(c => c.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);

    if (contract == null)
        {
            return NotFound();
        }

        _context.Contracts.Remove(contract);

        await _context.SaveChangesAsync();

        return NoContent();


    }

}