using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ContractService _service;

        public ContractsController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            ContractService service)
        {
            _context = context;
            _environment = environment;
            _service = service;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(
    ContractStatus? status,
    DateTime? startDate,
    DateTime? endDate)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate.Value);
            }

            ViewBag.Status = status;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            var contracts = await query.ToListAsync();

            return View(contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _service.GetByIdAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            LoadClientsDropdown();
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel")]
            Contract contract,
            IFormFile? agreementFile)
        {
            if (agreementFile == null)
            {
                ModelState.AddModelError(
                    "",
                    "Agreement PDF is required.");
            }

            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(agreementFile.FileName);

                if (extension.ToLower() != ".pdf")
                {
                    ModelState.AddModelError(
                        "",
                        "Only PDF files are allowed.");

                    LoadClientsDropdown();

                    return View(contract);
                }

                var fileName =
                    Guid.NewGuid().ToString() + ".pdf";

                var uploadPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "contracts");

                var filePath = Path.Combine(
                    uploadPath,
                    fileName);

                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    await agreementFile.CopyToAsync(stream);
                }

                contract.AgreementFilePath = fileName;

                var result = await _service.CreateAsync(contract);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message);

                    LoadClientsDropdown();

                    return View(contract);
                }

                return RedirectToAction(nameof(Index));
            }

            LoadClientsDropdown();

            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _service.GetByIdAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            LoadClientsDropdown();

            return View(contract);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel,AgreementFilePath")]
        Contract contract,
        IFormFile? agreementFile)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (agreementFile != null)
                {
                    if (agreementFile == null)
                    {
                        ModelState.AddModelError(
                            "",
                            "Agreement PDF is required.");

                        LoadClientsDropdown();

                        return View(contract);
                    }

                    var extension = Path.GetExtension(agreementFile.FileName);

                    if (extension.ToLower() != ".pdf")
                    {
                        ModelState.AddModelError("", "Only PDF files are allowed.");

                        LoadClientsDropdown();

                        return View(contract);
                    }

                    var fileName = Guid.NewGuid().ToString() + ".pdf";

                    var uploadPath = Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "contracts");

                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await agreementFile.CopyToAsync(stream);
                    }

                    contract.AgreementFilePath = fileName;
                }
                try
                {
                    if (agreementFile == null)
                    {
                        var existingContract = await _context.Contracts
                            .AsNoTracking()
                            .FirstOrDefaultAsync(c => c.Id == contract.Id);

                        contract.AgreementFilePath =
                            existingContract?.AgreementFilePath;
                    }
                    _context.Update(contract);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.Exists(contract.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadClientsDropdown();

            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _service.GetByIdAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                _context.Contracts.Remove(contract);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadClientsDropdown()
        {
            ViewData["ClientId"] =
                new SelectList(
                    _context.Clients,
                    "Id",
                    "Name");
        }
    }
}