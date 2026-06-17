using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using GLMS_V3.API.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _service;
    public ContractsController(IContractService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetContracts()
    {
        var contracts = await _service.GetAllAsync();

        return Ok(contracts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContract(int id)
    {
        var contract = await _service.GetByIdAsync(id);

        if (contract == null)
        {
            return NotFound();
        }

        return Ok(contract);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
    int id,
    [FromBody] ContractStatus status)
    {
        var contract = await _service.GetByIdAsync(id);

        if (contract == null)
        {
            return NotFound();
        }

        await _service.UpdateStatusAsync(id, status);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] Contract contract)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _service.CreateAsync(contract);

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

        await _service.UpdateAsync(contract);

        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        var contract = await _service.GetByIdAsync(id);

        if (contract == null)
        {
            return NotFound();
        }

        await _service.DeleteAsync(id);

        return NoContent();

    }
}